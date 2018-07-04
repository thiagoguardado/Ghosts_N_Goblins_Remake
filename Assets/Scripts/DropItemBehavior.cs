
public enum DroppableItem
{
    Doll,
    Weapon,
    Accesory,
    SpecialDoll
}

public static class DropItemBehavior
{


    private static DroppableItem[] itemDropList = new DroppableItem[7] { DroppableItem.Doll, DroppableItem.Weapon, DroppableItem.Doll, DroppableItem.Accesory, DroppableItem.Doll, DroppableItem.Weapon, DroppableItem.Doll };
    private static int itemListIndex = 0;
    private static int itemListRepeats = 0;


    public static DroppableItem NextItem()
    {
        DroppableItem item;
        if (itemListRepeats == 3 && itemListIndex == 0)
        {
            item = DroppableItem.SpecialDoll;
        }
        else
        {
            item = itemDropList[itemListIndex];
        }

        itemListIndex += 1;

        if (itemListIndex >= itemDropList.Length)
        {
            itemListIndex = 0;
            itemListRepeats += 1;
        }

        itemListRepeats = itemListRepeats % 4;

        return item;

    }

    public static void Reset()
    {
        itemListIndex = 0;
        itemListRepeats = 0;
    }

}
