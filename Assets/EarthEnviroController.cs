using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class EarthEnviroController : MonoBehaviour
{
    public SpriteRenderer sea;
    public SpriteRenderer grass;
    public SpriteRenderer grassDessert;
    public SpriteRenderer snow;
    public SpriteRenderer dessert;
    public SpriteRenderer mountain;
    public SpriteRenderer snow2;
    public SpriteRenderer mountainsTops;
    public SpriteRenderer dessertMountains;
    public SpriteRenderer snow3;
    public SpriteRenderer ice;
    public SpriteRenderer iceCracked;
    public SpriteRenderer floes;
    public SpriteRenderer burnMarks;
    public SpriteRenderer oceanBottomDry;
    public SpriteRenderer oceanBottomSemiDry;
    public SpriteRenderer trees;
    public SpriteRenderer treesHalfDead;
    public SpriteRenderer treesAllDead;

    [Range(0,10)] public float Temp;

    public float rotateSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {   
        
        updateEnviroment();
        
        if (Application.IsPlaying(gameObject))
        {
            transform.eulerAngles += Vector3.forward * rotateSpeed * Time.deltaTime;
            Temp = Mathf.Pow(GameManager.Instance.sunTemp - 1f, 1/3.714011909f);
        }
    }

    void transparetAll()
    {
        sea.color = new Color(1, 1, 1, 1);
        grass.color = new Color(1, 1, 1, 1);
        grassDessert.color = new Color(1, 1, 1, 0);
        snow.color = new Color(1, 1, 1, 0);
        dessert.color = new Color(1, 1, 1, 0);
        mountain.color = new Color(1, 1, 1, 0);
        snow2.color = new Color(1, 1, 1, 0);
        mountainsTops.color = new Color(1, 1, 1, 0);
        dessertMountains.color = new Color(1, 1, 1, 0);
        snow3.color = new Color(1, 1, 1, 1);
        ice.color = new Color(1, 1, 1, 1);
        iceCracked.color = new Color(1, 1, 1, 0);
        floes.color = new Color(1, 1, 1, 0);
        burnMarks.color = new Color(1, 1, 1, 0);
        oceanBottomDry.color = new Color(1, 1, 1, 1);
        oceanBottomSemiDry.color = new Color(1, 1, 1, 0);

        trees.color = new Color(1, 1, 1, 0);
        treesHalfDead.color = new Color(1, 1, 1, 0);
        treesAllDead.color = new Color(1, 1, 1, 0);
    }

    void updateEnviroment()
    {
        if(Temp == 0)
            transparetAll();
        else if (Temp <= 1)
        {
            //mountain tops
            snow3.color = new Color(1, 1, 1, 1 - Temp);
            mountainsTops.color = new Color(1, 1, 1, Temp);

            snow2.color = new Color(1, 1, 1, 1);
            ice.color = new Color(1, 1, 1, 1);
        }
        else if (Temp <= 2)
        {
            // mountain tops and craced ice
            snow2.color = new Color(1, 1, 1, 2 - Temp);
            mountain.color = new Color(1, 1, 1, Temp - 1);
            ice.color = new Color(1, 1, 1, 2 - Temp);

            mountainsTops.color = new Color(1, 1, 1, 1);
            snow.color = new Color(1, 1, 1, 1);
            iceCracked.color = new Color(1, 1, 1, 1);
        }
        else if (Temp <= 3)
        {
            // grass and floes
            iceCracked.color = new Color(1, 1, 1, 3 - Temp);
            snow.color = new Color(1, 1, 1, 3 - Temp);
            floes.color = new Color(1, 1, 1, 1);
        }
        else if (Temp <= 4)
        {
            floes.color = new Color(1, 1, 1, 4 - Temp);
        }
        else if(Temp <= 5)
        {
            trees.color = new Color(1, 1, 1, Temp - 4);
        }
        else if(Temp <= 6)
        {
            trees.color = new Color(1, 1, 1, 6 - Temp);
            treesHalfDead.color = new Color(1, 1, 1, Temp - 5);
            grassDessert.color = new Color(1, 1, 1, Temp - 5);
        }
        else if(Temp <= 7)
        {
            treesHalfDead.color = new Color(1, 1, 1, 7 - Temp);
            treesAllDead.color = new Color(1, 1, 1, Temp - 6);
            dessert.color = new Color(1, 1, 1, Temp - 6);
        }
        else if(Temp <= 8)
        {
            oceanBottomSemiDry.color = new Color(1, 1, 1, Temp - 7);
        }
        else if (Temp <=9)
        {
            dessertMountains.color = new Color(1, 1, 1, Temp - 8);
            sea.color = new Color(1, 1, 1, 9 - Temp);
        }
        else if (Temp <=10)
        {
            burnMarks.color = new Color(1, 1, 1, Temp - 9);
        }
    }
}
