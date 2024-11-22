using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{

    public static InventoryController Instance { get; private set; }


    public GameObject slotPrefab;
    public TextMeshProUGUI invt_item_detail;

    private int curChoosingIndex = 0;
    void Awake()
    {
        Instance = this;
    }
    public void HandleUpdate()
    {
        PlayerController playerController = PlayerController.Instance;
        var inventory = playerController.inventory;
        int inv_length = playerController.inventory.Count;

        if (Input.GetKeyDown(KeyCode.Return))
        {
            playerController.UseInventoryItem(curChoosingIndex);
            return;
        }
        if (Input.GetKeyDown(KeyCode.Delete))
        {

            if (curChoosingIndex == playerController.weaponInventoryIndex) return;
            if (playerController.weaponInventoryIndex > curChoosingIndex) playerController.weaponInventoryIndex--;
            inventory.RemoveAt(curChoosingIndex);
            return;
        }


        if (Input.GetKeyDown(KeyCode.LeftArrow) && curChoosingIndex > 0)
        {
            curChoosingIndex--;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) && curChoosingIndex < inv_length - 1)
        {
            curChoosingIndex++;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) && curChoosingIndex + 5 < inv_length)
        {
            curChoosingIndex += 5;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow) && curChoosingIndex - 5 > -1)
        {
            curChoosingIndex -= 5;
        }



        foreach (Transform child in gameObject.transform)
        {
            Destroy(child.gameObject); // Clear existing slots
        }
        curChoosingIndex = curChoosingIndex > inv_length - 1 ? inv_length - 1 : curChoosingIndex;
        invt_item_detail.text = inventory[curChoosingIndex].item.itemDes;
        for (int i = 0; i < inv_length; i++)
        {
            Item item = playerController.inventory[i].item;
            GameObject newSlot = Instantiate(slotPrefab, gameObject.transform);
            newSlot.transform.Find("icon").GetComponentInChildren<Image>().sprite = item.icon;

            TextMeshProUGUI itemEquip = newSlot.transform.Find("equip").GetComponentInChildren<TextMeshProUGUI>();
            TextMeshProUGUI itemAmount = newSlot.transform.Find("amount").GetComponentInChildren<TextMeshProUGUI>();

            Image selected = newSlot.transform.Find("selected").GetComponentInChildren<Image>();
            if (i == curChoosingIndex) selected.gameObject.SetActive(true);

            itemAmount.text = playerController.inventory[i].amount.ToString();
            if (item.itemType == ItemType.weapon || item.itemType == ItemType.amor)
            {

                if (i == playerController.weaponInventoryIndex) itemEquip.gameObject.SetActive(true);
                else itemEquip.gameObject.SetActive(false);

            }
        }

    }
}
