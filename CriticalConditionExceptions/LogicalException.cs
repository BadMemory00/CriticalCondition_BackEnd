using System;
using Microsoft.AspNetCore.Mvc;

namespace CriticalConditionBackend.CriticalConditionExceptions
{
    public class LogicalException : Exception
    {
        private readonly CriticalConditionExceptionsEnum exception;
        public readonly int statusCode;

        public LogicalException(CriticalConditionExceptionsEnum exception, int statusCode)
        {
            this.exception = exception;
            this.statusCode = statusCode;
        }

        public override string Message => exception.ToString();
    }
}
