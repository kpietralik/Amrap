using SQLite;

namespace Amrap.Core.Models;

public class ExerciseTypeModel
{
    [PrimaryKey]
    public int Id { get; set; }

    [Indexed]
    public string Guid { get; set; }

    public ExerciseKind ExerciseKind { get; set; }
    public string Name { get; set; }

    public string Description { get; set; }
    public string Img { get; set; }

    // For SQLite
    public ExerciseTypeModel() { }

    public ExerciseTypeModel(/*string guis, *//*ExerciseKind excerciseKind, */ int id, string guid, ExerciseKind exerciseKind, string name, string description, string img = null)
    {
        Id = id;
        Guid = guid;

        if (exerciseKind == ExerciseKind.Unknown)
            throw new ArgumentException($"Value {exerciseKind} not supported", nameof(exerciseKind));
        ExerciseKind = exerciseKind;

        Name = name;
        Description = string.IsNullOrWhiteSpace(description) ? name : description;
        Img = img;
    }
}
