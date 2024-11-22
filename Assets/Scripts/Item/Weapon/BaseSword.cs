using UnityEngine;

[CreateAssetMenu(fileName = "Base Iron Sword", menuName = "Inventory/Sword/Base Sword")]
public class BaseSword : Weapon
{
    public BaseSword() : base("Base Iron Sword", "Starting Sword", 1, 10) { }
}