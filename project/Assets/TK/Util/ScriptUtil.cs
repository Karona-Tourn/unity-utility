using UnityEngine;
using UnityEngine.Events;
using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.Globalization;

public static class ScriptUtil
{
	public static void EnumerateChildren (Transform parent, UnityAction<Transform> action)
	{
		if (action == null)
		{
			return;
		}

		foreach (Transform child in parent)
		{
			action (child);
			EnumerateChildren (child, action);
		}
	}

	public static Color RGBA2Color (float r, float g, float b, float a)
	{
		return new Color (r / 255f, g / 255f, b / 255f, a / 255f);
	}

	public static float DistanceIgnoreY (Vector3 a, Vector3 b)
	{
		Vector3 dir = a - b;
		dir.y = 0;
		return dir.magnitude;
	}

	public static Vector3 DirectionIgnoreY (Vector3 from, Vector3 to)
	{
		Vector3 dir = to - from;
		dir.y = 0;
		return dir;
	}

	public static DateTime ConvertDateTimeISO2Local (string dateTime)
	{
		return TimeZone.CurrentTimeZone.ToLocalTime (DateTime.Parse (dateTime));
	}

	public static DateTime ConvertDateTimeLocal2ISO (string dateTime)
	{
		return TimeZone.CurrentTimeZone.ToUniversalTime (DateTime.Parse (dateTime));
	}

	public static string AbbreviateLongNumber (double value)
	{
		string result = "";
		if (value > 99999999)
		{
			result = value.ToString ("#,##0,,,B", CultureInfo.InvariantCulture);
		}
		else if (value > 999999)
		{
			result = value.ToString ("#,##0,,M", CultureInfo.InvariantCulture);
		}
		else if (value > 99999)
		{
			result = value.ToString ("#,##0,K", CultureInfo.InvariantCulture);
		}
		else if (value > 999)
		{
			result = value.ToString ("#,#", CultureInfo.InvariantCulture);
		}
		else
		{
			result = value.ToString ();
		}
		return result;
	}

	public static T CopyFrom<T> (object from, params string[] ignoreFields) where T : new()
	{
		T to = new T ();
		FieldInfo[] fromF = from.GetType ().GetFields (BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).Where (e => !ignoreFields.Contains (e.Name)).ToArray ();
		var toF = to.GetType ().GetFields (BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).ToDictionary (e => e.Name);
		for (int i = fromF.Length - 1; i >= 0; i--)
		{
			var f1 = fromF[i];
			if (toF.ContainsKey (f1.Name))
			{
				var f2 = toF[f1.Name];
				if (f1.FieldType == f2.FieldType)
				{
					f2.SetValue (to, f1.GetValue (from));
				}
			}
		}
		return to;
	}

	public static void CopyOverride (ref object from, ref object to, params string[] ignoreFields)
	{
		FieldInfo[] fromF = from.GetType ().GetFields (BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).Where (e => !ignoreFields.Contains (e.Name)).ToArray ();
		var toF = to.GetType ().GetFields (BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).ToDictionary (e => e.Name);
		for (int i = fromF.Length - 1; i >= 0; i--)
		{
			var f1 = fromF[i];
			if (toF.ContainsKey (f1.Name))
			{
				var f2 = toF[f1.Name];
				if (f1.FieldType == f2.FieldType)
				{
					f2.SetValue (to, f1.GetValue (from));
				}
			}
		}
	}

	public static string CombineStreamingAssetsPath (string path)
	{
#if UNITY_EDITOR || UNITY_STANDALONE
		return string.Format ("file:{0}/{1}", Application.streamingAssetsPath, path);
#elif UNITY_ANDROID || UNITY_IPHONE
		return string.Format("{0}/{1}", Application.streamingAssetsPath, path);
#endif
	}

	/// <summary>
	/// Used to calculate a value of aspect ratio
	/// </summary>
	/// <param name="x"></param>
	/// <param name="y"></param>
	/// <returns></returns>
	public static float CalAspectRatio (float x, float y)
	{
		return Mathf.Clamp (x / y, 0.001f, 1000f);
	}

	/// <summary>
	/// Used to calculate height based on recent width and ratio
	/// </summary>
	/// <param name="width"></param>
	/// <param name="ratio"></param>
	/// <returns></returns>
	public static float CalHeightByAspectRatio (float width, float ratio)
	{
		return width / ratio;
	}

	/// <summary>
	/// Used to calculate weight based on recent height and ratio
	/// </summary>
	/// <param name="height"></param>
	/// <param name="ratio"></param>
	/// <returns></returns>
	public static float CalWidthByAspectRatio (float height, float ratio)
	{
		return height * ratio;
	}
}
