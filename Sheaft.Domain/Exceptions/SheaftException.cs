using System;
using System.Collections.Generic;
using System.Linq;
using Sheaft.Domain.Common;

namespace Sheaft.Domain.Exceptions
{
    public class SheaftException : Exception
    {
        public List<string> Errors { get; }

        public SheaftException(Result result)
            : base(result.Errors.FirstOrDefault(), result.Exception)
        {
            Errors = result.Errors;
        }

        public SheaftException(Exception exception, string error = null)
            : this(new Result(false, new List<string> {error ?? exception.Message}, exception))
        {
        }
    }
}