using System.Collections.Generic;

namespace SlicesNET.Tests;

public static class Util
{
	public static IEnumerable<T> BigEnumerable<T>(ulong size, T item)
	{
		for (var i = 0UL; i < size; i++)
			yield return item;
	}
}