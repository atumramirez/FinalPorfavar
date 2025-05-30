using UnityEngine;
using System.Collections.Generic;

public class ItemDatabase : MonoBehaviour
{
    public List<Item> items; 
    public Item GetItemById(int id)
    {
        foreach (Item item in items)
        {
            if (item.Id == id)
                return item;
        }
        Debug.LogWarning("Item with ID " + id + " not found in the database.");
        return null;
    }

    public int GetId(Item item)
    {
        return item.Id;
    }

    public string GetItemName(int id)
    {
        foreach (Item item in items)
        {
            if (item.Id == id)
                return item.itemName;
        }
        Debug.LogWarning("Item with ID " + id + " not found in the database.");
        return null;
    }

    public string GetItemDescription(int id)
    {
        foreach (Item item in items)
        {
            if (item.Id == id)
                return item.description;
        }
        Debug.LogWarning("Item with ID " + id + " not found in the database.");
        return null;
    }

    public int GetItemPrice(int id)
    {
        foreach (Item item in items)
        {
            if (item.Id == id)
                return item.price;
        }
        Debug.LogWarning("Item with ID " + id + " not found in the database.");
        return 0;
    }
}