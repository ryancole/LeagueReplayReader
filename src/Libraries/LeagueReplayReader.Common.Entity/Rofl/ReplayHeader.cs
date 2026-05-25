using System.Text;

namespace LeagueReplayReader.Common.Entity.Rofl
{
    public class ReplayHeader
    {
        private byte[] m_riot;
        private byte[] m_version;
        private string m_gameVersion;
        private ReplayMetadata m_metadata;

        public ReplayHeader(FileStream p_stream)
        {
            using (BinaryReader r = new BinaryReader(p_stream, Encoding.UTF8, true))
            {
                // "RIOT" identifier
                m_riot = r.ReadBytes(4);

                // format version bytes (0x02 0x00 for ROFL2)
                m_version = r.ReadBytes(2);

                // skip 8 unknown header bytes (offsets 6-13)
                p_stream.Seek(8, SeekOrigin.Current);

                // byte 14 is a length prefix for the game version string
                byte gameVersionLength = r.ReadByte();

                // read the version string using the length prefix
                m_gameVersion = Encoding.UTF8.GetString(r.ReadBytes(gameVersionLength));

                // metadata length is stored in the last 4 bytes of the file
                p_stream.Seek(-4, SeekOrigin.End);
                int metadataLength = r.ReadInt32();

                // metadata JSON immediately precedes the length field
                p_stream.Seek(-(metadataLength + 4), SeekOrigin.End);
                m_metadata = ReplayMetadata.Deserialize(r.ReadBytes(metadataLength));
            }
        }

        public override string ToString()
        {
            return string.Format("<ReplayHeader gameVersion={0}>", m_gameVersion);
        }

        public byte[] Riot => m_riot;
        public byte[] Version => m_version;
        public string GameVersion => m_gameVersion;
        public ReplayMetadata Metadata => m_metadata;
    }
}
