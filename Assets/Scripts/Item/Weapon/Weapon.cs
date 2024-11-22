using System;
using UnityEngine;

public abstract class Weapon : Item
{
    protected int ATK;
    protected int CritChance;

    public Weapon(string itemName, string itemDes, int ATK, int Crit) : base(itemName, itemDes, false, ItemType.weapon)
    {
        this.ATK = ATK;
        this.CritChance = Crit;
    }
    public int getATK()
    {
        return ATK;
    }
    public int GetCritChance()
    {
        return CritChance;
    }
    public override void Use()
    {
        PlayerController.Instance.SetCurWeapon(this);
    }

}