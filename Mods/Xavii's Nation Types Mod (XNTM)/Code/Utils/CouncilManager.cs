using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;
using XNTM.Code.Data;
using XNTM.Code.Features.Council;

namespace XNTM.Code.Utils
{
    
    
    
    
    public static class CouncilManager
    {
        private const string CouncilDataKey = "xntm_council_members";
        private const string CouncilRepublicId = "council_republic";
        private const int MinCouncilSeats = 3;
        private const int MaxCouncilSeats = 10;
        private const int EmergencyMinSeats = 1;
        private const int EmergencyMaxSeats = 2;

        
        private static readonly Dictionary<long, CouncilInfo> _infoCache = new Dictionary<long, CouncilInfo>();

        #region Public API
        public static bool IsCouncilNation(Kingdom kingdom)
        {
            return NationTypeManager.GetDefinition(kingdom).SuccessionMode == NationSuccessionMode.Council;
        }

        public static bool IsLeaderlessCouncilNation(Kingdom kingdom)
        {
            string id = NationTypeManager.GetDefinition(kingdom)?.Id;
            return string.Equals(id, CouncilRepublicId, StringComparison.Ordinal);
        }

        public static void TickCouncil(Kingdom kingdom)
        {
            if (kingdom == null || !IsCouncilNation(kingdom))
                return;

            bool leaderless = IsLeaderlessCouncilNation(kingdom);
            if (!leaderless && kingdom.data.timer_new_king > 0f)
                return;

            var members = LoadCouncilMembers(kingdom, out var info);
            bool temporary = info.Temporary;

            
            if (PruneCouncil(kingdom, members))
                temporary = UpdateTemporaryFlag(kingdom, members);

            int electableCount;
            using (ListPool<Actor> pool = GetElectable(kingdom, null, out electableCount))
            {
                int targetSeats = DetermineTargetSeats(kingdom, electableCount, out bool shouldBeTemporary);
                temporary |= shouldBeTemporary;

                if (targetSeats == 0)
                {
                    if (members.Count > 0)
                    {
                        members.Clear();
                        SaveCouncilMembers(kingdom, members, temporary);
                    }
                    else
                    {
                        SaveCouncilMembers(kingdom, members, temporary);
                    }
                    return;
                }

                if (members.Count < targetSeats)
                {
                    ElectMembers(kingdom, members, targetSeats, temporary, pool);
                }
                else if (members.Count > targetSeats)
                {
                    
                    members = members
                        .OrderByDescending(a => BuildCouncilScore(kingdom, a))
                        .Take(targetSeats)
                        .ToList();
                }
            }

            SaveCouncilMembers(kingdom, members, temporary);
            AssignChair(kingdom, members);
        }

        public static void OnActorDied(Actor actor, Kingdom kingdomBeforeDeath, Actor killer)
        {
            if (actor == null || kingdomBeforeDeath == null)
                return;
            if (!IsCouncilNation(kingdomBeforeDeath))
                return;

            var members = LoadCouncilMembers(kingdomBeforeDeath, out var info);
            int seatNumber = GetSeatNumber(kingdomBeforeDeath, members, actor);
            bool wasTemporary = info.Temporary;
            if (!members.Remove(actor))
                return;

            UpdateTemporaryFlag(kingdomBeforeDeath, members);

            LogCouncilDeath(kingdomBeforeDeath, actor, killer, seatNumber, wasTemporary);

            
            TickCouncil(kingdomBeforeDeath);
        }

        public static string GetCouncilSummary(Kingdom kingdom)
        {
            if (kingdom == null)
                return "Council";
            return BuildCachedSummary(kingdom);
        }

        public static bool HasMultipleRulers(Kingdom kingdom)
        {
            return GetRulers(kingdom).Count > 1;
        }

        public static string GetRulerDisplay(Kingdom kingdom)
        {
            List<Actor> rulers = GetRulers(kingdom);
            if (rulers.Count == 0)
                return "-";

            List<string> names = new List<string>(rulers.Count);
            for (int i = 0; i < rulers.Count; i++)
            {
                string name = rulers[i]?.getName();
                if (string.IsNullOrWhiteSpace(name))
                    continue;
                names.Add(name);
            }

            if (names.Count == 0)
                return "-";

            return string.Join(", ", names);
        }

        public static List<Actor> GetCouncilorsBySeat(Kingdom kingdom)
        {
            List<Actor> result = new List<Actor>();
            if (kingdom == null || !IsCouncilNation(kingdom))
                return result;

            var members = LoadCouncilMembers(kingdom, out var _)
                .Where(member => IsElectable(kingdom, member))
                .Distinct()
                .ToList();

            if (!IsLeaderlessCouncilNation(kingdom))
            {
                Actor chair = kingdom.king;
                if (IsElectable(kingdom, chair) && !members.Contains(chair))
                    members.Add(chair);
            }

            return members
                .OrderByDescending(member => BuildCouncilScore(kingdom, member))
                .ThenBy(member => member.getID())
                .ToList();
        }

        public static int GetCouncilSlotCount(Kingdom kingdom)
        {
            if (kingdom == null || !IsCouncilNation(kingdom))
                return 0;

            int electableCount;
            using (ListPool<Actor> pool = GetElectable(kingdom, null, out electableCount))
            {
                int targetSlots = DetermineTargetSeats(kingdom, electableCount, out var _);
                int occupiedSlots = GetCouncilorsBySeat(kingdom).Count;
                return Mathf.Max(targetSlots, occupiedSlots);
            }
        }

        public static int GetCouncilTotalFunds(Kingdom kingdom)
        {
            List<Actor> councilors = GetCouncilorsBySeat(kingdom);
            int total = 0;
            for (int i = 0; i < councilors.Count; i++)
            {
                Actor councilor = councilors[i];
                if (councilor == null || councilor.isRekt())
                    continue;
                total += Mathf.Max(0, councilor.money);
            }
            return total;
        }

        public static string GetCouncilTooltipSummary(Kingdom kingdom)
        {
            if (kingdom == null)
                return "-";
            if (!IsCouncilNation(kingdom))
                return GetRulerDisplay(kingdom);

            int occupiedSlots = GetCouncilorsBySeat(kingdom).Count;
            int slotCount = Mathf.Max(occupiedSlots, GetCouncilSlotCount(kingdom));
            int totalFunds = GetCouncilTotalFunds(kingdom);
            string fundsLabel = LocalizedTextManager.stringExists("money") ? LocalizedTextManager.getText("money") : "Funds";
            return $"{occupiedSlots}/{slotCount} | {fundsLabel}: {totalFunds.ToString("N0", CultureInfo.InvariantCulture)}";
        }

        public static List<Actor> GetRulers(Kingdom kingdom)
        {
            List<Actor> result = new List<Actor>();
            if (kingdom == null)
                return result;

            if (IsCouncilNation(kingdom))
            {
                var members = LoadCouncilMembers(kingdom, out var _);
                for (int i = 0; i < members.Count; i++)
                {
                    Actor member = members[i];
                    if (!IsElectable(kingdom, member))
                        continue;
                    if (!result.Contains(member))
                        result.Add(member);
                }
            }

            if (IsLeaderlessCouncilNation(kingdom))
                return result;

            Actor chair = kingdom.king;
            if (chair != null && chair.isAlive() && chair.kingdom == kingdom)
            {
                int existingIndex = result.IndexOf(chair);
                if (existingIndex > 0)
                {
                    result.RemoveAt(existingIndex);
                    result.Insert(0, chair);
                }
                else if (existingIndex < 0)
                {
                    result.Insert(0, chair);
                }
            }

            return result;
        }
        #endregion

        #region Core helpers
        private static int DetermineTargetSeats(Kingdom kingdom, int electableCount, out bool temporary)
        {
            int fixedSeats = GetFixedCouncilSeatLimit(kingdom);
            if (fixedSeats > 0)
            {
                if (electableCount <= 0)
                {
                    temporary = true;
                    return 0;
                }
                int target = Mathf.Clamp(electableCount, EmergencyMinSeats, fixedSeats);
                temporary = target < fixedSeats;
                return target;
            }

            if (electableCount < MinCouncilSeats)
            {
                temporary = true;
                if (electableCount <= 0)
                    return 0;
                return Mathf.Clamp(electableCount, EmergencyMinSeats, EmergencyMaxSeats);
            }

            temporary = false;
            int desired = Mathf.Clamp(electableCount, MinCouncilSeats, MaxCouncilSeats);
            return desired;
        }

        private static bool PruneCouncil(Kingdom kingdom, List<Actor> members)
        {
            int before = members.Count;
            members.RemoveAll(actor => !IsElectable(kingdom, actor));
            return members.Count != before;
        }

        private static bool UpdateTemporaryFlag(Kingdom kingdom, List<Actor> members)
        {
            bool temporary = members.Count < MinCouncilSeats;
            SaveCouncilMembers(kingdom, members, temporary);
            return temporary;
        }

        private static void ElectMembers(Kingdom kingdom, List<Actor> members, int targetSeats, bool temporary, ListPool<Actor> electables)
        {
            if (kingdom == null)
                return;

            List<Actor> elected = new List<Actor>();
            
            foreach (Actor candidate in electables.OrderByDescending(a => BuildCouncilScore(kingdom, a)))
            {
                if (!IsElectable(kingdom, candidate))
                    continue;
                if (members.Contains(candidate))
                    continue;

                members.Add(candidate);
                elected.Add(candidate);
                if (members.Count >= targetSeats)
                    break;
            }

            for (int i = 0; i < elected.Count; i++)
            {
                Actor electedMember = elected[i];
                int seatNumber = GetSeatNumber(kingdom, members, electedMember);
                LogElection(kingdom, electedMember, seatNumber, targetSeats, temporary);
            }
        }

        private static void AssignChair(Kingdom kingdom, List<Actor> members)
        {
            if (kingdom == null)
                return;
            if (IsLeaderlessCouncilNation(kingdom))
            {
                if (kingdom.hasKing())
                    kingdom.kingLeftEvent();
                return;
            }

            Actor chair = members
                .OrderByDescending(actor => BuildCouncilScore(kingdom, actor))
                .FirstOrDefault();

            if (chair == null)
            {
                if (kingdom.hasKing())
                    kingdom.kingLeftEvent();
                return;
            }

            if (kingdom.king != chair)
            {
                if (chair.hasCity())
                {
                    chair.stopBeingWarrior();
                    if (chair.isCityLeader())
                        chair.city.removeLeader();
                }

                if (kingdom.hasCapital() && chair.city != kingdom.capital)
                    chair.joinCity(kingdom.capital);

                kingdom.setKing(chair);
            }
        }
        #endregion

        #region Scoring & eligibility
        private static float BuildCouncilScore(Kingdom kingdom, Actor actor)
        {
            if (actor == null)
                return float.MinValue;

            bool atWar = kingdom != null && kingdom.hasEnemies();
            float diplomacy = actor.stats?.get("diplomacy") ?? 0f;
            float warfare = actor.stats?.get("warfare") ?? 0f;
            float renown = actor.data?.renown ?? 0f;
            float capitalBonus = kingdom != null && actor.city == kingdom.capital ? 2.5f : 0f;

            float diplomacyWeight = atWar ? 0.45f : 0.65f;
            float warfareWeight = atWar ? 0.35f : 0.15f;
            float renownWeight = 0.2f;

            return (diplomacy * diplomacyWeight) + (warfare * warfareWeight) + (renown * renownWeight) + capitalBonus;
        }

        private static ListPool<Actor> GetElectable(Kingdom kingdom, Actor exclude, out int count)
        {
            ListPool<Actor> pool = new ListPool<Actor>();
            count = 0;
            if (kingdom == null)
                return pool;

            foreach (Actor actor in kingdom.getUnits())
            {
                if (actor == null || actor == exclude)
                    continue;
                if (!IsElectable(kingdom, actor))
                    continue;

                pool.Add(actor);
            }

            count = pool.Count;
            return pool;
        }

        private static bool IsElectable(Kingdom kingdom, Actor actor)
        {
            return actor != null
                && actor.isAlive()
                && !actor.asset.is_boat
                && !actor.isBaby()
                && actor.kingdom == kingdom;
        }
        #endregion

        #region Persistence
        private static List<Actor> LoadCouncilMembers(Kingdom kingdom, out CouncilInfo info)
        {
            info = GetOrCreateInfo(kingdom);
            if (kingdom?.data?.custom_data_string == null)
                return new List<Actor>();

            if (!kingdom.data.custom_data_string.TryGetValue(CouncilDataKey, out string stored) || string.IsNullOrEmpty(stored))
                return new List<Actor>();

            string[] chunks = stored.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            List<Actor> members = new List<Actor>(chunks.Length);
            foreach (string chunk in chunks)
            {
                if (!long.TryParse(chunk, NumberStyles.Integer, CultureInfo.InvariantCulture, out long id))
                    continue;
                Actor actor = World.world.units.get(id);
                if (actor != null)
                    members.Add(actor);
            }

            return members;
        }

        private static void SaveCouncilMembers(Kingdom kingdom, List<Actor> members, bool temporary)
        {
            if (kingdom?.data == null)
                return;

            if (kingdom.data.custom_data_string == null)
                kingdom.data.custom_data_string = new CustomDataContainer<string>();

            if (members == null || members.Count == 0)
            {
                kingdom.data.custom_data_string.Remove(CouncilDataKey);
                UpdateCache(kingdom, 0, temporary);
                return;
            }

            string serialized = string.Join(",", members.Select(m => m.getID().ToString(CultureInfo.InvariantCulture)));
            kingdom.data.custom_data_string[CouncilDataKey] = serialized;
            UpdateCache(kingdom, members.Count, temporary);
        }

        private static CouncilInfo GetOrCreateInfo(Kingdom kingdom)
        {
            long id = kingdom?.getID() ?? -1;
            if (!_infoCache.TryGetValue(id, out var info))
            {
                info = new CouncilInfo();
                _infoCache[id] = info;
            }
            return info;
        }

        private static void UpdateCache(Kingdom kingdom, int seatCount, bool temporary)
        {
            if (kingdom == null)
                return;
            long id = kingdom.getID();
            _infoCache[id] = new CouncilInfo
            {
                SeatCount = seatCount,
                Temporary = temporary
            };
        }
        #endregion

        #region Logging
        private static void LogElection(Kingdom kingdom, Actor actor, int seatNumber, int targetSeats, bool temporary)
        {
            if (kingdom == null || actor == null)
                return;

            string title = BuildCouncilorTitle(seatNumber, temporary, actor.isSexFemale());
            string summary = BuildCouncilSummary(Mathf.Max(seatNumber, targetSeats), temporary);
            WorldLogAsset asset = CouncilLogAssets.CouncilorElected;
            if (asset == null)
                return;
            Color color = kingdom.getColor()?.getColorText() ?? Color.white;
            WorldLogMessage msg = new WorldLogMessage(asset, actor.getName(), title, summary)
            {
                kingdom = kingdom,
                unit = actor,
                location = actor.current_position,
                color_special1 = color,
                color_special2 = color
            };
            msg.add();
        }

        private static void LogCouncilDeath(Kingdom kingdom, Actor actor, Actor killer, int seatNumber, bool temporary)
        {
            if (kingdom == null || actor == null)
                return;

            string title = BuildCouncilorTitle(seatNumber, temporary, actor.isSexFemale());
            string summary = BuildCachedSummary(kingdom);
            WorldLogAsset asset = killer != null ? CouncilLogAssets.CouncilorKilled : CouncilLogAssets.CouncilorDead;
            if (asset == null)
                return;
            Color kingdomColor = kingdom.getColor()?.getColorText() ?? Color.white;
            Color killerColor = killer?.kingdom?.getColor()?.getColorText() ?? kingdomColor;
            WorldLogMessage msg = new WorldLogMessage(asset, actor.getName(), title, killer?.getName() ?? summary)
            {
                kingdom = kingdom,
                unit = actor,
                location = actor.current_position,
                color_special1 = kingdomColor,
                color_special2 = kingdomColor,
                color_special3 = killerColor
            };
            msg.add();
        }
        #endregion

        #region Titles
        private static string BuildCouncilorTitle(int seatNumber, bool temporary, bool isFemale)
        {
            string baseTitle = temporary ? "Acting Councillor" : "Councillor";
            return seatNumber <= 0 ? baseTitle : $"{baseTitle} #{seatNumber}";
        }

        private static int GetSeatNumber(Kingdom kingdom, List<Actor> members, Actor actor)
        {
            if (kingdom == null || members == null || actor == null || members.Count == 0)
                return 0;

            List<Actor> ordered = members
                .Where(member => IsElectable(kingdom, member))
                .Distinct()
                .OrderByDescending(member => BuildCouncilScore(kingdom, member))
                .ThenBy(member => member.getID())
                .ToList();

            for (int i = 0; i < ordered.Count; i++)
            {
                if (ordered[i] == actor)
                    return i + 1;
            }

            return 0;
        }

        private static int GetFixedCouncilSeatLimit(Kingdom kingdom)
        {
            string id = NationTypeManager.GetDefinition(kingdom)?.Id;
            return id switch
            {
                "diarchy" => 2,
                "triarchy" => 3,
                "tetrarchy" => 4,
                _ => 0
            };
        }

        private static string BuildCachedSummary(Kingdom kingdom)
        {
            long id = kingdom?.getID() ?? -1;
            if (_infoCache.TryGetValue(id, out var info))
                return BuildCouncilSummary(info.SeatCount, info.Temporary);
            return "Council";
        }

        private static string BuildCouncilSummary(int seats, bool temporary)
        {
            string baseLabel = temporary ? "Emergency Council" : "Council";
            if (seats <= 0)
                return baseLabel;
            return $"{baseLabel} of {seats}";
        }
        #endregion

        private sealed class CouncilInfo
        {
            public int SeatCount;
            public bool Temporary;
        }
    }
}
