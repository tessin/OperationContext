using Newtonsoft.Json;
using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Threading;
using Tessin.Diagnostics.Internal;

namespace Tessin.Diagnostics
{
    [JsonConverter(typeof(OperationContextJsonConverter))]
    public class OperationContext
    {
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
                    State = context2.State,
                    Location = context2._loc,
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

        private OperationContext _parent;
        public CancellationToken CancellationToken { get; }
        public OperationValueDictionary State { get; }
        private OperationLocation _loc;

        // ================

        public OperationContext(
            CancellationToken cancellationToken = default(CancellationToken),
            OperationValueDictionary state = null,
            [CallerMemberName] string memberName = null,
            [CallerFilePath] string filePath = null,
            [CallerLineNumber] int lineNumber = 0
            )
        {
            CancellationToken = cancellationToken;
            State = state ?? OperationValueDictionary.Empty;
            _loc = new OperationLocation(memberName, filePath, lineNumber);
        }

        private OperationContext(
            OperationContext parent,
            CancellationToken cancellationToken,
            OperationValueDictionary state,
            OperationLocation location
            )
        {
            _parent = parent;
            CancellationToken = cancellationToken;
            State = state;
            _loc = location;
        }

        // ================

        public OperationContext WithCancellationToken(CancellationToken cancellationToken)
        {
            return new OperationContext(
                parent: _parent,
                cancellationToken: cancellationToken,
                state: State,
                location: _loc
            );
        }

        // ================

        public OperationContext WithState(string key, OperationValue value)
        {
            return new OperationContext(
                parent: _parent,
                cancellationToken: CancellationToken,
                state: State.SetItem(key, value),
                location: _loc
            );
        }

        public OperationContext WithState(
            OperationValueDictionary newState
            )
        {
            if (newState == null)
            {
                throw new ArgumentNullException(nameof(newState));
            }
            return new OperationContext(
                parent: _parent,
                cancellationToken: CancellationToken,
                state: newState,
                location: _loc
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
                state: State,
                location: new OperationLocation(memberName, filePath, lineNumber)
            );
        }

        // ================

        public OperationError<TError> Error<TError>(TError error)
        {
            return new OperationError<TError>(error, this);
        }

        // ================

        /// <summary>
        /// Does not throw. You MUST throw the returned exception if you want to propagate the error this way.
        /// </summary>
        public OperationException CreateException(FormattableString message, Exception innerException = null)
        {
            return new OperationException(message, innerException, this);
        }
    }
}
