using System;
using System.IO;

namespace LeagueReplayReader.Common.Entity.Rofl
{
    public class Replay
    {
        private string m_path;
        private FileStream m_stream;
        private ReplayHeader m_header;

        public Replay(string p_path)
        {
            m_path = p_path;
            m_stream = new FileStream(m_path, FileMode.Open, FileAccess.Read, FileShare.Read);
            m_header = new ReplayHeader(m_stream);
        }

        public ReplayPayload ReadPayload()
        {
            return new ReplayPayload(m_stream, m_header.PayloadStartOffset, m_header.MetadataStartOffset);
        }

        public override string ToString()
        {
            return string.Format("<Replay file={0}>", Path.GetFileName(m_path));
        }

        public string FilePath => m_path;
        public ReplayHeader Header => m_header;
    }
}
