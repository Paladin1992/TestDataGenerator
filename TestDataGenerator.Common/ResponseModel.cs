using System;

namespace TestDataGenerator.Common
{
    public class ResponseModel
    {
        public bool Succeeded { get; set; }

        public Exception Exception { get; set; }


        public ResponseModel()
        {

        }

        public ResponseModel(bool succeeded) : this(succeeded, null)
        {

        }

        public ResponseModel(bool succeeded, Exception exception)
        {
            Succeeded = succeeded;
            Exception = exception;
        }

        public static implicit operator bool(ResponseModel obj)
        {
            return obj.Succeeded;
        }
    }
}