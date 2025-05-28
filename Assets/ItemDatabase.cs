using UnityEngine;
using System.Collections.Generic;

public class ItemDatabase : MonoBehaviour
{
    public List<Item> items; // List of all Item ScriptableObjects
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
}