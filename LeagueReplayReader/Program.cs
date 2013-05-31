using System;
using System.IO;
using LeagueReplayReader.Types;

namespace LeagueReplayReader
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Args: <filepath>");
                return;
            }

            string filepath = args[0];

            if (!File.Exists(filepath))
            {
                Console.WriteLine("Error: file not found: {0}", filepath);
                return;
            }

            // instanciate the replay file
            Replay replay = new Replay(filepath);

            Console.WriteLine(replay);
            Console.WriteLine(replay.Header);
            Console.WriteLine(replay.PayloadHeader);
            Console.WriteLine(replay.Header.Metadata.gameId);

            // handle the entries within the replay file
            while (replay.ReadEntry())
            {
                // decrypt the payload entry data
                byte[] data = replay.PayloadEntry.GetDecryptedData(replay);

                File.WriteAllBytes(string.Format(@"c:\users\ryan\desktop\foo\{0}_{1}.txt", replay.PayloadEntry.ID, replay.PayloadEntry.Type), data);

                Console.WriteLine(replay.PayloadEntry);
            }
        }
    }
}
