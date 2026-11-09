using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct Slot
{
    //public ItemId item;
    public string itemName;
    public int count;
};

public class Inventory : MonoBehaviour
{

    [SerializeField] private GameObject _woodSocket;
    private InventoryUI inventoryUI;
    public Dictionary<string, int> slots;

    void Awake()
    {
        ToggleWoodVisibility(false);
        inventoryUI = GameObject.Find("InventoryUI").GetComponent<InventoryUI>();
        slots = new Dictionary<string, int>();
        slots["wood"] = 0;
    }

    public int Add(Pickup pickup)
    {
        // adds to existing item count or creates slot and add
        slots[pickup.typeName] = slots.GetValueOrDefault(pickup.typeName, 0) + pickup.amount;

        // specific for wood, toggles visibility (back of the truck)
        if (pickup.typeName == "wood" && slots[pickup.typeName] > 0)
        {
            ToggleWoodVisibility(true);
        }

        inventoryUI.UpdateItemAmount(pickup.typeName, slots[pickup.typeName]);
        return 0;
    }

    public void ToggleWoodVisibility(bool visible)
    {
        _woodSocket.SetActive(visible);

    }

}
