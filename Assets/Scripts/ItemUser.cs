using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ItemUser : ConditionalCaller
{
    [SerializeField]
    private InventoryManager inventory = null;
    [SerializeField]
    private string itemName = "";
    [SerializeField]
    private bool destroyItem = false;

    [Space(10)]
    public UnityEvent broadcastEvent = new UnityEvent();

    public override void Broadcast()
    {
        broadcastEvent.Invoke();
    }

    public override void Callback()
    {
        if (destroyItem)
            inventory.RemoveItem(itemName);
    }

    public override bool ConditionMet()
    {
        return inventory.HasItem(itemName);
    }
}
