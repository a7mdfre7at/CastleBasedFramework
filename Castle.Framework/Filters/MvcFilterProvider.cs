using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Castle.Windsor;

namespace Castle.Framework.Filters
{
    public class MvcFilterProvider : IFilterProvider
    {
        private readonly IWindsorContainer _container;

        public MvcFilterProvider(IWindsorContainer container)
        {
            _container = container;
        }

        public IEnumerable<Filter> GetFilters(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
        {
            var filters = _container.ResolveAll<IMvcAggregatableFilter>();
            return filters.Reverse().Select((filter, index) => new Filter(filter, FilterScope.Action, -(index + 1)));
        }
    }
}
