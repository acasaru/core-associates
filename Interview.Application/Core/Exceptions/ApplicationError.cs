using System.Collections.Generic;

namespace Interview.Application.Core.Exceptions
{
    public class ApplicationError
    {
        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public List<string> ErrorAdditonalInfo { get; set; }

        public ApplicationError(string code, string message)
        {
            ErrorCode = code;
            ErrorMessage = message;
        }

        public ApplicationError(string code, string message, List<string> errorAdditionalInfo)
        {
            ErrorCode = code;
            ErrorMessage = message;
            ErrorAdditonalInfo = errorAdditionalInfo;
        }
    }
}
