using System;

namespace XaviiWindowsMod.API
{
    public static class XwmElementSelector
    {
        public static bool Matches(XwmElementRef element, string selector)
        {
            if (element == null || string.IsNullOrWhiteSpace(selector))
            {
                return false;
            }

            string[] groups = selector.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            if (groups.Length == 0)
            {
                return false;
            }

            for (int i = 0; i < groups.Length; i++)
            {
                if (MatchesAll(element, groups[i]))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool MatchesAll(XwmElementRef element, string rawGroup)
        {
            if (element == null)
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(rawGroup))
            {
                return false;
            }

            string[] tokens = SplitTokens(rawGroup);
            if (tokens.Length == 0)
            {
                return false;
            }

            for (int i = 0; i < tokens.Length; i++)
            {
                if (!MatchesToken(element, tokens[i]))
                {
                    return false;
                }
            }

            return true;
        }

        private static string[] SplitTokens(string group)
        {
            if (group.IndexOf('&') >= 0)
            {
                return group.Split(new[] { '&' }, StringSplitOptions.RemoveEmptyEntries);
            }

            return group.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        }

        private static bool MatchesToken(XwmElementRef element, string rawToken)
        {
            if (element == null || string.IsNullOrWhiteSpace(rawToken))
            {
                return false;
            }

            string token = rawToken.Trim();
            if (token == "*")
            {
                return true;
            }

            if (token.StartsWith("#", StringComparison.Ordinal))
            {
                return element.IsId(token.Substring(1));
            }

            if (token.StartsWith(".", StringComparison.Ordinal))
            {
                return element.IsName(token.Substring(1));
            }

            if (token.StartsWith("@", StringComparison.Ordinal))
            {
                return element.IsType(token.Substring(1));
            }

            int colon = token.IndexOf(':');
            if (colon > 0)
            {
                string key = token.Substring(0, colon).Trim();
                string value = token.Substring(colon + 1).Trim();
                return MatchesKeyValue(element, key, value);
            }

            return element.Matches(token) || element.IsType(token);
        }

        private static bool MatchesKeyValue(XwmElementRef element, string key, string value)
        {
            if (element == null || string.IsNullOrWhiteSpace(key))
            {
                return false;
            }

            if (string.Equals(key, "id", StringComparison.OrdinalIgnoreCase))
            {
                return element.IsId(value);
            }

            if (string.Equals(key, "name", StringComparison.OrdinalIgnoreCase))
            {
                return element.IsName(value);
            }

            if (string.Equals(key, "type", StringComparison.OrdinalIgnoreCase) || string.Equals(key, "t", StringComparison.OrdinalIgnoreCase))
            {
                return element.IsType(value);
            }

            if (string.Equals(key, "contains", StringComparison.OrdinalIgnoreCase))
            {
                return ContainsIgnoreCase(element.Id, value) || ContainsIgnoreCase(element.Name, value) || ContainsIgnoreCase(element.Type, value);
            }

            if (string.Equals(key, "path", StringComparison.OrdinalIgnoreCase))
            {
                return ContainsIgnoreCase(element.HierarchyPath, value);
            }

            if (string.Equals(key, "text", StringComparison.OrdinalIgnoreCase))
            {
                return string.Equals(element.GetText(), value ?? string.Empty, StringComparison.OrdinalIgnoreCase);
            }

            if (string.Equals(key, "active", StringComparison.OrdinalIgnoreCase))
            {
                if (TryParseBool(value, out bool parsed))
                {
                    return element.IsActive == parsed;
                }

                return false;
            }

            if (string.Equals(key, "visible", StringComparison.OrdinalIgnoreCase))
            {
                if (TryParseBool(value, out bool parsed))
                {
                    return element.IsVisibleInHierarchy == parsed;
                }

                return false;
            }

            if (string.Equals(key, "interactable", StringComparison.OrdinalIgnoreCase))
            {
                if (TryParseBool(value, out bool parsed))
                {
                    return element.GetInteractable() == parsed;
                }

                return false;
            }

            if (string.Equals(key, "has", StringComparison.OrdinalIgnoreCase))
            {
                if (string.Equals(value, "image", StringComparison.OrdinalIgnoreCase))
                {
                    return element.HasImage;
                }

                if (string.Equals(value, "text", StringComparison.OrdinalIgnoreCase))
                {
                    return element.HasText || element.HasInputField;
                }

                if (string.Equals(value, "button", StringComparison.OrdinalIgnoreCase))
                {
                    return element.HasButton;
                }

                if (string.Equals(value, "input", StringComparison.OrdinalIgnoreCase))
                {
                    return element.HasInputField;
                }
            }

            return false;
        }

        private static bool TryParseBool(string value, out bool parsed)
        {
            parsed = false;
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            if (bool.TryParse(value, out parsed))
            {
                return true;
            }

            if (string.Equals(value, "1", StringComparison.OrdinalIgnoreCase) || string.Equals(value, "yes", StringComparison.OrdinalIgnoreCase) || string.Equals(value, "on", StringComparison.OrdinalIgnoreCase))
            {
                parsed = true;
                return true;
            }

            if (string.Equals(value, "0", StringComparison.OrdinalIgnoreCase) || string.Equals(value, "no", StringComparison.OrdinalIgnoreCase) || string.Equals(value, "off", StringComparison.OrdinalIgnoreCase))
            {
                parsed = false;
                return true;
            }

            return false;
        }

        private static bool ContainsIgnoreCase(string value, string query)
        {
            if (string.IsNullOrWhiteSpace(value) || string.IsNullOrWhiteSpace(query))
            {
                return false;
            }

            return value.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0;
        }
    }
}
