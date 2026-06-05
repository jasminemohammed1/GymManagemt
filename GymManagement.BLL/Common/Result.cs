using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Common
{
    public sealed record Result (bool Success , string ?ErrorMessage = null , ResultKind result = ResultKind.Ok )
    {
        public static Result Ok() => new (true);
      
        public static Result Fail(string errormessage, ResultKind result = ResultKind.Conflict) => new(false, errormessage, result);
        public static Result NotFound(string errormessage = "Not Found") => new (false, errormessage,  ResultKind.NotFound);
        public static Result Validation(string message  ) => new (false , message , ResultKind.Validation);




    }

    public sealed record Result<T> (bool Sucess ,T ? value  , string? ErrorMessage = null , ResultKind result = ResultKind.Ok )
    {
        public static Result<T> Ok(T value) => new (true , value);

        public static Result<T> Fail(string errormessage, ResultKind result = ResultKind.Conflict) => new(false,default, errormessage, result);
        public static Result<T> NotFound(string errormessage = "Not Found") => new (false, default,errormessage, ResultKind.NotFound);
        public static Result<T> Validation(string message) => new (false,default, message, ResultKind.Validation);

    }
}
