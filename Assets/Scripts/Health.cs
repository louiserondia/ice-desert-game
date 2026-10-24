using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class Health : MonoBehaviour
{

    //[SerializeField] private int _startHealth = 10;
    //private int _currentHealth = 0;

    //[SerializeField] private int _startTemperature = 0;
    [SerializeField] private Weather _weather;
    private float _temperature;
    public Slider slider;

    private void Awake()
    {
        //_currentHealth = _startHealth;
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
        float sign = _weather.GetTemperature > 0 ? 1 : -1;
        _temperature += _weather.GetTemperature * UnityEngine.Time.deltaTime * 0.02f;
        slider.value = _temperature;

        //UnityEngine.Debug.Log("Player: " + _temperature);
        if (_temperature > 100f || _temperature < -100f)
        {
            Kill();
        }
    }

    //public void Damage(int amount)
    //{
    //    _currentHealth -= amount;
    //    if (_currentHealth <= 0)
    //    {
    //        Kill();
    //    }    
    //}

    void Kill()
    {
        Debug.Log("DEAD !!!!!!");
        //Destroy(gameObject);
    }

    public void AddToTemperature(float amount)
    {
        _temperature += amount;
        //UnityEngine.Debug.Log("Player: " + _temperature + ", amount: " + amount);
    }

}
