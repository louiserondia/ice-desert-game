using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private Inventory _inventory;
    [SerializeField] private GameObject _campFirePrefab;

    private List<GameObject> AvailablePickups = new List<GameObject>();
    private List<GameObject> AvailableHeatSources = new List<GameObject>();

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pickup"))
        {
            AvailablePickups.Add(other.gameObject);
        }
        if (other.CompareTag("HeatSource"))
        {
            AvailableHeatSources.Add(other.gameObject);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Pickup"))
        {
            AvailablePickups.Remove(other.gameObject);
        }
        if (other.CompareTag("HeatSource"))
        {
            AvailableHeatSources.Remove(other.gameObject);
        }
    }

    //public List<GameObject> GetSortedByDistance()
    //{
    //    Vector3 center = transform.position;
    //    pickupsInside.Sort((a, b) =>
    //        Vector3.SqrMagnitude(a.transform.position - center)
    //        .CompareTo(Vector3.SqrMagnitude(b.transform.position - center))
    //    );
    //    return pickupsInside;
    //}

    void Update()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame)
            TryPickup();

        // if list is not empty, lets health know it need to heat up
        GetComponent<Health>().ToggleHeatSource(AvailableHeatSources.Count > 0);
    }

    void TryPickup()
    {
        if (AvailablePickups.Count == 0) return;

        GameObject pickupObject = AvailablePickups[0].gameObject;
        Pickup pickup = pickupObject.GetComponent<Pickup>();

        if (_inventory.Add(pickup) != -1)
        {
            AvailablePickups.RemoveAt(0);
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
        GameObject instance = Instantiate(_campFirePrefab, position, Quaternion.identity);
        StartCoroutine(RemoveAfterDelay(instance, 15f));
    }

    // co routine that will delete heatsource after delay and remove from list
    IEnumerator RemoveAfterDelay(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        AvailableHeatSources.Remove(obj);
        Destroy(obj);
    }
}