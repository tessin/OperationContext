using System;

namespace Tessin.Diagnostics
{
    public class OperationException : Exception
    {
        public object[] MessageArguments { get; }
        public OperationContext Context { get; }

        public OperationException(FormattableString message, Exception innerException, OperationContext context)
            : base(message.Format, innerException)
        {
            MessageArguments = message.GetArguments();
            Context = context;
        }
    }
}
