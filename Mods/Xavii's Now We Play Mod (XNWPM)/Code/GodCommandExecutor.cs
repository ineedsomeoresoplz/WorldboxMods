using System.Collections.Generic;
using UnityEngine;

namespace XaviiNowWePlayMod.Code
{
    internal static class GodCommandExecutor
    {
        private const int MaxCachedCommands = 1024;
        private static readonly Queue<string> _recentCommandIds = new Queue<string>(MaxCachedCommands);
        private static readonly HashSet<string> _processedCommandIds = new HashSet<string>();
        private static readonly object _lock = new object();

        public static bool Execute(Networking.GodCommandMessage message)
        {
            if (message == null || string.IsNullOrWhiteSpace(message.PowerId))
            {
                return false;
            }

            if (!TryRegister(message.CommandId))
            {
                return false;
            }

            GodPower power = AssetManager.powers.get(message.PowerId);
            if (power == null)
            {
                Debug.LogWarning($"XNWPM: power '{message.PowerId}' could not be resolved.");
                return false;
            }

            PlayerControl playerControl = World.world?.player_control;
            if (playerControl == null)
            {
                Debug.LogWarning("XNWPM: PlayerControl reference is missing.");
                return false;
            }

            int clampedX = Mathf.Clamp(message.TargetX, 0, MapBox.width - 1);
            int clampedY = Mathf.Clamp(message.TargetY, 0, MapBox.height - 1);
            playerControl.clickedFinal(new Vector2Int(clampedX, clampedY), power, false);
            return true;
        }

        private static bool TryRegister(string commandId)
        {
            if (string.IsNullOrWhiteSpace(commandId))
            {
                return false;
            }

            lock (_lock)
            {
                if (_processedCommandIds.Contains(commandId))
                {
                    return false;
                }

                _processedCommandIds.Add(commandId);
                _recentCommandIds.Enqueue(commandId);
                if (_recentCommandIds.Count > MaxCachedCommands)
                {
                    string oldest = _recentCommandIds.Dequeue();
                    _processedCommandIds.Remove(oldest);
                }
            }

            return true;
        }
    }
}
