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

        #region Methods

        public Replay(string p_path)
        {
            // save the file's absolute path
            m_path = p_path;

            // instanciate a binary file stream
            m_stream = File.Open(m_path, FileMode.Open, FileAccess.Read);

            // instanciate the replay file header
            m_header = new ReplayHeader(m_stream);
        }

        #endregion

        #region Properties

        public string Path
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

        #endregion
    }
}