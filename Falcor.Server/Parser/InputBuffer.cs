using System;

namespace Falcor.Server.Parser
{
    public class InputBuffer
    {
        public InputBuffer(string text)
        {
            Text = text;
            _position = 0;
        }

        private bool _endOfText;
        private int _position;

        public string Text { get; set; }

        public char Current
        {
            get
            {                
                return _endOfText ? Char.MaxValue : Text[_position];
            }
        }

        public bool Next()
        {
            if (_endOfText)
                return false;

            _position++;
            _endOfText = _position >= Text.Length;
            return !_endOfText;
        }

        public void SkipWhiteSpaces()
        {
            SkipWhile(Char.IsWhiteSpace);
        }

        public void SkipWhile(Func<char, bool> whileFunc)
        {
            var skipCurrent = whileFunc(Current);
            while (skipCurrent)
            {
                Next();
                skipCurrent = whileFunc(Current);
            }
        }

        public string GetText(Func<char, bool> whileFunc)
        {
            var pos = _position;
            SkipWhile(whileFunc);
            return Text.Substring(pos, _position - pos);
        }
    }
}