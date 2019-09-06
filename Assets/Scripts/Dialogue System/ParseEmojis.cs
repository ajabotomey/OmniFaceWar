using GEmojiSharp;

public static class ParseEmojis
{
    public static string Parse(string text)
    {
        char[] separators = { '[', ']' };

        // Go through and detect parenthesis and make substrings
        string[] textChunks = text.Split(separators);

        // Find all chunks that start with \\
        for (int i = 0; i < textChunks.Length; i++) {
            //if (textChunks[i].Contains("\\")) {
            //    //string emoji = ConvertToUnicode(textChunks[i]);
            //    //textChunks[i] = emoji;
            //    //textChunks[i] = Parser.ParseEmoji(textChunks[i].Substring(1));
            //}
            if (textChunks[i].Contains(":")) {
                textChunks[i] = Emoji.Get(textChunks[i]).Raw;
            }
        }

        string result = "";

        // Reform the string
        for (int i = 0; i < textChunks.Length; i++) {
            result += textChunks[i];
        }

        return result;
    }

    private static string ConvertToUnicode(string iconUnicode)
    {
        // Remove the "\\u"
        iconUnicode = iconUnicode.Substring(3);

        int unicode = int.Parse(iconUnicode, System.Globalization.NumberStyles.HexNumber);
        string result = char.ConvertFromUtf32(unicode);

        return result;
    }
}
