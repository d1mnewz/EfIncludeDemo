using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace EfIncludeDemo.Data.Infrastructure
{
 public static class StringExtensions
  {
    private static readonly Random Random = new Random((int) DateTime.Now.Ticks);
    private const string Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

    public static string StripHtml(this string text)
    {
      return Regex.Replace(text, "<(.|\\n)*?>", string.Empty);
    }

    public static string ToUrlAlias(this string value, IDictionary<string, string> charReplacements, bool replaceDoubleDashes, bool stripNonAscii, bool urlEncode)
    {
      value = value.ToLowerInvariant();
      charReplacements = charReplacements ?? (IDictionary<string, string>) new Dictionary<string, string>();
      value = charReplacements.Aggregate<KeyValuePair<string, string>, string>(value, (Func<string, KeyValuePair<string, string>, string>) ((current, kvp) => current.Replace(kvp.Key, kvp.Value)));
      if (stripNonAscii)
      {
        value = Encoding.ASCII.GetString(Encoding.Convert(Encoding.UTF8, Encoding.GetEncoding(Encoding.ASCII.EncodingName, (EncoderFallback) new EncoderReplacementFallback(string.Empty), (DecoderFallback) new DecoderExceptionFallback()), Encoding.UTF8.GetBytes(value)));
        IEnumerable<int> validCodeRanges = Enumerable.Range(48, 10).Concat<int>(Enumerable.Range(97, 26));
        StringBuilder stringBuilder = new StringBuilder();
        foreach (char ch in value.Where<char>((Func<char, bool>) (c =>
        {
          if (!charReplacements.Values.Contains(c.ToString()))
            return validCodeRanges.Contains<int>((int) c);
          return true;
        })))
          stringBuilder.Append(ch);
        value = stringBuilder.ToString();
      }
      value = value.Trim('-', '_');
      value = replaceDoubleDashes ? Regex.Replace(value, "([-_]){2,}", "$1", RegexOptions.Compiled) : value;
      if (!urlEncode)
        return value;
      return WebUtility.UrlEncode(value);
    }

    public static bool IsEmptyString(this string str)
    {
      return string.IsNullOrWhiteSpace(str);
    }

    public static string CreateUrlSafeTitle(this string title)
    {
      if (title.IsEmptyString())
        return (string) null;
      char[] charArray = title.ToCharArray();
      int length1 = charArray.Length;
      for (int index = 0; index < length1; ++index)
        charArray[index] = char.IsLetterOrDigit(charArray[index]) ? char.ToLowerInvariant(charArray[index]) : '-';
      int length2 = 0;
      for (int index = 0; index < length1; ++index)
      {
        if ((int) charArray[index] != 45 || length2 != 0 && (int) charArray[length2 - 1] != 45)
        {
          charArray[length2] = charArray[index];
          ++length2;
        }
      }
      string str = new string(charArray, 0, length2).Trim('-');
      if (!str.IsEmptyString())
        return str;
      return (string) null;
    }

    public static string UrlFriendly(this string title)
    {
      if (title == null)
        return "";
      int length1 = title.Length;
      bool flag = false;
      StringBuilder stringBuilder = new StringBuilder(length1);
      for (int index = 0; index < length1; ++index)
      {
        char ch = title[index];
        if ((int) ch >= 97 && (int) ch <= 122 || (int) ch >= 48 && (int) ch <= 57)
        {
          stringBuilder.Append(ch);
          flag = false;
        }
        else if ((int) ch >= 65 && (int) ch <= 90)
        {
          stringBuilder.Append((char) ((uint) ch | 32U));
          flag = false;
        }
        else if ((int) ch == 32 || (int) ch == 44 || ((int) ch == 46 || (int) ch == 47) || ((int) ch == 92 || (int) ch == 45 || ((int) ch == 95 || (int) ch == 61)))
        {
          if (!flag && stringBuilder.Length > 0)
          {
            stringBuilder.Append('-');
            flag = true;
          }
        }
        else if ((int) ch >= 128)
        {
          int length2 = stringBuilder.Length;
          stringBuilder.Append(ch);
          int length3 = stringBuilder.Length;
          if (length2 != length3)
            flag = false;
        }
        if (index == 80)
          break;
      }
      if (flag)
        --stringBuilder.Length;
      return stringBuilder.ToString();
    }

    public static string ValueOrDefault(this string str, params string[] vals)
    {
      if (string.IsNullOrEmpty(str))
        return ((IEnumerable<string>) vals).FirstOrDefault<string>((Func<string, bool>) (e => !string.IsNullOrEmpty(e))) ?? "";
      return str;
    }

    public static string UrlCombine(this string uri1, string uri2)
    {
      uri1 = uri1 ?? "";
      uri2 = uri2 ?? "";
      uri1 = uri1.TrimEnd('/');
      uri2 = uri2.TrimStart('/');
      return string.Format("{0}/{1}", (object) uri1, (object) uri2);
    }

    public static string UrlCombineQuery(this string baseUrl, Dictionary<string, string> query, bool encode = false)
    {
      if (query.IsEmpty<KeyValuePair<string, string>>())
        return baseUrl;
      string str = query.MakeString<KeyValuePair<string, string>>((Func<KeyValuePair<string, string>, string>) (pair => pair.Key + "=" + (encode ? WebUtility.UrlEncode(pair.Value) : pair.Value)), "&");
      return baseUrl + "?" + str;
    }

    public static string UrlSafeCombineQuery(this string baseUrl, Dictionary<string, string> query, bool encode = false)
    {
      if (baseUrl.IsEmptyString())
        return (string) null;
      if (query.IsEmpty<KeyValuePair<string, string>>())
        return baseUrl;
      string str = query.MakeString<KeyValuePair<string, string>>((Func<KeyValuePair<string, string>, string>) (pair => pair.Key + "=" + (encode ? WebUtility.UrlEncode(pair.Value) : pair.Value)), "&");
      if (baseUrl.Contains("?"))
        return baseUrl + "&" + str;
      return baseUrl + "?" + str;
    }

    public static string Shorten(this string value, int maxCharLength, string postfix = "...")
    {
      if (string.IsNullOrEmpty(value) || value.Length <= maxCharLength)
        return value;
      return value.Substring(0, maxCharLength - postfix.Length) + postfix;
    }

    public static string ShortenByWords(this string value, int maxCharLength, string postfix = "...")
    {
      if (string.IsNullOrEmpty(value) || value.Length < maxCharLength || value.Length - maxCharLength + postfix.Length < maxCharLength)
        return value;
      string[] strArray = value.Split(' ');
      StringBuilder stringBuilder = new StringBuilder();
      for (int index = 0; stringBuilder.Length < maxCharLength && index < strArray.Length; ++index)
      {
        stringBuilder.Append(strArray[index]);
        if (index < strArray.Length - 1)
          stringBuilder.Append(" ");
      }
      return stringBuilder.ToString() + postfix;
    }

    public static string Remove(this string value, char[] characters)
    {
      if (characters == null)
        return (string) null;
      return ((IEnumerable<char>) characters).Aggregate<char, string>(value, (Func<string, char, string>) ((current, c) => current.Replace(c.ToString(), "")));
    }

    public static string Remove(this string value, string[] strings)
    {
      if (strings == null)
        return (string) null;
      return ((IEnumerable<string>) strings).Aggregate<string, string>(value, (Func<string, string, string>) ((current, c) => current.Replace(c, "")));
    }

    public static string GenerateRandomAlphaNumericString(int length = 8)
    {
      return new string(Enumerable.Repeat<string>("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", length).Select<string, char>((Func<string, char>) (s => s[StringExtensions.Random.Next(s.Length)])).ToArray<char>());
    }

    public static string GenerateRandomNumericString(int length = 4)
    {
      return StringExtensions.Random.Next(10000).ToString("D4");
    }

    public static string ReplaceFirst(this string text, string search, string replace)
    {
      int length = text.IndexOf(search, StringComparison.Ordinal);
      if (length < 0)
        return text;
      return text.Substring(0, length) + replace + text.Substring(length + search.Length);
    }

    public static List<string> SplitCamelCase(this string stringToSplit)
    {
      if (stringToSplit.IsEmptyString())
        return new List<string>();
      List<string> stringList = new List<string>();
      bool flag = false;
      StringBuilder stringBuilder = new StringBuilder();
      foreach (char c in stringToSplit)
      {
        if (char.IsUpper(c))
        {
          if (flag)
          {
            stringBuilder.Append(c);
          }
          else
          {
            if (stringBuilder.Length > 0)
            {
              stringList.Add(stringBuilder.ToString());
              stringBuilder.Length = 0;
            }
            stringBuilder.Append(c);
          }
          flag = true;
        }
        else
        {
          if (flag && stringBuilder.Length > 1)
          {
            char ch = stringBuilder[stringBuilder.Length - 1];
            --stringBuilder.Length;
            stringList.Add(stringBuilder.ToString());
            stringBuilder.Length = 0;
            stringBuilder.Append(ch);
          }
          flag = false;
          if (stringBuilder.Length == 0)
            stringBuilder.Append(char.ToUpper(c));
          else
            stringBuilder.Append(c);
        }
      }
      if (stringBuilder.Length > 0)
        stringList.Add(stringBuilder.ToString());
      return stringList;
    }

    public static string SplitCamelCaseToString(this string stringToSplit)
    {
      return stringToSplit.SplitCamelCase().MakeString<string>(" ");
    }

    public static bool SafeEquals(this string str, string other, StringComparison sc = StringComparison.InvariantCultureIgnoreCase)
    {
      if (str == null && other == null)
        return true;
      if (str != null)
        return str.Equals(other, sc);
      return false;
    }

    public static bool SafeEndsWith(this string str, string other, StringComparison sc = StringComparison.InvariantCultureIgnoreCase)
    {
      if (str != null && other != null)
        return str.EndsWith(other, sc);
      return false;
    }

    public static bool SafeContains(this string str, string other, StringComparison sc = StringComparison.InvariantCultureIgnoreCase)
    {
      if (str != null && other != null)
        return str.IndexOf(other, sc) >= 0;
      return false;
    }

    public static bool IsDigitsOnly(this string str)
    {
      if (str != null)
        return str.All<char>((Func<char, bool>) (c =>
        {
          if ((int) c >= 48)
            return (int) c <= 57;
          return false;
        }));
      return true;
    }
  }
}