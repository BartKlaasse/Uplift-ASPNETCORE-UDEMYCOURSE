using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Uplift.DataAccess.Data.IRepository;

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