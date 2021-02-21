using FluentAssertions;
using Interview.Application;
using Interview.Application.Core.Entitities;
using Interview.Application.Interfaces;
using Interview.Application.Invoices.UpdateInvoice;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Interview.UnitTests.Application
{
    public class UpdateInvoiceCommandHandlerTests: BaseHandlerTests
    {
        private IRequestHandler<UpdateInvoiceCommand, UpdateInvoiceCommandResponse> _systemUnderTest;
        private Mock<ILogger<UpdateInvoiceCommandHandler>> _logger;
        public UpdateInvoiceCommandHandlerTests()
        {
            _logger = _mockRepository.Create<ILogger<UpdateInvoiceCommandHandler>>(MockBehavior.Loose);

            _systemUnderTest = new UpdateInvoiceCommandHandler(
                _appUnitOfWork, _logger.Object, _authenticatedUserService.Object);
        }

        [Fact]
        public async Task Given_ThereIsAnInvoiceUpdate_When_HandlerIsCalled_Then_TheInvoiceIsUpdatedAndOkStatusReturned()
        {
            //arrange
            var setupInvoice = new Invoice { Amount = 100, Identifier = "TEST-100" };
            
            await _applicationDbContext.Invoices.AddAsync(setupInvoice);

            await _applicationDbContext.SaveChangesAsync();

            var setupUpdateInvoiceDto = new UpdateInvoiceDto() 
            { 
                Amount = 200, 
                Identifier = "TEST-200", 
                InvoiceId = setupInvoice.InvoiceId 
            };

            var setupUpdateInvoiceCommand = new UpdateInvoiceCommand(setupUpdateInvoiceDto);

            //act
            var response = await _systemUnderTest.Handle(setupUpdateInvoiceCommand, default);

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Error.Should().BeNull();
            response.HasError.Should().BeFalse();

            var databaseInvoice = await _applicationDbContext.Invoices
                .SingleOrDefaultAsync(invoice => invoice.InvoiceId == setupInvoice.InvoiceId);

            databaseInvoice.Should().NotBeNull();
            databaseInvoice.Amount.Should().Be(setupUpdateInvoiceDto.Amount);
            databaseInvoice.Identifier.Should().Be(setupUpdateInvoiceDto.Identifier);
        }

        [Fact]
        public async Task Given_ThereIsAnInvoiceUpdateAndInvoiceDoesNotExistInTheDatabase_When_HandlerIsCalled_Then_NotFoundResponseIsReturned()
        {
            //arrange
            var setupInvoice = new Invoice { Amount = 100, Identifier = "TEST-100" };

            await _applicationDbContext.Invoices.AddAsync(setupInvoice);

            await _applicationDbContext.SaveChangesAsync();

            var setupUpdateInvoiceDto = new UpdateInvoiceDto()
            {
                Amount = 200,
                Identifier = "TEST-200",
                InvoiceId = 100
            };

            var setupUpdateInvoiceCommand = new UpdateInvoiceCommand(setupUpdateInvoiceDto);

            //act
            var response = await _systemUnderTest.Handle(setupUpdateInvoiceCommand, default);

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            response.Error.Should().NotBeNull();
            response.HasError.Should().BeTrue();
            response.Error.ErrorCode.Should().Be(ApplicationConstants.ErrorCodes.BusinessValidationError);
            response.Error.ErrorMessage.Should().Be(string.Format(ApplicationConstants.ErrorMessages.InvoiceWithIdDoesNotExist, setupUpdateInvoiceDto.InvoiceId));

            var databaseInvoice = await _applicationDbContext.Invoices
                .SingleOrDefaultAsync(invoice => invoice.InvoiceId == setupInvoice.InvoiceId);

            databaseInvoice.Should().NotBeNull();
            databaseInvoice.Amount.Should().Be(setupInvoice.Amount);
            databaseInvoice.Identifier.Should().Be(setupInvoice.Identifier);
        }

        [Fact]
        public async Task Given_OnlyAmountIsUpdatedForInvoice_When_HandlerIsCalled_Then_TheInvoiceIsPartiallyUpdatedAndOkStatusReturned()
        {
            //arrange
            var setupInvoice = new Invoice { Amount = 100, Identifier = "TEST-100" };

            await _applicationDbContext.Invoices.AddAsync(setupInvoice);

            await _applicationDbContext.SaveChangesAsync();

            var setupUpdateInvoiceDto = new UpdateInvoiceDto()
            {
                Amount = 200,
                InvoiceId = setupInvoice.InvoiceId
            };

            var setupUpdateInvoiceCommand = new UpdateInvoiceCommand(setupUpdateInvoiceDto);

            //act
            var response = await _systemUnderTest.Handle(setupUpdateInvoiceCommand, default);

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Error.Should().BeNull();
            response.HasError.Should().BeFalse();

            var databaseInvoice = await _applicationDbContext.Invoices
                .SingleOrDefaultAsync(invoice => invoice.InvoiceId == setupInvoice.InvoiceId);

            databaseInvoice.Should().NotBeNull();
            databaseInvoice.Amount.Should().Be(setupUpdateInvoiceDto.Amount);
            databaseInvoice.Identifier.Should().Be(setupInvoice.Identifier);
        }

        [Fact]
        public async Task Given_OnlyIdentifierIsUpdatedForInvoice_When_HandlerIsCalled_Then_TheInvoiceIsPartiallyUpdatedAndOkStatusReturned()
        {
            //arrange
            var setupInvoice = new Invoice { Amount = 100, Identifier = "TEST-100" };

            await _applicationDbContext.Invoices.AddAsync(setupInvoice);

            await _applicationDbContext.SaveChangesAsync();

            var setupUpdateInvoiceDto = new UpdateInvoiceDto()
            {
                Identifier = "TEST-200",
                InvoiceId = setupInvoice.InvoiceId
            };

            var setupUpdateInvoiceCommand = new UpdateInvoiceCommand(setupUpdateInvoiceDto);

            //act
            var response = await _systemUnderTest.Handle(setupUpdateInvoiceCommand, default);

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Error.Should().BeNull();
            response.HasError.Should().BeFalse();

            var databaseInvoice = await _applicationDbContext.Invoices
                .SingleOrDefaultAsync(invoice => invoice.InvoiceId == setupInvoice.InvoiceId);

            databaseInvoice.Should().NotBeNull();
            databaseInvoice.Amount.Should().Be(setupInvoice.Amount);
            databaseInvoice.Identifier.Should().Be(setupUpdateInvoiceDto.Identifier);
        }

        [Fact]
        public async Task Given_ThereIsAnInvoiceUpdateAndAnotherInvoiceHasTheSameIdentifier_When_HandlerIsCalled_Then_BadRequestResponseIsReturned()
        {
            //arrange
            var setupInvoice1 = new Invoice { Amount = 100, Identifier = "TEST-100" };
            var setupInvoice2 = new Invoice { Amount = 200, Identifier = "TEST-200" };

            await _applicationDbContext.Invoices.AddAsync(setupInvoice1);
            await _applicationDbContext.Invoices.AddAsync(setupInvoice2);

            await _applicationDbContext.SaveChangesAsync();

            var setupUpdateInvoiceDto = new UpdateInvoiceDto()
            {
                Amount = 200,
                Identifier = "TEST-200",
                InvoiceId = setupInvoice1.InvoiceId
            };

            var setupUpdateInvoiceCommand = new UpdateInvoiceCommand(setupUpdateInvoiceDto);

            //act
            var response = await _systemUnderTest.Handle(setupUpdateInvoiceCommand, default);

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            response.Error.Should().NotBeNull();
            response.HasError.Should().BeTrue();
            response.Error.ErrorCode.Should().Be(ApplicationConstants.ErrorCodes.BusinessValidationError);
            response.Error.ErrorMessage.Should().Be(string.Format(ApplicationConstants.ErrorMessages.DuplicateInvoiceIdentifier, setupUpdateInvoiceDto.Identifier));

            var databaseInvoice1 = await _applicationDbContext.Invoices
                .SingleOrDefaultAsync(invoice => invoice.InvoiceId == setupInvoice1.InvoiceId);

            databaseInvoice1.Amount.Should().Be(setupInvoice1.Amount);
            databaseInvoice1.Identifier.Should().Be(setupInvoice1.Identifier);

            var databaseInvoice2 = await _applicationDbContext.Invoices
                .SingleOrDefaultAsync(invoice => invoice.InvoiceId == setupInvoice2.InvoiceId);

            databaseInvoice2.Amount.Should().Be(setupInvoice2.Amount);
            databaseInvoice2.Identifier.Should().Be(setupInvoice2.Identifier);
        }

        [Fact]
        public async Task Given_ThereIsAnInvoiceUpdateForAnInvoiceCreatedByOtherUser_When_HandlerIsCalled_Then_ForbidenResponseIsReturned()
        {
            //arrange
            var setupInvoice = new Invoice { Amount = 100, Identifier = "TEST-100" };

            await _applicationDbContext.Invoices.AddAsync(setupInvoice);

            await _applicationDbContext.SaveChangesAsync();

            var setupUpdateInvoiceDto = new UpdateInvoiceDto()
            {
                Amount = 200,
                Identifier = "TEST-200",
                InvoiceId = setupInvoice.InvoiceId
            };

            var setupUpdateInvoiceCommand = new UpdateInvoiceCommand(setupUpdateInvoiceDto);

            _authenticatedUserService.Setup(x => x.GetUserId()).Returns(200);

            //act
            var response = await _systemUnderTest.Handle(setupUpdateInvoiceCommand, default);

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
            response.Error.Should().NotBeNull();
            response.HasError.Should().BeTrue();

            response.Error.ErrorCode.Should().Be(ApplicationConstants.ErrorCodes.AuthorizationError);
            response.Error.ErrorMessage.Should().Be(string.Format(ApplicationConstants.ErrorMessages.InvoiceCreatedByDifferentUser, setupUpdateInvoiceDto.InvoiceId));

            var databaseInvoice = await _applicationDbContext.Invoices
                .SingleOrDefaultAsync(invoice => invoice.InvoiceId == setupInvoice.InvoiceId);

            databaseInvoice.Should().NotBeNull();
            databaseInvoice.Amount.Should().Be(setupInvoice.Amount);
            databaseInvoice.Identifier.Should().Be(setupInvoice.Identifier);
        }

        [Fact]
        public async Task Given_TheUpdateInvoiceFailsInDatabase_When_HandlerIsCalled_Then_ItShouldReturnInternalServerError()
        {
            using (var cancellationTokenSource = new CancellationTokenSource())
            {
                //arrange
                var cancellationToken = cancellationTokenSource.Token;

                var setupException = new Exception("test update exception");

                var setupInvoice = new Invoice { Amount = 100, Identifier = "TEST-100" };

                await _applicationDbContext.Invoices.AddAsync(setupInvoice);

                await _applicationDbContext.SaveChangesAsync();

                var setupUpdateInvoiceDto = new UpdateInvoiceDto()
                {
                    Amount = 200,
                    Identifier = "TEST-200",
                    InvoiceId = setupInvoice.InvoiceId
                };

                var setupUpdateInvoiceCommand = new UpdateInvoiceCommand(setupUpdateInvoiceDto);

                var mockRepositoryOutput = new Queue<Invoice>(new[] { setupInvoice, null });

                var mockApplicationUnitOfWork = _mockRepository.Create<IApplicationUnitOfWork>();
                var mockInvoiceRepository = _mockRepository.Create<IApplicationRepository<Invoice>>();

                mockInvoiceRepository.Setup(x => x.SingleOrDefaultAsync(It.IsAny<Expression<Func<Invoice, bool>>>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(()=> mockRepositoryOutput.Dequeue());
                mockInvoiceRepository.Setup(x => x.Update(It.IsAny<Invoice>()));

                mockApplicationUnitOfWork.Setup(x => x.CommitAsync(cancellationToken)).ThrowsAsync(setupException);
                mockApplicationUnitOfWork.Setup(x => x.Invoices).Returns(mockInvoiceRepository.Object);

                _systemUnderTest = new UpdateInvoiceCommandHandler(mockApplicationUnitOfWork.Object, _logger.Object, _authenticatedUserService.Object);

                //act
                var response = await _systemUnderTest.Handle(setupUpdateInvoiceCommand, cancellationToken);

                //assert
                response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
                response.Error.Should().NotBeNull();
                response.Error.ErrorCode.Should().Be(ApplicationConstants.ErrorCodes.UpdateInvoiceError);
                response.Error.ErrorMessage.Should().Be(setupException.Message);

                var databaseInvoice = await _applicationDbContext.Invoices
                    .SingleOrDefaultAsync(invoice => invoice.InvoiceId == setupInvoice.InvoiceId);

                databaseInvoice.Should().NotBeNull();
                databaseInvoice.Amount.Should().Be(setupInvoice.Amount);
                databaseInvoice.Identifier.Should().Be(setupInvoice.Identifier);
            }
        }

        [Fact]
        public async Task Given_TheCallIsCancelledByUser_When_HandlerIsCalled_Then_TheHandlerShouldThrowOperationCancelledException()
        {
            using (var cancellationTokenSource = new CancellationTokenSource())
            {
                var cancellationToken = cancellationTokenSource.Token;
                cancellationTokenSource.Cancel();

                var setupInvoice = new Invoice { Amount = 100, Identifier = "TEST-100" };

                await _applicationDbContext.Invoices.AddAsync(setupInvoice);

                await _applicationDbContext.SaveChangesAsync();

                var setupUpdateInvoiceDto = new UpdateInvoiceDto()
                {
                    Amount = 200,
                    Identifier = "TEST-200",
                    InvoiceId = setupInvoice.InvoiceId
                };

                var setupUpdateInvoiceCommand = new UpdateInvoiceCommand(setupUpdateInvoiceDto);

                Func<Task<UpdateInvoiceCommandResponse>> func = async () => await _systemUnderTest.Handle(setupUpdateInvoiceCommand, cancellationToken);

                await func.Should().ThrowAsync<OperationCanceledException>();
            }
        }
    }
}
