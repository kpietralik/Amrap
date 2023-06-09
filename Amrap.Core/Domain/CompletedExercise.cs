using Amrap.Core.Models;

namespace Amrap.Core.Domain;

public class CompletedExercise
{
    public int Id { get; set; }
    public string Guid { get; set; }

    public ExerciseType ExerciseType { get; set; }

    public DateTimeOffset Time { get; set; }
    public int Sets { get; set; }
    public int Reps { get; set; }
    public float Weight { get; set; }

    public static CompletedExercise FromModel(CompletedExerciseModel model, ExerciseType exerciseType)
    {
        if (model.ExerciseTypeGuid != exerciseType.Guid.ToString())
            throw new Exception($"Data id missmatch: {nameof(CompletedExerciseModel)}, model={model.ExerciseTypeGuid}, data={exerciseType.Guid}");

        return new CompletedExercise(model.Id, model.Guid, exerciseType, model.Time, model.Sets, model.Reps, model.Weight);
    }

    public CompletedExercise(int id, string guid, ExerciseType exerciseType, DateTimeOffset time, int sets, int reps, float weight)
    {
        Id = id;
        Guid = guid;
        ExerciseType = exerciseType;
        Time = time;
        Sets = sets;
        Reps = reps;
        Weight = weight;
    }
}
