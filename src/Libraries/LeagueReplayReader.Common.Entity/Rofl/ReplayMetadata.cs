using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LeagueReplayReader.Common.Entity.Rofl
{
    public class ReplayMetadata
    {
        public override string ToString()
        {
            return string.Format("<ReplayMetadata gameLength={0} lastChunk={1} lastKeyframe={2}>",
                GameLength, LastGameChunkId, LastKeyframeId);
        }

        public static ReplayMetadata Deserialize(byte[] p_json)
        {
            return JsonSerializer.Deserialize<ReplayMetadata>(p_json)
                ?? throw new InvalidDataException("Failed to deserialize replay metadata");
        }

        [JsonPropertyName("gameLength")]
        public ulong GameLength { get; set; }

        [JsonPropertyName("lastGameChunkId")]
        public uint LastGameChunkId { get; set; }

        [JsonPropertyName("lastKeyFrameId")]
        public uint LastKeyframeId { get; set; }

        [JsonPropertyName("statsJson")]
        public string StatsJson { get; set; } = string.Empty;
    }
}
