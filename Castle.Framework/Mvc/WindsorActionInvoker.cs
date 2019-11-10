using System.Collections.Generic;
using System.Web.Mvc;
using Castle.DependencyInjection.Extensions;
using Castle.Windsor;

namespace Castle.Framework.Mvc
{
    public class WindsorActionInvoker : ControllerActionInvoker, IActionInvoker
    {
        readonly IWindsorContainer container;

        public WindsorActionInvoker(IWindsorContainer container)
        {
            this.container = container;
        }

        protected override ActionExecutedContext InvokeActionMethodWithFilters(
                ControllerContext controllerContext,
                IList<IActionFilter> filters,
                ActionDescriptor actionDescriptor,
                IDictionary<string, object> parameters)
        {
            foreach (IActionFilter actionFilter in filters)
            {
                container.Kernel.InjectProperties(actionFilter);
            }
            return base.InvokeActionMethodWithFilters(controllerContext, filters, actionDescriptor, parameters);
        }
    }
}