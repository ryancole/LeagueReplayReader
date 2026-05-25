using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LeagueReplayReader.Common.Entity.Rofl
{
    public class PlayerStats
    {
        public static PlayerStats[] Deserialize(string json)
        {
            return JsonSerializer.Deserialize<PlayerStats[]>(json)
                ?? throw new InvalidDataException("Failed to deserialize player stats JSON");
        }

        #region Player Identity

        [JsonPropertyName("ID")]
        public string? Id { get; set; }

        [JsonPropertyName("NAME")]
        public string? Name { get; set; }

        [JsonPropertyName("PUUID")]
        public string? Puuid { get; set; }

        [JsonPropertyName("SUMMONER_ID")]
        public string? SummonerId { get; set; }

        [JsonPropertyName("RIOT_ID_GAME_NAME")]
        public string? RiotIdGameName { get; set; }

        [JsonPropertyName("RIOT_ID_TAG_LINE")]
        public string? RiotIdTagLine { get; set; }

        [JsonPropertyName("SKIN")]
        public string? Skin { get; set; }

        [JsonPropertyName("PING")]
        public string? Ping { get; set; }

        #endregion

        #region Team & Position

        [JsonPropertyName("TEAM")]
        public string? Team { get; set; }

        [JsonPropertyName("WIN")]
        public string? Win { get; set; }

        [JsonPropertyName("PLAYER_ROLE")]
        public string? PlayerRole { get; set; }

        [JsonPropertyName("PLAYER_POSITION")]
        public string? PlayerPosition { get; set; }

        [JsonPropertyName("INDIVIDUAL_POSITION")]
        public string? IndividualPosition { get; set; }

        [JsonPropertyName("TEAM_POSITION")]
        public string? TeamPosition { get; set; }

        [JsonPropertyName("PLAYER_SUBTEAM")]
        public string? PlayerSubteam { get; set; }

        [JsonPropertyName("PLAYER_SUBTEAM_PLACEMENT")]
        public string? PlayerSubteamPlacement { get; set; }

        #endregion

        #region Combat

        [JsonPropertyName("CHAMPIONS_KILLED")]
        public string? ChampionsKilled { get; set; }

        [JsonPropertyName("NUM_DEATHS")]
        public string? Deaths { get; set; }

        [JsonPropertyName("ASSISTS")]
        public string? Assists { get; set; }

        [JsonPropertyName("KILLING_SPREES")]
        public string? KillingSprees { get; set; }

        [JsonPropertyName("LARGEST_KILLING_SPREE")]
        public string? LargestKillingSpree { get; set; }

        [JsonPropertyName("DOUBLE_KILLS")]
        public string? DoubleKills { get; set; }

        [JsonPropertyName("TRIPLE_KILLS")]
        public string? TripleKills { get; set; }

        [JsonPropertyName("QUADRA_KILLS")]
        public string? QuadraKills { get; set; }

        [JsonPropertyName("PENTA_KILLS")]
        public string? PentaKills { get; set; }

        [JsonPropertyName("UNREAL_KILLS")]
        public string? UnrealKills { get; set; }

        [JsonPropertyName("LARGEST_MULTI_KILL")]
        public string? LargestMultiKill { get; set; }

        [JsonPropertyName("LAST_TAKEDOWN_TIME")]
        public string? LastTakedownTime { get; set; }

        [JsonPropertyName("TURRET_TAKEDOWNS")]
        public string? TurretTakedowns { get; set; }

        [JsonPropertyName("BARRACKS_TAKEDOWNS")]
        public string? BarracksTakedowns { get; set; }

        [JsonPropertyName("HQ_TAKEDOWNS")]
        public string? HqTakedowns { get; set; }

        #endregion

        #region Damage Dealt

        [JsonPropertyName("TOTAL_DAMAGE_DEALT")]
        public string? TotalDamageDealt { get; set; }

        [JsonPropertyName("TOTAL_DAMAGE_DEALT_TO_CHAMPIONS")]
        public string? TotalDamageDealtToChampions { get; set; }

        [JsonPropertyName("PHYSICAL_DAMAGE_DEALT_PLAYER")]
        public string? PhysicalDamageDealt { get; set; }

        [JsonPropertyName("PHYSICAL_DAMAGE_DEALT_TO_CHAMPIONS")]
        public string? PhysicalDamageDealtToChampions { get; set; }

        [JsonPropertyName("MAGIC_DAMAGE_DEALT_PLAYER")]
        public string? MagicDamageDealt { get; set; }

        [JsonPropertyName("MAGIC_DAMAGE_DEALT_TO_CHAMPIONS")]
        public string? MagicDamageDealtToChampions { get; set; }

        [JsonPropertyName("TRUE_DAMAGE_DEALT_PLAYER")]
        public string? TrueDamageDealt { get; set; }

        [JsonPropertyName("TRUE_DAMAGE_DEALT_TO_CHAMPIONS")]
        public string? TrueDamageDealtToChampions { get; set; }

        [JsonPropertyName("LARGEST_ABILITY_DAMAGE")]
        public string? LargestAbilityDamage { get; set; }

        [JsonPropertyName("LARGEST_ATTACK_DAMAGE")]
        public string? LargestAttackDamage { get; set; }

        [JsonPropertyName("LARGEST_CRITICAL_STRIKE")]
        public string? LargestCriticalStrike { get; set; }

        [JsonPropertyName("TOTAL_DAMAGE_DEALT_TO_BUILDINGS")]
        public string? TotalDamageDealtToBuildings { get; set; }

        [JsonPropertyName("TOTAL_DAMAGE_DEALT_TO_TURRETS")]
        public string? TotalDamageDealtToTurrets { get; set; }

        [JsonPropertyName("TOTAL_DAMAGE_DEALT_TO_EPIC_MONSTERS")]
        public string? TotalDamageDealtToEpicMonsters { get; set; }

        [JsonPropertyName("TOTAL_DAMAGE_DEALT_TO_OBJECTIVES")]
        public string? TotalDamageDealtToObjectives { get; set; }

        #endregion

        #region Damage Taken & Mitigation

        [JsonPropertyName("TOTAL_DAMAGE_TAKEN")]
        public string? TotalDamageTaken { get; set; }

        [JsonPropertyName("PHYSICAL_DAMAGE_TAKEN")]
        public string? PhysicalDamageTaken { get; set; }

        [JsonPropertyName("MAGIC_DAMAGE_TAKEN")]
        public string? MagicDamageTaken { get; set; }

        [JsonPropertyName("TRUE_DAMAGE_TAKEN")]
        public string? TrueDamageTaken { get; set; }

        [JsonPropertyName("TOTAL_DAMAGE_SELF_MITIGATED")]
        public string? TotalDamageSelfMitigated { get; set; }

        [JsonPropertyName("TOTAL_DAMAGE_SHIELDED_ON_TEAMMATES")]
        public string? TotalDamageShieldedOnTeammates { get; set; }

        #endregion

        #region Healing

        [JsonPropertyName("TOTAL_HEAL")]
        public string? TotalHeal { get; set; }

        [JsonPropertyName("TOTAL_HEAL_ON_TEAMMATES")]
        public string? TotalHealOnTeammates { get; set; }

        [JsonPropertyName("TOTAL_UNITS_HEALED")]
        public string? TotalUnitsHealed { get; set; }

        #endregion

        #region Economy

        [JsonPropertyName("GOLD_EARNED")]
        public string? GoldEarned { get; set; }

        [JsonPropertyName("GOLD_SPENT")]
        public string? GoldSpent { get; set; }

        [JsonPropertyName("EXP")]
        public string? Exp { get; set; }

        [JsonPropertyName("LEVEL")]
        public string? Level { get; set; }

        #endregion

        #region Farming

        [JsonPropertyName("MINIONS_KILLED")]
        public string? MinionsKilled { get; set; }

        [JsonPropertyName("NEUTRAL_MINIONS_KILLED")]
        public string? NeutralMinionsKilled { get; set; }

        [JsonPropertyName("NEUTRAL_MINIONS_KILLED_ENEMY_JUNGLE")]
        public string? NeutralMinionsKilledEnemyJungle { get; set; }

        [JsonPropertyName("NEUTRAL_MINIONS_KILLED_YOUR_JUNGLE")]
        public string? NeutralMinionsKilledYourJungle { get; set; }

        #endregion

        #region Objectives

        [JsonPropertyName("TURRETS_KILLED")]
        public string? TurretsKilled { get; set; }

        [JsonPropertyName("BARRACKS_KILLED")]
        public string? BarracksKilled { get; set; }

        [JsonPropertyName("HQ_KILLED")]
        public string? HqKilled { get; set; }

        [JsonPropertyName("FRIENDLY_TURRET_LOST")]
        public string? FriendlyTurretLost { get; set; }

        [JsonPropertyName("FRIENDLY_DAMPEN_LOST")]
        public string? FriendlyDampenLost { get; set; }

        [JsonPropertyName("FRIENDLY_HQ_LOST")]
        public string? FriendlyHqLost { get; set; }

        [JsonPropertyName("DRAGON_KILLS")]
        public string? DragonKills { get; set; }

        [JsonPropertyName("BARON_KILLS")]
        public string? BaronKills { get; set; }

        [JsonPropertyName("RIFT_HERALD_KILLS")]
        public string? RiftHeraldKills { get; set; }

        [JsonPropertyName("HORDE_KILLS")]
        public string? HordeKills { get; set; }

        [JsonPropertyName("ATAKHAN_KILLS")]
        public string? AtakhanKills { get; set; }

        [JsonPropertyName("OBJECTIVES_STOLEN")]
        public string? ObjectivesStolen { get; set; }

        [JsonPropertyName("OBJECTIVES_STOLEN_ASSISTS")]
        public string? ObjectivesStolenAssists { get; set; }

        [JsonPropertyName("NODE_CAPTURE")]
        public string? NodeCapture { get; set; }

        [JsonPropertyName("NODE_CAPTURE_ASSIST")]
        public string? NodeCaptureAssist { get; set; }

        [JsonPropertyName("NODE_NEUTRALIZE")]
        public string? NodeNeutralize { get; set; }

        [JsonPropertyName("NODE_NEUTRALIZE_ASSIST")]
        public string? NodeNeutralizeAssist { get; set; }

        [JsonPropertyName("TEAM_OBJECTIVE")]
        public string? TeamObjective { get; set; }

        #endregion

        #region Vision

        [JsonPropertyName("VISION_SCORE")]
        public string? VisionScore { get; set; }

        [JsonPropertyName("SIGHT_WARDS_BOUGHT_IN_GAME")]
        public string? SightWardsBought { get; set; }

        [JsonPropertyName("VISION_WARDS_BOUGHT_IN_GAME")]
        public string? VisionWardsBought { get; set; }

        [JsonPropertyName("WARD_PLACED")]
        public string? WardPlaced { get; set; }

        [JsonPropertyName("WARD_PLACED_DETECTOR")]
        public string? WardPlacedDetector { get; set; }

        [JsonPropertyName("WARD_KILLED")]
        public string? WardKilled { get; set; }

        #endregion

        #region Crowd Control

        [JsonPropertyName("TIME_CCING_OTHERS")]
        public string? TimeCcingOthers { get; set; }

        [JsonPropertyName("TOTAL_TIME_CROWD_CONTROL_DEALT")]
        public string? TotalTimeCrowdControlDealt { get; set; }

        [JsonPropertyName("TOTAL_TIME_CROWD_CONTROL_DEALT_TO_CHAMPIONS")]
        public string? TotalTimeCrowdControlDealtToChampions { get; set; }

        #endregion

        #region Time

        [JsonPropertyName("TIME_PLAYED")]
        public string? TimePlayed { get; set; }

        [JsonPropertyName("TOTAL_TIME_SPENT_DEAD")]
        public string? TotalTimeSpentDead { get; set; }

        [JsonPropertyName("LONGEST_TIME_SPENT_LIVING")]
        public string? LongestTimeSpentLiving { get; set; }

        [JsonPropertyName("TIME_SPENT_DISCONNECTED")]
        public string? TimeSpentDisconnected { get; set; }

        [JsonPropertyName("TIME_OF_FROM_LAST_DISCONNECT")]
        public string? TimeOfFromLastDisconnect { get; set; }

        #endregion

        #region Pings

        [JsonPropertyName("ALL_IN_PINGS")]
        public string? AllInPings { get; set; }

        [JsonPropertyName("ASSIST_ME_PINGS")]
        public string? AssistMePings { get; set; }

        [JsonPropertyName("BASIC_PINGS")]
        public string? BasicPings { get; set; }

        [JsonPropertyName("COMMAND_PINGS")]
        public string? CommandPings { get; set; }

        [JsonPropertyName("DANGER_PINGS")]
        public string? DangerPings { get; set; }

        [JsonPropertyName("ENEMY_MISSING_PINGS")]
        public string? EnemyMissingPings { get; set; }

        [JsonPropertyName("ENEMY_VISION_PINGS")]
        public string? EnemyVisionPings { get; set; }

        [JsonPropertyName("GET_BACK_PINGS")]
        public string? GetBackPings { get; set; }

        [JsonPropertyName("HOLD_PINGS")]
        public string? HoldPings { get; set; }

        [JsonPropertyName("NEED_VISION_PINGS")]
        public string? NeedVisionPings { get; set; }

        [JsonPropertyName("ON_MY_WAY_PINGS")]
        public string? OnMyWayPings { get; set; }

        [JsonPropertyName("PUSH_PINGS")]
        public string? PushPings { get; set; }

        [JsonPropertyName("RETREAT_PINGS")]
        public string? RetreatPings { get; set; }

        [JsonPropertyName("VISION_CLEARED_PINGS")]
        public string? VisionClearedPings { get; set; }

        #endregion

        #region Items

        [JsonPropertyName("ITEM0")]
        public string? Item0 { get; set; }

        [JsonPropertyName("ITEM1")]
        public string? Item1 { get; set; }

        [JsonPropertyName("ITEM2")]
        public string? Item2 { get; set; }

        [JsonPropertyName("ITEM3")]
        public string? Item3 { get; set; }

        [JsonPropertyName("ITEM4")]
        public string? Item4 { get; set; }

        [JsonPropertyName("ITEM5")]
        public string? Item5 { get; set; }

        [JsonPropertyName("ITEM6")]
        public string? Item6 { get; set; }

        [JsonPropertyName("ITEMS_PURCHASED")]
        public string? ItemsPurchased { get; set; }

        [JsonPropertyName("CONSUMABLES_PURCHASED")]
        public string? ConsumablesPurchased { get; set; }

        [JsonPropertyName("ROLE_BOUND_ITEM")]
        public string? RoleBoundItem { get; set; }

        [JsonPropertyName("KEYSTONE_ID")]
        public string? KeystoneId { get; set; }

        #endregion

        #region Summoner Spells

        [JsonPropertyName("SUMMONER_SPELL_1")]
        public string? SummonerSpell1 { get; set; }

        [JsonPropertyName("SUMMONER_SPELL_2")]
        public string? SummonerSpell2 { get; set; }

        [JsonPropertyName("SPELL1_CAST")]
        public string? Spell1Cast { get; set; }

        [JsonPropertyName("SPELL2_CAST")]
        public string? Spell2Cast { get; set; }

        [JsonPropertyName("SPELL3_CAST")]
        public string? Spell3Cast { get; set; }

        [JsonPropertyName("SPELL4_CAST")]
        public string? Spell4Cast { get; set; }

        [JsonPropertyName("SUMMON_SPELL1_CAST")]
        public string? SummonSpell1Cast { get; set; }

        [JsonPropertyName("SUMMON_SPELL2_CAST")]
        public string? SummonSpell2Cast { get; set; }

        #endregion

        #region Runes & Perks

        [JsonPropertyName("PERK_PRIMARY_STYLE")]
        public string? PerkPrimaryStyle { get; set; }

        [JsonPropertyName("PERK_SUB_STYLE")]
        public string? PerkSubStyle { get; set; }

        [JsonPropertyName("PERK0")]
        public string? Perk0 { get; set; }

        [JsonPropertyName("PERK0_VAR1")]
        public string? Perk0Var1 { get; set; }

        [JsonPropertyName("PERK0_VAR2")]
        public string? Perk0Var2 { get; set; }

        [JsonPropertyName("PERK0_VAR3")]
        public string? Perk0Var3 { get; set; }

        [JsonPropertyName("PERK1")]
        public string? Perk1 { get; set; }

        [JsonPropertyName("PERK1_VAR1")]
        public string? Perk1Var1 { get; set; }

        [JsonPropertyName("PERK1_VAR2")]
        public string? Perk1Var2 { get; set; }

        [JsonPropertyName("PERK1_VAR3")]
        public string? Perk1Var3 { get; set; }

        [JsonPropertyName("PERK2")]
        public string? Perk2 { get; set; }

        [JsonPropertyName("PERK2_VAR1")]
        public string? Perk2Var1 { get; set; }

        [JsonPropertyName("PERK2_VAR2")]
        public string? Perk2Var2 { get; set; }

        [JsonPropertyName("PERK2_VAR3")]
        public string? Perk2Var3 { get; set; }

        [JsonPropertyName("PERK3")]
        public string? Perk3 { get; set; }

        [JsonPropertyName("PERK3_VAR1")]
        public string? Perk3Var1 { get; set; }

        [JsonPropertyName("PERK3_VAR2")]
        public string? Perk3Var2 { get; set; }

        [JsonPropertyName("PERK3_VAR3")]
        public string? Perk3Var3 { get; set; }

        [JsonPropertyName("PERK4")]
        public string? Perk4 { get; set; }

        [JsonPropertyName("PERK4_VAR1")]
        public string? Perk4Var1 { get; set; }

        [JsonPropertyName("PERK4_VAR2")]
        public string? Perk4Var2 { get; set; }

        [JsonPropertyName("PERK4_VAR3")]
        public string? Perk4Var3 { get; set; }

        [JsonPropertyName("PERK5")]
        public string? Perk5 { get; set; }

        [JsonPropertyName("PERK5_VAR1")]
        public string? Perk5Var1 { get; set; }

        [JsonPropertyName("PERK5_VAR2")]
        public string? Perk5Var2 { get; set; }

        [JsonPropertyName("PERK5_VAR3")]
        public string? Perk5Var3 { get; set; }

        [JsonPropertyName("STAT_PERK_0")]
        public string? StatPerk0 { get; set; }

        [JsonPropertyName("STAT_PERK_1")]
        public string? StatPerk1 { get; set; }

        [JsonPropertyName("STAT_PERK_2")]
        public string? StatPerk2 { get; set; }

        [JsonPropertyName("CHAMPION_MISSION_STAT_0")]
        public string? ChampionMissionStat0 { get; set; }

        [JsonPropertyName("CHAMPION_MISSION_STAT_1")]
        public string? ChampionMissionStat1 { get; set; }

        [JsonPropertyName("CHAMPION_MISSION_STAT_2")]
        public string? ChampionMissionStat2 { get; set; }

        [JsonPropertyName("CHAMPION_MISSION_STAT_3")]
        public string? ChampionMissionStat3 { get; set; }

        [JsonPropertyName("CHAMPION_TRANSFORM")]
        public string? ChampionTransform { get; set; }

        #endregion

        #region Player Scores

        [JsonPropertyName("PLAYER_SCORE_0")]
        public string? PlayerScore0 { get; set; }

        [JsonPropertyName("PLAYER_SCORE_1")]
        public string? PlayerScore1 { get; set; }

        [JsonPropertyName("PLAYER_SCORE_2")]
        public string? PlayerScore2 { get; set; }

        [JsonPropertyName("PLAYER_SCORE_3")]
        public string? PlayerScore3 { get; set; }

        [JsonPropertyName("PLAYER_SCORE_4")]
        public string? PlayerScore4 { get; set; }

        [JsonPropertyName("PLAYER_SCORE_5")]
        public string? PlayerScore5 { get; set; }

        [JsonPropertyName("PLAYER_SCORE_6")]
        public string? PlayerScore6 { get; set; }

        [JsonPropertyName("PLAYER_SCORE_7")]
        public string? PlayerScore7 { get; set; }

        [JsonPropertyName("PLAYER_SCORE_8")]
        public string? PlayerScore8 { get; set; }

        [JsonPropertyName("PLAYER_SCORE_9")]
        public string? PlayerScore9 { get; set; }

        [JsonPropertyName("PLAYER_SCORE_10")]
        public string? PlayerScore10 { get; set; }

        [JsonPropertyName("PLAYER_SCORE_11")]
        public string? PlayerScore11 { get; set; }

        [JsonPropertyName("VICTORY_POINT_TOTAL")]
        public string? VictoryPointTotal { get; set; }

        #endregion

        #region Arena Augments

        [JsonPropertyName("PLAYER_AUGMENT_1")]
        public string? PlayerAugment1 { get; set; }

        [JsonPropertyName("PLAYER_AUGMENT_2")]
        public string? PlayerAugment2 { get; set; }

        [JsonPropertyName("PLAYER_AUGMENT_3")]
        public string? PlayerAugment3 { get; set; }

        [JsonPropertyName("PLAYER_AUGMENT_4")]
        public string? PlayerAugment4 { get; set; }

        [JsonPropertyName("PLAYER_AUGMENT_5")]
        public string? PlayerAugment5 { get; set; }

        [JsonPropertyName("PLAYER_AUGMENT_6")]
        public string? PlayerAugment6 { get; set; }

        #endregion

        #region Behavior Flags

        [JsonPropertyName("WAS_AFK")]
        public string? WasAfk { get; set; }

        [JsonPropertyName("WAS_LEAVER")]
        public string? WasLeaver { get; set; }

        [JsonPropertyName("WAS_SURRENDER_DUE_TO_AFK")]
        public string? WasSurrenderDueToAfk { get; set; }

        [JsonPropertyName("WAS_AFK_AFTER_FAILED_SURRENDER")]
        public string? WasAfkAfterFailedSurrender { get; set; }

        [JsonPropertyName("WAS_EARLY_SURRENDER_ACCOMPLICE")]
        public string? WasEarlySurrenderAccomplice { get; set; }

        [JsonPropertyName("GAME_ENDED_IN_SURRENDER")]
        public string? GameEndedInSurrender { get; set; }

        [JsonPropertyName("GAME_ENDED_IN_EARLY_SURRENDER")]
        public string? GameEndedInEarlySurrender { get; set; }

        [JsonPropertyName("TEAM_EARLY_SURRENDERED")]
        public string? TeamEarlySurrendered { get; set; }

        [JsonPropertyName("MUTED_ALL")]
        public string? MutedAll { get; set; }

        [JsonPropertyName("PLAYERS_I_MUTED")]
        public string? PlayersIMuted { get; set; }

        [JsonPropertyName("PLAYERS_THAT_MUTED_ME")]
        public string? PlayersThatMutedMe { get; set; }

        [JsonPropertyName("PlayerBehavior_IsHeroInCombat")]
        public string? PlayerBehaviorIsHeroInCombat { get; set; }

        #endregion

        #region Season Missions & Events

        [JsonPropertyName("2026_S1A1_SR_FaerieWards")]
        public string? S1A1FaerieWards { get; set; }

        [JsonPropertyName("2026_S1A1_SR_GrowthSmashed")]
        public string? S1A1GrowthSmashed { get; set; }

        [JsonPropertyName("2026_S1A1_SR_RoleQuestComplete")]
        public string? S1A1RoleQuestComplete { get; set; }

        [JsonPropertyName("2026_S1A1_Skins_Ashe")]
        public string? S1A1SkinsAshe { get; set; }

        [JsonPropertyName("2026_S1A1_Skins_AurelionSol")]
        public string? S1A1SkinsAurelionSol { get; set; }

        [JsonPropertyName("2026_S1A1_Skins_Briar")]
        public string? S1A1SkinsBriar { get; set; }

        [JsonPropertyName("2026_S1A1_Skins_Caitlyn")]
        public string? S1A1SkinsCaitlyn { get; set; }

        [JsonPropertyName("2026_S1A1_Skins_Camille")]
        public string? S1A1SkinsCamille { get; set; }

        [JsonPropertyName("2026_S1A1_Skins_Galio")]
        public string? S1A1SkinsGalio { get; set; }

        [JsonPropertyName("2026_S1A1_Skins_Jayce")]
        public string? S1A1SkinsJayce { get; set; }

        [JsonPropertyName("2026_S1A1_Skins_Katarina")]
        public string? S1A1SkinsKatarina { get; set; }

        [JsonPropertyName("2026_S1A1_Skins_Lillia")]
        public string? S1A1SkinsLillia { get; set; }

        [JsonPropertyName("2026_S1A1_Skins_Nautilus")]
        public string? S1A1SkinsNautilus { get; set; }

        [JsonPropertyName("2026_S1A1_Skins_Ornn")]
        public string? S1A1SkinsOrnn { get; set; }

        [JsonPropertyName("2026_S1A1_Skins_Poppy")]
        public string? S1A1SkinsPoppy { get; set; }

        [JsonPropertyName("2026_S1A1_Skins_Samira")]
        public string? S1A1SkinsSamira { get; set; }

        [JsonPropertyName("2026_S1A1_Skins_Seraphine")]
        public string? S1A1SkinsSeraphine { get; set; }

        [JsonPropertyName("2026_S1A1_Skins_Viego")]
        public string? S1A1SkinsViego { get; set; }

        [JsonPropertyName("2026_S1A1_Skins_Yasuo")]
        public string? S1A1SkinsYasuo { get; set; }

        [JsonPropertyName("2026_S1A1_Skins_Yuumi")]
        public string? S1A1SkinsYuumi { get; set; }

        [JsonPropertyName("2026_S1A1_Skins_Ziggs")]
        public string? S1A1SkinsZiggs { get; set; }

        [JsonPropertyName("2026_S1A2_Shyvana_AOE")]
        public string? S1A2ShyvanaAoe { get; set; }

        [JsonPropertyName("2026_S1A2_Skins_Blitzcrank")]
        public string? S1A2SkinsBlitzcrank { get; set; }

        [JsonPropertyName("2026_S1A2_Skins_Maokai")]
        public string? S1A2SkinsMaokai { get; set; }

        [JsonPropertyName("2026_S1A2_Skins_Mordekaiser")]
        public string? S1A2SkinsMordekaiser { get; set; }

        [JsonPropertyName("2026_S1A2_Skins_Naafiri")]
        public string? S1A2SkinsNaafiri { get; set; }

        [JsonPropertyName("2026_S1A2_Skins_Sejuani")]
        public string? S1A2SkinsSejuani { get; set; }

        [JsonPropertyName("2026_S1A2_Skins_Senna")]
        public string? S1A2SkinsSenna { get; set; }

        [JsonPropertyName("2026_S1A2_Skins_Sivir")]
        public string? S1A2SkinsSivir { get; set; }

        [JsonPropertyName("2026_S1A2_Skins_Swain")]
        public string? S1A2SkinsSwain { get; set; }

        [JsonPropertyName("2026_S1A2_Skins_TahmKench")]
        public string? S1A2SkinsTahmKench { get; set; }

        [JsonPropertyName("2026_S1A2_Skins_Vex")]
        public string? S1A2SkinsVex { get; set; }

        [JsonPropertyName("2026_S1A2_Skins_Vladimir")]
        public string? S1A2SkinsVladimir { get; set; }

        [JsonPropertyName("2026_S1A2_Skins_Warwick")]
        public string? S1A2SkinsWarwick { get; set; }

        [JsonPropertyName("2026_S1A2_Skins_Xerath")]
        public string? S1A2SkinsXerath { get; set; }

        [JsonPropertyName("2026_S1A2_Skins_Zac")]
        public string? S1A2SkinsZac { get; set; }

        [JsonPropertyName("2026_S2A1_Event_Arena_BraveryChampions")]
        public string? S2A1EventArenaBraveryChampions { get; set; }

        [JsonPropertyName("2026_S2A1_Event_Arena_ReviveAllies")]
        public string? S2A1EventArenaReviveAllies { get; set; }

        [JsonPropertyName("2026_S2A1_Event_Arena_UsePowerFlowers")]
        public string? S2A1EventArenaUsePowerFlowers { get; set; }

        [JsonPropertyName("2026_S2A1_Event_ChampionHitFromBrush")]
        public string? S2A1EventChampionHitFromBrush { get; set; }

        [JsonPropertyName("2026_S2A1_Event_UseScryersBloom")]
        public string? S2A1EventUseScryersBloom { get; set; }

        [JsonPropertyName("2026_S2A2_Event_DealBurnDamage")]
        public string? S2A2EventDealBurnDamage { get; set; }

        [JsonPropertyName("2026_S2A2_Event_HitSnowballs")]
        public string? S2A2EventHitSnowballs { get; set; }

        [JsonPropertyName("2026_S2A2_Locke_BurstDamage")]
        public string? S2A2LockeBurstDamage { get; set; }

        [JsonPropertyName("ActMission_S1_A2_ArenaRoundsWon")]
        public string? ActMissionS1A2ArenaRoundsWon { get; set; }

        [JsonPropertyName("ActMission_S1_A2_BloodyPetalsCollected")]
        public string? ActMissionS1A2BloodyPetalsCollected { get; set; }

        [JsonPropertyName("ActMission_S1_A2_FeatsOfStrength")]
        public string? ActMissionS1A2FeatsOfStrength { get; set; }

        [JsonPropertyName("DemonsHand_MissionPointsA")]
        public string? DemonsHandMissionPointsA { get; set; }

        [JsonPropertyName("DemonsHand_MissionPointsB")]
        public string? DemonsHandMissionPointsB { get; set; }

        [JsonPropertyName("DemonsHand_MissionPointsC")]
        public string? DemonsHandMissionPointsC { get; set; }

        [JsonPropertyName("DemonsHand_MissionPointsD")]
        public string? DemonsHandMissionPointsD { get; set; }

        [JsonPropertyName("DemonsHand_MissionPointsE")]
        public string? DemonsHandMissionPointsE { get; set; }

        [JsonPropertyName("DemonsHand_MissionPointsF")]
        public string? DemonsHandMissionPointsF { get; set; }

        [JsonPropertyName("Event_2025LR_StructuresEpicMonsters")]
        public string? Event2025LRStructuresEpicMonsters { get; set; }

        [JsonPropertyName("Event_ARAM_Docks")]
        public string? EventAramDocks { get; set; }

        [JsonPropertyName("Event_ARAM_Hexgates")]
        public string? EventAramHexgates { get; set; }

        [JsonPropertyName("Event_Brawl_Jungle")]
        public string? EventBrawlJungle { get; set; }

        [JsonPropertyName("Event_Brawl_Minions")]
        public string? EventBrawlMinions { get; set; }

        [JsonPropertyName("Event_S1_A1_AprilFools_Dragon")]
        public string? EventS1A1AprilFoolsDragon { get; set; }

        [JsonPropertyName("Event_S1_A1_AprilFools_Snowball")]
        public string? EventS1A1AprilFoolsSnowball { get; set; }

        [JsonPropertyName("Event_S1_A2_AprilFools_Dragon")]
        public string? EventS1A2AprilFoolsDragon { get; set; }

        [JsonPropertyName("Event_S1_A2_AprilFools_Garen_Play")]
        public string? EventS1A2AprilFoolsGarenPlay { get; set; }

        [JsonPropertyName("Event_S1_A2_AprilFools_Garen_Takedown")]
        public string? EventS1A2AprilFoolsGarenTakedown { get; set; }

        [JsonPropertyName("Event_S1_A2_AprilFools_Snowball")]
        public string? EventS1A2AprilFoolsSnowball { get; set; }

        [JsonPropertyName("Event_S1_A2_Arena_BraveryChampions")]
        public string? EventS1A2ArenaBraveryChampions { get; set; }

        [JsonPropertyName("Event_S1_A2_Arena_NoxianChampions")]
        public string? EventS1A2ArenaNoxianChampions { get; set; }

        [JsonPropertyName("Event_S1_A2_Arena_ReviveAllies")]
        public string? EventS1A2ArenaReviveAllies { get; set; }

        [JsonPropertyName("Event_S1_A2_Esports_TakedownEpicMonstersSingleGame")]
        public string? EventS1A2EsportsTakedownEpicMonstersSingleGame { get; set; }

        [JsonPropertyName("Event_S1_A2_Mordekaiser")]
        public string? EventS1A2Mordekaiser { get; set; }

        [JsonPropertyName("Event_S2A2_Exalted")]
        public string? EventS2A2Exalted { get; set; }

        [JsonPropertyName("Event_S2A2_MV")]
        public string? EventS2A2Mv { get; set; }

        [JsonPropertyName("Event_S2A2_PetalPoints")]
        public string? EventS2A2PetalPoints { get; set; }

        [JsonPropertyName("Event_S2A2Champ_DamageAbilities")]
        public string? EventS2A2ChampDamageAbilities { get; set; }

        [JsonPropertyName("Event_S2A2Champ_DamageAutos")]
        public string? EventS2A2ChampDamageAutos { get; set; }

        [JsonPropertyName("HoL_ChampionsDamagedWhileHidden")]
        public string? HoLChampionsDamagedWhileHidden { get; set; }

        [JsonPropertyName("HoL_ControlWardsKilled")]
        public string? HoLControlWardsKilled { get; set; }

        [JsonPropertyName("HoL_Elite_AsheCrystalArrowTakedowns")]
        public string? HoLEliteAsheCrystalArrowTakedowns { get; set; }

        [JsonPropertyName("HoL_Elite_AsheHawkshotChampsRevealed")]
        public string? HoLEliteAsheHawkshotChampsRevealed { get; set; }

        [JsonPropertyName("HoL_Elite_EzrealEssenceFluxDetonated")]
        public string? HoLEliteEzrealEssenceFluxDetonated { get; set; }

        [JsonPropertyName("HoL_Elite_EzrealTrueshotBarrageMultiHit")]
        public string? HoLEliteEzrealTrueshotBarrageMultiHit { get; set; }

        [JsonPropertyName("HoL_Elite_KaiSaAbilitiesUpgraded")]
        public string? HoLEliteKaiSaAbilitiesUpgraded { get; set; }

        [JsonPropertyName("HoL_Elite_KaiSaKillerInstinctKills")]
        public string? HoLEliteKaiSaKillerInstinctKills { get; set; }

        [JsonPropertyName("HoL_Elite_LucianCullingHits")]
        public string? HoLEliteLucianCullingHits { get; set; }

        [JsonPropertyName("HoL_Elite_LucianPiercingLightMultiHit")]
        public string? HoLEliteLucianPiercingLightMultiHit { get; set; }

        [JsonPropertyName("HoL_Elite_VayneCondemnStun")]
        public string? HoLEliteVayneCondemnStun { get; set; }

        [JsonPropertyName("HoL_Elite_VayneTumbleDodge")]
        public string? HoLEliteVayneTumbleDodge { get; set; }

        [JsonPropertyName("HoL_EnemyTakedownUnderTower")]
        public string? HoLEnemyTakedownUnderTower { get; set; }

        [JsonPropertyName("HoL_FightsSurvivedWhileLowHealth")]
        public string? HoLFightsSurvivedWhileLowHealth { get; set; }

        [JsonPropertyName("HoL_HiddenEnemiesDamaged")]
        public string? HoLHiddenEnemiesDamaged { get; set; }

        [JsonPropertyName("HoL_JungleCampsStolen")]
        public string? HoLJungleCampsStolen { get; set; }

        [JsonPropertyName("HoL_KillsWhileLowHealth")]
        public string? HoLKillsWhileLowHealth { get; set; }

        [JsonPropertyName("HoL_OutnumberedTakedowns")]
        public string? HoLOutnumberedTakedowns { get; set; }

        [JsonPropertyName("HoL_ShutdownGoldCollected")]
        public string? HoLShutdownGoldCollected { get; set; }

        [JsonPropertyName("HoL_SoloKills")]
        public string? HoLSoloKills { get; set; }

        [JsonPropertyName("HoL_TurretsTakenWithinMinutes")]
        public string? HoLTurretsTakenWithinMinutes { get; set; }

        [JsonPropertyName("Missions_BXP_EarnedPerGame")]
        public string? MissionsBxpEarnedPerGame { get; set; }

        [JsonPropertyName("Missions_CannonMinionsKilled")]
        public string? MissionsCannonMinionsKilled { get; set; }

        [JsonPropertyName("Missions_ChampionsHitWithAbilitiesEarlyGame")]
        public string? MissionsChampionsHitWithAbilitiesEarlyGame { get; set; }

        [JsonPropertyName("Missions_ChampionsKilled")]
        public string? MissionsChampionsKilled { get; set; }

        [JsonPropertyName("Missions_ChampionTakedownsWhileGhosted")]
        public string? MissionsChampionTakedownsWhileGhosted { get; set; }

        [JsonPropertyName("Missions_ChampionTakedownsWithIgnite")]
        public string? MissionsChampionTakedownsWithIgnite { get; set; }

        [JsonPropertyName("Missions_CreepScore")]
        public string? MissionsCreepScore { get; set; }

        [JsonPropertyName("Missions_CreepScoreBy10Minutes")]
        public string? MissionsCreepScoreBy10Minutes { get; set; }

        [JsonPropertyName("Missions_Crepe_DamageDealtSpeedZone")]
        public string? MissionsCrepeDamageDealtSpeedZone { get; set; }

        [JsonPropertyName("Missions_Crepe_SnowballLanded")]
        public string? MissionsCrepeSnowballLanded { get; set; }

        [JsonPropertyName("Missions_Crepe_TakedownsWithInhibBuff")]
        public string? MissionsCrepeTakedownsWithInhibBuff { get; set; }

        [JsonPropertyName("Missions_DamageToChampsWithItems")]
        public string? MissionsDamageToChampsWithItems { get; set; }

        [JsonPropertyName("Missions_DamageToStructures")]
        public string? MissionsDamageToStructures { get; set; }

        [JsonPropertyName("Missions_DestroyPlants")]
        public string? MissionsDestroyPlants { get; set; }

        [JsonPropertyName("Missions_DominationRune")]
        public string? MissionsDominationRune { get; set; }

        [JsonPropertyName("Missions_GoldFromStructuresDestroyed")]
        public string? MissionsGoldFromStructuresDestroyed { get; set; }

        [JsonPropertyName("Missions_GoldFromTurretPlatesTaken")]
        public string? MissionsGoldFromTurretPlatesTaken { get; set; }

        [JsonPropertyName("Missions_GoldPerMinute")]
        public string? MissionsGoldPerMinute { get; set; }

        [JsonPropertyName("Missions_HealingFromLevelObjects")]
        public string? MissionsHealingFromLevelObjects { get; set; }

        [JsonPropertyName("Missions_HexgatesUsed")]
        public string? MissionsHexgatesUsed { get; set; }

        [JsonPropertyName("Missions_ImmobilizeChampions")]
        public string? MissionsImmobilizeChampions { get; set; }

        [JsonPropertyName("Missions_InspirationRune")]
        public string? MissionsInspirationRune { get; set; }

        [JsonPropertyName("Missions_LegendaryItems")]
        public string? MissionsLegendaryItems { get; set; }

        [JsonPropertyName("Missions_MinionsKilled")]
        public string? MissionsMinionsKilled { get; set; }

        [JsonPropertyName("Missions_PeriodicDamage")]
        public string? MissionsPeriodicDamage { get; set; }

        [JsonPropertyName("Missions_PlaceUsefulControlWards")]
        public string? MissionsPlaceUsefulControlWards { get; set; }

        [JsonPropertyName("Missions_PlaceUsefulWards")]
        public string? MissionsPlaceUsefulWards { get; set; }

        [JsonPropertyName("Missions_PorosFed")]
        public string? MissionsPorosFed { get; set; }

        [JsonPropertyName("Missions_PrecisionRune")]
        public string? MissionsPrecisionRune { get; set; }

        [JsonPropertyName("Missions_ResolveRune")]
        public string? MissionsResolveRune { get; set; }

        [JsonPropertyName("Missions_SnowballsHit")]
        public string? MissionsSnowballsHit { get; set; }

        [JsonPropertyName("Missions_SorceryRune")]
        public string? MissionsSorceryRune { get; set; }

        [JsonPropertyName("Missions_TakedownBaronsElderDragons")]
        public string? MissionsTakedownBaronsElderDragons { get; set; }

        [JsonPropertyName("Missions_TakedownDragons")]
        public string? MissionsTakedownDragons { get; set; }

        [JsonPropertyName("Missions_TakedownEpicMonsters")]
        public string? MissionsTakedownEpicMonsters { get; set; }

        [JsonPropertyName("Missions_TakedownEpicMonstersSingleGame")]
        public string? MissionsTakedownEpicMonstersSingleGame { get; set; }

        [JsonPropertyName("Missions_TakedownGold")]
        public string? MissionsTakedownGold { get; set; }

        [JsonPropertyName("Missions_TakedownStructures")]
        public string? MissionsTakedownStructures { get; set; }

        [JsonPropertyName("Missions_TakedownWards")]
        public string? MissionsTakedownWards { get; set; }

        [JsonPropertyName("Missions_TakedownsAfterExhausting")]
        public string? MissionsTakedownsAfterExhausting { get; set; }

        [JsonPropertyName("Missions_TakedownsAfterTeleporting")]
        public string? MissionsTakedownsAfterTeleporting { get; set; }

        [JsonPropertyName("Missions_TakedownsBefore15Min")]
        public string? MissionsTakedownsBefore15Min { get; set; }

        [JsonPropertyName("Missions_TakedownsUnderTurret")]
        public string? MissionsTakedownsUnderTurret { get; set; }

        [JsonPropertyName("Missions_TakedownsWithHelpFromMonsters")]
        public string? MissionsTakedownsWithHelpFromMonsters { get; set; }

        [JsonPropertyName("Missions_TimeSpentActivelyPlaying")]
        public string? MissionsTimeSpentActivelyPlaying { get; set; }

        [JsonPropertyName("Missions_TotalGold")]
        public string? MissionsTotalGold { get; set; }

        [JsonPropertyName("Missions_TrueDamageToStructures")]
        public string? MissionsTrueDamageToStructures { get; set; }

        [JsonPropertyName("Missions_TurretPlatesDestroyed")]
        public string? MissionsTurretPlatesDestroyed { get; set; }

        [JsonPropertyName("Missions_TwoChampsKilledWithSameAbility")]
        public string? MissionsTwoChampsKilledWithSameAbility { get; set; }

        [JsonPropertyName("Missions_VoidMitesSummoned")]
        public string? MissionsVoidMitesSummoned { get; set; }

        [JsonPropertyName("S3A1_Event_DoombotsTakenDownBefore5")]
        public string? S3A1EventDoombotsTakenDownBefore5 { get; set; }

        [JsonPropertyName("S3A1_PlayAsDemaciansOrAgainstNoxians")]
        public string? S3A1PlayAsDemaciansOrAgainstNoxians { get; set; }

        [JsonPropertyName("S3A1_Takedowns")]
        public string? S3A1Takedowns { get; set; }

        [JsonPropertyName("S3A2_PrismaticAug")]
        public string? S3A2PrismaticAug { get; set; }

        [JsonPropertyName("S3A2_ZaahenUnlock")]
        public string? S3A2ZaahenUnlock { get; set; }

        [JsonPropertyName("SeasonalMissions_TakedownAtakhan")]
        public string? SeasonalMissionsTakedownAtakhan { get; set; }

        [JsonPropertyName("WeeklyMission_S2_DamagingAbilities")]
        public string? WeeklyMissionS2DamagingAbilities { get; set; }

        [JsonPropertyName("WeeklyMission_S2_FeatsOfStrength")]
        public string? WeeklyMissionS2FeatsOfStrength { get; set; }

        [JsonPropertyName("WeeklyMission_S2_SpiritPetals")]
        public string? WeeklyMissionS2SpiritPetals { get; set; }

        #endregion
    }
}
