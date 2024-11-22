using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedNormalPotionCollectable : MonoBehaviour, Collectable
{
    public Item Collect()
    {
        return ItemManager.Instance.GetItem("Normal HP Potion");
    }
}
