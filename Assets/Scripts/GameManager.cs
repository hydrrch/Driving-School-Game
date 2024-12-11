using System;
using UnityEngine.UI;
using UnityEngine;
using System.Threading.Tasks;
using DG.Tweening;


public class GameManager : MonoBehaviour
{
    #region Career
    [Serializable]
    class LevelBehaviourObjects
    {
        public float fillerAmount;
        public GameObject bar;
        public Image fillableBar;
        public Transform positionRef;
        public GameObject levelObj;
        public GameObject instructionBox;
        public Text instructionsText;
        public string instructions;
        public GameObject gameplay;
        public Image barIconImage;
        public Sprite barIcon;
        public bool showInstructions;
        public bool showHealthBar;
    }

    [Header("Career Parking")]
    [SerializeField]
    LevelBehaviourObjects[] levelBehaviours;
    #endregion


    #region Advanced
    [Serializable]
    class AdvancedModeBehaviour
    {
        public GameObject parkingEnv;
        public GameObject level;
        public Transform carPosRef;
        public GameObject gameplayControls;
    }

    [Header("Advanced Parking")]
    [SerializeField]
    AdvancedModeBehaviour[] advancedModeBehaviours;

    #endregion


    #region One Touch
    [Serializable]
    class OTModeBehaviour
    {
        public GameObject parkingEnv;
        public GameObject level;
        public Transform carPosRef;
        public GameObject gameplayControls;

    }

    [Header("Advanced Parking")]
    [SerializeField]
    OTModeBehaviour[] otModeBehaviours;

    #endregion


    [Space(20)]
    public GameObject[] gears;
    public Sprite[] gearsSprites;
    public Transform[] cars;
    public Image gearBtn;
    public RCC_Camera rCC_Camera;
    public Scrollbar gear;
    public Image steering;
    public AudioSource musicSource;
    public GameObject ot_Tut_Image;
    [SerializeField] AudioSource WellDoneSfx;

    public bool isTesting;
    public int testingLevelNum;
    public int testingCarNum;
    public string testingModeName;
    public bool enableSteering;

    GameObject chain;
    public bool isTutorial;

    public static GameManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    void Start()
    {
        if (MainMenuManager.modeLoaded == "Career" || testingModeName == "Career")
        {
            SetCareerScene();
        }
        else if (MainMenuManager.modeLoaded == "Advanced" || testingModeName == "Advanced")
        {
            SetAdvanceScene();
        }
        else
        {
            SetOneTouchScene();
        }

        if (PlayerPrefs.GetInt("Music") == 0)
            musicSource.mute = false;
        else
            musicSource.mute = true;

    }

    void SetCareerScene()
    {
        if (isTesting)
        {
            levelBehaviours[testingLevelNum].levelObj.SetActive(true);
            cars[testingCarNum].gameObject.SetActive(true);
            cars[testingCarNum].position = levelBehaviours[testingLevelNum].positionRef.position;
            cars[testingCarNum].rotation = levelBehaviours[testingLevelNum].positionRef.rotation;
            rCC_Camera.playerCar = cars[testingCarNum];

            if (levelBehaviours[testingLevelNum].showInstructions)
            {
                ShowInstructions();
            }
        }
        else
        {
            levelBehaviours[MainMenuManager.currentLevelNum].levelObj.SetActive(true);
            cars[MainMenuManager.currentCarNum].gameObject.SetActive(true);
            cars[MainMenuManager.currentCarNum].position = levelBehaviours[MainMenuManager.currentLevelNum].positionRef.position;
            cars[MainMenuManager.currentCarNum].rotation = levelBehaviours[MainMenuManager.currentLevelNum].positionRef.rotation;
            rCC_Camera.playerCar = cars[MainMenuManager.currentCarNum];

            if (levelBehaviours[MainMenuManager.currentLevelNum].showInstructions)
            {
                ShowInstructions();
            }

            if (MainMenuManager.currentLevelNum != 4)
            {
                chain = GameObject.FindWithTag("ChainedCarParent");
                chain.SetActive(false);
            }

        }
    }

    void SetAdvanceScene()
    {
        if (isTesting)
        {
            advancedModeBehaviours[testingLevelNum].parkingEnv.SetActive(true);
            advancedModeBehaviours[testingLevelNum].level.SetActive(true);
            cars[testingCarNum].gameObject.SetActive(true);
            cars[testingCarNum].position = advancedModeBehaviours[testingLevelNum].carPosRef.position;
            cars[testingCarNum].rotation = advancedModeBehaviours[testingLevelNum].carPosRef.rotation;
            rCC_Camera.playerCar = cars[testingCarNum];
            //advancedModeBehaviours[testingLevelNum].gameplayControls.SetActive(true);
            EnableControls();
        }
        else
        {
            advancedModeBehaviours[MainMenuManager.currentLevelNum].parkingEnv.SetActive(true);
            advancedModeBehaviours[MainMenuManager.currentLevelNum].level.SetActive(true);
            cars[MainMenuManager.currentCarNum].gameObject.SetActive(true);
            cars[MainMenuManager.currentCarNum].position = advancedModeBehaviours[MainMenuManager.currentLevelNum].carPosRef.position;
            cars[MainMenuManager.currentCarNum].rotation = advancedModeBehaviours[MainMenuManager.currentLevelNum].carPosRef.rotation;
            rCC_Camera.playerCar = cars[MainMenuManager.currentCarNum];
            //advancedModeBehaviours[MenuManager.currentCarNum].gameplayControls.SetActive(true);
            EnableControls();

        }
    }

    void SetOneTouchScene()
    {
        if (isTesting)
        {
            otModeBehaviours[testingLevelNum].parkingEnv.SetActive(true);
            otModeBehaviours[testingLevelNum].level.SetActive(true);
            cars[testingCarNum].gameObject.SetActive(true);
            cars[testingCarNum].position = otModeBehaviours[testingLevelNum].carPosRef.position;
            cars[testingCarNum].rotation = otModeBehaviours[testingLevelNum].carPosRef.rotation;
            rCC_Camera.playerCar = cars[testingCarNum];
            //advancedModeBehaviours[testingLevelNum].gameplayControls.SetActive(true);
            EnableControls();
        }
        else
        {
            otModeBehaviours[MainMenuManager.currentLevelNum].parkingEnv.SetActive(true);
            otModeBehaviours[MainMenuManager.currentLevelNum].level.SetActive(true);
            cars[MainMenuManager.currentCarNum].gameObject.SetActive(true);
            cars[MainMenuManager.currentCarNum].position = otModeBehaviours[MainMenuManager.currentLevelNum].carPosRef.position;
            cars[MainMenuManager.currentCarNum].rotation = otModeBehaviours[MainMenuManager.currentLevelNum].carPosRef.rotation;
            rCC_Camera.playerCar = cars[MainMenuManager.currentCarNum];
            //otModeBehaviours[MenuManager.currentCarNum].gameplayControls.SetActive(true);
            EnableControls();
            if (PlayerPrefs.GetInt("ShownTut") == 0)
            {
                PlayerPrefs.SetInt("ShownTut", 1);
                ShowOTTutorial();
            }

        }
    }

    async void ShowOTTutorial()
    {
        var t = Time.time + 2;

        while (t > Time.time)
            await Task.Yield();

        if (ot_Tut_Image)
            ot_Tut_Image.SetActive(true);

        var t2 = Time.time + 4;

        while (t2 > Time.time)
        {

            await Task.Yield();
        }
        if (ot_Tut_Image)
            ot_Tut_Image.SetActive(false);
    }

    async void EnableControls()
    {
        steering.enabled = false;
        var t = Time.time + 1;

        while (t > Time.time)
        {
            await Task.Yield();
        }


        if (MainMenuManager.modeLoaded == "Advanced" || testingModeName == "Advanced")
        {
            if (isTesting)
                advancedModeBehaviours[testingLevelNum].gameplayControls.SetActive(true);
            else
                advancedModeBehaviours[MainMenuManager.currentCarNum].gameplayControls.SetActive(true);
        }
        else
        {
            if (isTesting)
                otModeBehaviours[testingLevelNum].gameplayControls.SetActive(true);
            else
                otModeBehaviours[MainMenuManager.currentCarNum].gameplayControls.SetActive(true);
        }
        steering.enabled = true;
    }



    async void ShowInstructions()
    {
        //print("steering false");
        steering.enabled = false;

        if (isTesting)
        {
            var t = Time.time + 1;
            while (t > Time.time)
            {
                await Task.Yield();
            }

            levelBehaviours[testingLevelNum].instructionBox.SetActive(true);
            levelBehaviours[testingLevelNum].instructionsText.text = levelBehaviours[testingLevelNum].instructions;
            //levelBehaviours[testingLevelNum].instructionBox.GetComponent<DOTweenAnimation>().DOPlay();
            var t2 = Time.time + 5;
            while (t2 > Time.time)
            {
                await Task.Yield();
            }

            levelBehaviours[testingLevelNum].instructionBox.SetActive(false);
            levelBehaviours[testingLevelNum].gameplay.SetActive(true);
            if (levelBehaviours[testingLevelNum].showHealthBar)
            {
                levelBehaviours[testingLevelNum].bar.SetActive(true);
                levelBehaviours[testingLevelNum].barIconImage.sprite = levelBehaviours[testingLevelNum].barIcon;
            }
        }
        else
        {
            var t = Time.time + 1;
            while (t > Time.time)
            {
                await Task.Yield();
            }

            levelBehaviours[MainMenuManager.currentLevelNum].instructionBox.SetActive(true);
            levelBehaviours[MainMenuManager.currentLevelNum].instructionsText.text = levelBehaviours[MainMenuManager.currentLevelNum].instructions;
            //levelBehaviours[testingLevelNum].instructionBox.GetComponent<DOTweenAnimation>().DOPlay();
            var t2 = Time.time + 5;
            while (t2 > Time.time)
            {
                await Task.Yield();
            }

            levelBehaviours[MainMenuManager.currentLevelNum].instructionBox.SetActive(false);
            levelBehaviours[MainMenuManager.currentLevelNum].gameplay.SetActive(true);
            if (levelBehaviours[MainMenuManager.currentLevelNum].showHealthBar)
            {
                levelBehaviours[MainMenuManager.currentLevelNum].bar.SetActive(true);
                levelBehaviours[MainMenuManager.currentLevelNum].barIconImage.sprite = levelBehaviours[MainMenuManager.currentLevelNum].barIcon;
            }
        }

        steering.enabled = true;
        //print("steering true");
    }

    public void ChangeGear(Scrollbar gear)
    {
        if (!isTutorial)
        {
            if (gear.value < 0.5f)
            {
                gear.value = 1f;
                gearBtn.sprite = gearsSprites[1];
            }
            else
            {
                gear.value = 0f;
                gearBtn.sprite = gearsSprites[0];
            }

            for (int i = 0; i < gears.Length; i++)
            {
                if (gears[i].activeInHierarchy)
                    gears[i].SetActive(false);
                else
                    gears[i].SetActive(true);
            }
        }

    }

    public async virtual void FillTheBar()
    {
        var t = Time.time + 2;
        float lerpingTime = 0;

        float currentFilledAmount = levelBehaviours[testingLevelNum].fillableBar.fillAmount;
        float desiredFilledAmount = levelBehaviours[testingLevelNum].fillableBar.fillAmount + levelBehaviours[testingLevelNum].fillerAmount;
        levelBehaviours[testingLevelNum].bar.GetComponent<DOTweenAnimation>().DORestart();

        while (t > Time.time)
        {
            lerpingTime += Time.deltaTime / 2;

            levelBehaviours[testingLevelNum].fillableBar.fillAmount = Mathf.Lerp(currentFilledAmount, desiredFilledAmount, lerpingTime);

            await Task.Yield();
        }

    }

    public void FillTheBar2()
    {
        levelBehaviours[MainMenuManager.currentLevelNum].fillableBar.fillAmount =
            levelBehaviours[MainMenuManager.currentLevelNum].fillableBar.fillAmount + levelBehaviours[MainMenuManager.currentLevelNum].fillerAmount;
        levelBehaviours[MainMenuManager.currentLevelNum].bar.GetComponent<DOTweenAnimation>().DORestart();
    }

    public void SteerRace(RCC_UIController gas)
    {
        gas.pressing = true;
    }

    public void SteerBrake(RCC_UIController gas)
    {
        gas.pressing = false;
        cars[MainMenuManager.currentCarNum].GetComponent<Rigidbody>().velocity = Vector3.zero;
    }



    //public async void DisplayHealthBar()
    //{
    //    var t = Time.time + 2;
    //    float lerpingTime = 0;
    //    Vector3 startingPos = levelBehaviours[0].bar.transform.position;

    //    while (t > Time.time)
    //    {
    //        lerpingTime += Time.deltaTime / 2;

    //        levelBehaviours[0].bar.transform.position = Vector3.Lerp(startingPos, levelBehaviours[0].endingPos, lerpingTime);
    //        await Task.Yield();
    //    }
    //}


    public void PlayWellDoneSfx() => WellDoneSfx.Play();


    #region Temp Coding
    public void ChangeLevel()
    {
        testingLevelNum++;

        if (testingLevelNum == 10)
            testingLevelNum = 0;

        Application.LoadLevel(Application.loadedLevel);
    }
    #endregion

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Space))
    //        FillTheBar();
    //    //DisplayHealthBar();
    //}

    //moving one point to another in specified time
    //    if (Input.GetKey(KeyCode.End))
    //        {
    //            startingPos = player.position;
    //        }

    //if (Input.GetKey(KeyCode.Space) && endedTime < startedTime + totalTime)
    ////if (Input.GetKeyDown(KeyCode.Space))
    //{
    //    if (startedTime == 0)
    //        startedTime = Time.time;

    //    lerpingTime += Time.deltaTime / totalTime;

    //    player.position = Vector3.Lerp(startingPos, target.position, lerpingTime);
    //    endedTime = Time.time;

    //}
}
