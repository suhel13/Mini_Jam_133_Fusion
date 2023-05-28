using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunScript : MonoBehaviour
{
    public float bounseAngle;
    public float returnSpeed;
    float spawnTimer = 0;
    public SpriteRenderer sunSprite;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        updateSunRadiusAndColor();
    }

    void updateSunRadiusAndColor()
    {
        float scale = Mathf.Pow((GameManager.Instance.sunTemp + 8.375f) / 600f, 1f / 6f);
        Debug.Log(GameManager.Instance.sunTemp + 8.375f);
        Debug.Log((GameManager.Instance.sunTemp + 8.375f) / 600f);
        Debug.Log("Sun scale: " +scale );
        this.transform.localScale = Vector3.one * scale;
        sunSprite.color = new Color(1,( -1 * 270.5882352941176f * scale + 365.29411764705884f)/255 , 0 , 1);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(Mathf.Abs( Vector3.Angle(collision.gameObject.GetComponent<Rigidbody2D>().velocity, collision.transform.position - this.transform.position)) < bounseAngle)
        {
            collision.gameObject.GetComponent<Rigidbody2D>().velocity *= -1;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        Vector2 dir = (this.transform.position - collision.transform.position).normalized;
        if(collision.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude < 1)
            collision.gameObject.GetComponent<Rigidbody2D>().velocity += dir * returnSpeed;
    }
}
