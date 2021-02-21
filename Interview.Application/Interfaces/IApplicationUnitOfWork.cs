using Interview.Application.Core.Entitities;
using System.Threading;
using System.Threading.Tasks;

namespace Interview.Application.Interfaces
{
    public interface IApplicationUnitOfWork
    {
        IApplicationRepository<UserInfo> UserInfos { get; }
        IApplicationRepository<Note> Notes { get; }
        IApplicationRepository<Invoice> Invoices { get; }
        Task CommitAsync(CancellationToken cancellationToken);
    }
}
