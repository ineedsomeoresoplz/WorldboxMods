using System;
using System.Collections.Generic;
using UnityEngine;

namespace XaviiWindowsMod.API
{
    public enum XwmEase
    {
        Linear,
        InQuad,
        OutQuad,
        InOutQuad,
        InCubic,
        OutCubic,
        InOutCubic,
        InQuart,
        OutQuart,
        InOutQuart,
        InQuint,
        OutQuint,
        InOutQuint,
        InSine,
        OutSine,
        InOutSine,
        InExpo,
        OutExpo,
        InOutExpo,
        InCirc,
        OutCirc,
        InOutCirc,
        InBack,
        OutBack,
        InOutBack
    }

    public sealed class XwmTweenHandle
    {
        private Action _cancel;

        public bool IsAlive { get; private set; } = true;
        public bool IsCompleted { get; internal set; }

        internal void BindCancel(Action cancel)
        {
            _cancel = cancel;
        }

        public void Cancel()
        {
            if (!IsAlive)
            {
                return;
            }

            IsAlive = false;
            _cancel?.Invoke();
            _cancel = null;
        }

        internal void MarkCompleted()
        {
            IsAlive = false;
            IsCompleted = true;
            _cancel = null;
        }
    }

    internal sealed class XwmTweenRunner : MonoBehaviour
    {
        private sealed class XwmTweenState
        {
            public XwmTweenHandle Handle;
            public float Duration;
            public float Elapsed;
            public XwmEase Ease;
            public Action<float> Step;
            public Action Complete;
            public bool IgnoreTimeScale;
            public bool Cancelled;
        }

        private readonly List<XwmTweenState> _tweens = new List<XwmTweenState>();
        private readonly List<XwmTweenState> _pending = new List<XwmTweenState>();

        public XwmTweenHandle Run(float duration, XwmEase ease, Action<float> onStep, Action onComplete, bool ignoreTimeScale)
        {
            XwmTweenHandle handle = new XwmTweenHandle();
            if (onStep == null)
            {
                handle.MarkCompleted();
                return handle;
            }

            XwmTweenState state = new XwmTweenState
            {
                Handle = handle,
                Duration = Mathf.Max(0.0001f, duration),
                Elapsed = 0f,
                Ease = ease,
                Step = onStep,
                Complete = onComplete,
                IgnoreTimeScale = ignoreTimeScale,
                Cancelled = false
            };

            handle.BindCancel(() => state.Cancelled = true);
            _pending.Add(state);
            return handle;
        }

        private void LateUpdate()
        {
            if (_pending.Count > 0)
            {
                _tweens.AddRange(_pending);
                _pending.Clear();
            }

            if (_tweens.Count == 0)
            {
                return;
            }

            for (int i = _tweens.Count - 1; i >= 0; i--)
            {
                XwmTweenState state = _tweens[i];
                if (state == null || state.Handle == null)
                {
                    _tweens.RemoveAt(i);
                    continue;
                }

                if (state.Cancelled)
                {
                    state.Handle.Cancel();
                    _tweens.RemoveAt(i);
                    continue;
                }

                float delta = state.IgnoreTimeScale ? Time.unscaledDeltaTime : Time.deltaTime;
                state.Elapsed += Mathf.Max(0f, delta);
                float t = Mathf.Clamp01(state.Elapsed / state.Duration);
                float eased = EvaluateEase(state.Ease, t);
                state.Step?.Invoke(eased);

                if (t >= 1f)
                {
                    state.Complete?.Invoke();
                    state.Handle.MarkCompleted();
                    _tweens.RemoveAt(i);
                }
            }
        }

        private static float EvaluateEase(XwmEase ease, float t)
        {
            t = Mathf.Clamp01(t);

            switch (ease)
            {
                case XwmEase.Linear:
                    return t;
                case XwmEase.InQuad:
                    return t * t;
                case XwmEase.OutQuad:
                    return 1f - (1f - t) * (1f - t);
                case XwmEase.InOutQuad:
                    return t < 0.5f ? 2f * t * t : 1f - Mathf.Pow(-2f * t + 2f, 2f) * 0.5f;
                case XwmEase.InCubic:
                    return t * t * t;
                case XwmEase.OutCubic:
                    return 1f - Mathf.Pow(1f - t, 3f);
                case XwmEase.InOutCubic:
                    return t < 0.5f ? 4f * t * t * t : 1f - Mathf.Pow(-2f * t + 2f, 3f) * 0.5f;
                case XwmEase.InQuart:
                    return t * t * t * t;
                case XwmEase.OutQuart:
                    return 1f - Mathf.Pow(1f - t, 4f);
                case XwmEase.InOutQuart:
                    return t < 0.5f ? 8f * t * t * t * t : 1f - Mathf.Pow(-2f * t + 2f, 4f) * 0.5f;
                case XwmEase.InQuint:
                    return t * t * t * t * t;
                case XwmEase.OutQuint:
                    return 1f - Mathf.Pow(1f - t, 5f);
                case XwmEase.InOutQuint:
                    return t < 0.5f ? 16f * t * t * t * t * t : 1f - Mathf.Pow(-2f * t + 2f, 5f) * 0.5f;
                case XwmEase.InSine:
                    return 1f - Mathf.Cos((t * Mathf.PI) * 0.5f);
                case XwmEase.OutSine:
                    return Mathf.Sin((t * Mathf.PI) * 0.5f);
                case XwmEase.InOutSine:
                    return -(Mathf.Cos(Mathf.PI * t) - 1f) * 0.5f;
                case XwmEase.InExpo:
                    return t <= 0f ? 0f : Mathf.Pow(2f, 10f * t - 10f);
                case XwmEase.OutExpo:
                    return t >= 1f ? 1f : 1f - Mathf.Pow(2f, -10f * t);
                case XwmEase.InOutExpo:
                    if (t <= 0f)
                    {
                        return 0f;
                    }

                    if (t >= 1f)
                    {
                        return 1f;
                    }

                    if (t < 0.5f)
                    {
                        return Mathf.Pow(2f, 20f * t - 10f) * 0.5f;
                    }

                    return (2f - Mathf.Pow(2f, -20f * t + 10f)) * 0.5f;
                case XwmEase.InCirc:
                    return 1f - Mathf.Sqrt(1f - t * t);
                case XwmEase.OutCirc:
                    return Mathf.Sqrt(1f - (t - 1f) * (t - 1f));
                case XwmEase.InOutCirc:
                    if (t < 0.5f)
                    {
                        return (1f - Mathf.Sqrt(1f - 4f * t * t)) * 0.5f;
                    }

                    return (Mathf.Sqrt(1f - Mathf.Pow(-2f * t + 2f, 2f)) + 1f) * 0.5f;
                case XwmEase.InBack:
                {
                    const float c1 = 1.70158f;
                    const float c3 = c1 + 1f;
                    return c3 * t * t * t - c1 * t * t;
                }
                case XwmEase.OutBack:
                {
                    const float c1 = 1.70158f;
                    const float c3 = c1 + 1f;
                    float n = t - 1f;
                    return 1f + c3 * n * n * n + c1 * n * n;
                }
                case XwmEase.InOutBack:
                {
                    const float c1 = 1.70158f;
                    const float c2 = c1 * 1.525f;
                    if (t < 0.5f)
                    {
                        float n = 2f * t;
                        return n * n * ((c2 + 1f) * n - c2) * 0.5f;
                    }

                    {
                        float n = 2f * t - 2f;
                        return (n * n * ((c2 + 1f) * n + c2) + 2f) * 0.5f;
                    }
                }
                default:
                    return t;
            }
        }
    }

    public static class XwmTweens
    {
        public static XwmTweenHandle Value(GameObject owner, float from, float to, float duration, Action<float> onValue, XwmEase ease = XwmEase.OutCubic, Action onComplete = null, bool ignoreTimeScale = true)
        {
            if (onValue == null)
            {
                return CreateInactiveHandle();
            }

            XwmTweenRunner runner = EnsureRunner(owner);
            if (runner == null)
            {
                return CreateInactiveHandle();
            }

            return runner.Run(duration, ease, t =>
            {
                float value = Mathf.LerpUnclamped(from, to, t);
                onValue(value);
            }, onComplete, ignoreTimeScale);
        }

        public static XwmTweenHandle FadeWindow(XwmWindowHandle handle, float toOpacity, float duration, XwmEase ease = XwmEase.OutCubic, Action onComplete = null, bool ignoreTimeScale = true)
        {
            if (handle == null || handle.IsDestroyed)
            {
                return CreateInactiveHandle();
            }

            float from = handle.Opacity;
            float to = Mathf.Clamp01(toOpacity);
            return Value(handle.RootObject, from, to, duration, value => handle.Opacity = value, ease, onComplete, ignoreTimeScale);
        }

        public static XwmTweenHandle MoveWindow(XwmWindowHandle handle, Vector2 toPosition, float duration, XwmEase ease = XwmEase.OutCubic, Action onComplete = null, bool ignoreTimeScale = true)
        {
            if (handle == null || handle.IsDestroyed)
            {
                return CreateInactiveHandle();
            }

            Vector2 from = handle.Position;
            return Value(handle.RootObject, 0f, 1f, duration, value => handle.Position = Vector2.LerpUnclamped(from, toPosition, value), ease, onComplete, ignoreTimeScale);
        }

        public static XwmTweenHandle ResizeWindow(XwmWindowHandle handle, Vector2 toSize, float duration, XwmEase ease = XwmEase.OutCubic, Action onComplete = null, bool ignoreTimeScale = true)
        {
            if (handle == null || handle.IsDestroyed)
            {
                return CreateInactiveHandle();
            }

            Vector2 from = handle.Size;
            return Value(handle.RootObject, 0f, 1f, duration, value => handle.Size = Vector2.LerpUnclamped(from, toSize, value), ease, onComplete, ignoreTimeScale);
        }

        public static XwmTweenHandle ScaleWindow(XwmWindowHandle handle, Vector2 toScale, float duration, XwmEase ease = XwmEase.OutCubic, Action onComplete = null, bool ignoreTimeScale = true)
        {
            if (handle == null || handle.IsDestroyed || handle.RootRect == null)
            {
                return CreateInactiveHandle();
            }

            Vector2 from = new Vector2(handle.RootRect.localScale.x, handle.RootRect.localScale.y);
            return Value(handle.RootObject, 0f, 1f, duration, value =>
            {
                Vector2 scale = Vector2.LerpUnclamped(from, toScale, value);
                handle.RootRect.localScale = new Vector3(scale.x, scale.y, 1f);
            }, ease, onComplete, ignoreTimeScale);
        }

        public static XwmTweenHandle RotateWindow(XwmWindowHandle handle, float toRotation, float duration, XwmEase ease = XwmEase.OutCubic, Action onComplete = null, bool ignoreTimeScale = true)
        {
            if (handle == null || handle.IsDestroyed || handle.RootRect == null)
            {
                return CreateInactiveHandle();
            }

            float from = handle.RootRect.localEulerAngles.z;
            return Value(handle.RootObject, from, toRotation, duration, value => handle.RootRect.localEulerAngles = new Vector3(0f, 0f, value), ease, onComplete, ignoreTimeScale);
        }

        public static XwmTweenHandle MoveElement(XwmElementRef element, Vector2 toPosition, float duration, XwmEase ease = XwmEase.OutCubic, Action onComplete = null, bool ignoreTimeScale = true)
        {
            if (element == null || element.RectTransform == null)
            {
                return CreateInactiveHandle();
            }

            Vector2 from = element.Position;
            return Value(element.GameObject, 0f, 1f, duration, value => element.Position = Vector2.LerpUnclamped(from, toPosition, value), ease, onComplete, ignoreTimeScale);
        }

        public static XwmTweenHandle ResizeElement(XwmElementRef element, Vector2 toSize, float duration, XwmEase ease = XwmEase.OutCubic, Action onComplete = null, bool ignoreTimeScale = true)
        {
            if (element == null || element.RectTransform == null)
            {
                return CreateInactiveHandle();
            }

            Vector2 from = element.Size;
            return Value(element.GameObject, 0f, 1f, duration, value => element.Size = Vector2.LerpUnclamped(from, toSize, value), ease, onComplete, ignoreTimeScale);
        }

        public static XwmTweenHandle FadeElement(XwmElementRef element, float toAlpha, float duration, XwmEase ease = XwmEase.OutCubic, Action onComplete = null, bool ignoreTimeScale = true)
        {
            if (element == null)
            {
                return CreateInactiveHandle();
            }

            float from = element.GetAlpha();
            float to = Mathf.Clamp01(toAlpha);
            return Value(element.GameObject, from, to, duration, value => element.SetAlpha(value), ease, onComplete, ignoreTimeScale);
        }

        private static XwmTweenRunner EnsureRunner(GameObject owner)
        {
            GameObject resolved = ResolveOwner(owner);
            if (resolved == null)
            {
                return null;
            }

            XwmTweenRunner runner = resolved.GetComponent<XwmTweenRunner>();
            if (runner == null)
            {
                runner = resolved.AddComponent<XwmTweenRunner>();
            }

            return runner;
        }

        private static GameObject ResolveOwner(GameObject owner)
        {
            if (owner != null)
            {
                return owner;
            }

            if (WindowService.Instance != null)
            {
                return WindowService.Instance.gameObject;
            }

            return null;
        }

        private static XwmTweenHandle CreateInactiveHandle()
        {
            XwmTweenHandle handle = new XwmTweenHandle();
            handle.Cancel();
            return handle;
        }
    }
}
