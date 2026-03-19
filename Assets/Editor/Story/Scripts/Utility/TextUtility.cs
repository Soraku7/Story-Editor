using System.Text;

public static class TextUtility
{
    public static bool IsWiteSpace(this char character)
    {
        switch (character)
        {
            case '\u0020':
            case '\u00A0':
            case '\u1680':
            case '\u2000':
            case '\u2001':
            case '\u2002':
            case '\u2003':
            case '\u2004':
            case '\u2005':
            case '\u2006':
            case '\u2007':
            case '\u2008':
            case '\u2009':
            case '\u200A':
            case '\u202F':
            case '\u205F':
            case '\u3000':
            case '\u2028':
            case '\u2029':
            case '\u000A':
            case '\u000B':
            case '\u000C':
            case '\u000D':
            case '\u0085':
                return true;
            default:
                return false;
        }
    }

    public static bool IsSpecialCharacter(this char character)
    {
        bool isLetterOrDigit = char.IsLetterOrDigit(character);
        bool isWhiteSpace = character.IsWiteSpace();
        bool isOther = character == '-' || character == '_' || character == '.' || character == '(' || character == ')';

        return !isLetterOrDigit && !isWhiteSpace && !isOther;
    }

    public static bool HasWhitespace(this string text)
    {
        foreach (char character in text)
        {
            if (character.IsWiteSpace())
            {
                return true;
            }
        }

        return false;
    }
    
    public static bool HasSpecialCharacter(this string text)
    {
        foreach (char character in text)
        {
            if (character.IsSpecialCharacter())
            {
                return true;
            }
        }

        return false;
    }

    public static string RemoveWhitespace(this string text)
    {
        int textLength = text.Length;
        char[] textCharacters = text.ToCharArray();
        int currentWhitespacelessTextLength = 0;

        for (int currentCharacterIndex = 0; currentCharacterIndex < textLength; currentCharacterIndex++)
        {
            char currentTextCharacter = textCharacters[currentCharacterIndex];

            if (currentTextCharacter.IsWiteSpace())
            {
                continue;
            }
            textCharacters[currentWhitespacelessTextLength++] = currentTextCharacter;
        }

        return new string(textCharacters, 0, currentWhitespacelessTextLength);
    }
    
    public static string RemoveSpecialCharacters(this string text)
    {
        int textLength = text.Length;
        char[] textCharacters = text.ToCharArray();
        int currentWhitespacelessTextLength = 0;

        for (int currentCharacterIndex = 0; currentCharacterIndex < textLength; currentCharacterIndex++)
        {
            char currentTextCharacter = textCharacters[currentCharacterIndex];

            if (currentTextCharacter.IsSpecialCharacter())
            {
                continue;
            }
            textCharacters[currentWhitespacelessTextLength++] = currentTextCharacter;
        }

        return new string(textCharacters, 0, currentWhitespacelessTextLength);
    }
}