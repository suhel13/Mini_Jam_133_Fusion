using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpManager : MonoBehaviour
{
    int popUpCounter = 0;
    public int knownPopUp = 0;
    public List<PopUpController> tutorialPopUps;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void next()
    {
        if (popUpCounter + 1 < tutorialPopUps.Count && popUpCounter + 1 < knownPopUp)
        {
            tutorialPopUps[popUpCounter].closePopUp();

            popUpCounter++;
            tutorialPopUps[popUpCounter].show(true);
        }
    }
    public void prev()
    {
        if(popUpCounter > 0)
        {
            tutorialPopUps[popUpCounter].closePopUp();

            popUpCounter--;
            tutorialPopUps[popUpCounter].show(true);
        }
    }
    public void lernNextPopUp()
    {
        knownPopUp++;
        tutorialPopUps[knownPopUp - 1].show(true);
        popUpCounter = knownPopUp - 1;
    }

    public void showHelp()
    {
        tutorialPopUps[popUpCounter].show(true);
    }

}
