using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class buttonsScript : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] Button playBtn;
    [SerializeField] Button staminaBtn;
    [SerializeField] Button incomeBtn;
    [SerializeField] Button speedBtn;
    [SerializeField] Button pauseBtn;
    [SerializeField] Button tryAgainBtn;
    [SerializeField] Button quitBtn;

    [Header("Texts")]
    [SerializeField] TextMeshProUGUI staminaLvTxt;
    [SerializeField] TextMeshProUGUI staminaValueTxt;
    [SerializeField] TextMeshProUGUI incomelvTxt;
    [SerializeField] TextMeshProUGUI incomeValueTxt;
    [SerializeField] TextMeshProUGUI speedLvTxt;
    [SerializeField] TextMeshProUGUI speedValueTxt;

    [Header("Sounds")]
    [SerializeField] AudioSource loopMusic;
    [SerializeField] AudioSource stairSound;

    [Space(25)]
    [SerializeField] GameObject buttonsPanel;
    [SerializeField] GameObject pausePanel;
    [SerializeField] GameObject pauseBtnBg;
    [SerializeField] GameObject musicCheck;
    [SerializeField] GameObject vibrationCheck;

    int staminaLv;
    int incomeLv;
    int speedLv;
    int musicActive, vibrationActive;

    float staminaValue, staminaValueFactor;
    float incomeValue, incomeValueFactor,incomeIncrease;
    float speedValue, speedValueFactor;


    public bool tryAgain;


    void Start()
    {
        PlayerPrefsLoad();
        tryAgain = false;
        pauseBtnBg.SetActive(false);
        pausePanel.SetActive(false);
    }

    
    void Update()
    {
        if (gameManager.instance.isStart)
        {
            buttonsPanel.SetActive(false);
            pauseBtnBg.SetActive(true);
        }

        if (gameManager.instance.isPause)
        {
            Time.timeScale = 0;
            pausePanel.SetActive(true);
        }
        else
        {
            Time.timeScale = 1;
            pausePanel.SetActive(false);
        }
        if (musicActive == 0)
        {
            musicCheck.SetActive(true);
            loopMusic.enabled = true;
            stairSound.enabled = true;
        }
        else
        {
            musicCheck.SetActive(false);
            loopMusic.enabled = false;
            stairSound.enabled = false;
        }
        if (vibrationActive == 0)
        {
            vibrationCheck.SetActive(true);
        }
        else
        {
            vibrationCheck.SetActive(false);
        }

        PlayerPrefsSave();
        UpdateTexts();
        CheckButtons();
        CalculateFactor();
        CalculateIncomeIncrease();
    }

    void PlayerPrefsSave()
    {
        PlayerPrefs.SetInt("StaminaLv", staminaLv);
        PlayerPrefs.SetInt("IncomeLv", incomeLv);
        PlayerPrefs.SetInt("SpeedLv", speedLv);
        PlayerPrefs.SetFloat("StaminaValue", staminaValue);
        PlayerPrefs.SetFloat("IncomeValue", incomeValue);
        PlayerPrefs.SetFloat("SpeedValue", speedValue);
        PlayerPrefs.SetInt("MusicActive", musicActive);
        PlayerPrefs.SetInt("VibrationActive", vibrationActive);
    }

    void PlayerPrefsLoad()
    {
        if (PlayerPrefs.HasKey("StaminaLv"))
            staminaLv = PlayerPrefs.GetInt("StaminaLv");
        else
        {
            staminaLv = 1;
            PlayerPrefs.SetInt("StaminaLv", staminaLv);
        }
        if (PlayerPrefs.HasKey("IncomeLv"))
            incomeLv = PlayerPrefs.GetInt("IncomeLv");
        else
        {
            incomeLv = 1;
            PlayerPrefs.SetInt("IncomeLv", incomeLv);
        }
        if (PlayerPrefs.HasKey("SpeedLv"))
            speedLv = PlayerPrefs.GetInt("SpeedLv");
        else
        {
            speedLv = 1;
            PlayerPrefs.SetInt("SpeedLv", speedLv);
        }
        if (PlayerPrefs.HasKey("StaminaValue"))
            staminaValue = PlayerPrefs.GetFloat("StaminaValue");
        else
        {
            staminaValue = 24;
            PlayerPrefs.SetFloat("StaminaValue", staminaValue);
        }
        if (PlayerPrefs.HasKey("IncomeValue"))
            incomeValue = PlayerPrefs.GetFloat("IncomeValue");
        else
        {
            incomeValue = 24;
            PlayerPrefs.SetFloat("IncomeValue", incomeValue);
        }
        if (PlayerPrefs.HasKey("SpeedValue"))
            speedValue = PlayerPrefs.GetFloat("SpeedValue");
        else
        {
            speedValue = 24;
            PlayerPrefs.SetFloat("SpeedValue", speedValue);
        }
        if (PlayerPrefs.HasKey("MusicActive"))
            musicActive = PlayerPrefs.GetInt("MusicActive");
        else
        {
            musicActive = 0;
            musicActive = PlayerPrefs.GetInt("MusicActive");
        }
        if (PlayerPrefs.HasKey("VibrationActive"))
            vibrationActive = PlayerPrefs.GetInt("VibrationActive");
        else
        {
            vibrationActive = 0;
            vibrationActive = PlayerPrefs.GetInt("VibrationActive");
        }
    }

    void UpdateTexts()
    {
        staminaLvTxt.text = "LVL " + staminaLv;
        incomelvTxt.text = "LVL " + incomeLv;
        speedLvTxt.text = "LVL " + speedLv;

        staminaValueTxt.text = ((int)staminaValue).ToString();
        incomeValueTxt.text = ((int)incomeValue).ToString();
        speedValueTxt.text = ((int)speedValue).ToString();
    }

    void CheckButtons()
    {
        if (staminaValue > gameManager.instance.money)
            staminaBtn.interactable = false;
        else
            staminaBtn.interactable = true;

        if (incomeValue > gameManager.instance.money)
            incomeBtn.interactable = false;
        else
            incomeBtn.interactable = true;

        if (speedValue > gameManager.instance.money)
            speedBtn.interactable = false;
        else
            speedBtn.interactable = true;
    }

    void CalculateFactor()
    {
        if (staminaLv < 6)
            staminaValueFactor = 1.35f;
        else if (staminaLv < 13)
            staminaValueFactor = 1.25f;
        else 
            staminaValueFactor = 1.15f;
        

        if (incomeLv < 6)
            incomeValueFactor = 1.35f;
        else if (incomeLv < 13)
            incomeValueFactor = 1.25f;
        else 
            incomeValueFactor = 1.15f;
        

        if (speedLv < 6)
            speedValueFactor = 1.35f;
        else if (speedLv < 13)
            speedValueFactor = 1.25f;
        else
            speedValueFactor = 1.15f;
 

    }

    void CalculateIncomeIncrease()
    {
        if (incomeLv < 6)
        {
            incomeIncrease = 0.1f;
        }
        else if (incomeLv < 11)
        {
            incomeIncrease = 0.2f;
        }
        else if (incomeLv < 16)
        {
            incomeIncrease = 0.3f;
        }
        else
        {
            incomeIncrease = 0.5f;
        }
    }

    public void PlayBtn()
    {
        gameManager.instance.isStart = true;
    }

    public void StaminaBtn()
    {
        staminaLv++;
        gameManager.instance.money -= staminaValue;
        staminaValue *= staminaValueFactor;
        gameManager.instance.maxStamina += 5;
        gameManager.instance.stamina = gameManager.instance.maxStamina;
    }

   public void IncomeBtn()
    {
        incomeLv++;
        gameManager.instance.money -= incomeValue;
        gameManager.instance.income += incomeIncrease;
        incomeValue *= incomeValueFactor;
    }

    public void SpeedBtn()
    {
        speedLv++;
        gameManager.instance.money -= speedValue;
        gameManager.instance.maxSpeed -= (gameManager.instance.maxSpeed / 10);
        speedValue *= speedValueFactor;
    }

    public void TryAgainBtn()
    {
        for (int i = 1; i < gameManager.instance.stairs.Count; i++)
        {
            gameManager.instance.stairs[i].GetComponent<Rigidbody>().useGravity = true;
            gameManager.instance.stairs[i].GetComponent<BoxCollider>().isTrigger = false;
        }
        tryAgain = true;
        gameManager.instance.gameOverPanel.SetActive(false);
        Invoke("TryAgain", 1f);
    }

    void TryAgain()
    {
        SceneManager.LoadScene(0);
    }

    public void PauseBtn()
    {
        gameManager.instance.isPause = true;
    }

    public void ResumeBtn()
    {
        gameManager.instance.isPause = false;
    }

    public void RestartBtn()
    {
        SceneManager.LoadScene(0);
    }

    public void MusicBtn()
    {
        if (musicActive == 0)
            musicActive = 1;
        else
            musicActive = 0;
    }

    public void VibrationBtn()
    {
        if (vibrationActive == 0)
            vibrationActive = 1;
        else
            vibrationActive = 0;
    }

    public void Quit()
    {
        Application.Quit();
    }
}
