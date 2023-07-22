using NimaTask.API.UnitOfWork.Repositories.Role;
using NimaTask.API.UnitOfWork.Repositories.User;
using NimaTask.API.UnitOfWork.Repositories.UserRole;
using NimaTask.API.UnitOfWork.Repositories.UserToken;
using NimaTask.API.UnitOfWork.Repositories.UserTokenLog;

namespace NimaTask.API.UnitOfWork
{
    public interface IUnitOfWork
    {
        public IRoleRepository RoleRepository { get; }
        public IUserRoleRepository UserRoleRepository { get; }
        public IUserRepository UserRepository { get; }
        public IUserTokenRepository UserTokenRepository { get; }
        public IUserTokenLogRepository UserTokenLogRepository { get; }

        public Task SaveChangesAsync();
        public Task EnsureCreated();
        public void Dispose();
    }
}
