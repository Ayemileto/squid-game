using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static bool isGameActive = false;
    public static bool pause = false;
    public bool isGreenLight = false;
    public bool isPaused = false;
    public bool isComplete = false;

    public float finishLine = 85.0f;
    private float timeLeft = 120;
    public float greenLightTime, redLightTime;
    private int[] greenLightTimeValues = {3, 5, 7};
    private string[] greenLightSounds = {"greenlight_3s", "greenlight_5s", "greenlight_7s"};
    private string greenLightSound;

    public GameObject gameCompletePanel;
    public GameObject gameOverPanel;
    public GameObject startGamePanel;

    public Button redLight;
    public Button greenLight;
    public Button pauseButton;
    public Button playButton;
    public Button restartButton;
    public Button startButton;
    
    public TextMeshProUGUI timeText;

    private AudioManager audioManager;

    // Start is called before the first frame update
    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        RedLight();
        Time.timeScale = 0;
    }

    void GreenLight()
    {
        isGreenLight = true;
        
        greenLightSound = greenLightSounds[Random.Range(0, greenLightSounds.Length)];
        greenLightTime = audioManager.Length(greenLightSound);

        audioManager.Play(greenLightSound);

        redLight.gameObject.SetActive(false);
        greenLight.gameObject.SetActive(true);

        // if(greenLightTime == 3)
        // {
        //     AudioManager.Play("greenlight_3s");
        // }

        // else if(greenLightTime == 5)
        // {
        //     FindObjectOfType<AudioManager>().Play("greenlight_5s");
        // }

        // else if(greenLightTime == 7)
        // {
        //     FindObjectOfType<AudioManager>().Play("greenlight_5s");
        // }
    }

    void RedLight()
    {
        isGreenLight = false;
        redLightTime = Random.Range(2, 5);

        greenLight.gameObject.SetActive(false);
        redLight.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if(isGameActive)
        {
            if(isComplete)
            {
                GameCompleted();
            }

            if(isGreenLight && greenLightTime <= 0)
            {
                RedLight();
            }
            else if(!isGreenLight && redLightTime <= 0)
            {
                GreenLight();
            }

            greenLightTime -= Time.deltaTime;
            redLightTime -= Time.deltaTime;

            if(isGameActive)
            {
                timeLeft -= Time.deltaTime;
                timeText.text = "Time: " + Mathf.Round(timeLeft) + "s";

                if(timeLeft < 0)
                {
                    GameOver();
                }
            }
        }
    }

    public void GameOver()
    {
        isGameActive = false;
        gameOverPanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void GameCompleted()
    {
        isGameActive = false;
        gameCompletePanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void PauseGame()
    {
        pauseButton.gameObject.SetActive(false);
        playButton.gameObject.SetActive(true);

        isPaused = true;
        Time.timeScale = 0;
    }

    public void PlayGame()
    {
        playButton.gameObject.SetActive(false);
        pauseButton.gameObject.SetActive(true);

        isPaused = false;
        Time.timeScale = 1;
    }

    public void StartGame()
    {
        Time.timeScale = 1;
        startGamePanel.SetActive(false);
        isGameActive = true;
    }

    public void RestartGame()
    {
        gameCompletePanel.SetActive(false);
        gameOverPanel.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
