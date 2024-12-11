using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FreeModeGM : MonoBehaviour
{


    [SerializeField] GameObject LevelStartCountPanel;
    [SerializeField] GameObject[] LevelStartCounts;
    [SerializeField] GameObject ClearPanel;
    [SerializeField] GameObject GoodJobPanel;
    [SerializeField] DOTweenAnimation GoodJobPanelAnim;
    [SerializeField] DOTweenAnimation ClearPanelAnim;
    [SerializeField] Text DescriptionText;
    [SerializeField] GameObject[] Missions;
    [SerializeField] GameObject WelcomeNote;
    [SerializeField] AudioSource BackgroundMusic;

    [Header("Tutorial")]
    [SerializeField] GameObject DragTutorial;
    [SerializeField] GameObject JoystickTutorial;
    [SerializeField] GameObject JoystickBlocker;
    [SerializeField] Transform TutorialDest;
    [SerializeField] Transform Player;



    [Space(10)]

    public FreeModeLM FreeModeLevelManagerObj;
    public static int rewardValue;

    public CarResetCheckPoint CarResetCheckPointInstance;
    public static FreeModeGM instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    private void Start()
    {
        PlayerCamera.IsLerp = false;

        if (PlayerPrefs.GetInt("TutorialDescribed") == 0)
        {
            StartCoroutine(TutorialRoutine());
        }
        StartCoroutine(WelcomeNoteRoutine());
    }

    IEnumerator WelcomeNoteRoutine()
    {
        yield return new WaitForSeconds(1f);
        WelcomeNote.SetActive(true);
        WelcomeNote.GetComponent<Text>().text = string.Empty;
        WelcomeNote.GetComponent<Text>().text = "Go ! Dany D is anxiously waiting to drive ";
        BackgroundMusic.enabled = true;
        //yield return new WaitForSeconds(3f);
        //WelcomeNote.GetComponent<Text>().text = string.Empty;
        //WelcomeNote.SetActive(false);
        yield return null;
    }

    IEnumerator TutorialRoutine()
    {

        yield return new WaitForSeconds(1f);
        DragTutorial.SetActive(true);
        JoystickBlocker.SetActive(true);

        yield return new WaitUntil(() => !DragTutorial.activeInHierarchy);
        yield return new WaitForSeconds(2f);
        JoystickBlocker.SetActive(false);
        JoystickTutorial.SetActive(true);

        yield return new WaitUntil(() => !JoystickTutorial.activeInHierarchy);
        Player.GetComponent<Animator>().SetBool("TutRun", true);
        while ((Player.position - TutorialDest.position).magnitude > 2f)
        {
            Player.LookAt(TutorialDest);
            Player.eulerAngles = new Vector3(0, Player.transform.eulerAngles.y, 0);
            yield return new WaitForEndOfFrame();
        }
        Player.GetComponent<Animator>().SetBool("TutRun", false);
        PlayerPrefs.SetInt("TutorialDescribed", 1);

        yield return null;
    }



    [SerializeField] string Skateboard;
    [SerializeField] GameObject FreeModeCam;
    [SerializeField] Canvas[] FreeModeCanvases;

    bool isLoaded;
    public void LoadSkateboard()
    {
        isLoaded = !isLoaded;
        if (isLoaded)
            StartCoroutine(LoadSkateBoardScene(true));
        else
            StartCoroutine(LoadSkateBoardScene(false));
    }

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.L))
    //    {
    //        StartCoroutine(LoadSkateBoardScene(true));
    //    }

    //    if (Input.GetKeyDown(KeyCode.U))
    //    {
    //        StartCoroutine(LoadSkateBoardScene(false));
    //    }

    //    if (Input.GetKey(KeyCode.R))
    //    {
    //        FindObjectOfType<ThirdPersonUserControl>().v = 1;
    //        print("adsdsdsd");
    //    }
    //}


    IEnumerator LoadSkateBoardScene(bool isLoadScene)
    {
        PlayerCamera.IsLerp = false;
        Loading.SetActive(true);

        yield return null;

        AsyncOperation asyncOperation;

        if (isLoadScene)
        {
            asyncOperation = SceneManager.LoadSceneAsync(Skateboard, LoadSceneMode.Additive);
            asyncOperation.allowSceneActivation = false;
        }
        else
        {
            asyncOperation = SceneManager.UnloadSceneAsync(Skateboard, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
            asyncOperation.allowSceneActivation = false;
        }

        if (!asyncOperation.isDone) yield return null;

        asyncOperation.allowSceneActivation = true;

        yield return null;

        Loading.SetActive(false);
        if (isLoadScene)
        {
            FreeModeCam.SetActive(false);
            for (int i = 0; i < FreeModeCanvases.Length; i++)
            {
                FreeModeCanvases[i].enabled = false;
            }
        }
        else
        {
            FreeModeCam.SetActive(true);
            for (int i = 0; i < FreeModeCanvases.Length; i++)
            {
                FreeModeCanvases[i].enabled = true;
            }
        }


    }



    public void StartLevel()
    {
        if (FreeModeLevelManagerObj != null)
        {
            FreeModeLevelManagerObj.StartLevel();
            for (int i = 0; i < Missions.Length; i++)
            {
                Missions[i].SetActive(false);
            }
            StartCoroutine(StartCountRoutine());
        }
    }

    IEnumerator StartCountRoutine()
    {
        LevelStartCountPanel.SetActive(true);
        yield return new WaitForSeconds(1f);
        LevelStartCounts[0].SetActive(true);
        LevelStartCounts[0].GetComponent<DOTweenAnimation>().DORestartById("1");
        LevelStartCounts[0].GetComponent<DOTweenAnimation>().DORestartById("2");
        yield return new WaitForSeconds(1f);
        LevelStartCounts[0].SetActive(false);
        LevelStartCounts[1].SetActive(true);
        LevelStartCounts[1].GetComponent<DOTweenAnimation>().DORestartById("1");
        LevelStartCounts[1].GetComponent<DOTweenAnimation>().DORestartById("2");
        yield return new WaitForSeconds(1f);
        LevelStartCounts[1].SetActive(false);
        LevelStartCounts[2].SetActive(true);
        LevelStartCounts[2].GetComponent<DOTweenAnimation>().DORestartById("1");
        LevelStartCounts[2].GetComponent<DOTweenAnimation>().DORestartById("2");
        yield return new WaitForSeconds(1f);
        LevelStartCounts[2].SetActive(false);
        LevelStartCounts[3].SetActive(true);
        LevelStartCounts[3].GetComponent<DOTweenAnimation>().DORestartById("1");
        LevelStartCounts[3].GetComponent<DOTweenAnimation>().DORestartById("2");
        LevelStartCountPanel.GetComponent<Image>().raycastTarget = false;
        yield return new WaitForSeconds(2f);
        LevelStartCounts[3].SetActive(false);
        LevelStartCountPanel.SetActive(false);
        yield return null;
    }

    public void EnableMissions()
    {
        for (int i = 0; i < Missions.Length; i++)
        {
            Missions[i].SetActive(true);
        }
    }

    public void LevelCleared()
    {

        StartCoroutine(LevelClearRoutine());
    }

    IEnumerator LevelClearRoutine()
    {
        GoodJobPanel.SetActive(true);
        GoodJobPanelAnim.DORestart();
        yield return new WaitForSeconds(2f);
        GoodJobPanel.SetActive(false);
        ClearPanel.SetActive(true);
        ClearPanelAnim.DORestart();
        DescriptionText.text = "Congrats !" + "\n\n" +
            "You have completed the level successfully." +
            "\n\n" +
            "You earned " + rewardValue + " coins";
        FindObjectOfType<CarResetCheckPoint>().ResetCheckPoint();
        if (AdsHandler.instance.IsInternetAvailable())
            StartCoroutine(ShowAds(false));
        yield return null;
    }

    [SerializeField] GameObject AdLoading;
    [SerializeField] GameObject Loading;
    public void Home()
    {
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            GameControl.driving = false;
            StartCoroutine(ShowAds(false));//Ads Comment
            StartCoroutine(LoadHomeScene(3));
            HideRec();
        }
        else
        {
            StartCoroutine(LoadHomeScene(0));
            GameControl.driving = false;
        }
    }
    public void ShowAdPause()
    {
        if (AdsHandler.instance.IsInternetAvailable())
            StartCoroutine(ShowAds(true));
    }

    public void HideRec() => AdsHandler.instance.HideRec();
    public IEnumerator ShowAds(bool recBanner)
    {
        yield return null;
        AdLoading.SetActive(true);
        yield return new WaitForSeconds(2f);
        AdLoading.SetActive(false);
        if (AdsHandler.instance)
        {
            AdsHandler.instance.ShowInterstitial();
            if (recBanner)
                AdsHandler.instance.ShowRec();
        }
        yield return null;
    }

    IEnumerator LoadHomeScene(float initialWait)
    {
        PlayerCamera.IsLerp = false;
        Loading.SetActive(true);

        if (initialWait == 0)
            yield return null;
        else
            yield return new WaitForSeconds(initialWait);

        SceneManager.LoadSceneAsync(0);
        yield return null;

    }

    public void ClearTheMission()
    {
        if (FindObjectOfType<FreeModeLevelClearerGeneric>())
            FindObjectOfType<FreeModeLevelClearerGeneric>().LevelCleared();

        if (CarResetCheckPointInstance)
            CarResetCheckPointInstance.ClearCPs();

        //if (Application.internetReachability != NetworkReachability.NotReachable)//Ads Comment
        //    StartCoroutine(ShowAds());

    }

    public void ResetCar()
    {
        if (CarResetCheckPointInstance)
            CarResetCheckPointInstance.ResetCar();
    }

    #region Auto Signaling Of Vehicle
    [SerializeField] VehicleCamera VehicleCameraObj;
    public void SignalPressed(int signalNum)
    {
        //VehicleCameraObj.target.GetComponent<CarResetCheckPoint>().Indicators[signalNum].SetActive(true);
        VehicleCameraObj.target.GetComponent<CarResetCheckPoint>().Signal(true, signalNum);
    }
    public void SignalReleased(int signalNum)
    {
        //VehicleCameraObj.target.GetComponent<CarResetCheckPoint>().Indicators[signalNum].SetActive(false);
        VehicleCameraObj.target.GetComponent<CarResetCheckPoint>().Signal(false, signalNum);
    }
    #endregion


    #region Crash System
    [Header("Car Hit Resources")]
    [SerializeField] string[] Dialogues;
    [SerializeField] DOTweenAnimation[] Emojis;
    [SerializeField] Text DialogueText;
    int emojiNum;
    int prevDialogue = -1;
    public void ShowDialogue()
    {
        emojiNum = Random.Range(0, Dialogues.Length);

        do
        {
            emojiNum = Random.Range(0, Dialogues.Length);
            //print(emojiNum);
        } while (emojiNum == prevDialogue);

        prevDialogue = emojiNum;
        Emojis[emojiNum].gameObject.SetActive(true);
        Emojis[emojiNum].DORestart();
    }
    public void HideDalogue()
    {
        Emojis[emojiNum].gameObject.SetActive(false);
    }
    #endregion


    #region  HoverBoardSystem
    [Header("HoverBoard")]
    [SerializeField] GameObject Controls;
    [SerializeField] GameObject PlayerControls;
    [SerializeField] Rigidbody RB;
    [SerializeField] GameObject HoverBoard;
    [SerializeField] CapsuleCollider Collider;
    [SerializeField] ThirdPersonUserControl TPC;
    [SerializeField] ThirdPersonCharacter TPCharacter;
    [SerializeField] HoverBoardController HoverBoardControllerObj;
    [SerializeField] RopeClimbSystem ropeClimbSystemObj;
    [SerializeField] Animator AnimatorObj;
    [SerializeField] Camera MainCamera;
    [SerializeField] DOTweenAnimation HoverBoardAnim;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backspace))
            SwitchToHoverBoard(true);
        if (Input.GetKeyDown(KeyCode.End))
            SwitchToHoverBoard(false);
    }
    bool isChange;
    public void SwitchToHoverBoard(bool isHoverBoard)
    {
        isChange = !isChange;

        if (isChange)
        {
            if (isHoverBoard)
            {
                Collider.enabled = false;
                TPC.enabled = false;
                TPCharacter.enabled = false;
                ropeClimbSystemObj.enabled = false;
                HoverBoardControllerObj.enabled = true;
                RB.drag = 0.35f;
                RB.angularDrag = 2;
                RB.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
                AnimatorObj.SetBool("HoverBoard", true);
                HoverBoard.SetActive(true);
                PlayerControls.SetActive(false);
                Controls.SetActive(true);
                MainCamera.enabled = false;
                HoverBoardAnim.DORestartAllById("1");
            }

        }
        else
        {
            Collider.enabled = true;
            TPC.enabled = true;
            TPCharacter.enabled = true;
            ropeClimbSystemObj.enabled = true;
            HoverBoardControllerObj.enabled = false;
            RB.drag = 0;
            RB.angularDrag = 0.05f;
            RB.constraints = RigidbodyConstraints.FreezeRotation;
            AnimatorObj.SetBool("HoverBoard", false);
            //HoverBoard.SetActive(false);
            PlayerControls.SetActive(true);
            Controls.SetActive(false);
            MainCamera.enabled = true;
            HoverBoardAnim.DORestartAllById("2");
        }

    }
    #endregion



    [SerializeField] GameObject danny;
    [SerializeField] GameObject ironMan;
    [SerializeField] GameObject optimus;
    [SerializeField] PlayerCamera playerCamera;
    public static Transform lastActivePlayer;
    public void ChangePlayer()
    {
        if (danny.activeInHierarchy)
        {
            lastActivePlayer = ironMan.transform;
            ironMan.transform.position = danny.transform.position;
            danny.SetActive(false);
            optimus.SetActive(false);
            ironMan.SetActive(true);
            playerCamera.target = ironMan.transform;
        }
        else
        {
            lastActivePlayer = danny.transform;
            danny.transform.position = ironMan.transform.position;
            ironMan.SetActive(false);
            optimus.SetActive(false);
            danny.SetActive(true);
            playerCamera.target = danny.transform;
        }
    }

    public void Optimus()
    {
        ironMan.SetActive(false);
        danny.SetActive(false);

        if (lastActivePlayer == null)
            optimus.transform.position = danny.transform.position;
        else
            optimus.transform.position = lastActivePlayer.position;
        optimus.SetActive(true);
        playerCamera.target = optimus.transform;

    }
}