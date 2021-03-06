﻿using System;
using System.IO;
using System.Text;

namespace LeagueReplayReader.Types
{
    public class ReplayPayloadHeader
    {
        private long m_gameId;
        private int m_gameLength;
        private int m_keyframeCount;
        private int m_chunkCount;
        private int m_endStartupChunkId;
        private int m_startGameChunkId;
        private int m_keyframeInterval;
        private short m_encryptionKeyLength;
        private byte[] m_encryptionKey;

        #region Methods

        public ReplayPayloadHeader(FileStream p_stream)
        {
            using (BinaryReader r = new BinaryReader(p_stream, Encoding.UTF8, true))
            {
                m_gameId = r.ReadInt64();
                m_gameLength = r.ReadInt32();
                m_keyframeCount = r.ReadInt32();
                m_chunkCount = r.ReadInt32();
                m_endStartupChunkId = r.ReadInt32();
                m_startGameChunkId = r.ReadInt32();
                m_keyframeInterval = r.ReadInt32();
                m_encryptionKeyLength = r.ReadInt16();
                m_encryptionKey = Convert.FromBase64String(Encoding.UTF8.GetString(r.ReadBytes(m_encryptionKeyLength)));
            }
        }

        public override string ToString()
        {
            return string.Format("<ReplayPayloadHeader gameId={0} gameLen={1} keyframes={2} chunks={3}>", m_gameId, m_gameLength, m_keyframeCount, m_chunkCount);
        }

        #endregion

        #region Properties

        public byte[] EncryptionKey
        {
            get
            {
                return m_encryptionKey;
            }
        }

        public long GameId
        {
            get
            {
                return m_gameId;
            }
        }

        public int GameLength
        {
            get
            {
                return m_gameLength;
            }
        }

        public int KeyframeCount
        {
            get
            {
                return m_keyframeCount;
            }
        }

        public int ChunkCount
        {
            get
            {
                return m_chunkCount;
            }
        }

        #endregion
    }
}