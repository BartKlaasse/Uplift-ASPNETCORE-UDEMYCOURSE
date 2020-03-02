using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Uplift.DataAccess.Data.IRepository;
using Uplift.Models;

namespace Uplift.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {

        private readonly IUnitOfWork _unitOfWOrk;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWOrk = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            Category category = new Category();
            if (id == null)
            {
                return View(category);
            }
            category = _unitOfWOrk.Category.Get(id.GetValueOrDefault());
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        #region API Calls
        [HttpGet]
        public IActionResult GetAll()
        {
            return Json(new { data = _unitOfWOrk.Category.GetAll() });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objFromDb = _unitOfWOrk.Category.Get(id);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "error while deleting category" });
            }
            _unitOfWOrk.Category.Remove(objFromDb);
            _unitOfWOrk.Save();
            return Json(new { success = true, message = "Deleting successful" });
        }
        #endregion
    }
}