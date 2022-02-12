using NUnit.Framework;

namespace SlicesNET.Tests;

public class BigListTests
{
	[Test]
	public void BuildTest()
	{
		var bigList = new BigList<byte>(Util.BigEnumerable((ulong) ushort.MaxValue * 512, (byte) 0));
		Assert.IsNotEmpty(bigList);
	}

	[Test]
	public void EnumerateTest()
	{
		var bigEnumerable = Util.BigEnumerable(ushort.MaxValue * 16, (byte) 0);
		var bigList       = new BigList<byte>(bigEnumerable);

		using var enumerator = bigEnumerable.GetEnumerator();
		foreach (var e in bigList)
		{
			Assert.IsTrue(enumerator.MoveNext());
			Assert.AreEqual(enumerator.Current, e);
		}
	}

	[Test]
	public void IndexTest()
	{
		var bigEnumerable = Util.BigEnumerable(ushort.MaxValue * 16, (byte) 0);
		var bigList       = new BigList<byte>(bigEnumerable);

		using var enumerator = bigEnumerable.GetEnumerator();
		// ReSharper disable once ForCanBeConvertedToForeach
		for (var i = 0UL; i < bigList.LongCount; i++)
		{
			var e = bigList[i];
			Assert.IsTrue(enumerator.MoveNext());
			Assert.AreEqual(enumerator.Current, e);
		}
	}

	[TestCase(4u, TestName = "One list")]
	[TestCase(ushort.MaxValue, TestName = "Overflow to 2nd")]
	[TestCase((uint) ushort.MaxValue * 2 + 5, TestName = "Partially into 3rd")]
	public void AddTest(uint size)
	{
		var bigList = new BigList<int>(Util.BigEnumerable(size, 4));
		Assert.AreEqual(size, bigList.Count);
		bigList.Add(5);
		Assert.AreEqual(size + 1, bigList.Count);
		Assert.AreEqual(5,        bigList[^1]);
	}
	
	/*[Test]
	public void AddOneArrayTest()
	{
		// add into the 1st backing list
		var bigList = new BigList<int>(new[] { 1, 2, 3, 4 });
		Assert.AreEqual(4, bigList.Count);
		bigList.Add(5);
		Assert.AreEqual(5, bigList.Count);
		Assert.AreEqual(5, bigList[^1]);
	}

	[Test]
	public void AddOverflowTest()
	{
		// the .Add() call will create a 2nd backing list
		const ushort size = ushort.MaxValue;

		var bigList = new BigList<int>(Util.BigEnumerable(size, 4));
		Assert.AreEqual(size, bigList.Count);
		bigList.Add(5);
		Assert.AreEqual(size + 1, bigList.Count);
		Assert.AreEqual(5,        bigList[^1]);
	}

	[Test]
	public void AddManyArrayTest()
	{
		// partially into the 3rd backing list
		const uint size = ushort.MaxValue * 2 + 5;

		var bigList = new BigList<int>(Util.BigEnumerable(size, 4));
		Assert.AreEqual(size, bigList.Count);
		bigList.Add(5);
		Assert.AreEqual(size + 1, bigList.Count);
		Assert.AreEqual(5,        bigList[^1]);
	}*/
}