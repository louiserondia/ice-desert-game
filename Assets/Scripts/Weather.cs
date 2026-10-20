using System.Diagnostics;
using UnityEngine;

public class Weather : MonoBehaviour
{

    [SerializeField] private float _minTemperature;
    [SerializeField] private float _maxTemperature;
    [SerializeField] private float _temperature;
    [SerializeField] private DayNightCycle _dayNightCycle;
    [SerializeField] private float _weatherChangeSpeed;
    private Transform _lightTransform;

    public float GetTemperature
    {
        get { return _temperature; }
    }


    private void Awake()
    {
        _lightTransform = _dayNightCycle.GetDirectionalLight.transform;
    }

    private void Update()
    {
        //UnityEngine.Debug.Log("weather: " + _temperature);

        if (_temperature > _minTemperature || _temperature < _maxTemperature)
        {
            float sign = _lightTransform.forward.y < 0 ? 1 : -1;
            _temperature += sign * Time.deltaTime * _weatherChangeSpeed;
        }
    }
}
