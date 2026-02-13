using System;
using System.Collections.Generic;

namespace XaviiWindowsMod.Xwm
{
    internal static class XwmTypeLibrary
    {
        public const string Frame = "Frame";
        public const string ScrollingFrame = "ScrollingFrame";
        public const string TextLabel = "TextLabel";
        public const string ImageLabel = "ImageLabel";
        public const string TextButton = "TextButton";
        public const string TextBox = "TextBox";
        public const string ImageButton = "ImageButton";
        public const string UICorner = "UICorner";
        public const string UIScale = "UIScale";
        public const string UIListLayout = "UIListLayout";
        public const string UIDragDetector = "UIDragDetector";
        public const string UIGradient = "UIGradient";
        public const string UIGridLayout = "UIGridLayout";
        public const string UIPadding = "UIPadding";
        public const string UIPageLayout = "UIPageLayout";
        public const string UITableLayout = "UITableLayout";

        public static readonly string[] CreatableTypes =
        {
            Frame,
            ScrollingFrame,
            TextLabel,
            ImageLabel,
            TextButton,
            TextBox,
            ImageButton,
            UICorner,
            UIScale,
            UIListLayout,
            UIDragDetector,
            UIGradient,
            UIGridLayout,
            UIPadding,
            UIPageLayout,
            UITableLayout
        };

        private static readonly HashSet<string> HelperTypes = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            UICorner,
            UIScale,
            UIListLayout,
            UIDragDetector,
            UIGradient,
            UIGridLayout,
            UIPadding,
            UIPageLayout,
            UITableLayout
        };

        private static readonly HashSet<string> ClickableTypes = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            TextButton,
            ImageButton
        };

        public static bool IsSupported(string type)
        {
            if (string.IsNullOrWhiteSpace(type))
            {
                return false;
            }

            for (int i = 0; i < CreatableTypes.Length; i++)
            {
                if (string.Equals(CreatableTypes[i], type, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        public static bool IsHelperType(string type)
        {
            return !string.IsNullOrWhiteSpace(type) && HelperTypes.Contains(type);
        }

        public static bool IsClickable(string type)
        {
            return !string.IsNullOrWhiteSpace(type) && ClickableTypes.Contains(type);
        }

        public static string Normalize(string type)
        {
            if (string.IsNullOrWhiteSpace(type))
            {
                return Frame;
            }

            for (int i = 0; i < CreatableTypes.Length; i++)
            {
                if (string.Equals(CreatableTypes[i], type, StringComparison.OrdinalIgnoreCase))
                {
                    return CreatableTypes[i];
                }
            }

            return Frame;
        }
    }
}
