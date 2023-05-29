using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject nextButton;
    public GameObject prevButton;
    public void closePopUp()
    {
        Time.timeScale = 1f;
        this.gameObject.SetActive(false);
    }

    public void show(bool showNavigatoion)
    {
        Time.timeScale = 0f;
        this.gameObject.SetActive(true);
        if(showNavigatoion)
        {
            prevButton?.SetActive(true);
            nextButton?.SetActive(true);
        }
        else
        {
            prevButton?.SetActive(false);
            nextButton?.SetActive(false);
        }
    }

    public void next()
    {
        GameManager.Instance.popUpManager.next();
    }
    public void prev()
    {
        GameManager.Instance.popUpManager.prev();
    }
}
