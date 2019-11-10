using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Castle.Services;

namespace Castle.Mvc.Attributes
{
    public class CastleDemoAttribute : ActionFilterAttribute
    {
        public IUserService UserService { get; set; }
        private readonly Type type;

        public CastleDemoAttribute(Type type)
        {
            this.type = type;
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var username = UserService.GetUsername();
            var typeName = type.Name;
            base.OnActionExecuting(filterContext);
        }
    }
}