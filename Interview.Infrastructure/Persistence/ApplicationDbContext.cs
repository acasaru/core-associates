using Interview.Application.Core.Entitities;
using Interview.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Interview.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        private readonly IAutheticatedUserService _currentUserService;
        private readonly IDateTimeService _dateTimeService;

        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<UserInfo> UserInfos { get; set; }

        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options,
            IAutheticatedUserService currentUserService,
            IDateTimeService dateTime) : base(options)
        {
            _currentUserService = currentUserService;
            _dateTimeService = dateTime;
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            var userId = _currentUserService.GetUserId();
            var time = _dateTimeService.GetTime();

            AddAuditInformation(userId, time);

            return await base.SaveChangesAsync(cancellationToken);
        }

        public override int SaveChanges()
        {
            var userId = _currentUserService.GetUserId();
            var time = _dateTimeService.GetTime();

            AddAuditInformation(userId, time);

            return base.SaveChanges();
        }

        private void AddAuditInformation(int userId, DateTime time)
        {
            foreach (var entry in base.ChangeTracker.Entries<AuditableEntity>().Where(e => e.State == EntityState.Added))
            {
                AddAuditInformationToEntry(entry, userId, time);
            }
        }

        private static void AddAuditInformationToEntry(EntityEntry<AuditableEntity> entry, int userId, DateTime time)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedBy = userId;
                entry.Entity.CreatedAt = time;
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            foreach (var entityType in builder.Model.GetEntityTypes().Where(e => typeof(AuditableEntity).IsAssignableFrom(e.ClrType)))
            {
                builder.Entity(entityType.ClrType)
                    .Property<DateTime>(nameof(AuditableEntity.CreatedAt))
                    .IsRequired(true);

                builder.Entity(entityType.ClrType)
                    .Property<int>(nameof(AuditableEntity.CreatedBy))
                    .IsRequired(true);
            }

            builder.Seed();

            base.OnModelCreating(builder);
        }
    }
}
