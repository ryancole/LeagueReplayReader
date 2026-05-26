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

            Console.WriteLine();
            Console.WriteLine("=== Decompressed chunk data (first 64 bytes each) ===");
            foreach (var chunk in chunks)
                PrintEntryHex(chunk, 64);

            Console.WriteLine();
            Console.WriteLine("=== Decompressed keyframe data (first 64 bytes each) ===");
            foreach (var kf in keyframes)
                PrintEntryHex(kf, 64);

            if (keyframes.Count > 0)
            {
                Console.WriteLine();
                Console.WriteLine("=== First keyframe — raw bytes (first 256) ===");
                PrintHex(keyframes[0].Data, 256);

                Console.WriteLine();
                Console.WriteLine("=== First keyframe — block parse trace ===");
                TraceKeyframeBlocks(keyframes[0].Data, maxBlocks: 8);
            }

            Console.WriteLine();
            Console.WriteLine("Saving decompressed entries...");
            SaveEntries(source, chunks, keyframes);

            Console.ReadLine();
        }

        static void SaveEntries(string roflPath, List<Rofl2PayloadEntry> chunks, List<Rofl2PayloadEntry> keyframes)
        {
            // Output folder: sibling of the .rofl file, named after it (without extension)
            string outputDir = Path.Combine(
                Path.GetDirectoryName(roflPath)!,
                Path.GetFileNameWithoutExtension(roflPath));

            Directory.CreateDirectory(outputDir);

            foreach (var chunk in chunks)
            {
                string path = Path.Combine(outputDir, $"chunk_{chunk.Id:D4}.bin");
                File.WriteAllBytes(path, chunk.Data);
            }

            foreach (var kf in keyframes)
            {
                string path = Path.Combine(outputDir, $"keyframe_{kf.Id:D4}.bin");
                File.WriteAllBytes(path, kf.Data);
            }

            Console.WriteLine("  Output folder : {0}", outputDir);
            Console.WriteLine("  Chunks saved  : {0}", chunks.Count);
            Console.WriteLine("  Keyframes saved: {0}", keyframes.Count);
        }

        static void TraceKeyframeBlocks(byte[] data, int maxBlocks)
        {
            using var ms = new MemoryStream(data);
            using var reader = new BinaryReader(ms);

            float lastTime = 0f;
            uint lastBlockParam = 0;
            ushort lastType = 0;
            int blockNum = 0;

            while (ms.Position < ms.Length && blockNum < maxBlocks)
            {
                long blockStart = ms.Position;
                Console.WriteLine("  --- Block {0} at offset 0x{1:X} ---", blockNum, blockStart);

                byte marker = reader.ReadByte();
                bool timeIs1Byte      = (marker & 0x80) != 0;
                bool noType           = (marker & 0x40) != 0;
                bool paramIs1Byte     = (marker & 0x20) != 0;
                bool lenIs1Byte       = (marker & 0x10) != 0;
                int  channel          = marker & 0x0F;
                Console.WriteLine("    marker=0x{0:X2}  ch={1}  timeIs1B={2}  noType={3}  paramIs1B={4}  lenIs1B={5}",
                    marker, channel, timeIs1Byte, noType, paramIs1Byte, lenIs1Byte);

                if (ms.Position + (timeIs1Byte ? 1 : 4) > ms.Length) { Console.WriteLine("    !! ran out of bytes reading time"); break; }
                float timeSeconds;
                if (timeIs1Byte) { byte d = reader.ReadByte(); timeSeconds = lastTime + d / 1000f; Console.WriteLine("    time delta={0}ms => {1:F3}s", d, timeSeconds); }
                else             { timeSeconds = reader.ReadSingle(); Console.WriteLine("    time float={0:F3}s", timeSeconds); }

                if (ms.Position + (lenIs1Byte ? 1 : 4) > ms.Length) { Console.WriteLine("    !! ran out of bytes reading contentLength"); break; }
                int contentLength;
                if (lenIs1Byte) { contentLength = reader.ReadByte(); Console.WriteLine("    contentLength(1B)={0}", contentLength); }
                else            { contentLength = (int)reader.ReadUInt32(); Console.WriteLine("    contentLength(4B)={0}", contentLength); }

                if (!noType)
                {
                    if (ms.Position + 1 > ms.Length) { Console.WriteLine("    !! ran out of bytes reading type"); break; }
                    byte t = reader.ReadByte();
                    lastType = t;
                    Console.WriteLine("    type=0x{0:X2}", t);
                }
                else Console.WriteLine("    type=0x{0:X2} (repeated)", lastType);

                if (ms.Position + (paramIs1Byte ? 1 : 4) > ms.Length) { Console.WriteLine("    !! ran out of bytes reading blockparam"); break; }
                uint blockParam;
                if (paramIs1Byte) { byte d = reader.ReadByte(); blockParam = lastBlockParam + d; Console.WriteLine("    blockparam delta={0} => 0x{1:X8}", d, blockParam); }
                else              { blockParam = reader.ReadUInt32(); Console.WriteLine("    blockparam(4B)=0x{0:X8}", blockParam); }

                Console.WriteLine("    content: {0} bytes starting at offset 0x{1:X}", contentLength, ms.Position);
                if (contentLength > 0 && ms.Position + Math.Min(contentLength, 16) <= ms.Length)
                {
                    byte[] preview = reader.ReadBytes(Math.Min(contentLength, 16));
                    Console.WriteLine("    content preview: {0}", string.Join(" ", preview.Select(b => b.ToString("X2"))));
                    // skip remaining content bytes
                    int remaining = contentLength - preview.Length;
                    if (remaining > 0) ms.Seek(remaining, SeekOrigin.Current);
                }
                else if (contentLength > 0)
                {
                    Console.WriteLine("    !! ran out of bytes reading content ({0} bytes needed)", contentLength);
                    break;
                }

                lastTime = timeSeconds;
                lastBlockParam = blockParam;
                blockNum++;
            }
        }

        static void PrintEntryHex(Rofl2PayloadEntry entry, int maxBytes)
        {
            Console.WriteLine("  [{0}] id={1} nextChunk={2} uncompressed={3} bytes",
                entry.EntryType, entry.Id, entry.NextChunkId, entry.UncompressedLength);
            PrintHex(entry.Data, maxBytes);
        }

        static void PrintHex(byte[] data, int maxBytes)
        {
            int count = Math.Min(data.Length, maxBytes);
            for (int i = 0; i < count; i += 16)
            {
                string hex = string.Join(" ", data.Skip(i).Take(16).Select(b => b.ToString("X2")));
                string ascii = new string(data.Skip(i).Take(16).Select(b => b >= 32 && b <= 126 ? (char)b : '.').ToArray());
                Console.WriteLine("    {0:X4}: {1,-47}  {2}", i, hex, ascii);
            }
            if (data.Length > maxBytes)
                Console.WriteLine("    ... ({0} bytes total)", data.Length);
        }
    }
}
