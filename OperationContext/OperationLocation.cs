using Newtonsoft.Json;
using System.IO;
using System.Text;

namespace Tessin.Diagnostics
{
    public struct OperationLocation
    {
        [JsonProperty("m")]
        public string MemberName { get; }

        [JsonProperty("fn")]
        public string FilePath { get; }

        [JsonProperty("ln")]
        public int LineNumber { get; }

        public OperationLocation(string memberName, string filePath, int lineNumber)
        {
            MemberName = memberName;
            FilePath = filePath;
            LineNumber = lineNumber;
        }

        private static readonly char[] AnyDirectorySeparatorChar = new[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar };

        public OperationLocation Normalize()
        {
            // the idea of the normal form is that it is stable across time
            // for this reason we do not include the full path nor the exact
            // line number information

            var filePath = FilePath;
            if (filePath != null)
            {
                var i = filePath.LastIndexOfAny(AnyDirectorySeparatorChar);
                if (i != -1)
                {
                    var j = filePath.LastIndexOfAny(AnyDirectorySeparatorChar, i - 1);
                    if (j != -1)
                    {
                        var sb = new StringBuilder();

                        for (int k = j + 1; k < i; k++)
                        {
                            sb.Append(filePath[k]);
                        }

                        sb.Append('/');

                        for (int k = i + 1, l = filePath.Length; k < l; k++)
                        {
                            sb.Append(filePath[k]);
                        }

                        var filePath2 = sb.ToString();

                        return new OperationLocation(MemberName, filePath2, 1);
                    }
                }
            }
            return new OperationLocation(MemberName, filePath, 1);
        }
    }
}
