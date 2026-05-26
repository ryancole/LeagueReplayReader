namespace LeagueReplayReader.Common.Entity.Rofl
{
    public static class KeyframeParser
    {
        // Block layout (leaguespec wiki "General Binary Format"):
        //   marker + [time] + [contentlength] + (type) + [blockparam] + [blockcontent]
        //
        // marker byte flags (high to low):
        //   0x80 = time is 1-byte uint8 ms delta (else 4-byte float seconds)
        //   0x40 = type omitted, same as previous block (else type byte follows content length)
        //   0x20 = blockparam is 1-byte delta relative to previous (else 4-byte absolute uint32)
        //   0x10 = content length is 1 byte (else 4 bytes)
        //   0x0F = channel
        public static IReadOnlyList<KeyframeBlock> Parse(byte[] data)
        {
            var blocks = new List<KeyframeBlock>();
            using var ms = new MemoryStream(data);
            using var reader = new BinaryReader(ms);

            float lastTime = 0f;
            uint lastBlockParam = 0;
            ushort lastType = 0;

            while (ms.Position < ms.Length)
            {
                byte marker;
                try { marker = reader.ReadByte(); }
                catch { break; }

                bool timeIs1Byte = (marker & 0x80) != 0;
                bool noType = (marker & 0x40) != 0;
                bool paramIs1Byte = (marker & 0x20) != 0;
                bool lenIs1Byte = (marker & 0x10) != 0;
                var channel = (BlockChannel)(marker & 0x0F);

                float timeSeconds;
                try
                {
                    timeSeconds = timeIs1Byte
                        ? lastTime + reader.ReadByte() / 1000f
                        : reader.ReadSingle();
                }
                catch { break; }

                int contentLength;
                try
                {
                    contentLength = lenIs1Byte ? reader.ReadByte() : (int)reader.ReadUInt32();
                }
                catch { break; }

                ushort type;
                try
                {
                    type = noType ? lastType : reader.ReadByte();
                }
                catch { break; }

                uint blockParam;
                try
                {
                    blockParam = paramIs1Byte
                        ? lastBlockParam + reader.ReadByte()
                        : reader.ReadUInt32();
                }
                catch { break; }

                byte[] content;
                try
                {
                    content = reader.ReadBytes(contentLength);
                    if (content.Length < contentLength)
                        break;
                }
                catch { break; }

                // 0xFE means extended type: real 2-byte type is the first 2 bytes of content
                if (type == 0xFE && content.Length >= 2)
                    type = BitConverter.ToUInt16(content, 0);

                lastTime = timeSeconds;
                lastBlockParam = blockParam;
                lastType = type;

                blocks.Add(new KeyframeBlock
                {
                    Channel = channel,
                    TimeSeconds = timeSeconds,
                    PacketType = type,
                    BlockParam = blockParam,
                    Content = content,
                });
            }

            return blocks;
        }
    }
}
