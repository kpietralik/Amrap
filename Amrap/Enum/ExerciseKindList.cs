using Amrap.Core.Domain;
using Bit.BlazorUI;

namespace Amrap.Enum;

public static class ExerciseKindList
{
    public static readonly List<BitChoiceGroupItem<ExerciseKind>> Items = new()
        {
            new () { Text = "Push", Value = ExerciseKind.Push },
            new () { Text = "Pull", Value = ExerciseKind.Pull },
            new () { Text = "Core", Value = ExerciseKind.Core },
            new () { Text = "Legs", Value = ExerciseKind.Legs }
        };
}
