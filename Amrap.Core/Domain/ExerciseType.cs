using Amrap.Core.Models;

namespace Amrap.Core.Domain;

public class ExerciseType
{
    public int Id { get; set; }
    
    public string Guid { get; set; }

    public ExerciseKind ExerciseKind { get; set; }
    public string Name { get; set; }

    public string Description { get; set; }
    public string Img { get; set; }

    public static ExerciseType FromModel(ExerciseTypeModel model) =>
        new(model.Id, model.Guid, model.ExerciseKind, model.Name, model.Description, model.Img);

    private ExerciseType(int id, string guid, ExerciseKind exerciseKind, string name, string description, string img)
    {
        Id = id;
        Guid = guid;
        ExerciseKind = exerciseKind;
        Name = name;
        Description = description;
        Img = img;
    }
}
