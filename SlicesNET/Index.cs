namespace SlicesNET;

public readonly record struct Index(int Value, bool FromEnd = false)
{
	public int Resolve(int length) => FromEnd ? length - Value : Value;

	public static implicit operator Index(int        i) => new(i);
	public static implicit operator Index(System.Index i) => new(i.Value, i.IsFromEnd);
}