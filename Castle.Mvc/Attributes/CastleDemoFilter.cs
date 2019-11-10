using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Castle.Framework.Filters;
using Castle.Services;

namespace Castle.Mvc.Attributes
{
    public class CastleDemoFilter : IActionFilter, IMvcAggregatableFilter
    {
        public IUserService UserService { get; set; }
        private readonly IMapper mapper;

        public CastleDemoFilter(IMapper mapper)
        {
            this.mapper = mapper;
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            throw new NotImplementedException();
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var mapperType= mapper.GetType();
            var name = UserService.GetUsername();
        }
    }
}