using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Exceptions
{
    public sealed class ConcurrencyException : Exception
    {
        public ConcurrencyException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
