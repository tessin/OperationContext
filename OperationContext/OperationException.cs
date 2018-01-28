using System;

namespace Tessin.Diagnostics
{
    public class OperationException : Exception
    {
        public OperationContext Context { get; }

        public OperationException(string message, OperationContext context)
            : base(message)
        {
            Context = context;
        }
    }
}
