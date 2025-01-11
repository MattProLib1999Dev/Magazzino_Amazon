
using System.Diagnostics;
using Amazon.DAL.Models.Response;

namespace Amazon.Common
{
    public enum OperationObjectResultStatus
    {
        Ok = StatusCodes.Status200OK,
        BadRequest = StatusCodes.Status400BadRequest,
        NotFound = StatusCodes.Status404NotFound,
        Conflict = StatusCodes.Status409Conflict,
        Error = StatusCodes.Status500InternalServerError

    }
    public class OperationObjectResult<T>
    {
        public OperationObjectResultStatus Status { get; set; }

        public T Value { get; set; }

        public string Message { get; set; }

        protected OperationObjectResult() { }

        public static OperationObjectResult<List<UserDALResponse>> CreateCorrectResponse(List<UserDALResponse> value, string message = "")
        {
            return OperationObjectResult<List<UserDALResponse>>.CreateInternal(OperationObjectResultStatus.Ok, value, message);
        }

        public static OperationObjectResult<UserDALResponse> CreateCorrectResponseSingleObj(UserDALResponse value, string message)

        {
            return OperationObjectResult<UserDALResponse>.CreateInternal(OperationObjectResultStatus.Ok, value, message);
        }




        public static OperationObjectResult<T> CreateErrorResponse(OperationObjectResultStatus Status, string Message = null)
        {
            var fakeValue = default(T);
            return OperationObjectResult<T>.CreateInternal(Status, fakeValue, Message);
        }

        private static OperationObjectResult<T> CreateInternal(OperationObjectResultStatus Status, T Value, string Message)
        {
            return new OperationObjectResult<T>
            {
                Value = Value,
                Message = Message,
                Status = Status
            };
        }
    }
}