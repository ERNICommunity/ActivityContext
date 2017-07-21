using System.IO;

namespace ActivityContext.Serialization
{
    internal static class JsonUtils
    {
        // This code was taken from Jil - https://github.com/kevin-montrose/Jil
        // Appreciation goes to its original author. Thanks :)
        public static void JsonEscapeFast(string str, TextWriter output)
        {
            for (var i = 0; i < str.Length; i++)
            {
                var c = str[i];

                if (c == '\\')
                {
                    output.Write(@"\\");
                    continue;
                }

                if (c == '"')
                {
                    output.Write(@"\""");
                    continue;
                }

                switch (c)
                {
                    case '\u0000':
                        output.Write(@"\u0000");
                        continue;
                    case '\u0001':
                        output.Write(@"\u0001");
                        continue;
                    case '\u0002':
                        output.Write(@"\u0002");
                        continue;
                    case '\u0003':
                        output.Write(@"\u0003");
                        continue;
                    case '\u0004':
                        output.Write(@"\u0004");
                        continue;
                    case '\u0005':
                        output.Write(@"\u0005");
                        continue;
                    case '\u0006':
                        output.Write(@"\u0006");
                        continue;
                    case '\u0007':
                        output.Write(@"\u0007");
                        continue;
                    case '\u0008':
                        output.Write(@"\u0008");
                        continue;
                    case '\u0009':
                        output.Write(@"\t");
                        continue;
                    case '\u000A':
                        output.Write(@"\n");
                        continue;
                    case '\u000B':
                        output.Write(@"\v");
                        continue;
                    case '\u000C':
                        output.Write(@"\f");
                        continue;
                    case '\u000D':
                        output.Write(@"\r");
                        continue;
                    case '\u000E':
                        output.Write(@"\u000E");
                        continue;
                    case '\u000F':
                        output.Write(@"\u000F");
                        continue;
                    case '\u0010':
                        output.Write(@"\u0010");
                        continue;
                    case '\u0011':
                        output.Write(@"\u0011");
                        continue;
                    case '\u0012':
                        output.Write(@"\u0012");
                        continue;
                    case '\u0013':
                        output.Write(@"\u0013");
                        continue;
                    case '\u0014':
                        output.Write(@"\u0014");
                        continue;
                    case '\u0015':
                        output.Write(@"\u0015");
                        continue;
                    case '\u0016':
                        output.Write(@"\u0016");
                        continue;
                    case '\u0017':
                        output.Write(@"\u0017");
                        continue;
                    case '\u0018':
                        output.Write(@"\u0018");
                        continue;
                    case '\u0019':
                        output.Write(@"\u0019");
                        continue;
                    case '\u001A':
                        output.Write(@"\u001A");
                        continue;
                    case '\u001B':
                        output.Write(@"\u001B");
                        continue;
                    case '\u001C':
                        output.Write(@"\u001C");
                        continue;
                    case '\u001D':
                        output.Write(@"\u001D");
                        continue;
                    case '\u001E':
                        output.Write(@"\u001E");
                        continue;
                    case '\u001F':
                        output.Write(@"\u001F");
                        continue;
                }

                output.Write(c);
            }
        }
    }
}