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

    // ToDo: refactor other domain types to use this persistence handling
    // ToDo: flatten by skipping DatabaseHandler methods?
    public async Task Add(DatabaseHandler databaseHandler)
    {
        await databaseHandler.AddExerciseType(new ExerciseTypeModel 
        {
            Guid = Guid,
            ExerciseKind = ExerciseKind,
            Name = Name,
            Description = Description,
            Img = Img 
        });
    }

    public async Task Update(DatabaseHandler databaseHandler)
    {
        await databaseHandler.UpdateExerciseType(
            new ExerciseTypeModel(        
                Guid,
                ExerciseKind,
                Name,
                Description,
                Img));
    }
}
