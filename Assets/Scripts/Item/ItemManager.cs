using System.Buffers.Text;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour

{
    public static ItemManager Instance { get; private set; }
    public List<Item> items;
    private Dictionary<string, Item> itemDictionary = new Dictionary<string, Item>();

    private void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            foreach (Item item in items)
            {
                itemDictionary[item.itemName] = item;
            }
        }
        else
        {
            Destroy(gameObject);
        }

    }
    public Item GetItem(string itemName)
    {
        if (itemDictionary.TryGetValue(itemName, out Item newItem))
        {
            return newItem;
        }
        else
        {
            return null;
        }
    }
}
