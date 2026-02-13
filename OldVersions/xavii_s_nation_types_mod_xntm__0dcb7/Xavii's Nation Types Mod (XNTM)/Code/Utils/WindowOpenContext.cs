namespace XNTM.Code.Utils
{
    public static class WindowOpenContext
    {
        [System.ThreadStatic]
        private static string _windowId;

        public static string CurrentWindowId => _windowId;

        public static void Set(string windowId)
        {
            _windowId = windowId;
        }

        public static void Clear()
        {
            _windowId = null;
        }
    }
}
