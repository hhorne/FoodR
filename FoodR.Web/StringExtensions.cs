using System;
using System.Text.RegularExpressions;

public static class StringExtensions
{
	public static bool IsNullOrEmpty(this string str)
	{
		return string.IsNullOrEmpty(str);
	}

	public static string ConvertToUrlSlug(this string str)
	{
		string result = Regex.Replace(str, @"[^a-z0-9\s-]", " "); // non-alphanumerics into spaces
		result = Regex.Replace(str, @"[\s-]{2,}", " ").Trim(); // multiple spaces/hyphes into one space
		result = Regex.Replace(str, @"\s", "_"); // spaces into underscores 
		return result;
	}
}