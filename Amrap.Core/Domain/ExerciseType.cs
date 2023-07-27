using Amrap.Infrastructure.Db;
using SQLite;

namespace Amrap.Core.Domain;

public class ExerciseType
{
    [PrimaryKey]
    public string Guid { get; set; }

    public ExerciseKind ExerciseKind { get; set; }
    public string Name { get; set; }

    public string Description { get; set; }
    public string Img { get; set; }

    public string AddUrl => $"/newexercisetype";
    public string EditUrl => $"/editexercisetype/{Guid}";

    /// <remarks>
    /// SQLite only
    /// </remarks>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public ExerciseType()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    { }

    public ExerciseType(string guid, ExerciseKind exerciseKind, string name, string description, string img)
    {
        if (exerciseKind == ExerciseKind.Unknown)
            throw new ArgumentException($"Value {exerciseKind} not supported", nameof(exerciseKind));

        ExerciseKind = exerciseKind;
        Guid = guid;
        Name = name;
        Description = description;
        Img = img;
    }

    public Task Add(DatabaseHandler databaseHandler) => databaseHandler.AddExerciseType(this);

    public Task Update(DatabaseHandler databaseHandler) => databaseHandler.UpdateExerciseType(this);

    public static Task<IList<ExerciseType>> GetExerciseTypes(DatabaseHandler databaseHandler) =>
        databaseHandler.GetExerciseTypes();
}