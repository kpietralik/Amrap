using SQLite;
//using System.ComponentModel.DataAnnotations.Schema;

namespace Amrap.Core.Models;

//[Table("Exercises")]
public class CompletedExerciseModel
{
    [PrimaryKey]
    public string Guid { get; set; }

    // Not supported...
    //[ForeignKey(typeof(ExerciseType))]

    [Indexed]
    public string ExerciseTypeGuid { get; set; }

    public DateTimeOffset Time { get; set; }
    public int Sets { get; set; }
    public int Reps { get; set; }
    public float Weight { get; set; }

    // For SQLite
    public CompletedExerciseModel() { }

    public CompletedExerciseModel(string guid, ExerciseTypeModel exerciseType, DateTimeOffset time, int sets, int reps, float weight)
    {
        Guid = guid;
        ExerciseTypeGuid = exerciseType.Guid;
        Time = time;
        Sets = sets;
        Reps = reps;
        Weight = weight;
    }
}
