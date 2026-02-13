using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using NeoModLoader.constants;
using XaviiHistorybookMod.Code.Data;
using XaviiHistorybookMod.Code.UI;

namespace XaviiHistorybookMod.Code.Managers
{
    public class HistorybookManager : MonoBehaviour
    {
        private const string ConfigFolder = "xhm-historybook";
        private const string ExportFolder = "exports";
        public static HistorybookManager Instance { get; private set; }

        private HistorybookStorage _storage;
        private HistorybookWindow _window;

        public event Action OnHistoryChanged;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
            _storage = new HistorybookStorage(ConfigFolder);
            _storage.Load();
        }

        private IEnumerator Start()
        {
            while (CanvasMain.instance == null)
                yield return null;

            var windowHolder = new GameObject("XHM Historybook Window");
            _window = windowHolder.AddComponent<HistorybookWindow>();
            _window.Initialize(this);
            _window.SetVisible(false);
        }

        public HistoryRecord EnsureRecord(Actor actor, out bool isNew)
        {
            var record = _storage.GetOrCreate(actor.getID());
            isNew = !record.HasEverBeenFavorite;
            UpdateMetadata(record, actor);
            return record;
        }

        public void UpdateMetadata(HistoryRecord record, Actor actor)
        {
            record.Name = actor.getName();
            record.SpeciesId = actor.asset?.id ?? record.SpeciesId;
            record.SpeciesName = actor.asset?.getLocalizedName() ?? record.SpeciesName;
            record.BornAt = actor.data.created_time;
        }

        public void AddInitialSnapshot(HistoryRecord record, Actor actor)
        {
            var birthEntry = new HistoryEntry
            {
                Timestamp = actor.data.created_time > 0 ? actor.data.created_time : GetNow(),
                Category = HistoryEventCategory.Birth,
                Title = $"Born",
                Description = $"Origin story: {FormatTile(actor.current_tile)}",
                LocationHint = FormatTile(actor.current_tile)
            };

            var statsEntry = new HistoryEntry
            {
                Timestamp = GetNow(),
                Category = HistoryEventCategory.Note,
                Title = $"Status at favorite",
                Description = $"Age {actor.getAge()} / Kills {actor.data.kills}"
            };

            if (!record.Events.Exists(e => e.Category == HistoryEventCategory.Birth))
                AddEvent(record.UnitId, birthEntry, true);
            AddEvent(record.UnitId, statsEntry, true);
        }

        public void RecordBirth(Actor actor)
        {
            if (actor == null)
                return;

            var record = _storage.GetOrCreate(actor.getID());
            UpdateMetadata(record, actor);
            record.BornAt = actor.data.created_time;

            var entry = new HistoryEntry
            {
                Timestamp = actor.data.created_time > 0 ? actor.data.created_time : GetNow(),
                Category = HistoryEventCategory.Birth,
                Title = $"Newborn",
                Description = $"Entered the world near {FormatTile(actor.current_tile)}",
                LocationHint = FormatTile(actor.current_tile)
            };

            AddEvent(record.UnitId, entry, true);
        }

        public void AddEvent(long unitId, HistoryEntry entry, bool force = false)
        {
            if (!_storage.Records.TryGetValue(unitId, out var record))
                return;

            if (!force && !record.HasEverBeenFavorite)
                return;

            if (entry.Timestamp <= 0)
                entry.Timestamp = GetNow();

            record.Events.Insert(0, entry);
            NotifyChange();
        }

        public void SetFavorite(long unitId)
        {
            if (!_storage.Records.TryGetValue(unitId, out var record))
                return;
            record.IsFavorite = true;
            record.HasEverBeenFavorite = true;
        }

        public void ClearFavorite(long unitId)
        {
            if (!_storage.Records.TryGetValue(unitId, out var record))
                return;
            record.IsFavorite = false;
        }

        public List<HistoryRecord> GetFavoriteRecords()
        {
            return _storage.Records.Values
                .Where(r => r.HasEverBeenFavorite)
                .OrderBy(r => r.Name)
                .ToList();
        }

        public string ExportRecord(long unitId)
        {
            if (!_storage.Records.TryGetValue(unitId, out var record))
                return null;

            var payload = BuildExportText(record);
            if (string.IsNullOrWhiteSpace(payload))
                return null;

            var folder = Path.Combine(Paths.ModsConfigPath, ConfigFolder, ExportFolder);
            Directory.CreateDirectory(folder);
            var safeName = string.IsNullOrWhiteSpace(record.Name) ? $"unit_{unitId}" : SanitizeFileName(record.Name);
            var fileName = $"{safeName}_{unitId}_{DateTime.Now:yyyyMMdd_HHmmss}.txt";
            var filePath = Path.Combine(folder, fileName);
            File.WriteAllText(filePath, payload, Encoding.UTF8);
            return filePath;
        }

        public HistoryRecord TryGetRecord(long unitId)
        {
            return _storage.Records.TryGetValue(unitId, out var record) ? record : null;
        }

        public void DeleteRecord(long unitId)
        {
            _storage.Remove(unitId);
            NotifyChange();
        }

        public void DeleteAll()
        {
            _storage.Clear();
            NotifyChange();
        }

        public void FocusOnUnit(long unitId)
        {
            var actor = World.world.units.get(unitId);
            if (actor == null || !actor.isAlive())
                return;
            WorldLog.locationFollow(actor);
        }

        public void NotifyChange()
        {
            _storage.Save();
            OnHistoryChanged?.Invoke();
        }

        private double GetNow()
        {
            return World.world?.getCurWorldTime() ?? 0;
        }

        private string FormatTile(WorldTile tile)
        {
            if (tile == null)
                return "Unknown location";
            return $"Tile {tile.pos.x},{tile.pos.y}";
        }

        private string BuildExportText(HistoryRecord record)
        {
            if (record == null)
                return string.Empty;

            var builder = new StringBuilder();
            builder.AppendLine($"Historybook Export · {record.Name}");
            builder.AppendLine($"Species: {record.SpeciesName} ({record.SpeciesId})");
            builder.AppendLine($"Born: {Date.getDate(record.BornAt)}");
            builder.AppendLine($"Favorite history length: {record.Events.Count}");
            builder.AppendLine();
            builder.AppendLine("Timeline");
            builder.AppendLine("--------");

            foreach (var entry in record.Events.OrderBy(e => e.Timestamp))
            {
                builder.AppendLine($"{Date.getDate(entry.Timestamp)} · {entry.Title}");
                if (!string.IsNullOrEmpty(entry.Description))
                    builder.AppendLine($"   {entry.Description}");
                if (!string.IsNullOrEmpty(entry.LocationHint))
                    builder.AppendLine($"   Location: {entry.LocationHint}");
                builder.AppendLine();
            }

            return builder.ToString();
        }

        private static string SanitizeFileName(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return "history";

            var invalid = Path.GetInvalidFileNameChars();
            var sanitized = new string(value.Select(ch => invalid.Contains(ch) ? '_' : ch).ToArray()).Trim();
            return string.IsNullOrWhiteSpace(sanitized) ? "history" : sanitized;
        }
    }
}
