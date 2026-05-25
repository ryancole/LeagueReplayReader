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

                // skip 9 unknown header bytes (offsets 6-14) to reach the game version field
                p_stream.Seek(9, SeekOrigin.Current);

                // game version is a fixed 14-byte field starting at offset 15
                m_gameVersion = Encoding.UTF8.GetString(r.ReadBytes(14)).TrimEnd('\0', '\x01');

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
