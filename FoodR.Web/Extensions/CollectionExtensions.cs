using System;
using System.Collections;
using System.Collections.Generic;

public static class CollectionExtensions
{
	public static void ForEach<T>(this T[] array, Action<T> action)
	{
		foreach (var item in array)
		{
			action(item);
		}	
	}

	public static void ForEach<T>(this IEnumerable<T> enumeration, Action<T> action)
	{
		foreach (T item in enumeration)
		{
			action(item);
		}
	}
}
