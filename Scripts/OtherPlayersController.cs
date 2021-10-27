using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherPlayersController : MonoBehaviour
{
    private float lowSpeed = 3.0f;
    private float topSpeed = 15.0f;

    private Rigidbody otherPlayersRb;
    private Animator playerAnimation;
    private bool isMoving = false;
    private bool isDead = false;
    private bool isStopped = false;
    private bool isFinished = false;

    private float removeDeadPlayer = 7.00f; // HOW MANY SECONDS TO REMOVE DEAD PLAYER

    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        otherPlayersRb = GetComponent<Rigidbody>();
        playerAnimation = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.isGameActive)
        {
            if(transform.position.z > gameManager.finishLine)
            {
                isFinished = true;
                StopMoving(true);   
            }
            
            if(!isMoving && !isDead && !isFinished && gameManager.isGreenLight && gameManager.greenLightTime > 0.5)
            {
                StartMoving();
            }

            if(isMoving && gameManager.isGreenLight && gameManager.greenLightTime < 0.5 && !isStopped)
            { // WE WANT TO START RANDOMLY STOPPING MOVING PLAYERS 0.5s BEFORE RED LIGHT
                StopMoving();
            }

            if(!gameManager.isGreenLight && isMoving)
            {
                Kill();
            }

            if(isDead)
            {
                removeDeadPlayer -= Time.deltaTime;

                if(removeDeadPlayer <= 0)
                {
                    Destroy(gameObject);
                }
            }
        }
    }

    void StartMoving()
    {
        isStopped = false;
        isMoving = true;

        if(Random.Range(0, 10) < 7)
        {
            ResetAllTriggers("Walk");
            otherPlayersRb.velocity = new Vector3(otherPlayersRb.velocity.x, otherPlayersRb.velocity.y, Random.Range(lowSpeed, topSpeed));
        }
    }

    void StopMoving(bool forceStop = false)
    {
        isStopped = true;

        float stopVelocity = Random.Range(lowSpeed, topSpeed);
    
        if((stopVelocity > otherPlayersRb.velocity.z) || forceStop)
        {
            otherPlayersRb.velocity = new Vector3(0, 0, 0);
            isMoving = false;
            
            ResetAllTriggers("Stop");
        }
    }

    void Kill()
    {
        otherPlayersRb.velocity = new Vector3(0, 0, 0);

        FindObjectOfType<AudioManager>().Play("gunshot");
        ResetAllTriggers("Killed");
        isDead = true;
    }

    void ResetAllTriggers(string newTrigger)
    {
        playerAnimation.ResetTrigger("Stop");
        playerAnimation.ResetTrigger("Walk");
        playerAnimation.ResetTrigger("Killed");
        playerAnimation.SetTrigger(newTrigger);
    }
}
