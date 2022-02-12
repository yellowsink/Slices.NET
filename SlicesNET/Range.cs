namespace SlicesNET;

public readonly record struct Range(Index Start, Index End)
{
	public bool Includes<T>(int index, IReadOnlyCollection<T> c)
		=> index >= Start.Resolve(c.Count) && index <= End.Resolve(c.Count);

	public static implicit operator Range((int start, int end) t) => new(t.start, t.end);
	public static implicit operator Range(System.Range         r) => new(r.Start, r.End);

	/*public ulong Start;
	public ulong End;
	
	public Range(ulong start, ulong end)
	{
		if (start > end)
			throw new ArgumentException("Start cannot be larger than end");
		Start = start;
		End        = end;
	}*/
}