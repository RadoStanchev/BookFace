using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookFace.Controllers
{
    [Authorize]
    public class ChatsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
