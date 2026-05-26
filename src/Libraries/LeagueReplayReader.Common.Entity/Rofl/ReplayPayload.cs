using System.Text;

namespace LeagueReplayReader.Common.Entity.Rofl
{
    public class ReplayPayload
    {
        private readonly List<Rofl2PayloadEntry> m_entries = new();

        public ReplayPayload(FileStream stream, long payloadStartOffset, long metadataStartOffset)
        {
            // The payload section starts right after the ROFL2 header (after the game version string).
            // Entries are laid out sequentially: 17-byte header + Zstd-compressed data block.
            // For type=3 (startup) entries, an undocumented block of bytes follows the Zstd data
            // before the next entry; we detect this by scanning for the next valid entry header.
            stream.Seek(payloadStartOffset, SeekOrigin.Begin);

            using var reader = new BinaryReader(stream, Encoding.UTF8, leaveOpen: true);

            while (stream.Position < metadataStartOffset)
            {
                long entryStart = stream.Position;

                var entry = Rofl2PayloadEntry.TryRead(reader);
                if (entry == null)
                {
                    // Could not parse a valid entry at this position; advance one byte and retry.
                    // This handles the undocumented gap bytes after startup entries.
                    stream.Seek(entryStart + 1, SeekOrigin.Begin);

                    if (stream.Position >= metadataStartOffset)
                        break;

                    continue;
                }

                m_entries.Add(entry);
            }
        }

        public IReadOnlyList<Rofl2PayloadEntry> Entries => m_entries;

        public IEnumerable<Rofl2PayloadEntry> Chunks =>
            m_entries.Where(e => e.EntryType == Rofl2EntryType.Chunk);

        public IEnumerable<Rofl2PayloadEntry> Keyframes =>
            m_entries.Where(e => e.EntryType == Rofl2EntryType.Keyframe);

        public IEnumerable<Rofl2PayloadEntry> StartupEntries =>
            m_entries.Where(e => e.EntryType == Rofl2EntryType.Startup);
    }
}
