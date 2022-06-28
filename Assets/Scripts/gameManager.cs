using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class gameManager : MonoBehaviour
{
    public static gameManager instance;

    [Header("Attributes")]
    public float stamina;
    public float maxStamina;
    public float income;
    public float speed;
    public float maxSpeed;
    public float money;

    [Header("UI Objects")]
    public GameObject gameOverPanel;
    [SerializeField] GameObject levelUpPanel;
    [SerializeField] GameObject runTimePanel;
    [SerializeField] TextMeshProUGUI scoreTxt;
    [SerializeField] TextMeshProUGUI previousScoreTxt;
    [SerializeField] TextMeshProUGUI moneyTxt;
    [SerializeField] TextMeshProUGUI levelTxt;
    [SerializeField] Image AdvanceImage;
    [SerializeField] Image moneyImage;

    [Header("Parents")]
    [SerializeField] GameObject stairParent;
    [SerializeField] GameObject woodParent;
    [SerializeField] GameObject previousWoodParent;

    [Header("Prefabs")]
    [SerializeField] GameObject woodPref;
    [SerializeField] GameObject stairPref;
    [SerializeField] GameObject moneyTxtObj;

    [Header("Hierarchy Objects")]
    public GameObject previousScoreObj;
    [SerializeField] GameObject scoreObj;
    [SerializeField] GameObject playerMesh;

    [Header("Bools")]
    public bool isClimb;
    public bool isFinish;
    public bool isStart;
    public bool isPause;
    public bool isComplete;
    bool levelUp;

    [Header("Lists")]
    public List<GameObject> stairs = new List<GameObject>();
    [SerializeField] List<GameObject> woods = new List<GameObject>();

    [Space(25)]
    [SerializeField] Material playerMat;
    [SerializeField] float angle;
    [SerializeField] ParticleSystem bloodExplosion;
    [SerializeField] AudioSource stackAudio;
    [SerializeField] AudioClip stackSound;

    float score, maxScore, previousWoods, previousScore, advanceScale;
    int level;
    Vector3 woodOffset, stairOffset, woodScaleFirst, woodScaleLast;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        PlayerPrefsLoad();

        speed = maxSpeed; stamina = maxStamina; score = maxScore;
        isStart = false; isFinish = false; isClimb = false; isPause = false; isComplete = false; levelUp = true;

        if (previousScore == score)
            previousScoreObj.SetActive(false);
        else
        {
            StackPreviousWoods();
            previousScoreObj.SetActive(true);
        }

        playerMat.color = Color.white;

        woodScaleFirst = new Vector3(0.1f, 0.1f, 0.1f);
        woodScaleLast = new Vector3(1.5f, 0.6f, 1.5f);

        stairOffset = new Vector3(0f, 0.06f, 0f);
        woodOffset = new Vector3(0f, 0.06f, 0f);

        scoreTxt.text = System.String.Format("{0:0.0}", score) + "m";
        moneyTxt.text = ((int)money).ToString();
        levelTxt.text = "Level " + level;

        gameOverPanel.SetActive(false);
        levelUpPanel.SetActive(false);
        runTimePanel.SetActive(true);

        AdvanceImage.rectTransform.localScale = new Vector3(advanceScale / 1000, 1, 1);     
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if(!isFinish && isStart)
                isClimb = true;
        }

        if (Input.GetMouseButtonUp(0))
            isClimb = false;

        if (isClimb && !isComplete)
            ClimbTheStair();

        if (stamina <= 5)
            ChangePlayerColor();

        if (stamina <= 0)
        {
            isFinish = true;
            gameOverPanel.SetActive(true);
        }

        if (isFinish)
        {
            isClimb = false;
            runTimePanel.SetActive(false);
            playerMesh.SetActive(false);
        }
        if (stairs.Count==251)
        {
            isComplete = true;
            PlayerPrefs.SetFloat("MaxScore", score);
        }
        if (isComplete)
        {
            Invoke("LevelUp", 1f);
        }

        moneyTxt.text = ((int)money).ToString();
        PlayerPrefsSave();
    }

    void StackStair()
    {
        GameObject stair = Instantiate(stairPref, (stairs[stairs.Count - 1].transform.forward * (-1)) +
            (new Vector3(stairOffset.x, stairOffset.y + 0.25f, stairOffset.z)), Quaternion.Euler(0, stairs.Count * angle, 0));
        stair.transform.DOMove((stairs[stairs.Count - 1].transform.forward * (-1)) + stairOffset, 0.12f);
        stackAudio.PlayOneShot(stackSound);
        Instantiate(moneyTxtObj, transform.position, Quaternion.identity);
        stairOffset.y += 0.08f;
        stairs.Add(stair);
        stair.transform.parent = stairParent.transform;
        money += income;
        moneyImage.transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.1f).OnComplete(() =>
        moneyImage.transform.DOScale(new Vector3(1, 1, 1), 0.1f));
    }

    void StackWood()

    {
        GameObject wood = Instantiate(woodPref, woodOffset, Quaternion.identity);
        wood.transform.localScale = woodScaleFirst;
        wood.transform.DOScale(woodScaleLast, 0.15f);
        woodOffset.y += 0.08f;
        woods.Add(wood);
        wood.transform.parent = woodParent.transform;
        previousWoods++;
        scoreObj.transform.position = new Vector3(
            wood.transform.position.x, wood.transform.position.y + 0.25f, wood.transform.position.z);
    }

    void StackPreviousWoods()
    {
        for (int i = 0; i < previousWoods; i++)
        {
            GameObject wood = Instantiate(woodPref, new Vector3(woods[0].transform.position.x + 1.4f, woodOffset.y, 0.5f), Quaternion.identity);
            woodOffset.y += 0.08f;
            wood.transform.parent = previousWoodParent.transform;
            previousScoreObj.transform.position = new Vector3(
            wood.transform.position.x, wood.transform.position.y + 0.25f, wood.transform.position.z);
        }
        previousWoods = 0;
        woodOffset.y = 0.06f;
    }

    void ClimbTheStair()
    {
        stamina -= Time.deltaTime;
        speed += Time.deltaTime;
        if (speed >= maxSpeed)
        {
            StackStair();
            StackWood();
            UpdateScore();
            speed = 0f;
        }
    }

    void UpdateScore()
    {
        score += 0.4f;
        advanceScale += 0.4f;
        scoreTxt.text = System.String.Format("{0:0.0}", score) + "m";
        AdvanceImage.rectTransform.localScale = new Vector3(advanceScale / 1000, 1, 1);
    }

    void ChangePlayerColor()
    {
        if (isClimb)
            playerMat.color = Color.Lerp(playerMat.color, Color.red, 0.45f * Time.deltaTime);
        else
        {
            if (!isFinish)
                playerMat.color = Color.Lerp(playerMat.color, Color.white, 0.15f * Time.deltaTime);
            stamina += Time.deltaTime;
            if (stamina >= 5)
            {
                stamina = 5;
            }
        }
    }

    void LevelUp()
    {
        if (levelUp)
        {
            bloodExplosion.Play();
            level++;
            PlayerPrefs.SetInt("Level", level);
            runTimePanel.SetActive(false);
            levelUpPanel.SetActive(true);
            maxStamina = 15;
            maxSpeed = 0.3f;
            previousWoods = 0;
            PlayerPrefs.SetFloat("AdvanceScale", advanceScale);
            levelUp = false;
        }
    }

    void PlayerPrefsSave()
    {
        if (isStart)
        {
            PlayerPrefs.SetFloat("PreviousWoods", previousWoods);
            PlayerPrefs.SetFloat("PreviousScore", score);
        }
        PlayerPrefs.SetFloat("Money", money);
        PlayerPrefs.SetFloat("MaxStamina", maxStamina);
        PlayerPrefs.SetFloat("Income", income);
        PlayerPrefs.SetFloat("MaxSpeed", maxSpeed);
    }

    void PlayerPrefsLoad()
    {
        if (PlayerPrefs.HasKey("PreviousWoods"))
        {
            previousWoods = PlayerPrefs.GetFloat("PreviousWoods");
            previousScoreObj.SetActive(true);
        }
        else
        {
            previousWoods = 0;
            PlayerPrefs.SetFloat("PreviousWoods", previousWoods);
            previousScoreObj.SetActive(false);
        }
        if (PlayerPrefs.HasKey("PreviousScore"))
        {
            previousScore = PlayerPrefs.GetFloat("PreviousScore");
            previousScoreTxt.text = System.String.Format("{0:0.0}", previousScore) + "m";
        }
        else
        {
            previousScore = -500;
            PlayerPrefs.SetFloat("PreviousScore", score);
            previousScoreTxt.text = System.String.Format("{0:0.0}", previousScore) + "m";
        }
        if (PlayerPrefs.HasKey("Money"))
        {
            money = PlayerPrefs.GetFloat("Money");
        }
        else
        {
            money = 0;
            PlayerPrefs.SetFloat("Money", money);
            moneyTxt.text = ((int)money).ToString();
        }

        if (PlayerPrefs.HasKey("MaxStamina"))
        {
            maxStamina = PlayerPrefs.GetFloat("MaxStamina");
        }
        else
        {
            maxStamina = 15;
            PlayerPrefs.SetFloat("MaxStamina", maxStamina);
        }
        if (PlayerPrefs.HasKey("Income"))
        {
            income = PlayerPrefs.GetFloat("Income");
        }
        else
        {
            income = 0.5f;
            PlayerPrefs.SetFloat("Income", income);
        }
        if (PlayerPrefs.HasKey("MaxSpeed"))
        {
            maxSpeed = PlayerPrefs.GetFloat("MaxSpeed");
        }
        else
        {
            maxSpeed = 0.3f;
            PlayerPrefs.SetFloat("MaxSpeed", maxSpeed);
        }
        if (PlayerPrefs.HasKey("MaxScore"))
        {
            maxScore = PlayerPrefs.GetFloat("MaxScore");
        }
        else
        {
            maxScore = -500;
            PlayerPrefs.SetFloat("MaxScore", maxScore);
        }

        if (PlayerPrefs.HasKey("AdvanceScale"))
        {
            advanceScale = PlayerPrefs.GetFloat("AdvanceScale");
        }
        else
        {
            advanceScale = 0;
            PlayerPrefs.SetFloat("AdvanceScale", advanceScale);
        }

        if (PlayerPrefs.HasKey("Level"))
        {
            level = PlayerPrefs.GetInt("Level");
        }
        else
        {
            level = 1;
            PlayerPrefs.SetInt("Level", level);
        }
    }

}
