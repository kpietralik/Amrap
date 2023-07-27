using Bit.BlazorUI;

namespace Amrap.Enum;

public static class DayOfWeekList
{
    public static readonly List<BitChoiceGroupItem<DayOfWeek>> Items = new()
        {
            new () { Text = "Monday", Value = DayOfWeek.Monday},
            new () { Text = "Tuesday", Value = DayOfWeek.Tuesday},
            new () { Text = "Wednesday", Value = DayOfWeek.Wednesday},
            new () { Text = "Thursday", Value = DayOfWeek.Thursday},
            new () { Text = "Friday", Value = DayOfWeek.Friday},
            new () { Text = "Saturday", Value = DayOfWeek.Saturday},
            new () { Text = "Sunday", Value = DayOfWeek.Sunday},
        };
}