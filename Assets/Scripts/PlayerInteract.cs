using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private Inventory _inventory;
    [SerializeField] private GameObject _campFirePrefab;

    private List<GameObject> _availablePickups = new List<GameObject>();
    private List<GameObject> _availableHeatSources = new List<GameObject>();

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pickup"))
        {
            _availablePickups.Add(other.gameObject);
        }
        if (other.CompareTag("HeatSource"))
        {
            _availableHeatSources.Add(other.gameObject);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Pickup"))
        {
            _availablePickups.Remove(other.gameObject);
        }
        if (other.CompareTag("HeatSource"))
        {
            _availableHeatSources.Remove(other.gameObject);
        }
    }

    void Update()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            TryPickup();
        }

        // heats up if list is not empty
        GetComponent<Health>().ToggleHeatSource(_availableHeatSources.Count > 0);
    }

    void TryPickup()
    {
        if (_availablePickups.Count == 0) return;

        GameObject pickupObject = _availablePickups[0].gameObject;
        Pickup pickup = pickupObject.GetComponent<Pickup>();

        if (_inventory.Add(pickup) != -1)
        {
            _availablePickups.RemoveAt(0);
            Destroy(pickupObject);
        }
    }

    public void BurnWood()
    {
        Inventory inventory = GetComponent<Inventory>();

        if (inventory.slots["wood"] <= 0) return;
        inventory.slots["wood"]--;

        InventoryUI inventoryUI = GameObject.Find("InventoryUI").GetComponent<InventoryUI>();
        inventoryUI.UpdateItemAmount("wood", inventory.slots["wood"]);

        if (inventory.slots["wood"] <= 0)
        {
            inventory.ToggleWoodVisibility(false);
        }

        Vector3 position = transform.position + Vector3.right * 3f + Vector3.up;
        GameObject instanceCampFire = Instantiate(_campFirePrefab, position, Quaternion.identity);
        StartCoroutine(RemoveAfterDelay(instanceCampFire, 15f));
    }

    // co routine that will remove from list and delete heatsource after delay
    IEnumerator RemoveAfterDelay(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        _availableHeatSources.Remove(obj);
        Destroy(obj);
    }
}