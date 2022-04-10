using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserManagementApp.Models;
using UserManagementApp.Services;

namespace UserManagementApp.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: User
        [Authorize]
        public ActionResult Index()
        {
            IEnumerable<User> Users = _userService.GetUsers();
            return View(Users);
        }

        // GET: User/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            if (ModelState.IsValid)
            {
                User user = new User {
                    UserName = collection["UserName"],
                    Password = collection["Password"],
                    SureName = collection["SureName"],
                    FirstName = collection["FirstName"],
                    DateOfBirth = DateTime.Parse(collection["DateOfBirth"]),
                    PlaceOfBirth = collection["PlaceOfBirth"],
                    PlaceOfLiving = collection["PlaceOfLiving"]
                };
                _userService.CreateUser(user);
                try
                {
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    return View();
                }
            }

            return View();
        }

        // GET: User/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            User user = _userService.GetUser(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: User/Edit/5
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            if (ModelState.IsValid)
            {
                User user = new User
                {
                    Id = id,
                    UserName = collection["UserName"],
                    SureName = collection["SureName"],
                    FirstName = collection["FirstName"],
                    DateOfBirth = DateTime.Parse(collection["DateOfBirth"]),
                    PlaceOfBirth = collection["PlaceOfBirth"],
                    PlaceOfLiving = collection["PlaceOfLiving"]
                };

                _userService.EditUser(id, user);
                try
                {
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    return View();
                }
            }

            return View();

        }

        public ActionResult SaveXml()
        {
            _userService.WriteUsersToXml();
            return RedirectToAction(nameof(Index));
        }

    }
}
