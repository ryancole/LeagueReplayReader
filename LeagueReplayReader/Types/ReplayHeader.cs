using System;
using System.IO;
using System.Text;

namespace LeagueReplayReader.Types
{
    public class ReplayHeader
    {
        private byte[] m_magic;
        private byte[] m_signature;
        private short m_headerLength;
        private int m_fileLength;
        private int m_metadataOffset;
        private int m_metadataLength;
        private int m_payloadHeaderOffset;
        private int m_payloadHeaderLength;
        private int m_payloadOffset;
        private ReplayMetadata m_metadata;

        #region Methods

        public ReplayHeader(FileStream p_stream)
        {
            using (BinaryReader r = new BinaryReader(p_stream, Encoding.UTF8, true))
            {
                // the magic byte identifiers
                m_magic = r.ReadBytes(6);

                // file hash
                m_signature = r.ReadBytes(256);

                // various lengths and offsets
                m_headerLength = r.ReadInt16();
                m_fileLength = r.ReadInt32();
                m_metadataOffset = r.ReadInt32();
                m_metadataLength = r.ReadInt32();
                m_payloadHeaderOffset = r.ReadInt32();
                m_payloadHeaderLength = r.ReadInt32();
                m_payloadOffset = r.ReadInt32();

                // json metadata
                m_metadata = ReplayMetadata.Deserialize(r.ReadBytes(m_metadataLength));
            }
        }

        public override string ToString()
        {
            return string.Format("<ReplayHeader mo={0} ml={1} pho={2} phl={3} po={4}>", m_metadataOffset, m_metadataLength, m_payloadHeaderOffset, m_payloadHeaderLength, m_payloadOffset);
        }

        #endregion

        #region Properties

        public int PayloadOffset
        {
            get
            {
                return m_payloadOffset;
            }
        }

        public byte[] Magic
        {
            get
            {
                return m_magic;
            }
        }

        public byte[] Signature
        {
            get
            {
                return m_signature;
            }
        }

        public int MetadataOffset
        {
            get
            {
                return m_metadataOffset;
            }
        }

        public int MetadataLength
        {
            get
            {
                return m_metadataLength;
            }
        }

        public ReplayMetadata Metadata
        {
            get
            {
                return m_metadata;
            }
        }

        #endregion
    }
}