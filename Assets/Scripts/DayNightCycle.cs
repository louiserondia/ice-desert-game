using System;
using System.Collections.Specialized;
using System.Threading;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [SerializeField] private Light _light;
    [SerializeField] private float _rotationSpeed;

    [SerializeField] private GameObject _mesh;
    [SerializeField] private float _distSun; 

    private float _time = 6f;

    public Light GetDirectionalLight
    {
        get { return _light; }
    }

    public GameObject GetMesh
    {
        get { return _mesh; }
    }

    public float GetRelativeTime
    {
        get { return _time * _rotationSpeed; }
    }

    public float GetAbsoluteTime
    {
        get { return _time; }
    }


    void Update()
    {
        _time += Time.deltaTime;
    }

    void FixedUpdate()
    {
        Vector3 pos = _mesh.transform.position;
        pos.x = _distSun * Mathf.Cos(_time * _rotationSpeed);
        pos.y = _distSun * Mathf.Sin(_time * _rotationSpeed);
        _mesh.transform.position = pos;

        if (_light == null) return;

            _light.transform.rotation = Quaternion.LookRotation(Vector3.Normalize(-pos));
        if (pos.y < 0)
            _light.intensity = 0f;
        else
            _light.intensity = 1f;
    }

    public bool IsAboveHorizon()
    {
        return _mesh.transform.position.y > 0;
    }
}

