using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace Tessin.Diagnostics.Internal
{
    public struct OperationStackFrame
    {
        [JsonProperty("values")]
        public OperationValueDictionary State { get; set; }

        public bool ShouldSerializeState()
        {
            return State.HasValue;
        }

        [JsonProperty("location")]
        public OperationLocation Location { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            ToString(new StringWriter(sb, CultureInfo.InvariantCulture), Debugger.IsAttached);
            return sb.ToString();
        }

        public void ToString(TextWriter w, bool debuggerIsAttached = false)
        {
            var loc = Location;

            if (debuggerIsAttached)
            {
                // full path

                w.Write(loc.FilePath);
                w.Write('(');
                w.Write(loc.LineNumber);
                w.Write(')');
            }
            else
            {
                var loc2 = loc.Normalize();

                w.Write(loc2.FilePath);
                w.Write('(');
                w.Write(loc2.LineNumber);
                w.Write(')');
            }

            w.Write(' ');
            w.Write(loc.MemberName);
            w.Write(' ');

            var n = 0;
            foreach (var item in State.Reverse())
            {
                if (0 < n)
                {
                    w.Write(',');
                    w.Write(' ');
                }
                w.Write(FormattableString.Invariant($"{item.Key}={item.Value}")); // debug string version
            }

            w.WriteLine();
        }
    }
}
