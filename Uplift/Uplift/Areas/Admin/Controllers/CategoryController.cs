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
    }
}