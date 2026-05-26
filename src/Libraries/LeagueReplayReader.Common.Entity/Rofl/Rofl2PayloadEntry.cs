using System.Text;
using ZstdSharp;

namespace LeagueReplayReader.Common.Entity.Rofl
{
    public enum Rofl2EntryType : byte
    {
        Chunk = 1,
        Keyframe = 2,
        Startup = 3,
    }

    public class Rofl2PayloadEntry
    {
        private readonly uint m_id;
        private readonly uint m_nextChunkId;
        private readonly Rofl2EntryType m_type;
        private readonly uint m_uncompressedLength;
        private readonly uint m_compressedLength;
        private readonly byte[] m_data;

        private static readonly byte[] ZstdMagic = [0x28, 0xB5, 0x2F, 0xFD];

        public static Rofl2PayloadEntry? TryRead(BinaryReader reader)
        {
            // Each entry has a 17-byte header followed by a Zstd-compressed data block.
            // Header layout (all little-endian):
            //   [0:4]  uint32 = entry ID
            //   [4:8]  uint32 = nextChunkId
            //   [8]    byte   = entry type (1=chunk, 2=keyframe, 3=startup)
            //   [9:13] uint32 = uncompressed size
            //   [13:17] uint32 = compressed size (exact Zstd stream length)
            const int headerSize = 17;

            byte[] header;
            try { header = reader.ReadBytes(headerSize); }
            catch { return null; }

            if (header.Length < headerSize)
                return null;

            var id = BitConverter.ToUInt32(header, 0);
            var nextChunkId = BitConverter.ToUInt32(header, 4);
            var typeByte = header[8];
            var uncompressedLength = BitConverter.ToUInt32(header, 9);
            var compressedLength = BitConverter.ToUInt32(header, 13);

            if (!Enum.IsDefined(typeof(Rofl2EntryType), typeByte))
                return null;

            var compressed = reader.ReadBytes((int)compressedLength);
            if (compressed.Length < (int)compressedLength)
                return null;

            // Validate Zstd magic at start of compressed data
            if (compressed.Length < 4 ||
                compressed[0] != ZstdMagic[0] || compressed[1] != ZstdMagic[1] ||
                compressed[2] != ZstdMagic[2] || compressed[3] != ZstdMagic[3])
                return null;

            var data = Decompress(compressed, (int)uncompressedLength);

            return new Rofl2PayloadEntry(id, nextChunkId, (Rofl2EntryType)typeByte, uncompressedLength, compressedLength, data);
        }

        private Rofl2PayloadEntry(uint id, uint nextChunkId, Rofl2EntryType type, uint uncompressedLength, uint compressedLength, byte[] data)
        {
            m_id = id;
            m_nextChunkId = nextChunkId;
            m_type = type;
            m_uncompressedLength = uncompressedLength;
            m_compressedLength = compressedLength;
            m_data = data;
        }

        private static byte[] Decompress(byte[] compressed, int expectedSize)
        {
            using var decompressor = new Decompressor();
            var output = new byte[expectedSize];
            decompressor.Unwrap(compressed, output);
            return output;
        }

        public override string ToString()
        {
            return string.Format("<Rofl2PayloadEntry id={0} type={1} comp={2} uncomp={3}>",
                m_id, m_type, m_compressedLength, m_uncompressedLength);
        }

        public uint Id => m_id;
        public uint NextChunkId => m_nextChunkId;
        public Rofl2EntryType EntryType => m_type;
        public uint UncompressedLength => m_uncompressedLength;
        public uint CompressedLength => m_compressedLength;
        public byte[] Data => m_data;
    }
}
