using System;
using System.Collections.Generic;

namespace Interview.Application.Core.Exceptions
{
    public class ApplicationErrorException: Exception
    {
        public ApplicationError Error { get; set; }
        public ApplicationErrorException(ApplicationError error) : base(error?.ErrorMessage)
        {
            Error = error;
        }
        public ApplicationErrorException(string errorCode, string errorMessage): base (errorMessage)
        {
            Error = new ApplicationError(errorCode, errorMessage, new List<string>());
        }
        public ApplicationErrorException(string errorCode, string errorMessage, List<string> errorAdditionalInfo) : base(errorMessage)
        {
            Error = new ApplicationError(errorCode, errorMessage, errorAdditionalInfo);
        }
    }
}
