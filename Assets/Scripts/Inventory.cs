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
    //public Slot[] slots;
    public Dictionary<string, int> slots;

    //public event Action<int> OnSlotChanged;

    void Awake()
    {
        ToggleWoodVisibility(false);
        inventoryUI = GameObject.Find("InventoryUI").GetComponent<InventoryUI>();
        slots = new Dictionary<string, int>();
        slots["wood"] = 0;
    }

    public int Add(Pickup pickup)
    {
        // Ajoute 1 "Sword"
        slots[pickup.typeName] = slots.GetValueOrDefault(pickup.typeName, 0) + pickup.amount;

        if (slots[pickup.typeName] > pickup.maxStack)
        {
            UnityEngine.Debug.Log(slots[pickup.typeName] - pickup.maxStack + " items left");
        }
        UnityEngine.Debug.Log(slots[pickup.typeName] + " items in inventory of type " + pickup.typeName);

        // for wood
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
