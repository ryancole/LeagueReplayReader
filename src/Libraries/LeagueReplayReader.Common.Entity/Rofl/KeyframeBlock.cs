namespace LeagueReplayReader.Common.Entity.Rofl
{
    public enum BlockChannel : byte
    {
        Handshake = 0,
        C2S = 1,
        Gameplay = 2,
        S2C = 3,
        LowPriority = 4,
        Communication = 5,
        LoadingScreen = 7,
    }

    public class KeyframeBlock
    {
        private static readonly Dictionary<ushort, string> KnownTypes = new()
        {
            [0x62] = "StartSpawn",
            [0x4C] = "SpawnHero",
            [0x4B] = "SpawnHeroAlt",
            [0x2A] = "SummonerData",
            [0x46] = "PlayerStats",
            [0x45] = "PlayerStatsAlt",
            [0x15] = "SetAbilityLevels",
        };

        public BlockChannel Channel { get; init; }
        public float TimeSeconds { get; init; }
        public ushort PacketType { get; init; }
        public uint BlockParam { get; init; }
        public byte[] Content { get; init; } = [];

        public string TypeName => KnownTypes.TryGetValue(PacketType, out var name) ? name : $"0x{PacketType:X2}";

        public override string ToString() =>
            $"[{Channel,-14}] t={TimeSeconds,8:F3}s  type={TypeName,-18}  param=0x{BlockParam:X8}  len={Content.Length}";
    }
}
