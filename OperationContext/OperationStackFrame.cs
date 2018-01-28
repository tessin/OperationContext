using Newtonsoft.Json;
using System.Collections.Immutable;

namespace Tessin.Diagnostics
{
    public struct OperationStackFrame
    {
        [JsonProperty("s")]
        public ImmutableDictionary<string, OperationValue> State { get; set; }

        [JsonProperty("loc")]
        public OperationLocation Location { get; set; }
    }
}
