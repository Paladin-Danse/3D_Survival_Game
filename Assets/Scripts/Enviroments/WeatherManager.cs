using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Weather { SUNNY, RAIN }

public class WeatherManager : MonoBehaviour
{
    public AudioClip rainsound;
    public AudioSource audioSource;

    static public WeatherManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public Weather currentWeather;
    public ParticleSystem rain;
    public float weather_time = 55f;
    public int next_weather;

    public ParticleSystem lightning;

    void Start()
    {
        currentWeather = Weather.SUNNY;
        next_weather = 1;

    }

    public void ChangeWeather(Weather weatherType)
    {
        if (weatherType != this.currentWeather)
        {
            switch (weatherType)
            {
                case Weather.SUNNY:
                    currentWeather = Weather.SUNNY;
                    this.rain.Stop();
                    this.lightning.Stop();
                    break;
                case Weather.RAIN:
                    currentWeather = Weather.RAIN;
                    this.rain.Play();
                    this.lightning.Play();
                    audioSource.PlayOneShot(rainsound);
                    break;
            }
        }
         
    }

    void Update()
    {
        this.weather_time -= Time.deltaTime;
        if (next_weather == 1) 
        {
            if (this.weather_time <= 0) 
            {
                next_weather = Random.Range(0, 2);
                ChangeWeather(Weather.RAIN);
                weather_time = 55f;

            }
        }
        if (next_weather == 0)
        {
            if (this.weather_time <= 0) 
            {
                next_weather = Random.Range(0, 2);
                ChangeWeather(Weather.SUNNY);
                weather_time = 55f;
            }
        }
    }



}
