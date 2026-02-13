using System;
using UnityEngine;

namespace XaviiWindowsMod.API
{
    public static class XwmAnimationExtensions
    {
        public static XwmTweenHandle FadeTo(this XwmWindowHandle handle, float opacity, float duration, XwmEase ease = XwmEase.OutCubic, Action onComplete = null, bool ignoreTimeScale = true)
        {
            return XwmTweens.FadeWindow(handle, opacity, duration, ease, onComplete, ignoreTimeScale);
        }

        public static XwmTweenHandle MoveTo(this XwmWindowHandle handle, Vector2 position, float duration, XwmEase ease = XwmEase.OutCubic, Action onComplete = null, bool ignoreTimeScale = true)
        {
            return XwmTweens.MoveWindow(handle, position, duration, ease, onComplete, ignoreTimeScale);
        }

        public static XwmTweenHandle ResizeTo(this XwmWindowHandle handle, Vector2 size, float duration, XwmEase ease = XwmEase.OutCubic, Action onComplete = null, bool ignoreTimeScale = true)
        {
            return XwmTweens.ResizeWindow(handle, size, duration, ease, onComplete, ignoreTimeScale);
        }

        public static XwmTweenHandle ScaleTo(this XwmWindowHandle handle, Vector2 scale, float duration, XwmEase ease = XwmEase.OutCubic, Action onComplete = null, bool ignoreTimeScale = true)
        {
            return XwmTweens.ScaleWindow(handle, scale, duration, ease, onComplete, ignoreTimeScale);
        }

        public static XwmTweenHandle RotateTo(this XwmWindowHandle handle, float rotation, float duration, XwmEase ease = XwmEase.OutCubic, Action onComplete = null, bool ignoreTimeScale = true)
        {
            return XwmTweens.RotateWindow(handle, rotation, duration, ease, onComplete, ignoreTimeScale);
        }

        public static XwmTweenHandle FadeTo(this XwmElementRef element, float alpha, float duration, XwmEase ease = XwmEase.OutCubic, Action onComplete = null, bool ignoreTimeScale = true)
        {
            return XwmTweens.FadeElement(element, alpha, duration, ease, onComplete, ignoreTimeScale);
        }

        public static XwmTweenHandle MoveTo(this XwmElementRef element, Vector2 position, float duration, XwmEase ease = XwmEase.OutCubic, Action onComplete = null, bool ignoreTimeScale = true)
        {
            return XwmTweens.MoveElement(element, position, duration, ease, onComplete, ignoreTimeScale);
        }

        public static XwmTweenHandle ResizeTo(this XwmElementRef element, Vector2 size, float duration, XwmEase ease = XwmEase.OutCubic, Action onComplete = null, bool ignoreTimeScale = true)
        {
            return XwmTweens.ResizeElement(element, size, duration, ease, onComplete, ignoreTimeScale);
        }
    }
}
