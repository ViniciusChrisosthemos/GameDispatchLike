using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public static class StringUtils
{
    public static List<string> ExtractStringsInAngleBrackets(string input, string pattern)
    {
        List<string> extractedStrings = new List<string>();

        MatchCollection matches = Regex.Matches(input, pattern);

        foreach (Match match in matches)
        {
            // The first group (index 1) contains the captured string inside the brackets.
            extractedStrings.Add(match.Groups[1].Value);
        }

        return extractedStrings;
    }
}
