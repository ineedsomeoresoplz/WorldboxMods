using System.Collections.Generic;

namespace XNTM.Code.Utils
{
    public static class LocalizationContext
    {
        [System.ThreadStatic]
        private static Stack<Kingdom> _stack;

        public static Kingdom CurrentKingdom
        {
            get
            {
                if (_stack == null || _stack.Count == 0)
                    return null;
                return _stack.Peek();
            }
        }

        public static void Push(Kingdom kingdom)
        {
            _stack ??= new Stack<Kingdom>();
            _stack.Push(kingdom);
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
