using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TradingManager : MonoBehaviour
{
    public static TradingManager instance;

    private List<Item> npcInventory;
    private string npcDialog;
    private bool isChoosed = false;
    private int curOpiton = 0;
    private int curChoosingIndex = 0;

    public GameObject tradingDialog, tradingUI;
    public Image buyBtnSelected, sellBtnSelected, backBtnSelected;
    public GameObject slotPrefab;
    public TextMeshProUGUI invt_item_detail, cost_txt, buySell, player_coin, tradingDialogText;
    public GameObject npcInvParent, playerInvParent;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    public void SetNPC(List<Item> inventoryItems, string npcDialog) { this.npcInventory = inventoryItems; this.npcDialog = npcDialog; }
    public void ResetData()
    {
        tradingDialog.gameObject.SetActive(false);
        tradingUI.gameObject.SetActive(false);
        isChoosed = false;
        curOpiton = 0;
        curChoosingIndex = 0;
    }
    private void RenderNPCInventory(int npc_length, bool isSell)
    {
        foreach (Transform child in npcInvParent.transform)
        {
            Destroy(child.gameObject);
        }
        for (int i = 0; i < npc_length; i++)
        {
            Item item = npcInventory[i];
            GameObject newSlot = Instantiate(slotPrefab, npcInvParent.transform);
            newSlot.transform.Find("icon").GetComponentInChildren<Image>().sprite = item.icon;

            TextMeshProUGUI itemEquip = newSlot.transform.Find("equip").GetComponentInChildren<TextMeshProUGUI>();
            TextMeshProUGUI itemAmount = newSlot.transform.Find("amount").GetComponentInChildren<TextMeshProUGUI>();

            if (!isSell)
            {
                Image selected = newSlot.transform.Find("selected").GetComponentInChildren<Image>();
                if (i == curChoosingIndex) { selected.gameObject.SetActive(true); cost_txt.text = "Cost: " + item.cost; }
            }

            itemAmount.gameObject.SetActive(false);
        }

    }
    private void RenderPlayerInventory(bool isSell)
    {
        foreach (Transform child in playerInvParent.transform)
        {
            Destroy(child.gameObject);
        }
        var player_inventory = PlayerController.Instance.inventory;
        int player_inv_length = PlayerController.Instance.inventory.Count;
        for (int i = 0; i < player_inv_length; i++)
        {
            Item item = player_inventory[i].item;
            GameObject newSlot = Instantiate(slotPrefab, playerInvParent.transform);
            newSlot.transform.Find("icon").GetComponentInChildren<Image>().sprite = item.icon;

            TextMeshProUGUI itemEquip = newSlot.transform.Find("equip").GetComponentInChildren<TextMeshProUGUI>();
            TextMeshProUGUI itemAmount = newSlot.transform.Find("amount").GetComponentInChildren<TextMeshProUGUI>();

            if (isSell)
            {
                Image selected = newSlot.transform.Find("selected").GetComponentInChildren<Image>();
                if (i == curChoosingIndex) { selected.gameObject.SetActive(true); cost_txt.text = "Cost: " + item.cost; }
            }
            itemAmount.text = player_inventory[i].amount.ToString();
            if (item.itemType == ItemType.weapon || item.itemType == ItemType.amor)
            {

                if (i == PlayerController.Instance.weaponInventoryIndex) itemEquip.gameObject.SetActive(true);
                else itemEquip.gameObject.SetActive(false);

            }
        }

    }
    public void HandleUpdate()
    {

        tradingDialog.gameObject.SetActive(!isChoosed);
        tradingUI.gameObject.SetActive(isChoosed);
        if (!isChoosed)
        {
            tradingDialogText.text = npcDialog;
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                curOpiton = curOpiton - 1 >= 0 ? curOpiton - 1 : 2;
                return;
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                curOpiton = curOpiton + 1 <= 2 ? curOpiton + 1 : 0;
                return;

            }
            switch (curOpiton)
            {
                case 0:
                    buyBtnSelected.gameObject.SetActive(true);
                    sellBtnSelected.gameObject.SetActive(false);
                    backBtnSelected.gameObject.SetActive(false);

                    break;

                case 1:
                    buyBtnSelected.gameObject.SetActive(false);
                    sellBtnSelected.gameObject.SetActive(true);
                    backBtnSelected.gameObject.SetActive(false);
                    break;
                case 2:
                    buyBtnSelected.gameObject.SetActive(false);
                    sellBtnSelected.gameObject.SetActive(false);
                    backBtnSelected.gameObject.SetActive(true);
                    break;
                default: break;

            }
            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (curOpiton == 2) { GameController.instance.SetGameState(GAME_STATE.FREE_ROAM); ResetData(); }
                else isChoosed = true;
            }
        }
        else
        {
            player_coin.text = "Your coin: " + PlayerController.Instance.GetCoin();
            if (curOpiton == 0)
            {
                buySell.text = "Buy: Enter";

                int npc_length = npcInventory.Count;
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    var player = PlayerController.Instance;
                    if (player.GetCoin() >= npcInventory[curChoosingIndex].cost)
                    {
                        var item = npcInventory[curChoosingIndex];
                        if (!item.isStackble || item.itemType != ItemType.usable)
                            foreach (var item1 in player.inventory)
                            {
                                if (item.itemName.Equals(item1.item.itemName))
                                {
                                    ResetData();
                                    DialogManager.instance.ShowDialog("You can't buy more this item!");
                                    GameController.instance.SetGameState(GAME_STATE.DIALOG);
                                }
                            }
                        player.AddItemToInventory(item);
                        player.ChangeCoin(-npcInventory[curChoosingIndex].cost);
                    }
                    else
                    {
                        ResetData();
                        DialogManager.instance.ShowDialog("You need more coin!");
                        GameController.instance.SetGameState(GAME_STATE.DIALOG);
                    }
                    return;
                }

                if (Input.GetKeyDown(KeyCode.LeftArrow) && curChoosingIndex > 0)
                {
                    curChoosingIndex--;
                }
                else if (Input.GetKeyDown(KeyCode.RightArrow) && curChoosingIndex < npc_length - 1)
                {
                    curChoosingIndex++;
                }
                else if (Input.GetKeyDown(KeyCode.DownArrow) && curChoosingIndex + 5 < npc_length)
                {
                    curChoosingIndex += 5;
                }
                else if (Input.GetKeyDown(KeyCode.UpArrow) && curChoosingIndex - 5 > -1)
                {
                    curChoosingIndex -= 5;
                }



                curChoosingIndex = curChoosingIndex > npc_length - 1 ? npc_length - 1 : curChoosingIndex;
                invt_item_detail.text = npcInventory[curChoosingIndex].itemDes;
                RenderNPCInventory(npc_length, false);
                RenderPlayerInventory(false);
            }
            else if (curOpiton == 1)
            {
                buySell.text = "Sell: Enter";

                var player_inventory = PlayerController.Instance.inventory;
                int player_inv_length = PlayerController.Instance.inventory.Count;
                int npc_length = npcInventory.Count;
                if (Input.GetKeyDown(KeyCode.Return))
                {

                    var player = PlayerController.Instance;
                    if (player.weaponInventoryIndex == curChoosingIndex)
                    {
                        DialogManager.instance.ShowDialog("You can't sell equipped item!");
                        GameController.instance.SetGameState(GAME_STATE.DIALOG);
                        ResetData();
                        return;
                    }
                    var item = player.inventory[curChoosingIndex];
                    int cost = item.item.cost;
                    if (!item.item.isStackble)
                    {
                        player.inventory.RemoveAt(curChoosingIndex);
                        curChoosingIndex = curChoosingIndex - 1 >= 0 ? curChoosingIndex-- : 0;
                        if (curChoosingIndex < player.weaponInventoryIndex) player.weaponInventoryIndex -= 1;
                    }
                    else
                    {
                        if (item.amount > 1)
                        {
                            player.inventory[curChoosingIndex].amount--;
                        }
                        else
                        {
                            player.inventory.RemoveAt(curChoosingIndex);
                            if (curChoosingIndex < player.weaponInventoryIndex) player.weaponInventoryIndex -= 1;
                            curChoosingIndex = curChoosingIndex - 1 >= 0 ? curChoosingIndex-- : 0;

                        }
                    }
                    player.ChangeCoin(cost);
                    return;
                }



                if (Input.GetKeyDown(KeyCode.LeftArrow) && curChoosingIndex > 0)
                {
                    curChoosingIndex--;
                }
                else if (Input.GetKeyDown(KeyCode.RightArrow) && curChoosingIndex < player_inv_length - 1)
                {
                    curChoosingIndex++;
                }
                else if (Input.GetKeyDown(KeyCode.DownArrow) && curChoosingIndex + 5 < player_inv_length)
                {
                    curChoosingIndex += 5;
                }
                else if (Input.GetKeyDown(KeyCode.UpArrow) && curChoosingIndex - 5 > -1)
                {
                    curChoosingIndex -= 5;
                }



                foreach (Transform child in playerInvParent.transform)
                {
                    Destroy(child.gameObject);
                }
                curChoosingIndex = curChoosingIndex > player_inv_length - 1 ? player_inv_length - 1 : curChoosingIndex;
                invt_item_detail.text = player_inventory[curChoosingIndex].item.itemDes;
                RenderPlayerInventory(true);
                RenderNPCInventory(npc_length, true);

            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                isChoosed = false;
            }
        }

    }
}
