using Interview.Application.Core.Entitities;
using Interview.Application.Interfaces;
using Interview.Application.Invoices.CreateInvoice;
using Interview.Infrastructure.Persistence;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using AutoMapper;
using Interview.Application;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Interview.UnitTests.Application
{
    public class BaseHandlerTests
    {
        protected readonly DateTime _testDateTime = new DateTime(2020, 1, 1, 1, 1, 1, DateTimeKind.Utc);
        protected readonly int _testUserId = 100;
        protected readonly MockRepository _mockRepository;
        protected readonly Mock<IAutheticatedUserService> _authenticatedUserService;
        protected readonly Mock<IDateTimeService> _dateTimeService;
       
        protected readonly ApplicationDbContext _applicationDbContext;
        protected readonly IApplicationUnitOfWork _appUnitOfWork;
        protected readonly IApplicationRepository<Invoice> _invoiceRepository;
        protected readonly IApplicationRepository<Note> _noteRepository;
        protected readonly IApplicationRepository<UserInfo> _userInfoRepository;

        protected readonly IMapper _mapper;

        public BaseHandlerTests()
        {
            var mapperConfiguration = new MapperConfiguration(opts => opts.AddProfile(typeof(MappingsProfile)));

            _mapper = mapperConfiguration.CreateMapper();

            _mockRepository = new MockRepository(MockBehavior.Strict);
            _authenticatedUserService = _mockRepository.Create<IAutheticatedUserService>();
            _dateTimeService = _mockRepository.Create<IDateTimeService>();
            

            _dateTimeService.Setup(x => x.GetTime()).Returns(_testDateTime);
            _authenticatedUserService.Setup(x => x.GetUserId()).Returns(_testUserId);

            var dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                   .UseInMemoryDatabase(Guid.NewGuid().ToString())
                   .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                   .Options;

            _applicationDbContext = new ApplicationDbContext(dbContextOptions, _authenticatedUserService.Object, _dateTimeService.Object);

            _invoiceRepository = new ApplicationRepository<Invoice>(_applicationDbContext);
            _noteRepository = new ApplicationRepository<Note>(_applicationDbContext);
            _userInfoRepository = new ApplicationRepository<UserInfo>(_applicationDbContext);

            _appUnitOfWork = new ApplicationUnitOfWork(_applicationDbContext, _userInfoRepository, _noteRepository, _invoiceRepository);
        }
    }
}
