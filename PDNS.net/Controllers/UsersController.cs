using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PDNS.net.Data;
using PDNS.net.Models;

namespace PDNS.net.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private readonly DBContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly UserManager<User> _userManager;
        private Logger Logger { get; }
        private IPasswordHasher<User> _passwordHasher;
        private string Message = string.Empty, MessageTitle = "Failed", MessageIcon = "error";

        public UsersController(DBContext context, IWebHostEnvironment hostEnvironment, Logger logger, UserManager<User> userManager, IPasswordHasher<User> passwordHasher)
        {
            _context = context;
            webHostEnvironment = hostEnvironment;
            Logger = logger;
            _userManager = userManager;
            _passwordHasher = passwordHasher;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                return View(await _context.Users.ToListAsync());
            }
            catch (Exception ex)
            {
                return new ObjectResult(ex)
                {
                    Value = ex.Message,
                    StatusCode = 500
                };
            }
        }

        public IActionResult New()
        {
            return View();
        }

        [Route("/Profile")]
        public async Task<IActionResult> Profile()
        {
            var currentUser = await _userManager.GetUserAsync(this.User);
            return View(currentUser);
        }

        public async Task<IActionResult> Add(UserViewModel user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string profile = (user.Profile != null) ? Tools.UploadProfile(user.Profile, webHostEnvironment) : @"\assets\img\profiles\user.jpg";
                    var member = new User()
                    {
                        Birthday = Tools.GetDate(user.Birthday),
                        Email = user.Email,
                        Name = user.Name,
                        Profile = profile,
                        UserName = user.Username
                    };
                    var status = await _userManager.CreateAsync(member, user.Password);
                    if (status.Succeeded)
                    {
                        Logger.Log(Logger.LogType.INSERT,
                            JsonConvert.SerializeObject(member),
                            "Users",
                            member.Id);
                        await _context.SaveChangesAsync();
                        MessageTitle = "Done";
                        Message = "User added successfully.";
                        MessageIcon = "success";
                    }
                    else
                    {
                        MessageTitle = "Failed";
                        MessageIcon = "error";
                        Message = "User not added.";
                        foreach (var err in status.Errors)
                        {
                            Message += $@"<br>{err.Description}";
                        }
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
            catch(Exception ex)
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

        public async Task<IActionResult> Edit(UserViewModel user)
        {
            MessageTitle = "Failed";
            MessageIcon = "error";
            try
            {
                if (ModelState.IsValid)
                {
                    var member = await _userManager.FindByIdAsync(user.ID.ToString());
                    if (member != null)
                    {
                        var ByUsername = _userManager.Users.Where(x => x.UserName == user.Username);
                        if (ByUsername.Any() && ByUsername.Single() != member)
                        {
                            MessageTitle = "Failed";
                            Message = "Duplicate username.";
                            MessageIcon = "error";
                        }
                        else
                        {
                            member.Profile = (user.Profile != null) ? Tools.UploadProfile(user.Profile, webHostEnvironment) : member.Profile;
                            if (!string.IsNullOrWhiteSpace(user.ModifyPassword) && user.ModifyPassword.ToLower() == "on" && !string.IsNullOrWhiteSpace(user.Password))
                                member.PasswordHash = _passwordHasher.HashPassword(member, user.Password);
                            if (!string.IsNullOrWhiteSpace(user.Name))
                                member.Name = user.Name;
                            if (!string.IsNullOrWhiteSpace(user.Birthday) && user.Birthday != "01/01/0001")
                                member.Birthday = Tools.GetDate(user.Birthday);
                            if (!string.IsNullOrWhiteSpace(user.Email))
                                member.Email = user.Email;
                            if (!string.IsNullOrWhiteSpace(user.Username))
                                member.UserName = user.Username;
                            var status = await _userManager.UpdateAsync(member);
                            if (status.Succeeded)
                            {
                                Logger.Log(Logger.LogType.UPDATE,
                                    JsonConvert.SerializeObject(member),
                                    "Users",
                                    member.Id);
                                await _context.SaveChangesAsync();
                                MessageTitle = "Done";
                                Message = "User updated successfully.";
                                MessageIcon = "success";
                            }
                            else
                            {
                                MessageTitle = "Failed";
                                MessageIcon = "error";
                                Message = string.Empty;
                                foreach (var err in status.Errors)
                                {
                                    Message += $@"<br>{err.Description}";
                                }
                            }
                        }
                    }
                    else
                    {
                        Message = @"The requested user not found";
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

        public async Task<IActionResult> Delete(int id)
        {
            MessageTitle = "Failed";
            MessageIcon = "error";
            try
            {
                if (id == 1)
                {
                    MessageTitle = "Failed";
                    MessageIcon = "error";
                    Message = "You can't remove root account";
                }
                else
                {
                    var member = await _userManager.FindByIdAsync(id.ToString());
                    if (member != await _userManager.GetUserAsync(this.User))
                    {
                        var status = await _userManager.DeleteAsync(member);
                        if (status.Succeeded)
                        {
                            Logger.Log(Logger.LogType.DELETE,
                                JsonConvert.SerializeObject(member),
                                "Users",
                                member.Id);
                            await _context.SaveChangesAsync();
                            MessageTitle = "Done";
                            MessageIcon = "success";
                            Message = "Domain deleted successfully.";
                        }
                        else
                        {
                            MessageTitle = "Failed";
                            MessageIcon = "error";
                            Message = string.Empty;
                            foreach (var err in status.Errors)
                            {
                                Message += $@"<br>{err.Description}";
                            }
                        }
                    }
                    else
                    {
                        MessageTitle = "Failed";
                        MessageIcon = "error";
                        Message = "Can't remove current user.";
                    }
                }
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
    }
}
