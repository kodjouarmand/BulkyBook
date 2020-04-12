using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using Microsoft.AspNetCore.Mvc;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;

namespace BulkyBook.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
    public class UserController : Controller
    {
        IUnitOfWork _unitOfWork;
        ApplicationDbContext _db;
        public UserController(IUnitOfWork unitOfWork, ApplicationDbContext db)
        {
            _unitOfWork = unitOfWork;
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }



        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            var userList = _unitOfWork.ApplicationUser.GetAll(includeProperties: "Company");
            
            foreach(var user in userList)
            {
                user.Role = _unitOfWork.ApplicationUser.GetRole(user).Name;
                if (user.Company == null)
                {
                    user.Company = new Company()
                    {
                        Name = ""
                    };
                }
            }

            return Json(new { data = userList });
        }

      [HttpPost]
      public IActionResult Lock([FromBody] string id)
        {
            var objFromDb = _unitOfWork.ApplicationUser.Get(id);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error while Locking" });
            }
            objFromDb.LockoutEnd = DateTime.Now.AddYears(1000);
            
           _unitOfWork.Save();
            return Json(new { success = true, message = "Operation Successful." });
        }

        [HttpPost]
        public IActionResult Unlock([FromBody] string id)
        {
            var objFromDb = _unitOfWork.ApplicationUser.Get(id);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error while Unlocking" });
            }
            
            objFromDb.LockoutEnd = DateTime.Now;
            
            _unitOfWork.Save();
            return Json(new { success = true, message = "Operation Successful." });
        }

        #endregion
    }
}