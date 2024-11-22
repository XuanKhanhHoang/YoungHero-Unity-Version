using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Big HP Potion", menuName = "Inventory/UsebleItem/BigHP")]
public class BigPotion : Item
{
    public BigPotion() : base("Big HP Potion", "Use it to heal your HP 10 point", true, ItemType.usable, 16) { }
    public override void Use()
    {
        PlayerController.Instance.HealHP(10);
    }

}
