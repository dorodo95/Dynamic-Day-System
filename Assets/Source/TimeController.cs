using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TimeController : MonoBehaviour {

	public Light _sunLight;
    public Light _moonLight;
    private Transform _lightTransform;
    public AnimationCurve sunIntensityCurve;
    public AnimationCurve moonIntensityCurve;
    float time;
    public Gradient skyGradient;
    public Gradient horizonGradient;
    public Gradient sunGradient;
    public Gradient moonGradient;
    private Color _skyColor;
    private Color _horizonColor;
    private Light _sun;
    private Light _moon;
    private int zenithColorID;
    private int horizonColorID;
    private int directionalColorID;
    private int sunIntensityID;
    private int timeID;
    private int sunDirectionID;
    private int moonDirectionID;
    public float duration = 60f;
    [HideInInspector] public int hours;
    [HideInInspector] public int minutes;
    public bool stopTime;
    [HideInInspector] public float timeRange;
    private Material skyMat;

    void Awake()
    {
        _skyColor = RenderSettings.ambientLight;
        _sun = _sunLight.GetComponent<Light>();
        _moon = _moonLight.GetComponent<Light>();
        _lightTransform = _sunLight.GetComponent<Transform>();

        //Set Shader Properties
        zenithColorID = Shader.PropertyToID("_ZenithColor");
        horizonColorID = Shader.PropertyToID("_HorizonColor");
        directionalColorID = Shader.PropertyToID("_DirectionalColor");
        sunIntensityID = Shader.PropertyToID("_SunIntensity");
        timeID = Shader.PropertyToID("_CurrentTime");
        sunDirectionID = Shader.PropertyToID("_SunDirection");
        moonDirectionID = Shader.PropertyToID("_MoonDirection");


        skyMat = new Material(RenderSettings.skybox);
        RenderSettings.skybox = skyMat;
    }    

    void Update ()
    {
        if (!stopTime)
        {
            time += Time.deltaTime / duration;
            if (time >= 1) time -= 1;
            timeRange = time;
        }

        else
        {
            time = timeRange;
            RenderSettings.ambientLight = skyGradient.Evaluate(Mathf.Repeat(time, 1.0f));

        }
        

        hours = Mathf.FloorToInt(time * 24);
        minutes = Mathf.FloorToInt(time * 1440 - hours * 60);

        UpdateDayCycle();

        Debug.Log(time);
    }

    private void UpdateDayCycle()
    {
        _sun.enabled = _sunLight.intensity > 0;
        _moon.enabled = _moonLight.intensity > 0;
        _lightTransform.localEulerAngles = new Vector3(0, time * 360 + 170, 0);
        RenderSettings.ambientLight = skyGradient.Evaluate(Mathf.Repeat(time, 1.0f));
        _horizonColor = horizonGradient.Evaluate(Mathf.Repeat(time, 1.0f));
        _sun.color = sunGradient.Evaluate(Mathf.Repeat(time, 1.0f));
        _moon.color = moonGradient.Evaluate(Mathf.Repeat(time, 1.0f));
        skyMat.SetColor(zenithColorID, RenderSettings.ambientLight);
        skyMat.SetColor(directionalColorID, _sun.color);
        skyMat.SetColor(horizonColorID, _horizonColor);
        skyMat.SetVector(sunDirectionID, _sunLight.transform.forward);
        Shader.SetGlobalFloat(sunIntensityID, _sunLight.intensity);
        Shader.SetGlobalFloat(timeID, time);
        Shader.SetGlobalVector(sunDirectionID, _sunLight.transform.forward);
        Shader.SetGlobalVector(moonDirectionID, _moonLight.transform.forward);
        Debug.Log(_horizonColor);
        RenderSettings.fogColor = _horizonColor;
        _sunLight.intensity =  sunIntensityCurve.Evaluate(time * 24);
        _moonLight.intensity = moonIntensityCurve.Evaluate(time * 24);
    }
}
