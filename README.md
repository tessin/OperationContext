# OperationContext

Pass the `OperationContext` through you program and use `CreateScope` to create a virutal call stack.

The virutal call stack can be used to create a stacking context ID for any error that occur in a particular place in your code.

This ID is based on the calling member and partial file name. Thus, it should be more stable across development time.

## Async

The `OperationContext` is immutable so that it can be used more readily with asynchronous tasks.

## CancellationToken

The `OperationContext` can be associated with a `CancellationToken`, no need to pass an extra argument for either or.

~~~
var cts = new CancellationTokenSource();

var context = new OperationContext(cancellationToken: cts.Token);
// or
context.WithCancellationToken(cts.Token);

// then

async Task MethodAsync(..., OperationContext context) {
  await Task.Delay(context.CancellationToken)
}
~~~

## State

The `OperationContext` has an immutable state dictionary that can be used to tag the operation context with values. These values can then be inspected when troubleshooting.

~~~
for (int i = 0; i < ...; i++) {
  DoWork(i, context.CreateScope().WithState(nameof(i), i));
}
~~~

> This will create a new scope (scope has file and line number information) associate with loop variable `i`. Any error reported through the context will have access to this information.

## Error

The `Error` method can be used to create a custom error type with context information from the source of the error.

~~~
public enum Error
{
  None = 0, // not an error, default value is always treated as success
  ArgumentOutOfRangeError
}

public OperationError<Error> Method() {
  return context.Error(Error.ArgumentOutOfRangeError)
}
~~~

As long as the return type of the method has an implicit conversion from `OperationError<Error>` to return type the error can be returned from the function as is.

You may want to have a command result type, like this:

~~~
public class CommandResult
{
  public OperationError<Error> Error { get; set; }

  public bool Success => Error.Success;
  public bool HasError => Error.HasError;

  public static implicit operator CommandResult(OperationError<Error> error) {
    return new CommandResult { Error = error };
  }
}

public class CommandResult<TResult> : CommandResult 
{
  public TResult Result { get; set; }

  public static implicit operator CommandResult<TResult>(OperationError<Error> error) {
    return new CommandResult<TResult> { Error = error };
  }

  public static implicit operator CommandResult<TResult>(TResult result) {
    return new CommandResult<TResult> { Result = result };
  }
}
~~~

And create methods that return like this:

~~~
public CommandResult<string> Success(OperationContext context) {
  return "Hello World!";
}

public CommandResult<string> HasError(OperationContext context) {
  return context.Error(Error.ArgumentOutOfRangeError);
}
~~~

----

Go forth and write better code!

## Depedencies

- `<package id="Newtonsoft.Json" version="8.0.3" targetFramework="net461" />`

This is the most recent version of `Newtonsoft.Json` before the .NET standard was introduced. If you need to use a more recent version of this package, it should work just fine by redirecting assembly versions (NuGet typically does this for you).

## References

https://blog.golang.org/context
