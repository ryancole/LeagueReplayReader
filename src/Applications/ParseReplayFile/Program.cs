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
            SaveEntries(source, chunks, keyframes, players);

            if (keyframes.Count > 0)
            {
                Console.WriteLine();
                Console.WriteLine("=== Champion ID search in first keyframe ===");
                SearchChampionIds(players, keyframes[0].Data);
            }

            Console.ReadLine();
        }

        // Champion internal name (DDragon id field) → numeric champion key
        static readonly Dictionary<string, int> ChampionIds = new(StringComparer.OrdinalIgnoreCase)
        {
            ["Annie"] = 1, ["Olaf"] = 2, ["Galio"] = 3, ["TwistedFate"] = 4,
            ["XinZhao"] = 5, ["Urgot"] = 6, ["Leblanc"] = 7, ["Vladimir"] = 8,
            ["FiddleSticks"] = 9, ["Fiddlesticks"] = 9, ["Kayle"] = 10,
            ["MasterYi"] = 11, ["Alistar"] = 12, ["Ryze"] = 13, ["Sion"] = 14,
            ["Sivir"] = 15, ["Soraka"] = 16, ["Teemo"] = 17, ["Tristana"] = 18,
            ["Warwick"] = 19, ["Nunu"] = 20, ["NunuWillump"] = 20,
            ["MissFortune"] = 21, ["Ashe"] = 22, ["Tryndamere"] = 23,
            ["Jax"] = 24, ["Morgana"] = 25, ["Zilean"] = 26, ["Singed"] = 27,
            ["Evelynn"] = 28, ["Twitch"] = 29, ["Karthus"] = 30,
            ["Chogath"] = 31, ["Amumu"] = 32, ["Rammus"] = 33, ["Anivia"] = 34,
            ["Shaco"] = 35, ["DrMundo"] = 36, ["Sona"] = 37, ["Kassadin"] = 38,
            ["Irelia"] = 39, ["Janna"] = 40, ["Gangplank"] = 41, ["Corki"] = 42,
            ["Karma"] = 43, ["Taric"] = 44, ["Veigar"] = 45, ["Trundle"] = 48,
            ["Swain"] = 50, ["Caitlyn"] = 51, ["Blitzcrank"] = 53,
            ["Malphite"] = 54, ["Katarina"] = 55, ["Nocturne"] = 56,
            ["Maokai"] = 57, ["Renekton"] = 58, ["JarvanIV"] = 59, ["Elise"] = 60,
            ["Orianna"] = 61, ["MonkeyKing"] = 62, ["Wukong"] = 62, ["Brand"] = 63,
            ["LeeSin"] = 64, ["Vayne"] = 67, ["Rumble"] = 68, ["Cassiopeia"] = 69,
            ["Skarner"] = 72, ["Heimerdinger"] = 74, ["Nasus"] = 75,
            ["Nidalee"] = 76, ["Udyr"] = 77, ["Poppy"] = 78, ["Gragas"] = 79,
            ["Pantheon"] = 80, ["Ezreal"] = 81, ["Mordekaiser"] = 82,
            ["Yorick"] = 83, ["Akali"] = 84, ["Kennen"] = 85, ["Garen"] = 86,
            ["Leona"] = 89, ["Malzahar"] = 90, ["Talon"] = 91, ["Riven"] = 92,
            ["KogMaw"] = 96, ["Shen"] = 98, ["Lux"] = 99, ["Xerath"] = 101,
            ["Shyvana"] = 102, ["Ahri"] = 103, ["Graves"] = 104, ["Fizz"] = 105,
            ["Volibear"] = 106, ["Rengar"] = 107, ["Varus"] = 110,
            ["Nautilus"] = 111, ["Viktor"] = 112, ["Sejuani"] = 113,
            ["Fiora"] = 114, ["Ziggs"] = 115, ["Lulu"] = 117, ["Draven"] = 119,
            ["Hecarim"] = 120, ["Khazix"] = 121, ["Darius"] = 122, ["Jayce"] = 126,
            ["Lissandra"] = 127, ["Diana"] = 131, ["Quinn"] = 133, ["Syndra"] = 134,
            ["AurelionSol"] = 136, ["Kayn"] = 141, ["Zoe"] = 142, ["Zyra"] = 143,
            ["Kaisa"] = 145, ["Seraphine"] = 147, ["Gnar"] = 150, ["Zac"] = 154,
            ["Yasuo"] = 157, ["Velkoz"] = 161, ["Taliyah"] = 163, ["Camille"] = 164,
            ["Akshan"] = 166, ["Belveth"] = 200, ["Braum"] = 201, ["Jhin"] = 202,
            ["Kindred"] = 203, ["Zeri"] = 221, ["Jinx"] = 222, ["TahmKench"] = 223,
            ["Briar"] = 233, ["Viego"] = 234, ["Senna"] = 235, ["Lucian"] = 236,
            ["Zed"] = 238, ["Kled"] = 240, ["Ekko"] = 245, ["Qiyana"] = 246,
            ["Vi"] = 254, ["Aatrox"] = 266, ["Nami"] = 267, ["Azir"] = 268,
            ["Yuumi"] = 350, ["Samira"] = 360, ["Thresh"] = 412, ["Illaoi"] = 420,
            ["RekSai"] = 421, ["Ivern"] = 427, ["Kalista"] = 429, ["Bard"] = 432,
            ["Rakan"] = 497, ["Xayah"] = 498, ["Ornn"] = 516, ["Sylas"] = 517,
            ["Neeko"] = 518, ["Aphelios"] = 523, ["Rell"] = 526, ["Pyke"] = 555,
            ["Vex"] = 711, ["Yone"] = 777, ["Ambessa"] = 799, ["Mel"] = 800,
            ["Sett"] = 875, ["Lillia"] = 876, ["Gwen"] = 887, ["Renata"] = 888,
            ["Aurora"] = 893, ["Nilah"] = 895, ["KSante"] = 897, ["Smolder"] = 901,
            ["Milio"] = 902, ["Hwei"] = 910, ["Naafiri"] = 950,
        };

        static void SearchChampionIds(PlayerStats[] players, byte[] data)
        {
            foreach (var p in players)
            {
                if (string.IsNullOrEmpty(p.Skin)) continue;

                if (!ChampionIds.TryGetValue(p.Skin, out int id))
                {
                    Console.WriteLine("  {0} — SKIN={1} (unknown ID, skipping)", p.RiotIdGameName, p.Skin);
                    continue;
                }

                Console.WriteLine("  {0} — {1} (ID {2})", p.RiotIdGameName, p.Skin, id);

                // Search as uint16 little-endian
                SearchPattern(data, BitConverter.GetBytes((ushort)id), $"    uint16 LE [{id}]");

                // Search as uint32 little-endian
                SearchPattern(data, BitConverter.GetBytes((uint)id), $"    uint32 LE [{id}]");
            }
        }

        static void SearchPattern(byte[] data, byte[] pattern, string label)
        {
            var offsets = new List<int>();
            for (int i = 0; i <= data.Length - pattern.Length; i++)
            {
                bool match = true;
                for (int j = 0; j < pattern.Length; j++)
                    if (data[i + j] != pattern[j]) { match = false; break; }
                if (match) offsets.Add(i);
            }

            if (offsets.Count == 0) { Console.WriteLine("{0}: no matches", label); return; }

            // Print up to 20 offsets and 3 context previews
            Console.WriteLine("{0}: {1} match(es) at {2}{3}",
                label, offsets.Count,
                string.Join(", ", offsets.Take(20).Select(o => $"0x{o:X}")),
                offsets.Count > 20 ? " ..." : "");

            foreach (int off in offsets.Take(3))
            {
                int start = Math.Max(0, off - 4);
                int end   = Math.Min(data.Length, off + pattern.Length + 8);
                string hex = string.Join(" ", data[start..end].Select(b => b.ToString("X2")));
                Console.WriteLine("      [{0:X4}] {1}", off, hex);
            }
        }

        static void SaveEntries(string roflPath, List<Rofl2PayloadEntry> chunks, List<Rofl2PayloadEntry> keyframes, PlayerStats[] players)
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

            // Write players.json so analysis tools can use champion info without re-parsing the .rofl
            var playerRecords = players.Select(p => new
            {
                name       = $"{p.RiotIdGameName}#{p.RiotIdTagLine}",
                skin       = p.Skin,
                team       = p.Team,
                championId = p.Skin != null && ChampionIds.TryGetValue(p.Skin, out int id) ? id : -1,
            });
            string playersJson = System.Text.Json.JsonSerializer.Serialize(playerRecords,
                new System.Text.Json.JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(Path.Combine(outputDir, "players.json"), playersJson);

            Console.WriteLine("  Output folder : {0}", outputDir);
            Console.WriteLine("  Chunks saved  : {0}", chunks.Count);
            Console.WriteLine("  Keyframes saved: {0}", keyframes.Count);
            Console.WriteLine("  players.json  : written");
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
