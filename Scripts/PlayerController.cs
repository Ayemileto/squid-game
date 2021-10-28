using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float speed = 10.0f;
    private bool isMoving = false;
    private bool isDead = false;
    
    // private Rigidbody playerRb;
    private Animator playerAnimation;
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        playerAnimation = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.isGameActive && !isDead)
        {
            // if(Input.GetMouseButtonDown(0))
            // {
            //     StartMoving();
            // }
            // else if(Input.GetMouseButtonUp(0))
            // {
            //     StopMoving();
            // }

            if(Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                
                if(touch.phase == TouchPhase.Moved)
                {
                    StartMoving();
                }
                else if(touch.phase == TouchPhase.Ended)
                {
                    StopMoving();
                }
            }

            if(isMoving && !gameManager.isGreenLight)
            {
                FindObjectOfType<AudioManager>().Play("gunshot");
                playerAnimation.SetTrigger("Killed");
                gameManager.GameOver();
            }
        }
    }

    void StartMoving()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        isMoving = true;
        playerAnimation.ResetTrigger("Stop");
        playerAnimation.SetTrigger("Walk");

        if(transform.position.z > gameManager.finishLine)
        {
            playerAnimation.ResetTrigger("Walk");
            playerAnimation.SetTrigger("Stop");
    
            gameManager.isComplete = true;
        }
    }

    void StopMoving()
    {
        transform.Translate(Vector3.zero);
        isMoving = false;
        playerAnimation.ResetTrigger("Walk");
        playerAnimation.SetTrigger("Stop");
    }
}
