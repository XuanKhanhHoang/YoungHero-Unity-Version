using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawnManager : MonoBehaviour
{
    public static PlayerRespawnManager instance;
    private Vector2 pos;
    private int gameActionIndex, gameDialogIndex;
    private List<InventoryItem> inventoryItems = new List<InventoryItem>();
    private int curExp, LV, coin, curWeaponActiveIndex;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    public void SetRespawnPoint()
    {
        pos = PlayerController.Instance.gameObject.transform.position;
        gameActionIndex = GameController.instance.gameStoryManager.GetCurGameActionIndex();
        gameDialogIndex = GameController.instance.gameStoryManager.GetcurGameDialogIndex();
        curExp = PlayerController.Instance.GetCurEXP();
        LV = PlayerController.Instance.level;
        coin = PlayerController.Instance.GetCoin();
        curWeaponActiveIndex = PlayerController.Instance.weaponInventoryIndex;
        inventoryItems = new List<InventoryItem>();
        foreach (InventoryItem item in PlayerController.Instance.inventory)
        {
            inventoryItems.Add(new InventoryItem(item.item, item.amount));
        }
    }
    public void Respawn()
    {
        PlayerController.Instance.Respawn(pos, LV, curExp, coin, inventoryItems, curWeaponActiveIndex);
        GameController.instance.gameStoryManager.SetGameActionIndexAndDialogIndex(gameActionIndex, gameDialogIndex);
        GameController.instance.SetGameState(GAME_STATE.FREE_ROAM);
    }
}
