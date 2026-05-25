using System;
using System.IO;
using LeagueReplayReader.Types;

namespace LeagueReplayReader
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Args: <source> <dest>");
                return;
            }

            string source = args[0];
            string destination = args[1];

            if (!File.Exists(source))
            {
                Console.WriteLine("Error: file not found: {0}", source);
                return;
            }

            if (!Directory.Exists(destination))
            {
                Directory.CreateDirectory(destination);
            }

            // init the replay file
            Replay replay = new Replay(source);

            // handle the entries within the replay file
            while (replay.ReadEntry())
            {
                Console.WriteLine(replay.PayloadEntry);

                // write the payload out to disk
                File.WriteAllBytes(string.Format(@"{0}\{1}-{2}-{3}.bin", destination, replay.PayloadHeader.GameId, replay.PayloadEntry.ID, replay.PayloadEntry.Type), replay.PayloadEntry.Data);
            }
        }
    }
}
