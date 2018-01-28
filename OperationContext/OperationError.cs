using System.Collections.Generic;

namespace Tessin.Diagnostics
{
    public struct OperationError<TError>
    {
        public TError Error { get; }
        public OperationContext Context { get; }

        public bool Success => EqualityComparer<TError>.Default.Equals(Error, default(TError));
        public bool HasError => !EqualityComparer<TError>.Default.Equals(Error, default(TError));

        public OperationError(TError error, OperationContext context)
        {
            Error = error;
            Context = context;
        }
    }
}
