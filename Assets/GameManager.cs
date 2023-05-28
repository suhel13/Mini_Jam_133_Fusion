using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Slider tempSlider;
    public TMPro.TextMeshProUGUI tempValueText;

    public float atomScale = 1f;

    public float minTimeToFuze = 0.1f;

    [SerializeField] float MeVtoK_Ratio = 0.1f;
    public GameObject electronPrefab;
    public GameObject protonPrefab;
    public GameObject DeuterPrefab;
    public GameObject He_3Prefab;
    public GameObject He_4Prefab;
    public GameObject Be_7Prefab;
    public GameObject Be_8Prefab;
    public GameObject Li_7Prefab;
    public GameObject B_8Prefab;
    public GameObject C_12Prefab;
    public GameObject N_13Prefab;
    public GameObject C_13Prefab;
    public GameObject N_14Prefab;
    public GameObject O_15Prefab;
    public GameObject N_15Prefab;

    public float protonSpeed;
    public float electronSpeed;

    public float Be7LiveSpan;
    public float Be8LiveSpan;
    public float B8LiveSpan;
    public float N13LiveSpan;
    public float O15LiveSpan;
    public float Mg24LiveSpan;
    public float S32LiveSpan;
    public float sunTemp;

    public float pp2Temp;
    public float pp3Temp;
    public float CNOTemp;
    public float threeAlpha;
    public float threeAlpha2;
    public float carbonBurn;
    public float neonBurn;
    public float oxygenBurn;
    public float siliconBurn;

    public static GameManager Instance { get; private set; }
    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public void raiseSunTemperature(float EnergyMeV)
    {
        sunTemp += EnergyMeV * MeVtoK_Ratio;
        tempSlider.value = sunTemp / 50;
        tempValueText.text =Mathf.Round( sunTemp*10)/10 + " M";
    }
}
