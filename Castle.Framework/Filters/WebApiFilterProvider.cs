using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Filters;
using Castle.Windsor;

namespace Castle.Framework.Filters
{
    public class WebApiFilterProvider
    {
        private readonly IWindsorContainer _container;

        public WebApiFilterProvider(IWindsorContainer container)
        {
            _container = container;
        }

        public IEnumerable<IFilter> GetFilters()
        {
            return _container.Resolve<IEnumerable<IWebApiAggregatableFilter>>().Select(x => x as IFilter).Where(x => x != null);
        }
    }
}
