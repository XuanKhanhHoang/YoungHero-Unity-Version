using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraderGuard : MonoBehaviour, Interactable
{
    private List<Item> inventory;
    private void Awake()
    {
        inventory = new List<Item>()
        {
            ItemManager.Instance.GetItem("Normal HP Potion"),
            ItemManager.Instance.GetItem("Normal Slodier Iron Sword"),

        };
    }
    public void Interact()
    {
        GameController.instance.SetGameState(GAME_STATE.TRADING_STATE);
        TradingManager.instance.SetNPC(inventory, "Welcome , What do you want ?");
    }
}
