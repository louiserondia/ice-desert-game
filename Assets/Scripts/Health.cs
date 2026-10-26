using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class Health : MonoBehaviour
{
    [SerializeField] private Weather _weather;
    [SerializeField] private Material _hotMaterial;
    [SerializeField] private Material _coldMaterial;
    private float _temperature;
    public Slider slider;

    private GameObject _sunMesh;

    private void Awake()
    {
        _sunMesh = GameObject.FindWithTag("Sun");
        UnityEngine.Debug.Log(_sunMesh.name);


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
        RaycastHit hit;

        Vector3 direction = transform.position - _sunMesh.transform.position;
        Physics.Raycast(_sunMesh.transform.position, direction, out hit, Mathf.Infinity);
            var body = transform.Find("BodyVisual/Body");

        if (hit.collider.gameObject.name == "BodyVisual")
        {
            UnityEngine.Debug.DrawRay(_sunMesh.transform.position, direction * hit.distance, Color.yellow);
     
            UnityEngine.Debug.Log("temp " + _temperature);
            _temperature += (_sunMesh.transform.position.y / direction.magnitude) * UnityEngine.Time.deltaTime * 10;

            body.GetComponent<MeshRenderer>().material = _hotMaterial;
            //material gets red 
        }
        else
        {
            _temperature -= UnityEngine.Time.deltaTime * 5;
            UnityEngine.Debug.Log(UnityEngine.Time.deltaTime * 5);

            UnityEngine.Debug.DrawRay(_sunMesh.transform.position, direction * 1000, Color.white);
            body.GetComponent<MeshRenderer>().material = _coldMaterial;
            //material gets blue
        }


        //float sign = _weather.GetTemperature > 0 ? 1 : -1;
        //_temperature += _weather.GetTemperature * UnityEngine.Time.deltaTime * 0.02f;
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
        //UnityEngine.Debug.Log("DEAD !!!!!!");
        //Destroy(gameObject);
    }

    public void AddToTemperature(float amount)
    {
        _temperature += amount;
        //UnityEngine.Debug.Log("Player: " + _temperature + ", amount: " + amount);
    }

}
