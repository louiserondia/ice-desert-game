using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class Health : MonoBehaviour
{
    //[SerializeField] private Weather _weather; // maybe completely disable weather, only keep day night cycle

    [SerializeField] private Material _hotMaterial;
    [SerializeField] private Material _coldMaterial;

    private GameObject _sun;
    private float _temperature;
    public Slider slider;

    private bool isInsideEndZone = false;

    private void Awake()
    {
        _sun = GameObject.FindWithTag("Sun");
    }

    void Start()
    {
        _temperature = 0f;
    }

    void Update()
    {
        UpdateTemperature();
    }

    void UpdateTemperature()
    {
        if (isInsideEndZone) return;
        RaycastHit hit;
        var body = transform.Find("BodyVisual/Body");
        DayNightCycle sunCycle = _sun.GetComponent<DayNightCycle>();

        Vector3 sunPos = sunCycle.GetMesh.transform.position;
        Vector3 directionSun = transform.position - sunPos;

        // day time, sun is up, warm up in the sun and cools down in the shade but not lower than 0 degrees
        if (sunCycle.IsAboveHorizon())
        {
            Physics.Raycast(sunPos, directionSun, out hit, Mathf.Infinity);

            if (hit.collider.gameObject.name == "BodyVisual")
            {
                _temperature += (sunPos.y / directionSun.magnitude) * UnityEngine.Time.deltaTime * 10;
                body.GetComponent<MeshRenderer>().material = _hotMaterial;
            }
            else if (_temperature>0)
            {
                _temperature -= UnityEngine.Time.deltaTime * 5;
                body.GetComponent<MeshRenderer>().material = _coldMaterial;
            }
        }

        slider.value = _temperature;

        if (_temperature > 100f || _temperature < -100f)
        {
            Kill();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Optional: check it's the cylinder specifically
        if (other.name == "EndZone")
        {
            isInsideEndZone = true;
            UnityEngine.Debug.Log("Entered EndZone");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "EndZone")
        {
            isInsideEndZone = false;
            UnityEngine.Debug.Log("Exited EndZone");
        }
    }

    void Kill()
    {
        //UnityEngine.Debug.Log("DEAD !!!!!!");
        //Destroy(gameObject);
    }

    public void AddToTemperature(float amount)
    {
        _temperature += amount;
    }

}

//UnityEngine.Debug.DrawRay(_sun.transform.position, directionSun * 1000, Color.white);