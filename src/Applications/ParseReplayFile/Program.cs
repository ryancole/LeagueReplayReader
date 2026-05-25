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
        }
    }
}
