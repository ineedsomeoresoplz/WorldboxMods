using System;
using UnityEngine;

namespace XaviiNowWePlayMod.Code.Networking
{
    [Serializable]
    public class GodCommandMessage
    {
        public string CommandId;
        public string PowerId;
        public int TargetX;
        public int TargetY;
        public string Origin;
        public string Timestamp;

        public Vector2Int Target => new Vector2Int(TargetX, TargetY);

        public GodCommandMessage() { }

        public GodCommandMessage(string powerId, Vector2Int target, string origin)
        {
            CommandId = Guid.NewGuid().ToString("N");
            PowerId = powerId ?? string.Empty;
            TargetX = target.x;
            TargetY = target.y;
            Origin = origin;
            Timestamp = DateTime.UtcNow.ToString("o");
        }
    }
}
