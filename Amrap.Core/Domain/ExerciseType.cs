using Amrap.Core.Models;
using Amrap.Infrastructure.Db;

namespace Amrap.Core.Domain;

public class ExerciseType
{
    public string Guid { get; set; }

    public ExerciseKind ExerciseKind { get; set; }
    public string Name { get; set; }

    public string Description { get; set; }
    public string Img { get; set; }

    public string AddUrl => $"/newexercisetype";
    public string EditUrl => $"/editexercisetype/{Guid}";

    public static ExerciseType FromModel(ExerciseTypeModel model) =>
        new(model.Guid, model.ExerciseKind, model.Name, model.Description, model.Img);

    public ExerciseType(string guid, ExerciseKind exerciseKind, string name, string description, string img)
    {
        Guid = guid;
        ExerciseKind = exerciseKind;
        Name = name;
        Description = description;
        Img = img;
    }

    public Task Add(DatabaseHandler databaseHandler) => databaseHandler.AddExerciseType(new ExerciseTypeModel
    {
        Guid = Guid,
        ExerciseKind = ExerciseKind,
        Name = Name,
        Description = Description,
        Img = Img
    });

    public Task Update(DatabaseHandler databaseHandler) => databaseHandler.UpdateExerciseType(
            new ExerciseTypeModel(
                Guid,
                ExerciseKind,
                Name,
                Description,
                Img));

    public static async Task<IList<ExerciseType>> GetExerciseTypes(DatabaseHandler databaseHandler)
    {
        var exerciseTypeModels = await databaseHandler.GetExerciseTypes();

        var exercisesTypes = new List<ExerciseType>();
        foreach (var exerciseType in exerciseTypeModels)
        {
            exercisesTypes.Add(FromModel(exerciseType));
        }

        return exercisesTypes;
    }
}