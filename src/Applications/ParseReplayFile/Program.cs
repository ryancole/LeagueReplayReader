using LeagueReplayReader.Common.Entity.Rofl;

namespace LeagueReplayReader.Applications
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Args: <source>");
                return;
            }

            string source = args[0];

            if (!File.Exists(source))
            {
                Console.WriteLine("Error: file not found: {0}", source);
                return;
            }

            Replay replay = new Replay(source);

            Console.WriteLine(replay);
            Console.WriteLine(replay.Header);
            Console.WriteLine(replay.Header.Metadata);

            Console.WriteLine("Game version : {0}", replay.Header.GameVersion);
            Console.WriteLine("Game length  : {0}ms", replay.Header.Metadata.GameLength);
            Console.WriteLine("Last chunk   : {0}", replay.Header.Metadata.LastGameChunkId);
            Console.WriteLine("Last keyframe: {0}", replay.Header.Metadata.LastKeyframeId);

            var players = PlayerStats.Deserialize(replay.Header.Metadata.StatsJson);

            Console.WriteLine("Players      : {0}", players.Length);

            foreach (var p in players)
            {
                Console.WriteLine("  {0}#{1} | {2}/{3}/{4} | Gold: {5}", p.RiotIdGameName, p.RiotIdTagLine, p.ChampionsKilled, p.Deaths, p.Assists, p.GoldEarned);
            }

            Console.WriteLine();
            Console.WriteLine("Reading payload...");

            var payload = replay.ReadPayload();

            var chunks = payload.Chunks.ToList();
            var keyframes = payload.Keyframes.ToList();
            var startups = payload.StartupEntries.ToList();

            Console.WriteLine("Startup entries : {0}", startups.Count);
            Console.WriteLine("Chunks          : {0}", chunks.Count);
            Console.WriteLine("Keyframes       : {0}", keyframes.Count);
            Console.WriteLine("Total entries   : {0}", payload.Entries.Count);

            Console.WriteLine();
            Console.WriteLine("=== Entry listing ===");
            foreach (var entry in payload.Entries)
            {
                Console.WriteLine("  {0}", entry);
            }

            if (chunks.Count > 0)
            {
                Console.WriteLine();
                Console.WriteLine("=== First chunk data (hex, first 128 bytes) ===");
                PrintHex(chunks[0].Data, 128);
            }

            if (keyframes.Count > 0)
            {
                Console.WriteLine();
                Console.WriteLine("=== First keyframe data (hex, first 128 bytes) ===");
                PrintHex(keyframes[0].Data, 128);
            }

            Console.ReadLine();
        }

        static void PrintHex(byte[] data, int maxBytes)
        {
            int count = Math.Min(data.Length, maxBytes);
            for (int i = 0; i < count; i += 16)
            {
                string hex = string.Join(" ", data.Skip(i).Take(16).Select(b => b.ToString("X2")));
                string ascii = new string(data.Skip(i).Take(16).Select(b => b >= 32 && b <= 126 ? (char)b : '.').ToArray());
                Console.WriteLine("  {0:X4}: {1,-47}  {2}", i, hex, ascii);
            }
            if (data.Length > maxBytes)
                Console.WriteLine("  ... ({0} bytes total)", data.Length);
        }
    }
}
