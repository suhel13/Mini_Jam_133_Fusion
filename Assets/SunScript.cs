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
        if (GameManager.Instance.sunTemp < 4200)
        {
            updateSunRadiusAndColor();
        }
        else
        {
            GetComponent<SuperNova>().StartCollapseSequense();
        }
    }

    void updateSunRadiusAndColor()
    {
        float scale = Mathf.Pow((GameManager.Instance.sunTemp + 5.8f) / 3500f, 1f / 9f);
        this.transform.localScale = Vector3.one * scale;
        sunSprite.color = new Color(1, (-1 * 460f * scale + 460f) / 255, 0, 1);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(Mathf.Abs( Vector3.Angle(collision.gameObject.GetComponent<Rigidbody2D>().velocity, collision.transform.position - this.transform.position)) < bounseAngle)
        {
            float angle = Vector2.SignedAngle(collision.gameObject.GetComponent<Rigidbody2D>().velocity, (collision.transform.position - this.transform.position).normalized);
           
            collision.gameObject.GetComponent<Rigidbody2D>().velocity = Quaternion.AngleAxis((angle * 2) - 180, Vector3.forward) * collision.gameObject.GetComponent<Rigidbody2D>().velocity;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        Vector2 dir = (this.transform.position - collision.transform.position).normalized;
        if(collision.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude < 1)
            collision.gameObject.GetComponent<Rigidbody2D>().velocity += dir * returnSpeed;
    }
}
