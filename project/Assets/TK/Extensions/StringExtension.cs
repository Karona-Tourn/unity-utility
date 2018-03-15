using System.Text.RegularExpressions;

public static class StringExtension
{
	public static string WithSpace (this string value)
	{
		return Regex.Replace (value, "[A-Z]", " $0");
	}

	public static bool IsNullOrEmpty (this string value)
	{
		return string.IsNullOrEmpty (value);
	}

	public static string ClampString (this string value, int max, string suffix = "")
	{
		int length = value.Length;
		if (length > max)
		{
			string result = value.Substring (0, max);
			if (!string.IsNullOrEmpty (suffix))
				result += suffix;
			return result;
		}
		else
			return value;
	}
}
