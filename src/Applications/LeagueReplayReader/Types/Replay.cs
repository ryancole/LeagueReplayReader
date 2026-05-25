using System;
using System.IO;
using System.Text;

namespace LeagueReplayReader.Types
{
    public class Replay
    {
        private string m_path;
        private FileStream m_stream;
        private ReplayHeader m_header;
        private ReplayPayloadEntry m_payloadEntry;
        private ReplayPayloadHeader m_payloadHeader;
        private int m_currentEntry;
        private int m_entryDataOffset;

        #region Methods

        public Replay(string p_path)
        {
            // save the file's absolute path
            m_path = p_path;

            // instanciate a binary file stream
            m_stream = File.Open(m_path, FileMode.Open, FileAccess.Read);

            // instanciate the replay file header
            m_header = new ReplayHeader(m_stream);

            // instanciate the replay file payload header
            m_payloadHeader = new ReplayPayloadHeader(m_stream);

            // set state vars
            m_currentEntry = 0;
            m_entryDataOffset = m_header.PayloadOffset + (17 * (m_payloadHeader.ChunkCount + m_payloadHeader.KeyframeCount));
        }

        public bool ReadEntry()
        {
            // make sure we have no read beyond the bounds of the entry data
            if (m_currentEntry < (m_payloadHeader.ChunkCount + m_payloadHeader.KeyframeCount))
            {
                // seek to this entry's starting offset
                m_stream.Seek(m_header.PayloadOffset + (17 * m_currentEntry), SeekOrigin.Begin);

                // read out the payload entry
                m_payloadEntry = new ReplayPayloadEntry(this, m_stream, m_entryDataOffset);

                // set the current entry index
                m_currentEntry++;

                return true;
            }

            return false;
        }

        public override string ToString()
        {
            return string.Format("<Replay file={0}>", Path.GetFileName(m_path));
        }

        #endregion

        #region Properties

        public string FilePath
        {
            get
            {
                return m_path;
            }
        }

        public ReplayHeader Header
        {
            get
            {
                return m_header;
            }
        }

        public ReplayPayloadHeader PayloadHeader
        {
            get
            {
                return m_payloadHeader;
            }
        }

        public ReplayPayloadEntry PayloadEntry
        {
            get
            {
                return m_payloadEntry;
            }
        }

        #endregion
    }
}