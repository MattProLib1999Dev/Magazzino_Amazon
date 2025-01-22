using Amazon.DAL.Handlers.Models.Response.Response;
using Amazon.DAL.Models.Response;

namespace Amazon.Common
{
    public enum OperationObjectResultStatus
    {
        Ok = StatusCodes.Status200OK,
        BadRequest = StatusCodes.Status400BadRequest,
        NotFound = StatusCodes.Status404NotFound,
        Conflict = StatusCodes.Status409Conflict,
        Error = StatusCodes.Status500InternalServerError,
        TemporaryRedirect = StatusCodes.Status307TemporaryRedirect
    }

    public class OperationObjectResult<T>
    {
        public OperationObjectResultStatus Status { get; set; }
        public T Value { get; set; }
        public string Message { get; set; } = string.Empty;

        public OperationObjectResult() { }

        // Metodo generico per risposte corrette
        public static OperationObjectResult<T> CreateCorrectResponseGeneric(T value)
        {
            return new OperationObjectResult<T>
            {
                Status = OperationObjectResultStatus.Ok,
                Value = value,
                Message = string.Empty
            };
        }

        // Metodo specifico per un singolo oggetto UserDALResponse
        public static OperationObjectResult<T> CreateCorrectResponseSingleObj(T value,string message)
        {
            return CreateInternal(OperationObjectResultStatus.Ok, value, message);
        }

        // Metodo per errori generici
        public static OperationObjectResult<T> CreateErrorResponse(OperationObjectResultStatus status,string message = "An error occurred.")
        {
            var fakeValue = default(T);
            return CreateInternal(status, fakeValue, message);
        }

        // Metodo per risposte "not found"
        public static OperationObjectResult<T> CreateNotFoundResponse(string entityName = "Entity")
        {
            return CreateErrorResponse(OperationObjectResultStatus.NotFound, $"{entityName} not found.");
        }

        // Metodo interno per creare una risposta
        private static OperationObjectResult<T> CreateInternal(
            OperationObjectResultStatus status,
            T? value,
            string message)
        {
            if (status == OperationObjectResultStatus.Ok && value == null)
            {
                throw new ArgumentException(
                    "Success response must include a value.",
                    nameof(value)
                );
            }

            return new OperationObjectResult<T>
            {
                Value = value,
                Message = message,
                Status = status
            };
        }

    }
}
