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
    public GameObject O_16Prefab;
    public GameObject Ne_20Prefab;
    public GameObject Mg_24Prefab;
    public GameObject Si_28Prefab;
    public GameObject S_32Prefab;
    public GameObject Ni_56Prefab;

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

    float energyGainBoost = 1;

    public bool isColapsing = true;

    public static GameManager Instance { get; private set; }
    public PopUpManager popUpManager { get; private set; }
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
            popUpManager = GetComponentInChildren<PopUpManager>();
        }
    }
    private void Update()
    {
        
    }

    void updateTempUI()
    {
        tempSlider.value = Mathf.Pow(GameManager.Instance.sunTemp , 1/3.714011909f);
        tempValueText.text = Mathf.Round(sunTemp * 10) / 10 + " M";
    }
    public void raiseSunTemperature(float EnergyMeV)
    {
        if (sunTemp > CNOTemp)
            energyGainBoost = 4;
        else if (sunTemp > neonBurn)
            energyGainBoost = 8;
        else
            energyGainBoost = 1;

        sunTemp += EnergyMeV * MeVtoK_Ratio * energyGainBoost;
        updateTempUI();
        checkForNewTutorial();
    }

    void checkForNewTutorial()
    {
        switch (popUpManager.knownPopUp)
        {
            case 0:
                {
                    if (sunTemp > 1)
                        popUpManager.lernNextPopUp();
                }
                break;

            case 1:
                {
                    if (sunTemp > pp2Temp)
                        popUpManager.lernNextPopUp();
                }
                break;
            case 2:
                {
                    if (sunTemp > threeAlpha)
                        popUpManager.lernNextPopUp();
                }
                break;
            case 3:
                {
                    if (sunTemp > pp3Temp)
                        popUpManager.lernNextPopUp();
                }
                break;
            case 4:
                {
                    if (sunTemp > CNOTemp)
                        popUpManager.lernNextPopUp();
                }
                break;
            case 5:
                {
                    if (sunTemp > carbonBurn)
                        popUpManager.lernNextPopUp();
                }
                break;
            case 6:
                {
                    if (sunTemp > neonBurn)
                        popUpManager.lernNextPopUp();
                }
                break;
            case 7:
                {
                    if (sunTemp > oxygenBurn)
                        popUpManager.lernNextPopUp();
                }
                break;
            case 8:
                {
                    if (sunTemp > siliconBurn)
                        popUpManager.lernNextPopUp();
                }
                break;
        }
    }
}
