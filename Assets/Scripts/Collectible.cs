using System;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    // choose material depending on its properties

    [SerializeField] float _temperature; // create defaults 

    void Start()
    {

    }

    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponentInParent<Health>().AddToTemperature(_temperature);
            Destroy(gameObject);
        }
    }
}
