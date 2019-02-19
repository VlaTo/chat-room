using Microsoft.Extensions.Logging;
using System;
using System.Globalization;
using System.Text;

namespace LibraProgramming.ChatRoom.Common.Core
{
    public static class MessageDebug
    {
        public static void DebugWriteArray(ILogger logger, byte[] bytes)
        {
            var formatProvider = CultureInfo.InvariantCulture;
            var offset = 0;

            while (true)
            {
                var line = new StringBuilder();
                var count = Math.Max(0, Math.Min(16, bytes.Length - offset));

                if (0 == count)
                {

                    break;
                }

                line.AppendFormat(formatProvider, "{0:X04}", offset).Append(' ', 2);

                for (var index = 0; index < count; index++)
                {
                    if (8 == index)
                    {
                        line.Append(' ');
                    }

                    line.AppendFormat(formatProvider, "{0:X02}", bytes[offset + index]).Append(' ');
                }

                line.Append(' ', 57 - line.Length);

                for (var index = 0; index < count; index++)
                {
                    if (8 == index)
                    {
                        line.Append(' ');
                    }

                    var ch = (char)bytes[offset + index];

                    line
                        .Append(IsPrintable(ch) ? ch : '.')
                        .Append(' ');
                }

                logger.LogDebug(line.ToString());

                offset += count;
            }
        }

        private static bool IsPrintable(char ch)
        {
            return Char.IsLetterOrDigit(ch) || Char.IsPunctuation(ch) || Char.IsSeparator(ch);
        }
    }
}