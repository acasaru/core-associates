using FluentAssertions;
using Interview.Application;
using Interview.Application.Core.Entitities;
using Interview.Application.Invoices.GetInvoice;
using MediatR;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Interview.UnitTests.Application
{
    public class GetInvoiceHandlerTests: BaseHandlerTests
    {
        private IRequestHandler<GetInvoiceQuery, GetInvoiceQueryResponse> _systemUnderTest;
        public GetInvoiceHandlerTests()
        {
            _systemUnderTest = new GetInvoiceQueryHandler(_appUnitOfWork, _mapper);
        }

        [Fact]
        public async Task Given_ThereIsAnInvoiceInTheDatabase_When_HandlerIsCalled_Then_TheInvoiceIsReturnedAndTheStatusCodeIs200()
        {
            //arrange
            var setupInvoice = new Invoice { Amount = 100, Identifier = "TEST-100" };

            await _applicationDbContext.Invoices.AddAsync(setupInvoice);

            await _applicationDbContext.SaveChangesAsync();

            var getInvoiceQuery = new GetInvoiceQuery(setupInvoice.InvoiceId);

            //act
            var response = await _systemUnderTest.Handle(getInvoiceQuery, default);

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Error.Should().BeNull();
            response.HasError.Should().BeFalse();

            response.Content.Should().NotBeNull();
            response.Content.Identifier.Should().Be(setupInvoice.Identifier);
            response.Content.Amount.Should().Be(setupInvoice.Amount);
            response.Content.CreatedBy.Should().Be(_testUserId);
            response.Content.InvoiceId.Should().Be(setupInvoice.InvoiceId);
        }

        [Fact]
        public async Task Given_TheInvoiceIsNotInTheDatabase_When_HandlerIsCalled_Then_NotFoundResponseIsReturned()
        {
            //arrange
            var getInvoiceQuery = new GetInvoiceQuery(100);

            //act
            var response = await _systemUnderTest.Handle(getInvoiceQuery, default);

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            response.Error.Should().NotBeNull();
            response.HasError.Should().BeTrue();
            response.Content.Should().BeNull();

            response.Error.ErrorCode.Should().Be(ApplicationConstants.ErrorCodes.BusinessValidationError);
            response.Error.ErrorMessage.Should().Be(string.Format(ApplicationConstants.ErrorMessages.InvoiceWithIdDoesNotExist, 100));
        }

        [Fact]
        public void Given_TheCallIsCancelledByUser_When_HandlerIsCalled_Then_TheHandlerShouldThrowOperationCancelledException()
        {
            using (var cancellationTokenSource = new CancellationTokenSource())
            {
                var cancellationToken = cancellationTokenSource.Token;
                cancellationTokenSource.Cancel();

                var getInvoiceQuery = new GetInvoiceQuery(100);

                Func<Task<GetInvoiceQueryResponse>> func = async () => await _systemUnderTest.Handle(getInvoiceQuery, cancellationToken);

                func.Should().ThrowAsync<OperationCanceledException>();
            }
        }
    }
}
