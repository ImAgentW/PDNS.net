using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PDNS.net.Data;
using PDNS.net.Models;

namespace PDNS.net.Controllers
{
    [Authorize]
    public class DomainsController : Controller
    {
        private readonly DBContext _context;
        private Logger Logger { get; }
        private static byte SerialCounter = 0;
        private string Message = string.Empty, MessageTitle = "Failed", MessageIcon = "error";

        public DomainsController(DBContext context, Logger logger)
        {
            _context = context;
            Logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.SOAs = await _context.Records.Where(r => r.Type.ToUpper() == "SOA").ToListAsync();
            return View(await _context.Domains.ToListAsync());
        }

        public async Task<IActionResult> Records(int id)
        {
            ViewBag.Domain = await _context.Domains.Where(x => x.ID == id).SingleAsync();
            return View(await _context.Records.Where(x => x.Domain_ID == id).ToListAsync());
        }

        public IActionResult New()
        {
            return View();
        }

        public async Task<IActionResult> Add(DomainViewModel domain)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var member = new Domain()
                    {
                        Name = domain.Name,
                        Type = domain.Type.ToString()
                    };
                    _context.Add(member);
                    await _context.SaveChangesAsync();
                    if (domain.Type != Domain.DomainType.SLAVE)
                    {
                        domain.TTL = (domain.TTL == 0) ? 1800 : domain.TTL;
                        var soa = new Record()
                        {
                            Domain_ID = member.ID,
                            Name = domain.Name,
                            Type = Record.RecordType.SOA.ToString(),
                            TTL = domain.TTL,
                            Auth = 1,
                            Content = $@"{domain.Primary} {domain.Email.Replace('@', '.')} {DateTime.Now:yyyyMMdd}{++SerialCounter:00} {domain.Refresh} {domain.Retry} {domain.Expire} {domain.TTL}"
                        };
                        _context.Add(soa);
                    }

                    Logger.Log(Logger.LogType.INSERT,
                        JsonConvert.SerializeObject(domain),
                        "Domains",
                        member.ID);
                    await _context.SaveChangesAsync();
                    MessageTitle = "Done";
                    Message = "Domain added successfully.";
                    MessageIcon = "success";
                }
                else
                {
                    string errors = string.Empty;
                    foreach (var mv in ModelState.Values)
                    {
                        foreach (var error in mv.Errors)
                        {
                            errors += error.ErrorMessage + "<br>";
                        }
                    }
                    Message = @$"Invalid Data, {ModelState.ErrorCount} errors.<br>Errors:{errors}";
                    MessageTitle = "Failed";
                    MessageIcon = "error";
                }
            }
            catch (Exception ex)
            {
                Message = $@"{ex.Message}";
                if (ex.InnerException != null)
                    Message += $@"<br>{ex.InnerException.Message}";
                MessageTitle = "Failed";
                MessageIcon = "error";
            }
            return new JsonResult(new Dictionary<string, string>()
            {
                { "Message", Message },
                { "Title", MessageTitle },
                { "Icon", MessageIcon }
            })
            {
                StatusCode = (MessageIcon == "error") ? 500 : 200
            };
        }

        public async Task<IActionResult> AddRecord(RecordViewModel record)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var member = new Record()
                    {
                        Name = record.Name + $".{_context.Domains.Find(record.DomainID).Name}",
                        Type = record.Type.ToString(),
                        Content = record.Content,
                        Domain_ID = record.DomainID,
                        TTL = (record.TTL >= 0) ? record.TTL : 86400,
                        Prio = 0,
                        Change_Date = (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds,
                        Auth = 1
                    };
                    _context.Add(member);
                    Logger.Log(Logger.LogType.INSERT,
                        JsonConvert.SerializeObject(member),
                        "Records",
                        member.ID);
                    await _context.SaveChangesAsync();
                    MessageTitle = "Done";
                    Message = "Record added successfully.";
                    MessageIcon = "success";
                }
                else
                {
                    string errors = string.Empty;
                    foreach (var mv in ModelState.Values)
                    {
                        foreach (var error in mv.Errors)
                        {
                            errors += error.ErrorMessage + "<br>";
                        }
                    }
                    Message = @$"Invalid Data, {ModelState.ErrorCount} errors.<br>Errors:{errors}";
                    MessageTitle = "Failed";
                    MessageIcon = "error";
                }
            }
            catch (Exception ex)
            {
                Message = $@"{ex.Message}";
                if (ex.InnerException != null)
                    Message += $@"<br>{ex.InnerException.Message}";
                MessageTitle = "Failed";
                MessageIcon = "error";
            }
            return new JsonResult(new Dictionary<string, string>()
            {
                { "Message", Message },
                { "Title", MessageTitle },
                { "Icon", MessageIcon }
            })
            {
                StatusCode = (MessageIcon == "error") ? 500 : 200
            };
        }

        public async Task<IActionResult> Edit(DomainViewModel model)
        {
            MessageTitle = "Failed";
            MessageIcon = "error";
            try
            {
                if (ModelState.IsValid)
                {
                    if (model.Type != Domain.DomainType.SLAVE)
                    {
                        var s = _context.Records.Where(x => x.Domain_ID == model.ID && x.Type.ToUpper() == "SOA");
                        var soa = s.Single();
                        soa.TTL = model.TTL;
                        soa.Content = $@"{model.Primary} {model.Email.Replace('@', '.')} {DateTime.Now:yyyyMMdd}{++SerialCounter:00} {model.Refresh} {model.Retry} {model.Expire} {model.TTL}";
                    }
                    Logger.Log(Logger.LogType.UPDATE,
                        JsonConvert.SerializeObject(model),
                        "Domains",
                        model.ID);
                    await _context.SaveChangesAsync();
                    MessageTitle = "Done";
                    Message = "Domain updated successfully.";
                    MessageIcon = "success";
                }
                else
                {
                    string errors = string.Empty;
                    foreach (var mv in ModelState.Values)
                    {
                        foreach (var error in mv.Errors)
                        {
                            errors += error.ErrorMessage + "<br>";
                        }
                    }
                    Message = @$"Invalid Data, {ModelState.ErrorCount} errors.<hr>Errors:{errors}";
                    MessageTitle = "Failed";
                    MessageIcon = "error";
                }
            }
            catch (Exception ex)
            {
                Message = $@"{ex.Message}";
                if (ex.InnerException != null)
                    Message += $@"<br>{ex.InnerException.Message}";
                MessageTitle = "Error";
                MessageIcon = "error";
            }
            return new JsonResult(new Dictionary<string, string>()
            {
                { "Message", Message },
                { "Title", MessageTitle },
                { "Icon", MessageIcon }
            })
            {
                StatusCode = (MessageIcon == "error") ? 500 : 200
            };
        }

        public async Task<IActionResult> EditRecord(RecordViewModel model)
        {
            MessageTitle = "Failed";
            MessageIcon = "error";
            try
            {
                if (ModelState.IsValid)
                {
                    var record = _context.Records.Find(model.ID);
                    if (record != null)
                    {
                        record.TTL = model.TTL;
                        record.Name = model.Name;
                        record.Content = model.Content;
                        record.Type = model.Type.ToString();
                        Logger.Log(Logger.LogType.UPDATE,
                            JsonConvert.SerializeObject(model),
                            "Records",
                            model.ID);
                        await _context.SaveChangesAsync();
                        MessageTitle = "Done";
                        MessageIcon = "success";
                        Message = "Record updated successfully.";
                    }
                    else
                    {
                        Message = @"The requested record not found";
                        MessageTitle = "Failed";
                        MessageIcon = "error";
                    }
                }
                else
                {
                    string errors = string.Empty;
                    foreach (var mv in ModelState.Values)
                    {
                        foreach (var error in mv.Errors)
                        {
                            errors += error.ErrorMessage + "<br>";
                        }
                    }
                    Message = @$"Invalid Data, {ModelState.ErrorCount} errors.<br>Errors:{errors}";
                    MessageTitle = "Failed";
                    MessageIcon = "error";
                }
            }
            catch (Exception ex)
            {
                Message = $@"{ex.Message}";
                if (ex.InnerException != null)
                    Message += $@"<br>{ex.InnerException.Message}";
                MessageTitle = "Failed";
                MessageIcon = "error";
            }
            return new JsonResult(new Dictionary<string, string>()
            {
                { "Message", Message },
                { "Title", MessageTitle },
                { "Icon", MessageIcon }
            })
            {
                StatusCode = (MessageIcon == "error") ? 500 : 200
            };
        }

        public async Task<IActionResult> Delete(int id)
        {
            MessageTitle = "Failed";
            MessageIcon = "error";
            try
            {
                var member = _context.Domains.Find(id);
                Logger.Log(Logger.LogType.DELETE,
                    JsonConvert.SerializeObject(member),
                    "Domains",
                    id);
                _context.Domains.Remove(_context.Domains.Find(id));
                await _context.SaveChangesAsync();
                MessageTitle = "Done";
                MessageIcon = "success";
                Message = "Domain deleted successfully.";
            }
            catch (Exception ex)
            {
                MessageTitle = "Failed";
                MessageIcon = "error";
                Message = $@"{ex.Message}";
                if (ex.InnerException != null)
                    Message += $@"<br>{ex.InnerException.Message}";
            }
            return new JsonResult(new Dictionary<string, string>()
            {
                { "Message", Message },
                { "Title", MessageTitle },
                { "Icon", MessageIcon }
            })
            {
                StatusCode = (MessageIcon == "error") ? 500 : 200
            };
        }

        public async Task<IActionResult> DeleteRecord(int id)
        {
            MessageTitle = "Failed";
            MessageIcon = "error";
            var member = _context.Records.Find(id);
            try
            {
                _context.Records.Remove(_context.Records.Find(id));
                Logger.Log(Logger.LogType.DELETE,
                    JsonConvert.SerializeObject(member),
                    "Records",
                    member.ID);
                await _context.SaveChangesAsync();
                MessageTitle = "Done";
                MessageIcon = "success";
                Message = "Record deleted successfully.";
            }
            catch (Exception ex)
            {
                Message = $@"{ex.Message}";
                if (ex.InnerException != null)
                    Message += $@"<br>{ex.InnerException.Message}";
                MessageTitle = "Failed";
                MessageIcon = "error";
            }
            return new JsonResult(new Dictionary<string, string>()
            {
                { "Message", Message },
                { "Title", MessageTitle },
                { "Icon", MessageIcon }
            })
            {
                StatusCode = (MessageIcon == "error") ? 500 : 200
            };
        }
    }
}
