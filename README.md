# OperationContext

Pass the `OperationContext` through you program and use `CreateScope` to create a virtual call stack.

The virtual call stack can be used to create a stacking context ID for any error that occur in a particular place in your code.

This ID is based on the calling member and partial file name. Thus, it should be more stable across time.

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

> **Note:** there is no direct way to cancel an `OperationContext`. However, you can create a local `CancellationTokenSource` and use that.
> ~~~
> var cts = new CancellationTokenSource();
> DoAsync(..., context.WithCancellationToken(cts.Token));
> cts.Cancel();
> ~~~

## Timeout

The `Elapsed` property will give the you the time that has elapsed since the `OperationContext` was created. By default, every `OperationContext` is created with a 5 minute `Timeout`. `Remaining` is simply `Timeout - Elapsed` and can be negative. Occasionally, it is useful to know how much time is left to complete a certain task. 

> **Note:** `WithTimeout` cannot be used to increase the timeout value. If the `OperationContext` was created with a timeout of 5 minutes you can only use `WithTimeout` to lower this value.

Running out of time doesn't automatically cancel the `OperationContext`. If you wish to be signaled, you can use a local `CancellationTokenSource`.

~~~
var cts = new CancellationTokenSource();
cts.CancelAfter(...);
await DoAsync(..., context.WithCancellationToken(cts.Token));
~~~

## Values

The `OperationContext` can be used to associate a key with a value. This information can be serialized. The Go documentation recommends that this feature is only used for request-scoped data that transits processes and APIs.

You should use enumeration members for keys.

~~~
enum Key {
  Index
}

for (int i = 0; i < ...; i++) {
  DoWork(i, context.CreateScope().WithValue(Key.Index, i));
}
~~~

> This will create a new scope (scopes have distinct member, file and line number information) and associate loop variable `i` with key `Key.Index`. Any error reported through the context will have access to this information.

When serialized, two distinct keys won't necessarily retain their distinctiveness. An unfortunate quirk that you should be mindful of.

## Errors

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

As long as the return type of the method has an implicit conversion from `OperationError<Error>` to the actual return type the error can be returned from the function as is.

Therefore, you may also want to use a command result type, like this:

~~~
public class Result
{
  public OperationError<Error> Error { get; set; }

  public bool Success => Error.Success;
  public bool HasError => Error.HasError;

  public static implicit operator Result(OperationError<Error> error) {
    return new Result { Error = error };
  }
}

public class Result<TResult> : Result 
{
  public TResult Payload { get; set; }

  public static implicit operator Result<TResult>(OperationError<Error> error) {
    return new Result<TResult> { Error = error };
  }

  public static implicit operator Result<TResult>(TResult payload) {
    return new Result<TResult> { Payload = payload };
  }
}
~~~

And create methods that return like this:

~~~
public Result<string> Success(OperationContext context) {
  return "Hello World!";
}

public Result<string> HasError(OperationContext context) {
  return context.Error(Error.ArgumentOutOfRangeError);
}
~~~

----

Go forth and write better code!

## Dependencies

- `<package id="Newtonsoft.Json" version="8.0.3" targetFramework="net461" />`

This is the most recent version of `Newtonsoft.Json` before the .NET standard was introduced. If you need to use a more recent version of this package, it should work just fine by redirecting assembly versions (which NuGet will do for you).

## References

https://blog.golang.org/context
