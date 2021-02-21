using Interview.Application.Core.Entitities;
using Interview.Application.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Interview.Infrastructure.Persistence
{
    public class ApplicationUnitOfWork : IApplicationUnitOfWork
    {
        private readonly ApplicationDbContext _aplicationDbContext;

        public ApplicationUnitOfWork(
            ApplicationDbContext aplicationDbContext,
            IApplicationRepository<UserInfo> userInfoRepository,
            IApplicationRepository<Note> noteRepository,
            IApplicationRepository<Invoice> invoiceRepository
            )
        {
            _aplicationDbContext = aplicationDbContext;

            UserInfos = userInfoRepository;
            Notes = noteRepository;
            Invoices = invoiceRepository;

        }
        public IApplicationRepository<UserInfo> UserInfos { get; }
        public IApplicationRepository<Note> Notes { get; }
        public IApplicationRepository<Invoice> Invoices { get; }

        public async Task CommitAsync(CancellationToken cancellationToken = default)
        {
            await _aplicationDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
