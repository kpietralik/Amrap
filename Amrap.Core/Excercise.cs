using SQLite;

namespace Amrap.Core;

public class Excercise
{
	[PrimaryKey, AutoIncrement]
	public int Id { get; set; }

	public string Name { get; set; }
	public string Description { get; set; }
	
	public uint Sets { get; set; }
	public uint Reps { get; set; }
	
	// Simplified
	public float LastWeight { get; set; }

	//public ExcerciseKind ExcerciseKind { get; set; }
}

public class ExcerciseKind : Enumeration
{
	public static ExcerciseKind BenchPress = new(1, nameof(BenchPress), "Bench press");
	public static ExcerciseKind Deadlift = new(2, nameof(Deadlift), "Deadlift");
	public static ExcerciseKind WeightedDip = new(3, nameof(WeightedDip), "Weighted dip");

	// For SQLite
	public ExcerciseKind()
	{ }

	public ExcerciseKind(uint id, string name, string displayName) : base(id, name, displayName)
	{}
}