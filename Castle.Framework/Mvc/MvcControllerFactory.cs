using System;
using System.Web.Mvc;
using System.Web.Routing;
using Castle.Windsor;

namespace Castle.Framework.Mvc
{
    public class MvcControllerFactory : DefaultControllerFactory
    {
        readonly IWindsorContainer container;

        public MvcControllerFactory(IWindsorContainer container)
        {
            this.container = container;
        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            if (controllerType == null)
            {
                return base.GetControllerInstance(requestContext, controllerType);
            }
            var controller = container.Resolve(controllerType) as Controller;

            if (controller != null)
            {
                controller.ActionInvoker = container.Resolve<IActionInvoker>();
            }

            return controller;
        }

        public override void ReleaseController(IController controller)
        {
            container.Release(controller);
        }
    }
}