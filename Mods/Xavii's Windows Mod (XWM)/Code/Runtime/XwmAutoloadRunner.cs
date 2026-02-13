using UnityEngine;

namespace XaviiWindowsMod.Runtime
{
    internal sealed class XwmAutoloadRunner : MonoBehaviour
    {
        private bool _applied;
        private float _applyAt;

        private void Awake()
        {
            XwmAutoloadProfileStore.EnsureLoaded();
        }

        private void Start()
        {
            _applyAt = Time.unscaledTime + 0.5f;
        }

        private void Update()
        {
            if (_applied || Time.unscaledTime < _applyAt)
            {
                return;
            }

            _applied = true;
            XwmAutoloadProfileStore.ApplyAll();
        }
    }
}
