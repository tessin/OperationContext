using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Threading;
using Tessin.Diagnostics.Internal;

namespace Tessin.Diagnostics
{
    [JsonConverter(typeof(OperationContextJsonConverter))]
    public sealed class OperationContext
    {
        // _tick.Elapsed is used as a monotonic function for time, 
        // i.e. time that cannot go backwards due daylight savings 
        // or other system clock/timezone adjustments
        private static readonly Stopwatch _tick = Stopwatch.StartNew();

        // ================

        private static int Depth(OperationContext context)
        {
            var depth = 0;

            var context2 = context;
            while (context2 != null)
            {
                depth++;
                context2 = context2._parent;
            }

            return depth;
        }

        public static OperationStackFrame[] Backtrace(OperationContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            var trace = new OperationStackFrame[Depth(context)];

            var index = 0;

            var context2 = context;
            while (context2 != null)
            {
                trace[index++] = new OperationStackFrame
                {
                    State = context2._values,
                    Location = context2._location,
                };
                context2 = context2._parent;
            }

            return trace;
        }

        public static byte[] Id(OperationStackFrame[] stackTrace)
        {
            var buffer = new MemoryStream();

            var writer = new BinaryWriter(buffer);
            writer.Write(stackTrace.Length);
            for (int i = 0; i < stackTrace.Length; i++)
            {
                var loc = stackTrace[i].Location.Normalize();

                writer.Write(loc.MemberName);
                writer.Write((byte)0);
                writer.Write(loc.FilePath);
                writer.Write((byte)0);
            }

            buffer.Position = 0;

            var hasher = new SHA256Managed();
            return hasher.ComputeHash(buffer);
        }

        // ================

        private readonly OperationContext _parent;
        public CancellationToken CancellationToken { get; }
        private readonly OperationValueDictionary _values;
        private readonly TimeSpan _created;
        private readonly TimeSpan _timeout;
        private readonly OperationLocation _location;

        // ================

        /// <summary>
        /// The amount of time that has elapsed since the operation context was created.
        /// </summary>
        public TimeSpan Elapsed => _tick.Elapsed - _created;

        public TimeSpan Timeout => _timeout;

        /// <summary>
        /// Timeout - Elapsed, note that Remaining can be negative.
        /// </summary>
        public TimeSpan Remaining => _timeout - Elapsed;

        // ================

        public OperationContext(
            CancellationToken cancellationToken = default(CancellationToken),
            TimeSpan? timeout = null,
            [CallerMemberName] string memberName = null,
            [CallerFilePath] string filePath = null,
            [CallerLineNumber] int lineNumber = 0
            )
        {
            CancellationToken = cancellationToken;
            _values = OperationValueDictionary.Empty;
            _created = _tick.Elapsed;
            _timeout = timeout ?? TimeSpan.FromMinutes(5); // Azure function default timeout
            _location = new OperationLocation(memberName, filePath, lineNumber);
        }

        private OperationContext(
            OperationContext parent,
            CancellationToken cancellationToken,
            OperationValueDictionary values,
            TimeSpan created,
            TimeSpan timeout,
            OperationLocation location
            )
        {
            _parent = parent;
            CancellationToken = cancellationToken;
            _values = values;
            _created = created;
            _timeout = timeout;
            _location = location;
        }

        // ================

        public OperationContext WithCancellationToken(CancellationToken cancellationToken)
        {
            return new OperationContext(
                parent: _parent,
                cancellationToken: cancellationToken,
                values: _values,
                created: _created,
                timeout: _timeout,
                location: _location
            );
        }

        // ================

        public OperationContext WithValue<TKey>(TKey key, object value)
            where TKey : struct, IComparable, IFormattable, IConvertible
        {
            return new OperationContext(
                parent: _parent,
                cancellationToken: CancellationToken,
                values: _values.SetItem(OperationValueKey.Create(key), value),
                created: _created,
                timeout: _timeout,
                location: _location
            );
        }

        public object GetValue<TKey>(TKey key)
            where TKey : struct, IComparable, IFormattable, IConvertible
        {
            return _values.GetItem(OperationValueKey.Create(key));
        }

        // ================

        public OperationContext WithTimeout(TimeSpan timeout)
        {
            if (_timeout < timeout)
            {
                return this; // no op
            }
            return new OperationContext(
                parent: _parent,
                cancellationToken: CancellationToken,
                values: _values,
                created: _created,
                timeout: timeout,
                location: _location
            );
        }

        // ================

        public OperationContext CreateScope(
            [CallerMemberName] string memberName = null,
            [CallerFilePath] string filePath = null,
            [CallerLineNumber] int lineNumber = 0
            )
        {
            return new OperationContext(
                parent: this,
                cancellationToken: CancellationToken,
                values: _values,
                created: _created,
                timeout: _timeout,
                location: new OperationLocation(memberName, filePath, lineNumber));
        }

        // ================

        public OperationError<TError> Error<TError>(TError error,
            [CallerMemberName] string memberName = null,
            [CallerFilePath] string filePath = null,
            [CallerLineNumber] int lineNumber = 0
            )
        {
            return new OperationError<TError>(error, CreateScope(memberName, filePath, lineNumber));
        }

        // ================

        /// <summary>
        /// Does not throw. You MUST throw the returned exception if you want to propagate the error this way.
        /// </summary>
        public OperationException CreateException(
            FormattableString message, 
            Exception innerException = null,
            [CallerMemberName] string memberName = null,
            [CallerFilePath] string filePath = null,
            [CallerLineNumber] int lineNumber = 0
            )
        {
            return new OperationException(message, innerException, CreateScope(memberName, filePath, lineNumber));
        }
    }
}
