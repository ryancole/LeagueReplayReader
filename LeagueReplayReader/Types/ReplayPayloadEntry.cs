using System;
using System.IO;
using System.Text;

namespace LeagueReplayReader.Types
{
    public enum ReplayPayloadEntryType
    {
        Chunk,
        Keyframe,
        Unknown
    }

    public class ReplayPayloadEntry
    {
        private int m_id;
        private int m_length;
        private int m_nextChunkId;
        private int m_offset;
        private byte m_type;
        private byte[] m_data;

        #region Methods

        public ReplayPayloadEntry(FileStream p_stream, int p_payloadDataStartOffset)
        {
            using (BinaryReader r = new BinaryReader(p_stream, Encoding.UTF8, true))
            {
                m_id = r.ReadInt32();
                m_type = r.ReadByte();
                m_length = r.ReadInt32();
                m_nextChunkId = r.ReadInt32();
                m_offset = r.ReadInt32();
            }

            // seek to the entry's data location
            p_stream.Seek(p_payloadDataStartOffset + m_offset, SeekOrigin.Begin);

            // init the byte array to appropriate length
            m_data = new byte[m_length];

            // the entry data chunk
            p_stream.Read(m_data, 0, m_length);
        }

        public override string ToString()
        {
            return string.Format("<ReplayPayloadEntry id={0} type={1} len={2}", m_id, Type, m_length);
        }

        #endregion

        #region Properties

        public byte[] Data
        {
            get
            {
                return m_data;
            }
        }

        public int ID
        {
            get
            {
                return m_id;
            }
        }

        public ReplayPayloadEntryType Type
        {
            get
            {
                if (m_type == 1)
                {
                    return ReplayPayloadEntryType.Chunk;
                }
                else if (m_type == 2)
                {
                    return ReplayPayloadEntryType.Keyframe;
                }

                return ReplayPayloadEntryType.Unknown;
            }
        }

        public int Length
        {
            get
            {
                return m_length;
            }
        }

        public int Offset
        {
            get
            {
                return m_offset;
            }
        }

        public int NextChunkID
        {
            get
            {
                return m_nextChunkId;
            }
        }

        #endregion
    }
}