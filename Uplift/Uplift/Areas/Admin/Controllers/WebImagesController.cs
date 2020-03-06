using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Uplift.DataAccess.Data;
using Uplift.DataAccess.Data.IRepository;
using Uplift.Models;
using Uplift.Utility;

namespace Uplift.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class WebImagesController : Controller
    {

        private readonly ApplicationDbContext _db;

        public WebImagesController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            return View();
        }

        // public IActionResult Upsert(int? id)
        // {
        //     Category category = new Category();
        //     if (id == null)
        //     {
        //         return View(category);
        //     }
        //     category = _unitOfWOrk.Category.Get(id.GetValueOrDefault());
        //     if (category == null)
        //     {
        //         return NotFound();
        //     }
        //     return View(category);
        // }

        // [HttpPost]
        // [ValidateAntiForgeryToken]
        // public IActionResult Upsert(Category category)
        // {
        //     if (ModelState.IsValid)
        //     {
        //         if (category.Id == 0)
        //         {
        //             _unitOfWOrk.Category.Add(category);
        //         }
        //         else
        //         {
        //             _unitOfWOrk.Category.Update(category);
        //         }
        //         _unitOfWOrk.Save();
        //         return RedirectToAction(nameof(Index));
        //     }
        //     return View(category);
        // }

        #region API Calls
        [HttpGet]
        public IActionResult GetAll()
        {
            // return Json(new { data = _unitOfWOrk.Category.GetAll() });
            return Json(new { data = _db.WebImages.ToList() });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objFromDb = _db.Category.Find(id);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "error while deleting category" });
            }
            _db.Category.Remove(objFromDb);
            _db.SaveChanges();
            return Json(new { success = true, message = "Deleting successful" });
        }
        #endregion
    }
}