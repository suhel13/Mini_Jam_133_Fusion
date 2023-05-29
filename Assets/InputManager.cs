using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{


    Vector3 mousePos;
    Vector3 mousePosWorld;
    Camera cam;
    public GameObject tempProton;




    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void getMausePos(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            mousePos = ctx.ReadValue<Vector2>();
            mousePosWorld = cam.ScreenToWorldPoint(mousePos);
            mousePosWorld.z = 0;
        }
    }

    public void click(InputAction.CallbackContext ctx)
    {
        if (Time.timeScale > 0)
        {
            if (ctx.performed)
            {
                Debug.Log("L click performed");
                tempProton = Instantiate(GameManager.Instance.protonPrefab, mousePosWorld, Quaternion.identity);
            }

            if (ctx.canceled)
            {
                Debug.Log("L click canceled");
                tempProton.GetComponent<Rigidbody2D>().velocity = (mousePosWorld - tempProton.transform.position).normalized * GameManager.Instance.protonSpeed;
            }
        }
    }
}
