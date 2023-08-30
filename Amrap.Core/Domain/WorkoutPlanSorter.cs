using Amrap.Core.Infrastructure;

namespace Amrap.Core.Domain;

public class WorkoutPlanSorter
{
    public IOrderedEnumerable<WorkoutPlanItem> Sort(IEnumerable<WorkoutPlanItem> items)
    {
        return items.OrderBy(x => x.Day).ThenBy(x => x.Priority);
    }

    public async Task<IOrderedEnumerable<WorkoutPlanItem>> MoveDown(
        IEnumerable<WorkoutPlanItem> allItems, WorkoutPlanItem itemToMove, DatabaseHandler databaseHandler)
    {
        LinkedList<WorkoutPlanItem> ll = DayToLinkedList(allItems, itemToMove);

        var nodeToMove = ll.Find(itemToMove);
        var nodeToMoveAfter = nodeToMove?.Next;
        if (nodeToMove != null && nodeToMoveAfter != null)
        {
            ll.Remove(nodeToMove);
            ll.AddAfter(nodeToMoveAfter, nodeToMove);
        }
        else
            return Sort(allItems); // Was not found (error?) or last item in the list

        nodeToMove.Value.Priority++;
        nodeToMoveAfter.Value.Priority--;
        await nodeToMove.Value.Upsert(databaseHandler);
        await nodeToMoveAfter.Value.Upsert(databaseHandler);

        return Sort(allItems);
    }

    public async Task<IOrderedEnumerable<WorkoutPlanItem>> MoveUp(
        IEnumerable<WorkoutPlanItem> allItems, WorkoutPlanItem itemToMove, DatabaseHandler databaseHandler)
    {
        LinkedList<WorkoutPlanItem> ll = DayToLinkedList(allItems, itemToMove);

        var nodeToMove = ll.Find(itemToMove);
        var nodeToMoveBefore = nodeToMove?.Previous;
        if (nodeToMove != null && nodeToMoveBefore != null)
        {
            ll.Remove(nodeToMove);
            ll.AddBefore(nodeToMoveBefore, nodeToMove);
        }
        else
            return Sort(allItems); // Was not found (error?) or first item in the list

        nodeToMove.Value.Priority--;
        nodeToMoveBefore.Value.Priority++;
        await nodeToMove.Value.Upsert(databaseHandler);
        await nodeToMoveBefore.Value.Upsert(databaseHandler);

        return Sort(allItems);
    }

    private static LinkedList<WorkoutPlanItem> DayToLinkedList(IEnumerable<WorkoutPlanItem> allItems, WorkoutPlanItem itemToMove)
    {
        var groups = allItems.GroupBy(x => x.Day);
        var toMoveGroup = groups.Single(g => g.Key == itemToMove.Day);

        return new LinkedList<WorkoutPlanItem>(toMoveGroup);
    }
}
