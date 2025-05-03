using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using XelerationTask.Core.Exceptions;

namespace XelerationTask.API.Filters
{
    public class FileSystemExceptionFilter: IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            
            Exception exception = context.Exception;
            int statusCode = 500;

            switch(true)
            {
                case bool _ when exception is ResourceNotFoundException:
                    statusCode = 404;
                    break;
                case bool _ when exception is InvalidOperationError:
                    statusCode = 400;
                    break;
                case bool _ when exception is ResourceAlreadyExistsException:
                    statusCode = 409;
                    break;
                case bool _ when exception is NotAuthorized:
                    statusCode = 403;
                    break;
                default:
                    statusCode = 500;
                    break;
            }

            context.Result = new ObjectResult(exception.Message)
            {
                StatusCode = statusCode
            };
        }
    }

}
