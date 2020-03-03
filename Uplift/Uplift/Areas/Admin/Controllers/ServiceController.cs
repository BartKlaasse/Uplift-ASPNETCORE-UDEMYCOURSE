using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Uplift.DataAccess.Data.IRepository;
using Uplift.Models;

namespace Uplift.Controllers
{
    [Area("Admin")]
    public class ServiceController : Controller
    {

        private readonly IUnitOfWork _unitOfWOrk;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public ServiceController(IUnitOfWork unitOfWork, IWebHostEnvironment hostingEnvironment)
        {
            _unitOfWOrk = unitOfWork;
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        #region API Calls
        public IActionResult GetAll()
        {
            return Json(new
            {
                data = _unitOfWOrk.Service.GetAll(includeProperties: "Category,Frequency")
            });
        }
        #endregion

    }
}