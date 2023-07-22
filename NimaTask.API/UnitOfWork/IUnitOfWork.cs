using NimaTask.API.UnitOfWork.Repositories.Role;
using NimaTask.API.UnitOfWork.Repositories.User;
using NimaTask.API.UnitOfWork.Repositories.UserRole;

namespace NimaTask.API.UnitOfWork
{
    public interface IUnitOfWork
    {
        public IRoleRepository RoleRepository { get; }
        public IUserRoleRepository UserRoleRepository { get; }
        public IUserRepository UserRepository { get; }

        public Task SaveChangesAsync();
        public Task EnsureCreated();
        public void Dispose();
    }
}
