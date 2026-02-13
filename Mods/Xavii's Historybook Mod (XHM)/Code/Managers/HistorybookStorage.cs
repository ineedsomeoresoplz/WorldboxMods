using NeoModLoader.constants;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using XaviiHistorybookMod.Code.Data;

namespace XaviiHistorybookMod.Code.Managers
{
    public class HistorybookStorage
    {
        private readonly string _saveFilePath;
        private HistorybookSaveData _data = new HistorybookSaveData();

        public HistorybookStorage(string folderName)
        {
            var folderPath = Path.Combine(Paths.ModsConfigPath, folderName);
            Directory.CreateDirectory(folderPath);
            _saveFilePath = Path.Combine(folderPath, "historybook.json");
        }

        public void Load()
        {
            if (!File.Exists(_saveFilePath))
            {
                _data = new HistorybookSaveData();
                return;
            }

            var payload = File.ReadAllText(_saveFilePath);
            try
            {
                _data = JsonConvert.DeserializeObject<HistorybookSaveData>(payload) ?? new HistorybookSaveData();
            }
            catch (JsonException)
            {
                _data = new HistorybookSaveData();
            }
        }

        public void Save()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(_saveFilePath) ?? Paths.ModsConfigPath);
            File.WriteAllText(_saveFilePath, JsonConvert.SerializeObject(_data, Formatting.Indented));
        }

        public IReadOnlyDictionary<long, HistoryRecord> Records => _data.Records;

        public HistoryRecord GetOrCreate(long unitId)
        {
            if (!_data.Records.TryGetValue(unitId, out var record))
            {
                record = new HistoryRecord { UnitId = unitId };
                _data.Records[unitId] = record;
            }
            return record;
        }

        public void Remove(long unitId) => _data.Records.Remove(unitId);

        public void Clear() => _data.Records.Clear();
    }
}
