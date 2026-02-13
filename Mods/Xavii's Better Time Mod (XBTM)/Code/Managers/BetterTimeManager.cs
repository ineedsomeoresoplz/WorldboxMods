using System;
using UnityEngine;

namespace XaviiBetterTimeMod.Code.Managers
{
    public class BetterTimeManager : MonoBehaviour
    {
        public static BetterTimeManager Instance { get; private set; }

        public const float WorldTimeScaleFactor = 1f / 1800f;
        public const double DayLengthWorldTime = 1.0 / 6.0;
        public const double HalfDayWorldTime = DayLengthWorldTime / 2.0;

        private WorldAgeAsset _hopeAsset;
        private WorldAgeAsset _moonAsset;
        private bool _assetsReady;
        private bool _isDaytime;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void OnEnable()
        {
            MapBox.on_world_loaded += OnWorldLoaded;
        }

        private void OnDisable()
        {
            MapBox.on_world_loaded -= OnWorldLoaded;
        }

        private void Start()
        {
            RefreshAssets();
            if (World.world != null)
            {
                ForceState();
            }
        }

        private void Update()
        {
            if (World.world == null || World.world.map_stats == null)
                return;

            if (!_assetsReady)
                RefreshAssets();

            double worldTime = World.world.getCurWorldTime();
            double dayCycle = NormalizeDayCycle(worldTime);
            bool shouldBeDay = dayCycle < HalfDayWorldTime;

            if (shouldBeDay != _isDaytime)
            {
                ApplyState(shouldBeDay, worldTime, dayCycle);
            }

            SyncProgress(dayCycle, shouldBeDay);
        }

        private void OnWorldLoaded()
        {
            RefreshAssets();
            ForceState();
        }

        private void ForceState()
        {
            if (World.world?.map_stats == null)
                return;

            double worldTime = World.world.getCurWorldTime();
            double dayCycle = NormalizeDayCycle(worldTime);
            bool shouldBeDay = dayCycle < HalfDayWorldTime;

            _isDaytime = !shouldBeDay;
            ApplyState(shouldBeDay, worldTime, dayCycle);
            SyncProgress(dayCycle, shouldBeDay);
        }

        private void ApplyState(bool isDay, double worldTime, double dayCycle)
        {
            if (!_assetsReady || World.world?.era_manager == null)
                return;

            WorldAgeAsset target = isDay ? _hopeAsset : _moonAsset;
            if (target == null)
                return;

            _isDaytime = isDay;
            World.world.map_stats.world_age_slot_index = isDay ? 0 : 1;
            World.world.map_stats.world_age_id = target.id;
            World.world.map_stats.current_world_ages_duration = (float)DayLengthWorldTime;
            World.world.map_stats.current_age_progress = GetNormalizedHalfDayProgress(dayCycle);
            double offset = GetHalfDayOffset(dayCycle);
            double started = worldTime - offset;
            World.world.map_stats.world_age_started_at = started;
            World.world.map_stats.same_world_age_started_at = started;

            World.world.era_manager.setCurrentAge(target, false);
        }

        private void SyncProgress(double dayCycle, bool isDay)
        {
            if (World.world?.map_stats == null)
                return;

            World.world.map_stats.current_world_ages_duration = (float)DayLengthWorldTime;
            World.world.map_stats.current_age_progress = GetNormalizedHalfDayProgress(dayCycle);
            World.world.map_stats.world_age_slot_index = isDay ? 0 : 1;
            if (_assetsReady)
            {
                World.world.map_stats.world_age_id = isDay ? _hopeAsset?.id : _moonAsset?.id;
            }
        }

        private static double GetHalfDayOffset(double dayCycle)
        {
            double raw = dayCycle < HalfDayWorldTime ? dayCycle : dayCycle - HalfDayWorldTime;
            return raw;
        }

        private static float GetNormalizedHalfDayProgress(double dayCycle)
        {
            double raw = dayCycle < HalfDayWorldTime ? dayCycle : dayCycle - HalfDayWorldTime;
            return (float)(raw / HalfDayWorldTime);
        }

        private static double NormalizeDayCycle(double worldTime)
        {
            double cycle = worldTime % DayLengthWorldTime;
            if (cycle < 0.0)
            {
                cycle += DayLengthWorldTime;
            }
            return cycle;
        }

        private void RefreshAssets()
        {
            if (AssetManager.era_library == null)
            {
                _assetsReady = false;
                return;
            }

            _hopeAsset = AssetManager.era_library.get("age_hope");
            _moonAsset = AssetManager.era_library.get("age_moon");
            _assetsReady = _hopeAsset != null && _moonAsset != null;
        }

        public bool TryGetCurrentCycleAsset(out WorldAgeAsset asset)
        {
            asset = _isDaytime ? _hopeAsset : _moonAsset;
            return _assetsReady && asset != null;
        }
    }
}
