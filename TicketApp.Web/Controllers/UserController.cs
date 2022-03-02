using ExcelDataReader;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using TicketApp.Domain.DomainModels;
using TicketApp.Domain.Identity;

namespace TicketApp.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<TicketAppApplicationUser> _userManager;

        public UserController(UserManager<TicketAppApplicationUser> userManager)
        {
            _userManager = userManager;
        }

       

        public IActionResult ImportUsers()
        {
            return View();
        }

            [HttpPost]
        public IActionResult ImportUsers(IFormFile file)
        {

            //make a copy
            string pathToUpload = $"{Directory.GetCurrentDirectory()}\\files\\{file.FileName}";

            using (FileStream fileStream = System.IO.File.Create(pathToUpload))
            {
                file.CopyTo(fileStream);

                fileStream.Flush();
            }

            //read data from copy file

            List<User> users = getAllUsersFromFile(file.FileName);


            Boolean status = true;

            foreach (var item in users)
            {
                var userCheck = _userManager.FindByEmailAsync(item.Email).Result;

                if (userCheck == null)
                {
                    var user = new TicketAppApplicationUser
                    {
                        UserName = item.Email,
                        NormalizedUserName = item.Email,
                        Email = item.Email,
                        EmailConfirmed = true,
                        PhoneNumberConfirmed = true,
                        Role = item.Role,
                        //placeholder for ROLE
                        UserCart = new ShoppingCart()
                        
                    };
                    var result = _userManager.CreateAsync(user, item.Password).Result;

                    status = status && result.Succeeded;
                }
                else
                {
                    continue;
                }
            }

            ViewBag.Message = status;

            return View();
        }

        private List<User> getAllUsersFromFile(string fileName)
        {
            List<User> usersToRead = new List<User>();

            string filePath = $"{Directory.GetCurrentDirectory()}\\files\\{fileName}";

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            using (var stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    while (reader.Read())
                    {
                        usersToRead.Add(new User
                        {
                            Email = reader.GetValue(0).ToString(),
                            Password = reader.GetValue(1).ToString(),
                            ConfirmPassword = reader.GetValue(2).ToString(),
                            Role = reader.GetValue(3).ToString()
                        });
                    }
                }
            }

            return usersToRead;
        }
    }



}

