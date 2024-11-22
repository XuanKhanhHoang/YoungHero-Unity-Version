using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Normal HP Potion", menuName = "Inventory/UsebleItem")]
public class RedPotion : Item
{
    public RedPotion() : base("Normal HP Potion", "Use it to heal your HP 6 point", true, ItemType.usable, 16) { }
    public override void Use()
    {
        PlayerController.Instance.HealHP(6);
    }

}
