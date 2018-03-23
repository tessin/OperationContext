using Newtonsoft.Json;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace Tessin.Diagnostics
{
    public struct OperationStackFrame
    {
        [JsonProperty("s")]
        public OperationValueDictionary State { get; set; }

        [JsonProperty("loc")]
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

            var jsonWriter = new JsonTextWriter(w);
            jsonWriter.WriteStartObject();
            foreach (var item in State.Reverse())
            {
                jsonWriter.WritePropertyName(item.Key);
                item.Value.WriteJson(jsonWriter);
            }
            jsonWriter.WriteEnd();

            w.WriteLine();
        }
    }
}
