namespace Interview.Application
{
    public class ApplicationConstants
    {
        public class Authentication 
        {
            public const string AuthenticationScheme = "ApiKey";
            public const string AuthenticationHeaderName = "Authorization";
            public const string AuthenticationResponseContentType = "application/json";
        }

        public class AuthorizationRoles
        {
            public const string Admin = "Admin";
            public const string User = "User";
            public const string AllRoles = "User,Admin";
        }

        public class ErrorCodes
        {
            public const string InputValidationError = "INPUT VALIDATION";
            public const string BusinessValidationError = "BUSINESS_VALIDATION";
            public const string AuthorizationError = "AUTHORIZATION";
            public const string AutheticationError = "AUTHENTICATION";
            public const string CreateInvoiceError = "CREATE_INVOICE";
            public const string UpdateInvoiceError = "UPDATE_INVOICE";
            public const string CreateNoteError = "CREATE_NOTE";
            public const string UpdateNoteError = "UPDATE_NOTE";
            public const string OperationCancelledError = "OPERATION_CANCELLED";
            public const string UnknownError = "UNKNOWN";
        }

        public class ErrorMessages
        {
            public const string InputValidationErrors = "Input validation errors";

            public const string SameIdentifierInvoice = "Invoice with identifier {0} already exists.";
            public const string InvoiceCreatedByDifferentUser = "Invoice with invoiceId {0} has been created by another user";
            public const string DuplicateInvoiceIdentifier = "Another invoice has {0} as identifier.";
            public const string InvoiceWithIdDoesNotExist = "Invoice with invoiceId {0} does not exist";
            public const string InvoiceNotFound = "Invoice with id {0} was not found";
            public const string UpdateInvoiceAtLeastOneFieldRequired = "At least one of the fields Identifier or Amount must pe present on request";

            public const string NoteWithIdDoesNotExist = "Note with noteId {0} does not exist";
            public const string NoteCreatedByDifferentUser = "Note with noteId {0} has been created by another user";

            public const string IdentifierFieldIsMandatory = "Identifier field is mandatory.";
            public const string IdentifierFieldHasMaxLength = "Identifier field has max length of 64 characters";
            public const string AmountFieldIsMandatory = "Amount field is mandatory";
            public const string AmountFieldValueNotZero = "Amount should not be 0";
            public const string InvoiceIdFieldIsMandatory = "InvoiceId field is mandatory";
            public const string InvoiceIdFieldValueNotZero = "InvoiceId field should be greater than 0";
            public const string TextFieldHasMaxLength = "Identifier field has max length of 2048 characters";
            public const string TextFieldIsMandatory = "Text field is mandatory";
            public const string NoteIdFieldIsMandatory = "NoteId field is mandatory";
            public const string NoteIdFieldValueNotZero = "NoteId field should be greater than 0";

            public const string AutheticationHeaderNotFound = "Header 'Authorization' not found on request";
            public const string AutheticationHeaderInvalidValue = "Invalid value for header 'Authorization'";
            public const string AutheticationApiKeyNotFound = "Api key {0} does not exist";

            public const string AuthorizationErrorUserNotAutheticated = "User is not acountenticated";
            public const string AuthorizationErrorUserNotInRole = "User does not have one of the required roles: {0}";

            public const string OperationCancelledError = "Operation cancelled by caller";
            public const string UnknownError = "An unexpected exception occured";
        }
    }
}
