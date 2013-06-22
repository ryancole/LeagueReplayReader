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

        public ReplayPayloadEntry(Replay p_replay, FileStream p_stream, int p_payloadDataStartOffset)
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

            // store the decrypted data
            m_data = GetDecryptedData(p_replay, m_data);
        }

        private byte[] GetDecryptedData(Replay p_replay, byte[] p_data)
        {
            // string represenation of the game id
            string gameId = Convert.ToString(p_replay.PayloadHeader.GameId);

            // obtaining the chunk encryption key
            byte[] chunkEncryptionKey = DepadBytes(DecryptBytes(Encoding.UTF8.GetBytes(gameId), p_replay.PayloadHeader.EncryptionKey));

            // obtaining the decrypted chunk
            byte[] decryptedChunk = DepadBytes(DecryptBytes(chunkEncryptionKey, p_data));

            return DecompressBytes(decryptedChunk);
        }

        /// <summary>
        /// http://tools.ietf.org/html/rfc2898
        /// </summary>
        private byte[] DepadBytes(byte[] p_data)
        {
            int paddingLength = Convert.ToInt32(p_data[p_data.Length - 1]);

            return p_data.Take(p_data.Length - paddingLength).ToArray();
        }

        private byte[] DecryptBytes(byte[] p_key, byte[] p_data)
        {
            BufferedBlockCipher cipher = new BufferedBlockCipher(new BlowfishEngine());

            // init using the given key
            cipher.Init(false, new KeyParameter(p_key));

            // decrypt the given data
            return cipher.ProcessBytes(p_data);
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