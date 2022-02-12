using System.Collections;

namespace SlicesNET;

public class BigList<T> : IBigList<T>
{
	// could be int.MaxValue - 2 but then we hit an OutOfMemoryException.
	private const int ChunkSize = ushort.MaxValue;
	
	public BigList(IEnumerable<T> enumerable)
	{
		var split = enumerable.Chunk(ChunkSize);
		foreach (var collection in split) _backing.AddLast(collection.ToList());
	}

	private LinkedList<IList<T>> _backing = new();


	public IEnumerator<T> GetEnumerator() => _backing.SelectMany(collection => collection).GetEnumerator();

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();


	public bool  IsReadOnly => false;
	public int   Count      => (int) LongCount;
	public ulong LongCount  => _backing.Aggregate(0UL, (current, next) => current + (ulong) next.Count);

	public void Clear() => _backing = new();

	public bool Contains(T item) => _backing.Any(l => l.Contains(item));

	public void CopyTo(T[] array, int arrayIndex)
	{
		foreach (var item in _backing.SelectMany(c => c))
			array[arrayIndex++] = item;
	}

	public int IndexOf(T item)
	{
		var index = LongIndexOf(item);
		if (index == null)
			return -1;

		return (int) index;
	}

	public ulong? LongIndexOf(T item)
	{
		var i = 0UL;
		foreach (var elem in _backing.SelectMany(c => c))
		{
			if (Equals(item, elem))
				return i;
			i++;
		}

		return null;
	}


	public void Add(T item)
	{
		if (_backing.Last != null && _backing.Last.Value.Count < ChunkSize)
			// the backing isn't empty, and there is space before we cant index
			_backing.Last.Value.Add(item);
		else
			_backing.AddLast(new List<T> { item });
	}

	public void Insert(ulong index, T item)
	{
		var collection = _backing.First;
		while (index >= ChunkSize)
		{
			collection =  collection?.Next;
			index      -= ChunkSize;
		}

		if (collection == null) throw new ArgumentOutOfRangeException();
		
		InsertInCollection(item, (int) index, collection);
	}

	private static void InsertInCollection(T item, int index, LinkedListNode<IList<T>> node)
	{
		if (node.Value.Count < ChunkSize)
		{
			node.Value.Add(item);
			return;
		}

		var last = node.Value[^1];
		node.Value.RemoveAt(node.Value.Count - 1);
		node.Value.Insert(index, item);

		if (node.Next == null) return;
		InsertInCollection(last, 0, node.Next);
	}

	public void Insert(int index, T item) => Insert((ulong) index, item);

	public void RemoveAt(ulong index) => throw new NotImplementedException();

	public void RemoveAt(int index) => RemoveAt((ulong) index);

	public T this[int index]
	{
		get => this[(ulong) index];
		set => this[(ulong) index] = value;
	}

	public bool Remove(T item) => throw new NotImplementedException();

	public T this[ulong index]
	{
		get
		{
			var collection = _backing.First;
			while (index >= ChunkSize)
			{
				collection =  collection?.Next;
				index      -= ChunkSize;
			}

			if (collection == null)
				throw new IndexOutOfRangeException();

			return collection.Value[(int) index];
		}
		set
		{
			var collection = _backing.First;
			while (index >= ChunkSize)
			{
				collection =  collection?.Next;
				index      -= ChunkSize;
			}

			if (collection == null)
				throw new IndexOutOfRangeException();

			collection.Value[(int) index] = value;
		}
	}
}