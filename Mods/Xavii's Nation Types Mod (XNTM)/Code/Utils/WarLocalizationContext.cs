using System.Collections.Generic;

namespace XNTM.Code.Utils
{
    public static class WarLocalizationContext
    {
        [System.ThreadStatic]
        private static Stack<War> _stack;

        public static War CurrentWar
        {
            get
            {
                if (_stack == null || _stack.Count == 0)
                    return null;
                return _stack.Peek();
            }
        }

        public static void Push(War war)
        {
            _stack ??= new Stack<War>();
            _stack.Push(war);
        }

        public static void Pop()
        {
            if (_stack == null || _stack.Count == 0)
                return;
            _stack.Pop();
            if (_stack.Count == 0)
                _stack = null;
        }
    }
}
