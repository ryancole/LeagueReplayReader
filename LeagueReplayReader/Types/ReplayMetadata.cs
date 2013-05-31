using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace LeagueReplayReader.Types
{
    [DataContract]
    public class ReplayMetadata
    {
        private int m_gameId;

        #region Static Functions

        public static ReplayMetadata Deserialize(byte[] p_json)
        {
            using (MemoryStream m = new MemoryStream(p_json))
            {
                DataContractJsonSerializer s = new DataContractJsonSerializer(typeof(ReplayMetadata));
                return (ReplayMetadata)s.ReadObject(m);
            }
        }

        #endregion

        #region Properties

        [DataMember]
        public int gameId
        {
            get
            {
                return m_gameId;
            }

            set
            {
                m_gameId = value;
            }
        }

        #endregion
    }
}