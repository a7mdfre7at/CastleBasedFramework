using Castle.Framework.DependencyInjection;

namespace Castle.Services
{
    public interface IUserService : ITransientDependency
    {
        string GetUsername();
    }
}
