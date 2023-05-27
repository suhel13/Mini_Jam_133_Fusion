using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageFillSlidet : MonoBehaviour
{
    [SerializeField] Image fillImage;
    public float min = 0;
    public float max = 1;
    public float value;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        value = Mathf.Clamp(value, min, max);
        fillImage.fillAmount = value;
    }
}
