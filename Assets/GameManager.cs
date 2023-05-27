using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Slider tempSlider;

    public GameObject protonPrefab;
    public GameObject DeuterPrefab;
    public GameObject He_3Prefab;
    public GameObject He_4Prefab;
    public GameObject Li_7Prefab;
    public GameObject C_12Prefab;

    public float protonSpeed;


    float sunTemp;

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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
