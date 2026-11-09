using System.Diagnostics;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    //[SerializeField] private Weather _weather; // maybe completely disable weather, only keep day night cycle

    [SerializeField] private Material _hotMaterial;
    [SerializeField] private Material _coldMaterial;
    [SerializeField] private GameObject _temperatureIndicator;
    [SerializeField] private float _sunHeatSpeed;
    [SerializeField] private float _freezeSpeed;
    [SerializeField] private float _shadowCoolingSpeed;
    private float _fireSpeed;

    private GameObject _sun;
    private float _temperature;
    private const float _maxTemperature = 100f;

    private bool _isInsideEndZone = false;
    private bool _isDead = false;
    private bool _isInHeatSource = false;

    public bool IsDead
    {
        get { return _isDead; }
    }

    private void Awake()
    {
        _sun = GameObject.FindWithTag("Sun");
        _fireSpeed = _shadowCoolingSpeed * 2f;
    }

    void Start()
    {
        // reset all data if game restarts after death
        _temperature = 0f;
        _isDead = false;
        _isInHeatSource = false;
    }

    void Update()
    {
        if (_isDead)
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
        float width = _temperatureIndicator.transform.parent.GetComponent<RectTransform>().rect.width;
        float unit = width / (_maxTemperature * 2);

        return temperature * unit;
    }

    void UpdateMaterial(string path, Material material)
    {
        GameObject obj = GameObject.Find(path);
        obj.GetComponent<MeshRenderer>().material = material;
    }

    void UpdateCarMaterial(Material material)
    {
        UpdateMaterial("BodyVisual/Body", material);
        UpdateMaterial("BodyVisual/BodyFront", material);
        UpdateMaterial("BodyVisual/BodyBack/left", material);
        UpdateMaterial("BodyVisual/BodyBack/right", material);
        UpdateMaterial("BodyVisual/BodyBack/back", material);
    }

    void UpdateTemperature()
    {
        if (_isInsideEndZone) return;

        DayNightCycle sunCycle = _sun.GetComponent<DayNightCycle>();
        Vector3 sunPos = sunCycle.GetMesh.transform.position;
        Vector3 directionSun = transform.position - sunPos;
        Vector3 sunLightDirection = sunCycle.GetDirectionalLight.transform.forward;
        GameObject body = GameObject.Find("BodyVisual/Body");

        if (sunCycle.IsAboveHorizon())
        {
            bool did_hit = Physics.Raycast(body.transform.position, -sunLightDirection, 10000);
            UnityEngine.Debug.DrawRay(body.transform.position, -sunLightDirection * 10000, Color.red);

            //day in shadow
            if (did_hit)
            {
                // cools down but not lower than 0 degrees
                if (_temperature > 0)
                    _temperature -= UnityEngine.Time.deltaTime * _shadowCoolingSpeed;
                UpdateCarMaterial(_coldMaterial);
            }
            //day in sun
            else
            {
                // non linear effect of the heat for tension (slows down if almost dying) 
                float reduction = _temperature > (_maxTemperature / 5 * 4) ? 0.5f : 1f;
                if (_temperature < _maxTemperature)
                    _temperature += UnityEngine.Time.deltaTime * _sunHeatSpeed * reduction;
                UpdateCarMaterial(_hotMaterial);
            }
        }
        // night and no fire
        else if (!_isInHeatSource)
        {
            float reduction = _temperature < (-_maxTemperature / 5 * 4) ? 0.5f : 1f;
            _temperature -= UnityEngine.Time.deltaTime * _freezeSpeed * reduction;
            UpdateCarMaterial(_coldMaterial);
        }

        if (_isInHeatSource)
        {
            _temperature += UnityEngine.Time.deltaTime * _fireSpeed;
            UpdateCarMaterial(_hotMaterial);
        }

        _temperatureIndicator.transform.localPosition = new Vector3(
            GetTemperatureIndicatorPosition(_temperature),
            _temperatureIndicator.transform.localPosition.y,
            _temperatureIndicator.transform.localPosition.z
        );

        if (_temperature > _maxTemperature || _temperature < -_maxTemperature)
        {
            Kill();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "EndZone")
        {
            _isInsideEndZone = true;
            UnityEngine.Debug.Log("Entered EndZone");
            Kill();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "EndZone")
        {
            _isInsideEndZone = false;
            UnityEngine.Debug.Log("Exited EndZone");
        }
    }

    void Kill()
    {
        //UnityEngine.Debug.Log("DEAD !!!!!!");
        //Destroy(gameObject);
        _isDead = true;
    }

    public void AddToTemperature(float amount)
    {
        _temperature += amount;
    }

    public void ToggleHeatSource(bool on)
    {
        _isInHeatSource = on;
    }

}
