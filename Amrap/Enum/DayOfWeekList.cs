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

    public const string All = "All";

    public static List<BitDropdownItem> FilteringDayOfWeekItems()
    {
        return new()
        {
            new()
            {
                ItemType = BitDropdownItemType.Normal,
                Text = All,
                Value = All
            },
            new()
            {
                ItemType = BitDropdownItemType.Normal,
                Text = DayOfWeek.Monday.ToString(),
                Value = DayOfWeek.Monday.ToString()
            },
            new()
            {
                ItemType = BitDropdownItemType.Normal,
                Text = DayOfWeek.Tuesday.ToString(),
                Value = DayOfWeek.Tuesday.ToString()
            },
            new()
            {
                ItemType = BitDropdownItemType.Normal,
                Text = DayOfWeek.Wednesday.ToString(),
                Value = DayOfWeek.Wednesday.ToString()
            },
            new()
            {
                ItemType = BitDropdownItemType.Normal,
                Text = DayOfWeek.Thursday.ToString(),
                Value = DayOfWeek.Thursday.ToString()
            },
            new()
            {
                ItemType = BitDropdownItemType.Normal,
                Text = DayOfWeek.Friday.ToString(),
                Value = DayOfWeek.Friday.ToString()
            },
            new()
            {
                ItemType = BitDropdownItemType.Normal,
                Text = DayOfWeek.Saturday.ToString(),
                Value = DayOfWeek.Saturday.ToString()
            },
            new()
            {
                ItemType = BitDropdownItemType.Normal,
                Text = DayOfWeek.Sunday.ToString(),
                Value = DayOfWeek.Sunday.ToString()
            }
        };
    }
}