public class InventoryItem
{
    public int amount;
    public Item item;
    public InventoryItem(Item item, int amount = 1)
    {
        this.item = item;
        this.amount = amount;
    }
}