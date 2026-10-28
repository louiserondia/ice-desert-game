using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class Health : MonoBehaviour
{
    //[SerializeField] private Weather _weather; // maybe completely disable weather, only keep day night cycle

    [SerializeField] private Material _hotMaterial;
    [SerializeField] private Material _coldMaterial;
    [SerializeField] private GameObject _temperatureIndicator;
    [SerializeField] private float _heatSpeed;
    [SerializeField] private float _coolSpeed;
    [SerializeField] private float _freezeSpeed;

    private GameObject _sun;
    private float _temperature;

    private bool isInsideEndZone = false;
    private bool isDead = false;

    private void Awake()
    {
        _sun = GameObject.FindWithTag("Sun");
    }

    void Start()
    {
        // reset all data if game restarts after death
        _temperature = 0f;
        isDead = false;
      
    }

    void Update()
    {
        if (isDead)
        {
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                Start();
            }
            return;
        }
        UpdateTemperature();
    }

    float GetTemperatureIndicatorPosition(float temperature)
    {
        const float max = 100;
        float width = _temperatureIndicator.transform.parent.GetComponent<RectTransform>().rect.width;
        float unit = width / max;

        return temperature * unit;
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
                _temperature += (sunPos.y / directionSun.magnitude) * UnityEngine.Time.deltaTime * _heatSpeed;
                body.GetComponent<MeshRenderer>().material = _hotMaterial;
            }
            else if (_temperature > 0)
            {
                _temperature -= UnityEngine.Time.deltaTime * _coolSpeed;
                body.GetComponent<MeshRenderer>().material = _coldMaterial;
            }
        }
        else
        {
            _temperature -= UnityEngine.Time.deltaTime * _freezeSpeed;
        }

        _temperatureIndicator.transform.localPosition = new Vector3(
            GetTemperatureIndicatorPosition(_temperature),
            _temperatureIndicator.transform.localPosition.y,
            _temperatureIndicator.transform.localPosition.z
        );

        if (_temperature > 100f || _temperature < -100f)
        {
            Kill();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
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
        isDead = true;
    }

    public void AddToTemperature(float amount)
    {
        _temperature += amount;
    }

}

//UnityEngine.Debug.DrawRay(_sun.transform.position, directionSun * 1000, Color.white);