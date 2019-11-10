using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Castle.Mvc.Attributes;
using Castle.Mvc.Models;
using Castle.Services;

namespace Castle.Mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public HomeController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [CastleDemo(typeof(HomeController))]
        public ActionResult Index()
        {
            //var x = assemblies.GetUsername();
            var user = new User { Name = "Ahmad", Age = 28 };
            UserDto userDto = _mapper.Map<UserDto>(user);
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}