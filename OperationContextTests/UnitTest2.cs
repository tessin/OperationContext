using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tessin.Diagnostics
{
    public enum Error
    {
        None = 0,
        ArgumentOutOfRangeError
    }

    public class CommandResult
    {
        public OperationError<Error> Error { get; set; }

        public bool Success => Error.Success;
        public bool HasError => Error.HasError;

        public static implicit operator CommandResult(OperationError<Error> error)
        {
            return new CommandResult { Error = error };
        }
    }

    public class CommandResult<TResult> : CommandResult
    {
        public TResult Result { get; set; }

        public static implicit operator CommandResult<TResult>(OperationError<Error> error)
        {
            return new CommandResult<TResult> { Error = error };
        }

        public static implicit operator CommandResult<TResult>(TResult result)
        {
            return new CommandResult<TResult> { Result = result };
        }
    }

    [TestClass]
    public class UnitTest2
    {
        [TestMethod]
        public void TestMethod1()
        {
            Assert.IsTrue(Success(new OperationContext()).Success);
            Assert.IsTrue(HasError(new OperationContext()).HasError);
        }

        public CommandResult<string> Success(OperationContext context)
        {
            return "Hello World!";
        }

        public CommandResult<string> HasError(OperationContext context)
        {
            return context.Error(Error.ArgumentOutOfRangeError);
        }
    }
}
