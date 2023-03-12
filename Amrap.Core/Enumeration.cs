using System.Reflection;

namespace Amrap.Core;

// See: https://learn.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/enumeration-classes-over-enum-types
public abstract class Enumeration : IComparable
{
	public string DisplayName { get; private set; }

	public string Name { get; private set; }

	public uint Id { get; private set; }

	protected Enumeration(uint id, string name, string displayName) => (Id, Name, DisplayName) = (id, name, displayName);
	
	public Enumeration()
	{}

	public override string ToString() => DisplayName;

	public static IEnumerable<T> GetAll<T>() where T : Enumeration =>
		typeof(T).GetFields(BindingFlags.Public |
							BindingFlags.Static |
							BindingFlags.DeclaredOnly)
				 .Select(f => f.GetValue(null))
				 .Cast<T>();

	public override bool Equals(object obj)
	{
		if (obj is not Enumeration otherValue)
		{
			return false;
		}

		var typeMatches = GetType().Equals(obj.GetType());
		var valueMatches = Id.Equals(otherValue.Id);

		return typeMatches && valueMatches;
	}

	public int CompareTo(object other) => Id.CompareTo(((Enumeration)other).Id);
}
