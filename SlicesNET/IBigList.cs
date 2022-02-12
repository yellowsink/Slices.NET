namespace SlicesNET;

public interface IBigList<T> : IList<T>
{
	public ulong LongCount { get; }

	public ulong? LongIndexOf(T item);

	public T this[ulong index]
	{
		get;
		set;
	}

	public void Insert(ulong   index, T item);
	public void RemoveAt(ulong index);
}