using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PDNS.net
{
    public class Tools
    {
        public static async Task<PingResult> PingHost(string nameOrAddress)
        {
            Ping pinger = new Ping();
            PingReply[] pingReplies = new PingReply[4];

            for (int i = 0; i < 4; i++)
            {
                try
                {
                    pingReplies[i] = await pinger.SendPingAsync(nameOrAddress);
                }
                catch (PingException)
                {
                    // Discard PingExceptions and return false;
                }
            }
            pinger.Dispose();
            return new PingResult(pingReplies);
        }

        public static bool IsPrivateIP(string ipAddress)
        {
            int[] ipParts = ipAddress.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries)
                                     .Select(s => int.Parse(s)).ToArray();

            return (ipParts[0] == 10 || ipParts[0] == 127 ||
                (ipParts[0] == 192 && ipParts[1] == 168) ||
                (ipParts[0] == 172 && (ipParts[1] >= 16 && ipParts[1] <= 31)));
        }

        public static Task<Dictionary<string, int>> IPCountries(List<string> IPList)
        {
            var IPCountries = new Dictionary<string, int>
            {
                { "private", 0 }
            };
            foreach (var IP in IPList)
            {
                if (IsPrivateIP(IP))
                {
                    IPCountries["private"] += 1;
                    continue;
                }
                var result = new WebClient().DownloadString("https://freegeoip.app/json/" + IP);
                var json = JObject.Parse(result);
                var country = json["country_code"].ToString().ToLower();
                if (!IPCountries.ContainsKey(country))
                    IPCountries.Add(country, 0);
                IPCountries[country] += 1;
            }
            return Task.FromResult(IPCountries);
        }

        public static string MD5Hash(string input)
        {
            using (var md5 = MD5.Create())
            {
                var result = md5.ComputeHash(Encoding.ASCII.GetBytes(input));
                return Convert.ToBase64String(result);
                //return Encoding.ASCII.GetString(result);
            }
        }

        public static List<string> GetCurrentClients()
        {
            try
            {
                Process process = new Process
                {
                    StartInfo = new ProcessStartInfo()
                    {
                        UseShellExecute = false,
                        CreateNoWindow = true,
                        WindowStyle = ProcessWindowStyle.Hidden,
                        FileName = "/bin/bash",
                        Arguments = "pdns_control remotes",
                        RedirectStandardError = true,
                        RedirectStandardOutput = true
                    }
                };
                process.Start();
                process.WaitForExit(0);
                return new List<string>(process.StandardOutput.ReadToEnd().Split(Environment.NewLine));
            }
            catch
            {
                return new List<string>();
            }
        }

        public static string UploadProfile(IFormFile file, IWebHostEnvironment webHostEnvironment)
        {
            if (IsImage(file))
            {
                var uniqueFileName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(file.FileName);
                var uploads = Path.Combine(webHostEnvironment.WebRootPath, @"assets\img\profiles");
                var filePath = Path.Combine(uploads, uniqueFileName);
                file.CopyTo(new FileStream(filePath, FileMode.Create));
                return @"\assets\img\profiles\" + uniqueFileName;
            }
            else
                return @"\assets\img\profiles\user.jpg";
        }

        public static bool IsImage(IFormFile file)
        {
            try
            {
                var img = System.Drawing.Image.FromStream(file.OpenReadStream());
                img.Dispose();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static DateTime GetDate(string date) => DateTime.ParseExact(date, "dd/MM/yyyy", CultureInfo.CurrentCulture);
        public static string GetDate(DateTime date) => date.Date.ToString("dd/MM/yyyy");
    }

    public class PingResult
    {
        public long Requests { get; }
        public long Success { get; }
        public long Delay { get; }
        public byte SuccessPercentage { get; }

        public PingResult(params PingReply[] reply)
        {
            Requests = reply.Length;
            Success = reply.Where(x => x.Status == IPStatus.Success).Count();
            Delay = GetDelay(reply);
            SuccessPercentage = (byte)(Success * 100 / Requests);
        }

        private long GetDelay(params PingReply[] reply)
        {
            long d = 0;
            for (int i = 0; i < reply.Length; i++)
            {
                d += reply[i].RoundtripTime;
            }
            return d / reply.Length;
        }
    }
}
