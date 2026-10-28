using System;
using System.Collections.Specialized;
using System.Threading;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [SerializeField] private Light _light;
    [SerializeField] private float _rotationSpeed;

    [SerializeField] private GameObject _mesh; // could be found bc direct child no? or called mesh
    [SerializeField] private float _distSun; 

    public Light GetDirectionalLight
    {
        get { return _light; }
    }

    public GameObject GetMesh
    {
        get { return _mesh; }
    }

    private float time = 9f;

    void Update()
    {
        time += Time.deltaTime;
    }

    void FixedUpdate()
    {
        //_light.transform.Rotate(Vector3.right, _rotationSpeed * Time.deltaTime);

        Vector3 pos = _mesh.transform.position;
        pos.x = _distSun * Mathf.Cos(time * _rotationSpeed);
        pos.y = _distSun * Mathf.Sin(time * _rotationSpeed);
        _mesh.transform.position = pos;

        _light.transform.rotation = Quaternion.LookRotation(Vector3.Normalize(-pos));
    }

    public bool IsAboveHorizon()
    {
        return _mesh.transform.position.y > 0;
    }
}

