using System;
using System.Collections.Generic;
using System.Linq;

namespace Falcor.Router.Parser
{
    public class InputPathParser
    {
        public static IList<object> ParseInput(string text)
        {
            var result = new List<object>();
            var input = new InputBuffer(text);
            Parse(input, result);
            return (List<object>) result[0];
        }

        private static void Parse(InputBuffer input, IList<object> result)
        {
            input.SkipWhiteSpaces();
            ParseArray(input, result);
            ParseString(input, result);
            ParseNumber(input, result);
            ParseRange(input, result);
            input.SkipWhiteSpaces();
        }

        private static void ParseRange(InputBuffer input, IList<object> result)
        {
            if (input.Current != Symbols.FullStop)
            {
                return;
            }

            const string msg = "The full stop is expected only in range xx..yy";
            var last = result.LastOrDefault();

            if (last == null || last.GetType() != typeof (int))
            {
                throw new ParsingException(msg);
            }

            input.Next();
            if (input.Current != Symbols.FullStop)
            {
                throw new ParsingException(msg);
            }

            input.Next();
            if (input.Current == Symbols.FullStop) // this is support for ...
            {
                input.Next();
            }

            if (!char.IsNumber(input.Current))
            {
                throw new ParsingException(msg);
            }
            
            var from = (int) result.Last();
            result.RemoveAt(result.Count - 1);
            ParseNumber(input, result);
            var range = new RangeValue
            {
                From = from,
                To = (int)result.Last()
            };
            result.RemoveAt(result.Count - 1);
            result.Add(range);
        }

        private static void ParseString(InputBuffer input, ICollection<object> result)
        {
            var expected = char.MinValue;

            if (input.Current == Symbols.Quotation)
            {
                expected = Symbols.Quotation;
            } 
            else if (input.Current == Symbols.Apostrophe)
            {
                expected = Symbols.Apostrophe;
            }
            else if (expected == char.MinValue)
            {
                return;
            }

            input.Next();
            result.Add(input.GetText(i => i != expected));
            input.Next();
        }

        private static void ParseNumber(InputBuffer input, ICollection<object> result)
        {
            if (!char.IsNumber(input.Current))
            {
                return;
            }

            result.Add(int.Parse(input.GetText(char.IsNumber)));            
        }

        private static void ParseArray(InputBuffer input, ICollection<object> result)
        {
            if (input.Current != Symbols.OpeningBracket)
            {
                return;
            }

            var array = new List<object>();
            result.Add(array);

            input.Next();
            Parse(input, array);

            var hasNextItem = input.Current == Symbols.Comma;
            while (hasNextItem)
            {
                input.Next();
                Parse(input, array);

                hasNextItem = input.Current == Symbols.Comma;
            }

            if (input.Current != Symbols.ClosingBracket)
                throw new ArgumentException("The closing bracket ']' is expected");

            input.Next();
        }        

        private static class Symbols
        {
            public const char OpeningBracket = '[';
            public const char ClosingBracket = ']';
            public const char Apostrophe = '\'';
            public const char Quotation = '"';
            public const char Separator = ',';
            public const char Space = ' ';
            public const char Comma = ',';
            public const char FullStop = '.';
        }
    }
}