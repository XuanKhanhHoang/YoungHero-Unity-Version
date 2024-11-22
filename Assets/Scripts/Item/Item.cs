using UnityEngine;

public abstract class Item : ScriptableObject
{
    public string itemName, itemDes;
    public Sprite icon;
    public bool isStackble;
    public ItemType itemType;
    public int maxStack;
    public int cost = 0;
    public Item(string itemName, string itemDes, bool isStackble = true, ItemType itemType = ItemType.other, int maxStack = 1)
    {
        this.itemName = itemName;
        this.itemDes = itemDes;
        this.isStackble = isStackble;
        this.itemType = itemType;
        this.maxStack = maxStack;
    }
    public abstract void Use();
}
public enum ItemType { weapon, amor, usable, other }
