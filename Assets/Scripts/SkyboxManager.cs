using System.Threading;
using UnityEngine;

public class SkyboxManager : MonoBehaviour
{
    private GameObject _sun;
    private DayNightCycle _sunCycle;

    [SerializeField] private Material _skyboxMaterial;

    private void Awake()
    {
        _sun = GameObject.FindWithTag("Sun");
        _sunCycle = _sun.GetComponent<DayNightCycle>();
    }

    void Start()
    {
        RenderSettings.skybox = _skyboxMaterial;
    }

    void Update()
    {
        _skyboxMaterial.SetFloat("_time_value", _sunCycle.GetRelativeTime);
    }
}
