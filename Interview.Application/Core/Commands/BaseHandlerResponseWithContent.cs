using Interview.Application.Core.Exceptions;
using System.Net;

namespace Interview.Application.Core.Commands
{
    public class BaseHandlerResponseWithContent<T>: BaseHandlerResponse where T: class
    {
        public T Content { get; set; }

        public BaseHandlerResponseWithContent(HttpStatusCode status) : base(status) { }
        public BaseHandlerResponseWithContent(HttpStatusCode status, ApplicationError error) : base(status, error) { }
        public BaseHandlerResponseWithContent(HttpStatusCode status, T content) : base(status)
        {
            Content = content;
        }
    }
}
