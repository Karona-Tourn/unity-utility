using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public static class CollectionExtension
{
    public static void AddIfNotExist(this IList iList, object value)
    {
        if (!iList.Contains(value))
        {
            iList.Add(value);
        }
    }

    public static void AddOrUpdate(this IDictionary iDictionary, object key, object value)
    {
        if (!iDictionary.Contains(key))
        {
            iDictionary.Add(key, value);
        }
        else
        {
            iDictionary[key] = value;
        }
    }

	/// <summary>
	/// Compare the list1 with list2 , are the same conten or not
	/// </summary>
	/// <param name="list">List of Integer to compare</param>
	/// <param name="targetListCompare">Tartet list of Integer to compare</param>
	/// <returns> Return true are have the same conten , Return false are have deference conten </returns>
	public static bool AreTheSameIgnoringOrder(this List<int> list, List<int> targetListCompare)
	{
		//return list.Count() == targetListCompare.Count()
		//	&& !list.Except(targetListCompare).Any()
		//	&& !targetListCompare.Except(list).Any(); // re: Servy's comment.

		var difList = list.Where(a => !targetListCompare.Any(a1 => a1 == a))
				.Union(targetListCompare.Where(a => !list.Any(a1 => a1 == a)));

		return difList.Count() == 0;
	}

	public static T[] SubArray<T>(this T[] data, int index, int length)
	{
		T[] result = new T[length];
		Array.Copy(data, index, result, 0, length);
		return result;
	}
}
