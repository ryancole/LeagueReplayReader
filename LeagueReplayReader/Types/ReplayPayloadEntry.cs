using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Linq;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Parameters;

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

        public byte[] GetDecryptedData(Replay p_replay)
        {
            // string represenation of the game id
            string gameId = Convert.ToString(p_replay.PayloadHeader.GameId);

            BufferedBlockCipher blowfish = new BufferedBlockCipher(new BlowfishEngine());
            blowfish.Init(false, new KeyParameter(Encoding.UTF8.GetBytes(gameId)));

            // obtaining the chunk encryption key
            byte[] chunkEncryptionKey = blowfish.ProcessBytes(p_replay.PayloadHeader.EncryptionKey);

            // padding length to remove
            int paddingLength = Convert.ToInt32(chunkEncryptionKey[chunkEncryptionKey.Length - 1]);

            // adjusted encryption key
            chunkEncryptionKey = chunkEncryptionKey.Take(chunkEncryptionKey.Length - paddingLength).ToArray();

            BufferedBlockCipher blowfish2 = new BufferedBlockCipher(new BlowfishEngine());
            blowfish2.Init(false, new KeyParameter(chunkEncryptionKey));

            // obtaining the decrypted chunk
            byte[] decryptedChunk = blowfish2.ProcessBytes(m_data);

            // padding length to remove
            int paddingLength2 = Convert.ToInt32(decryptedChunk[decryptedChunk.Length - 1]);

            // adjusted decrypted chunk
            decryptedChunk = decryptedChunk.Take(decryptedChunk.Length - paddingLength2).ToArray();

            return DecompressBytes(decryptedChunk);
        }

        private byte[] DecompressBytes(byte[] p_data)
        {
            using (GZipStream stream = new GZipStream(new MemoryStream(p_data), CompressionMode.Decompress))
            {
                const int size = 4096;
                byte[] buffer = new byte[size];

                using (MemoryStream memory = new MemoryStream())
                {
                    int count = 0;

                    do
                    {
                        count = stream.Read(buffer, 0, size);

                        if (count > 0)
                        {
                            memory.Write(buffer, 0, count);
                        }
                    }
                    while (count > 0);

                    return memory.ToArray();
                }
            }
        }

        public override string ToString()
        {
            return string.Format("<ReplayPayloadEntry id={0} type={1} len={2} next={3}>", m_id, Type, m_length, m_nextChunkId);
        }

        #endregion

        #region Properties

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