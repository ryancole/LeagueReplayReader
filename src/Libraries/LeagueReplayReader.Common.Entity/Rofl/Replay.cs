using System;
using System.IO;

namespace LeagueReplayReader.Common.Entity.Rofl
{
    public class Replay
    {
        private string m_path;
        private FileStream m_stream;
        private ReplayHeader m_header;
        private ReplayPayloadHeader? m_payloadHeader;
        private ReplayPayloadEntry? m_payloadEntry;

        public Replay(string p_path)
        {
            m_path = p_path;
            m_stream = File.Open(m_path, FileMode.Open, FileAccess.Read);
            m_header = new ReplayHeader(m_stream);
        }

        // Payload parsing is not supported for ROFL2 (data is obfuscated).
        public bool ReadEntry() => false;

        public override string ToString()
        {
            return string.Format("<Replay file={0}>", Path.GetFileName(m_path));
        }

        public string FilePath => m_path;
        public ReplayHeader Header => m_header;
        public ReplayPayloadHeader? PayloadHeader => m_payloadHeader;
        public ReplayPayloadEntry? PayloadEntry => m_payloadEntry;
    }
}
