using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Weather : MonoBehaviour
{
    [SerializeField] GameObject weatherRain;
    [SerializeField] GameObject weatherSun;
    [SerializeField] GameObject thermometer;
    [SerializeField] public TextMeshProUGUI temperatureText;

    public int damage;
    public float damageRate;

    [HideInInspector] public int temperature;

    private HashSet<IDamagable> temperatureToDamage = new HashSet<IDamagable>();

    public static Weather _instance;
    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;

    }

    private void Start()
    {
        InvokeRepeating("TemperatureDamage", 0, damageRate);
    }

    private void Update()
    {
        temperature = TemperatureValue(int.Parse(temperatureText.text));
        TemperatureColorChange(temperature);
    }


    void TemperatureDamage()
    {
        int temperature = int.Parse(temperatureText.text);

        if(temperature < 10 || temperature > 25)
        {
            foreach (IDamagable damageable in temperatureToDamage)
            {
                damageable.TakePhysicalDamage(damage);
            }
        }
    }

    void TemperatureColorChange(int temperature)
    {
        temperatureText.text = temperature.ToString();

        if (temperature > 25)
        {
            temperatureText.color = new Color32(215, 56, 94, 255);
            this.gameObject.transform.GetChild(0).GetChild(2).GetComponent<Image>().color = new Color32(215, 56, 94, 255);
        }
        else if (temperature <= 10)
        {
            temperatureText.color = new Color32(12, 197, 243, 255);
            this.gameObject.transform.GetChild(0).GetChild(2).GetComponent<Image>().color = new Color32(12, 197, 243, 255);
        }
        else
        {
            temperatureText.color = new Color32(0, 0, 0, 255);
            this.gameObject.transform.GetChild(0).GetChild(2).GetComponent<Image>().color = new Color32(0, 0, 0, 255);
        }
    }

    private int TemperatureValue(float temperature)
    {
        AnimationCurve sunIntensity = DayNightCycle._instance.sunIntensity;
        AnimationCurve moonIntensity = DayNightCycle._instance.moonIntensity;

        float time = DayNightCycle._instance.time;
        float sunTime = sunIntensity.Evaluate(time);
        float moonTime = moonIntensity.Evaluate(time);

        if(moonTime == 0)
        {
            temperature = sunTime * 15f + 15f;
        }
        
        if(sunTime == 0)
        {
            temperature = 15f - moonTime * 15f;
        }
        
        
        return (int)temperature;
    }
}
