using System;
using System.Collections.Specialized;
using System.Threading;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [SerializeField] private Light _light;
    [SerializeField] private float _rotationSpeed;

    [SerializeField] private GameObject _sunMesh;
    [SerializeField] private float _distSun;

    public Light GetDirectionalLight
    {
        get { return _light; }
    }

    public GameObject GetSunMesh
    {
        get { return _sunMesh; }
    }

    private float time;

    void Update()
    {
        time += Time.deltaTime;
    }

    void FixedUpdate()
    {
        //_light.transform.Rotate(Vector3.right, _rotationSpeed * Time.deltaTime);

        Vector3 pos = _sunMesh.transform.position;
        pos.x = _distSun * Mathf.Cos(time * _rotationSpeed);
        pos.y = _distSun * Mathf.Sin(time * _rotationSpeed);
        _sunMesh.transform.position = pos;

        _light.transform.rotation = Quaternion.LookRotation(Vector3.Normalize(-pos));
    }
}

