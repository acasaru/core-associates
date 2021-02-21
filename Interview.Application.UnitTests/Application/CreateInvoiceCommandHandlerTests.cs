using Interview.Application.Core.Entitities;
using Interview.Application.Interfaces;
using Interview.Application.Invoices.CreateInvoice;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using Interview.Application;
using Xunit;
using System.Threading.Tasks;
using FluentAssertions;
using System.Net;
using System.Threading;
using System.Linq.Expressions;
using Microsoft.Extensions.Logging;

namespace Interview.UnitTests.Application
{
    public class CreateInvoiceCommandHandlerTests : BaseHandlerTests
    {
        private IRequestHandler<CreateInvoiceCommand, CreateInvoiceCommandResponse> _systemUnderTest;
        private Mock<ILogger<CreateInvoiceCommandHandler>> _logger;
        public CreateInvoiceCommandHandlerTests()
        {
            _logger = _mockRepository.Create<ILogger<CreateInvoiceCommandHandler>>(MockBehavior.Loose);

            _systemUnderTest = new CreateInvoiceCommandHandler(_appUnitOfWork, _logger.Object, _mapper);
        }

        [Fact]
        public async Task Given_ThereIsAValidInvoice_When_HandlerIsCalled_Then_TheInvoiceIsSavedAndReturned()
        {
            //arrange
            var setupInvoiceDto = new CreateInvoiceDto() { Amount = 100, Identifier = "TEST-100" };

            var setupCreateInvoiceCommand = new CreateInvoiceCommand(setupInvoiceDto);

            //act
            var response = await _systemUnderTest.Handle(setupCreateInvoiceCommand, default);

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            response.Error.Should().BeNull();
            response.HasError.Should().BeFalse();

            response.Content.Should().NotBeNull();
            response.Content.InvoiceId.Should().Be(1);
            response.Content.Amount.Should().Be(setupInvoiceDto.Amount);
            response.Content.Identifier.Should().Be(setupInvoiceDto.Identifier);
            response.Content.CreatedBy.Should().Be(_testUserId);
        }

        [Fact]
        public async Task Given_ThereIsAnInvoiceWithSameIdentifierInDatabase_When_HandlerIsCalled_Then_ItShouldReturnBadRequestResponse()
        {
            //arrange
            var setupExistingInvoice = new Invoice() { Identifier = "TEST-100", Amount = 200 };

            _applicationDbContext.Invoices.Add(setupExistingInvoice);

            await _applicationDbContext.SaveChangesAsync();

            var setupInvoiceDto = new CreateInvoiceDto() { Amount = 100, Identifier = "TEST-100" };
            var setupCreateInvoiceCommand = new CreateInvoiceCommand(setupInvoiceDto);

            //act
            var response = await _systemUnderTest.Handle(setupCreateInvoiceCommand, default);

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            response.Error.Should().NotBeNull();
            response.Error.ErrorCode.Should().Be(ApplicationConstants.ErrorCodes.BusinessValidationError);
            response.Error.ErrorMessage.Should().Be(string.Format(ApplicationConstants.ErrorMessages.SameIdentifierInvoice, setupExistingInvoice.Identifier));

            var countInvoices = await _applicationDbContext.Invoices.CountAsync();

            countInvoices.Should().Be(1);
        }

        [Fact]
        public async Task Given_TheSaveInvoiceFailsInDatabase_When_HandlerIsCalled_Then_ItShouldReturnInternalServerError()
        {
            using (var cancellationTokenSource = new CancellationTokenSource())
            {
                //arrange
                var cancellationToken = cancellationTokenSource.Token;

                var setupException = new Exception("test save exception");

                var setupInvoiceDto = new CreateInvoiceDto() { Amount = 100, Identifier = "TEST-100" };

                var setupCreateInvoiceCommand = new CreateInvoiceCommand(setupInvoiceDto);

                var mockApplicationUnitOfWork = _mockRepository.Create<IApplicationUnitOfWork>();
                var mockInvoiceRepository = _mockRepository.Create<IApplicationRepository<Invoice>>();

                mockInvoiceRepository.Setup(x => x.SingleOrDefaultAsync(It.IsAny<Expression<Func<Invoice, bool>>>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(null as Invoice);
                mockInvoiceRepository.Setup(x => x.AddAsync(It.IsAny<Invoice>()))
                    .ReturnsAsync(null as Invoice);

                mockApplicationUnitOfWork.Setup(x => x.CommitAsync(cancellationToken)).ThrowsAsync(setupException);
                mockApplicationUnitOfWork.Setup(x => x.Invoices).Returns(mockInvoiceRepository.Object);

                _systemUnderTest = new CreateInvoiceCommandHandler(mockApplicationUnitOfWork.Object, _logger.Object, _mapper);

                //act
                var response = await _systemUnderTest.Handle(setupCreateInvoiceCommand, cancellationToken);

                //assert
                response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
                response.Error.Should().NotBeNull();
                response.Error.ErrorCode.Should().Be(ApplicationConstants.ErrorCodes.CreateInvoiceError);
                response.Error.ErrorMessage.Should().Be(setupException.Message);
            }
        }
        [Fact]
        public void Given_TheCallIsCancelledByUser_When_HandlerIsCalled_Then_TheHandlerShouldThrowOperationCancelledException()
        {
            using (var cancellationTokenSource = new CancellationTokenSource())
            {
                var cancellationToken = cancellationTokenSource.Token;
                cancellationTokenSource.Cancel();

                var setupInvoiceDto = new CreateInvoiceDto() { Amount = 100, Identifier = "TEST-100" };

                var setupCreateInvoiceCommand = new CreateInvoiceCommand(setupInvoiceDto);

                Func<Task<CreateInvoiceCommandResponse>> func = async () => await _systemUnderTest.Handle(setupCreateInvoiceCommand, cancellationToken);

                func.Should().ThrowAsync<OperationCanceledException>();
            }
        }
    }
}
