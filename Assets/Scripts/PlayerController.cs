using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    Vector3 startingPosition;
    bool gameOver = false;

    public float speed = 10.0f;

    public GameObject winText;
    public GameObject winHole;
    Rigidbody rb;

    void Start()
    {
        startingPosition = gameObject.transform.position;
        winText.SetActive(false);
        rb = gameObject.GetComponent<Rigidbody>(); 
    }

    void Update()
    {
        Vector3 marblePosition = gameObject.transform.position;

#if UNITY_EDITOR
        if (!gameOver)
        {
            if (Input.GetMouseButton(0))
            {
                GetMousePosition();
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                SceneManager.LoadScene("SampleScene");
            }
        }

#endif
#if UNITY_ANDROID 
        if (!gameOver)
        {
            MobileAccelerometer();
        }
        else
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                SceneManager.LoadScene("SampleScene");
            }
        }
#endif
    }

    //dragging the mouse will control the marble
    void GetMousePosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.transform.position.y - transform.position.y;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        Vector3 moveDirection = worldPos - transform.position;
        moveDirection.y = 0;
        moveDirection.Normalize();
        rb.AddForce(moveDirection * speed);
    }

    //tilting the phone will control the marble
    void MobileAccelerometer()
    {
        //if -Input.acceleration, the marble moves in the opposite direction the phone is tilted
        Vector3 dir = Vector3.zero;
        dir.x = Input.acceleration.x;
        dir.z = Input.acceleration.y;
        if (dir.sqrMagnitude > 1)
            dir.Normalize();
        //dir *= Time.deltaTime;
        rb.AddForce(dir * speed);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Hole")
        {
            gameObject.transform.position = startingPosition;
        }
        if (other.gameObject == winHole)
        {
            winText.SetActive(true);
            gameOver = true;
        }

        //to score points
        if (other.gameObject.tag == "Coin")
        {
            Scoring.instance.AddScore();
            Destroy(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == winHole)
        {
            winText.SetActive(false);
        }
    }
}
