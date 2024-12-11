using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherSystem : MonoBehaviour
{

    [SerializeField] GameObject[] DirectionalLights;
    [SerializeField] Material[] Skyboxes;
    [SerializeField] ParticleSystem Rain;
    [SerializeField] Color RainyWeatherSkyColor;
    [SerializeField] Color SunnyWeatherSkyColor;
    [SerializeField] Light dirLight;
    [SerializeField] Material DayTimeSkybox;

    private void Start()
    {
        RenderSettings.skybox.SetFloat("_Exposure", 1f);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            RainyWeather();

        if (Input.GetKeyDown(KeyCode.Y))
            SunnyWeather();
    }

    public void RainyWeather()
    {

        //DirectionalLights[0].SetActive(false);

        //RenderSettings.skybox = Skyboxes[1];
        //RenderSettings.ambientSkyColor = RainyWeatherSkyColor;
        //Rain.Play();

        StartCoroutine(ChangeWeather(true));
    }

    public void SunnyWeather()
    {
        //DirectionalLights[0].SetActive(true);

        //RenderSettings.skybox = Skyboxes[0];
        //RenderSettings.ambientSkyColor = SunnyWeatherSkyColor;
        //Rain.Stop();

        StartCoroutine(ChangeWeather(false));
    }

    IEnumerator ChangeWeather(bool isRainyWeather)
    {
        if (isRainyWeather)
        {
            RenderSettings.ambientSkyColor = RainyWeatherSkyColor;
            float _lightColor = dirLight.intensity;
            float timeToLerp = 0;

            while (timeToLerp < 1)
            {
                timeToLerp += Time.deltaTime / 1;
                _lightColor = Mathf.Lerp(1.4f, 0, timeToLerp);
                dirLight.intensity = _lightColor;
                if (_lightColor > 0.4f)
                    RenderSettings.skybox.SetFloat("_Exposure", _lightColor);
                yield return new WaitForEndOfFrame();
            }

            RenderSettings.skybox = Skyboxes[1];
            Rain.Play();
        }
        else
        {
            RenderSettings.skybox = Skyboxes[0];
            RenderSettings.ambientSkyColor = SunnyWeatherSkyColor;
            float _lightColor = dirLight.intensity;
            float timeToLerp = 0;

            while (timeToLerp < 1)
            {
                timeToLerp += Time.deltaTime / 1;
                _lightColor = Mathf.Lerp(0, 1.4f, timeToLerp);
                dirLight.intensity = _lightColor;
                //if (timeToLerp < 1)
                RenderSettings.skybox.SetFloat("_Exposure", timeToLerp);
                yield return new WaitForEndOfFrame();
            }

            Rain.Stop();
        }
    }
}
