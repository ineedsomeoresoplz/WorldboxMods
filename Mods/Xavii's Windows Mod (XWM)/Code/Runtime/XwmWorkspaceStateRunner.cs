using UnityEngine;

namespace XaviiWindowsMod.Runtime
{
    internal sealed class XwmWorkspaceStateRunner : MonoBehaviour
    {
        private const float SaveIntervalSeconds = 8f;
        private float _nextSaveTime;

        private void Awake()
        {
            XwmWorkspaceStateStore.EnsureLoaded();
        }

        private void Start()
        {
            XwmWorkspaceStateStore.ApplyAllLoaded();
            _nextSaveTime = Time.unscaledTime + SaveIntervalSeconds;
        }

        private void Update()
        {
            if (Time.unscaledTime < _nextSaveTime)
            {
                return;
            }

            Flush(false);
            _nextSaveTime = Time.unscaledTime + SaveIntervalSeconds;
        }

        private void OnDestroy()
        {
            Flush(true);
        }

        private void OnApplicationQuit()
        {
            Flush(true);
        }

        public void ForceSave()
        {
            Flush(true);
            _nextSaveTime = Time.unscaledTime + SaveIntervalSeconds;
        }

        private void Flush(bool force)
        {
            XwmWorkspaceStateStore.CaptureAll();
            XwmWorkspaceStateStore.Save(force);
        }
    }
}
