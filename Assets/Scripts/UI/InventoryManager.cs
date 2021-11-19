using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;

public class InventoryManager : MonoBehaviour
{
    [SerializeField]
    private UISlide slider = null;
    [SerializeField]
    private Image[] inventorySlots = new Image[0];

    [Space(10)]
    [SerializeField, ReadOnly]
    private List<Item> items = new List<Item>();

    public void ToggleUI()
    {
        slider.ToggleUI();
    }

    /// <summary>
    /// Adds an item to your inventory
    /// </summary>
    /// <param name="item">Item to add</param>
    /// <returns>returns 0 on success, 1 on Inventory full, and 2 on already have Item</returns>
    public int AddItem(Item item)
    {
        if (items.Count == inventorySlots.Length)
            return 1;

        for(int x = 0; x < items.Count; ++x)
        {
            if (items[x].Name == item.Name)
                return 2;
        }

        int index = items.Count;
        items.Add(item);
        inventorySlots[index].sprite = items[index].Sprite;

        return 0;
    }

    public bool HasItem(string itemName)
    {
        for(int x = 0; x < items.Count; ++x)
        {
            if (items[x].Name == itemName)
                return true;
        }

        return false;
    }

    public void RemoveItem(Item item)
    {
        bool removed = false;
        for (int x = 0; x < inventorySlots.Length; ++x)
        {
            if (x < items.Count)
            {
                if (items[x] == item)
                {
                    items.RemoveAt(x);
                    inventorySlots[x].sprite = (x % items.Count != 0) ? items[x].Sprite : null;

                    removed = true;
                    continue;
                }

                if (removed)
                    inventorySlots[x].sprite = items[x].Sprite;

                continue;
            }

            inventorySlots[x].sprite = null;
        }
    }
    public void RemoveItem(string item)
    {
        bool removed = false;
        for (int x = 0; x < inventorySlots.Length; ++x)
        {
            if (x < items.Count)
            {
                if(items[x].Name == item)
                {
                    items.RemoveAt(x);
                    inventorySlots[x].sprite = (items.Count != 0 && x % items.Count != 0) ? items[x].Sprite : null;

                    removed = true;
                    continue;
                }

                if(removed)
                    inventorySlots[x].sprite = items[x].Sprite;

                continue;
            }

            inventorySlots[x].sprite = null;
        }
    }
}
