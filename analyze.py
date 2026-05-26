"""
analyze.py — ROFL2 keyframe / chunk binary analysis tool

Usage:
    python analyze.py <path-to-keyframe-or-chunk.bin>

The .bin files are produced by ParseReplayFile alongside the .rofl file,
e.g. NA1-5546423786/keyframe_0001.bin
"""

import sys
import struct
import collections

# ---------------------------------------------------------------------------
# Champion ID table  (DDragon internal id → numeric key)
# ---------------------------------------------------------------------------
CHAMPION_IDS = {
    "Annie": 1, "Olaf": 2, "Galio": 3, "TwistedFate": 4, "XinZhao": 5,
    "Urgot": 6, "Leblanc": 7, "Vladimir": 8, "FiddleSticks": 9, "Kayle": 10,
    "MasterYi": 11, "Alistar": 12, "Ryze": 13, "Sion": 14, "Sivir": 15,
    "Soraka": 16, "Teemo": 17, "Tristana": 18, "Warwick": 19, "Nunu": 20,
    "MissFortune": 21, "Ashe": 22, "Tryndamere": 23, "Jax": 24,
    "Morgana": 25, "Zilean": 26, "Singed": 27, "Evelynn": 28, "Twitch": 29,
    "Karthus": 30, "Chogath": 31, "Amumu": 32, "Rammus": 33, "Anivia": 34,
    "Shaco": 35, "DrMundo": 36, "Sona": 37, "Kassadin": 38, "Irelia": 39,
    "Janna": 40, "Gangplank": 41, "Corki": 42, "Karma": 43, "Taric": 44,
    "Veigar": 45, "Trundle": 48, "Swain": 50, "Caitlyn": 51,
    "Blitzcrank": 53, "Malphite": 54, "Katarina": 55, "Nocturne": 56,
    "Maokai": 57, "Renekton": 58, "JarvanIV": 59, "Elise": 60,
    "Orianna": 61, "MonkeyKing": 62, "Brand": 63, "LeeSin": 64, "Vayne": 67,
    "Rumble": 68, "Cassiopeia": 69, "Skarner": 72, "Heimerdinger": 74,
    "Nasus": 75, "Nidalee": 76, "Udyr": 77, "Poppy": 78, "Gragas": 79,
    "Pantheon": 80, "Ezreal": 81, "Mordekaiser": 82, "Yorick": 83,
    "Akali": 84, "Kennen": 85, "Garen": 86, "Leona": 89, "Malzahar": 90,
    "Talon": 91, "Riven": 92, "KogMaw": 96, "Shen": 98, "Lux": 99,
    "Xerath": 101, "Shyvana": 102, "Ahri": 103, "Graves": 104, "Fizz": 105,
    "Volibear": 106, "Rengar": 107, "Varus": 110, "Nautilus": 111,
    "Viktor": 112, "Sejuani": 113, "Fiora": 114, "Ziggs": 115, "Lulu": 117,
    "Draven": 119, "Hecarim": 120, "Khazix": 121, "Darius": 122,
    "Jayce": 126, "Lissandra": 127, "Diana": 131, "Quinn": 133,
    "Syndra": 134, "AurelionSol": 136, "Kayn": 141, "Zoe": 142, "Zyra": 143,
    "Kaisa": 145, "Seraphine": 147, "Gnar": 150, "Zac": 154, "Yasuo": 157,
    "Velkoz": 161, "Taliyah": 163, "Camille": 164, "Akshan": 166,
    "Belveth": 200, "Braum": 201, "Jhin": 202, "Kindred": 203, "Zeri": 221,
    "Jinx": 222, "TahmKench": 223, "Briar": 233, "Viego": 234, "Senna": 235,
    "Lucian": 236, "Zed": 238, "Kled": 240, "Ekko": 245, "Qiyana": 246,
    "Vi": 254, "Aatrox": 266, "Nami": 267, "Azir": 268, "Yuumi": 350,
    "Samira": 360, "Thresh": 412, "Illaoi": 420, "RekSai": 421,
    "Ivern": 427, "Kalista": 429, "Bard": 432, "Rakan": 497, "Xayah": 498,
    "Ornn": 516, "Sylas": 517, "Neeko": 518, "Aphelios": 523, "Rell": 526,
    "Pyke": 555, "Vex": 711, "Yone": 777, "Ambessa": 799, "Mel": 800,
    "Sett": 875, "Lillia": 876, "Gwen": 887, "Renata": 888, "Aurora": 893,
    "Nilah": 895, "KSante": 897, "Smolder": 901, "Milio": 902, "Hwei": 910,
    "Naafiri": 950,
}

# ---------------------------------------------------------------------------
# Helpers
# ---------------------------------------------------------------------------

def hexline(data: bytes, offset: int, width: int = 16) -> str:
    chunk = data[offset:offset + width]
    hex_part  = " ".join(f"{b:02X}" for b in chunk)
    ascii_part = "".join(chr(b) if 32 <= b <= 126 else "." for b in chunk)
    return f"  {offset:04X}: {hex_part:<{width*3}}  {ascii_part}"

def hexdump(data: bytes, max_bytes: int = 256):
    for i in range(0, min(len(data), max_bytes), 16):
        print(hexline(data, i))
    if len(data) > max_bytes:
        print(f"  ... ({len(data)} bytes total)")

def find_all(data: bytes, pattern: bytes) -> list[int]:
    offsets, start = [], 0
    while True:
        idx = data.find(pattern, start)
        if idx == -1:
            break
        offsets.append(idx)
        start = idx + 1
    return offsets

def context(data: bytes, offset: int, pattern_len: int, before: int = 4, after: int = 8) -> str:
    start = max(0, offset - before)
    end   = min(len(data), offset + pattern_len + after)
    chunk = data[start:end]
    return " ".join(f"{b:02X}" for b in chunk)

# ---------------------------------------------------------------------------
# Analysis passes
# ---------------------------------------------------------------------------

def load_players(bin_path: str) -> dict[str, int]:
    """
    Load players.json from the same folder as the .bin file.
    Returns {champion_name: champion_id} for the 10 players in this game.
    Falls back to all 163 champions if players.json is not found.
    """
    import os, json
    players_path = os.path.join(os.path.dirname(bin_path), "players.json")
    if os.path.exists(players_path):
        with open(players_path) as f:
            records = json.load(f)
        result = {}
        for r in records:
            skin = r.get("skin") or ""
            cid  = r.get("championId", -1)
            if skin and cid != -1:
                result[skin] = cid
        print(f"  Loaded {len(result)} champions from players.json:")
        for name, cid in result.items():
            print(f"    {name} (ID {cid})")
        return result
    else:
        print("  players.json not found — searching all 163 champion IDs")
        return CHAMPION_IDS


def search_champion_ids(data: bytes, bin_path: str):
    """Search for every champion ID stored as uint16-LE and uint32-LE."""
    print("\n=== Champion ID search ===")
    champs = load_players(bin_path)

    for name, cid in sorted(champs.items(), key=lambda x: x[1]):
        u16 = struct.pack("<H", cid)
        u32 = struct.pack("<I", cid)
        hits16 = find_all(data, u16)
        hits32 = find_all(data, u32)

        print(f"\n  {name} (ID {cid})")
        for label, hits, plen in [("uint16-LE", hits16, 2), ("uint32-LE", hits32, 4)]:
            if not hits:
                print(f"    {label}: no matches")
            else:
                preview = ", ".join(f"0x{o:X}" for o in hits[:20])
                suffix  = " ..." if len(hits) > 20 else ""
                print(f"    {label}: {len(hits)} hit(s) at {preview}{suffix}")
                for off in hits[:3]:
                    print(f"      [0x{off:04X}]  {context(data, off, plen)}")


def find_repeating_gaps(data: bytes):
    """
    Look for the most common gap between identical 4-byte sequences.
    If the format uses fixed-size records, the same 4-byte field value will
    repeat at a consistent stride — that stride is the record size.
    """
    print("\n=== Repeating 4-byte pattern analysis ===")
    gap_counts: dict[int, int] = collections.Counter()

    # Scan every 4-byte window, find all occurrences, record gaps
    seen: dict[bytes, list[int]] = collections.defaultdict(list)
    for i in range(len(data) - 3):
        word = data[i:i+4]
        # Skip all-zero and all-FF words — too common to be useful
        if word in (b'\x00\x00\x00\x00', b'\xFF\xFF\xFF\xFF', b'\xFE\xFE\xFE\xFE'):
            continue
        seen[word].append(i)

    for word, offsets in seen.items():
        if len(offsets) < 3:
            continue
        for a, b in zip(offsets, offsets[1:]):
            gap = b - a
            if 8 <= gap <= 8192:   # plausible record sizes
                gap_counts[gap] += 1

    print("  Most common gaps between repeated 4-byte values (possible record sizes):")
    for gap, count in sorted(gap_counts.items(), key=lambda x: -x[1])[:20]:
        print(f"    gap={gap:5d} (0x{gap:04X})  seen {count} times")


def search_known_stats(data: bytes, bin_path: str):
    """
    Search for each player's known end-game stats as raw bytes.
    Gold is the most distinctive (large, unique uint32).
    Level, kills, deaths are smaller but useful for confirmation.
    Best used against the LAST keyframe where values match the final stats.
    """
    import os, json
    players_path = os.path.join(os.path.dirname(bin_path), "players.json")
    if not os.path.exists(players_path):
        print("  players.json not found — skipping")
        return

    with open(players_path) as f:
        players = json.load(f)

    print("\n=== Known end-game stats search ===")
    print("  (Best run against the last keyframe)\n")

    for p in players:
        name = p.get("name", "?")
        skin = p.get("skin", "?")
        gold = p.get("goldEarned", -1)
        spent = p.get("goldSpent", -1)
        level = p.get("level", -1)
        kills = p.get("kills", -1)
        deaths = p.get("deaths", -1)
        assists = p.get("assists", -1)
        cs = p.get("minionsKilled", -1)

        print(f"  {name} ({skin})")
        print(f"    gold={gold}  spent={spent}  level={level}  "
              f"kda={kills}/{deaths}/{assists}  cs={cs}")

        # Gold earned — uint32 LE, most distinctive
        if gold > 0:
            hits = find_all(data, struct.pack("<I", gold))
            if hits:
                print(f"    goldEarned uint32 ({gold}): {len(hits)} hit(s)")
                for off in hits[:5]:
                    print(f"      [0x{off:05X}]  {context(data, off, 4, before=8, after=12)}")
            else:
                print(f"    goldEarned uint32 ({gold}): no matches")

        # Gold spent — uint32 LE
        if spent > 0:
            hits = find_all(data, struct.pack("<I", spent))
            if hits:
                print(f"    goldSpent  uint32 ({spent}): {len(hits)} hit(s)")
                for off in hits[:3]:
                    print(f"      [0x{off:05X}]  {context(data, off, 4, before=8, after=12)}")

        # Level — uint8 and uint16 LE (small so many false positives, show count only)
        if 1 <= level <= 18:
            hits8  = find_all(data, bytes([level]))
            hits16 = find_all(data, struct.pack("<H", level))
            print(f"    level uint8  ({level}): {len(hits8)} hit(s)  |  "
                  f"uint16 ({level}): {len(hits16)} hit(s)")

        # Kills / Deaths / Assists as uint8 and uint16
        for label, val in [("kills", kills), ("deaths", deaths), ("assists", assists)]:
            if val >= 0:
                hits = find_all(data, struct.pack("<H", val))
                print(f"    {label:7s} uint16 ({val:3d}): {len(hits):5d} hit(s)")

        print()


def scan_uint16_preceded_by_zeros(data: bytes):
    """
    Scan for every instance of 00 00 00 00 XX 00 where XX is non-zero.
    Leona and Malzahar confirmed this is where champion IDs live.
    This reveals what IDs actually appear in the file at that location,
    including any champions whose DDragon IDs might be wrong.
    """
    print("\n=== Scan for pattern: 00 00 00 00 [id_lo] 00 ===")
    id_offsets: dict[int, list[int]] = collections.defaultdict(list)

    for i in range(len(data) - 5):
        if (data[i]   == 0x00 and data[i+1] == 0x00 and
            data[i+2] == 0x00 and data[i+3] == 0x00 and
            data[i+4] != 0x00 and data[i+5] == 0x00):
            id_val = data[i+4]
            id_offsets[id_val].append(i + 4)   # offset of the ID byte itself

    # Build reverse map for labelling
    id_to_name = {v: k for k, v in CHAMPION_IDS.items()}

    print(f"  Found {len(id_offsets)} distinct ID values at this pattern:")
    for id_val in sorted(id_offsets):
        name   = id_to_name.get(id_val, "???")
        offs   = id_offsets[id_val]
        preview = ", ".join(f"0x{o:X}" for o in offs[:10])
        suffix  = " ..." if len(offs) > 10 else ""
        print(f"    id=0x{id_val:02X} ({id_val:3d}) [{name:>14}]  {len(offs):3d} hit(s)  @ {preview}{suffix}")


def float_scan(data: bytes, lo: float = 0.0, hi: float = 16000.0):
    """
    Scan for 4-byte IEEE 754 floats in a plausible game-coordinate range.
    League map is roughly 0–15000 in X and Y.
    """
    print(f"\n=== Float scan ({lo}–{hi}) ===")
    hits = []
    for i in range(0, len(data) - 3, 1):
        val = struct.unpack_from("<f", data, i)[0]
        if lo <= val <= hi:
            hits.append((i, val))

    print(f"  {len(hits)} floats in range (showing first 40)")
    for off, val in hits[:40]:
        print(f"    [0x{off:04X}]  {val:10.3f}   raw: {context(data, off, 4, before=0, after=0)}")


# ---------------------------------------------------------------------------
# Main
# ---------------------------------------------------------------------------

def main():
    if len(sys.argv) < 2:
        print("Usage: python analyze.py <file.bin>")
        sys.exit(1)

    path = sys.argv[1]
    with open(path, "rb") as f:
        data = f.read()

    print(f"File: {path}  ({len(data):,} bytes)")
    print("\n=== First 256 bytes ===")
    hexdump(data, 256)

    search_champion_ids(data, path)
    search_known_stats(data, path)
    scan_uint16_preceded_by_zeros(data)
    find_repeating_gaps(data)
    float_scan(data)


if __name__ == "__main__":
    main()
