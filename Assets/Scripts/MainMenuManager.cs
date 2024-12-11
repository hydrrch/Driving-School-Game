//using System.Collections;
//using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;


public class MainMenuManager : MonoBehaviour
{
    [Header("------------------------------     Panels     ------------------------------")]
    public GameObject TapToPlayPanel;
    public GameObject menuPanel;
    public GameObject carSelectionPanel;
    public GameObject quitPanel;
    public GameObject modesPanel;
    public GameObject levelsPanel;
    public GameObject drivingLevels;
    public GameObject OT_APLevels;
    public GameObject musicOnBtn;
    public GameObject musicOffBtn;
    public DOTweenAnimation logo;
    public AudioSource musicSource;


    [Space(20)]
    public DOTweenAnimation[] settingsDotween;
    public Text[] coinsText;
    public AudioSource OneShotPlayer;
    public AudioClip BtnPressClip;
    public AudioClip DontQuitClip;
    public AudioClip DoQuitClip;

    public int dummyCoins;

    public static bool GameLaunched;
    public static int ProcessorFrequency;

    public static MainMenuManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    void Start()
    {

        if (GameLaunched)
        {
            TapToPlayPanel.SetActive(false);
            menuPanel.SetActive(true);
        }
        GameLaunched = true;

        //PlayerPrefs.SetInt("coins", dummyCoins);
        ProcessorFrequency = SystemInfo.processorFrequency;
        SetCoinsText();
        CheckCarLockStatus(displayCarNum);


        //UnlockAllCars();

        if (PlayerPrefs.GetInt("Music") == 0)
        {
            musicOnBtn.SetActive(true);
            musicOffBtn.SetActive(false);
            musicSource.mute = false;
        }
        else
        {
            musicOnBtn.SetActive(false);
            musicOffBtn.SetActive(true);
            musicSource.mute = true;
        }

        StartCoroutine(AnimateLogo());


        if (IsShowDeviceInfo)
            ShowDeviceInfo();

        AdsHandler.instance.HideRec();
    }


    public void SetCoinsText()
    {
        foreach (Text t in coinsText)
            t.text = PlayerPrefs.GetInt("coins").ToString();
    }

    IEnumerator AnimateLogo()
    {
        yield return new WaitForSeconds(2);

        while (true)
        {
            yield return new WaitForSeconds(5f);
            logo.DORestartById("2");
        }
    }


    public void ShowSettings()
    {
        PlayOneShotAudio(BtnPressClip);
        foreach (DOTweenAnimation a in settingsDotween)
            a.DORestart();
    }

    public void OpenURL(string link)
    {
        Application.OpenURL(link);
        PlayOneShotAudio(BtnPressClip);
    }

    public void Quit()
    {
        StartCoroutine(QuitTheGame());
    }

    IEnumerator QuitTheGame()
    {
        yield return null;
        PlayOneShotAudio(DoQuitClip);
        yield return new WaitForSeconds(1f);
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void ShowCarSelection(GameObject customPanel)
    {
        carSelectionPanel.SetActive(true);
        customPanel.SetActive(false);
        carSelectionPanel.GetComponent<DOTweenAnimation>().DORestart();
        PlayOneShotAudio(BtnPressClip);
    }

    public void ShowMenu(GameObject customPanel)
    {
        if (quitPanel.activeInHierarchy)
            PlayOneShotAudio(DontQuitClip);
        else
            PlayOneShotAudio(BtnPressClip);
        customPanel.SetActive(false);
        menuPanel.SetActive(true);
        iTween.MoveFrom(menuPanel, iTween.Hash("x", -2000, "time", 0.6f));

        //if (AdsDisplayHelper.instance)//Ads Comment
        //    AdsDisplayHelper.instance.HideRecBanner();
    }

    public void ShowQuit()
    {
        quitPanel.SetActive(true);
        menuPanel.SetActive(false);
        quitPanel.GetComponent<DOTweenAnimation>().DORestart();
        PlayOneShotAudio(BtnPressClip);
    }
    bool slideInMode;

    public void SlideInSet(bool status)
    {
        /*return */
        slideInMode = status;
    }

    public void ShowModeScreen(GameObject customPanel)
    {
        if (PlayerPrefs.GetInt("FirstTimeFreeMode") == 0)
        {
            PlayerPrefs.SetInt("FirstTimeFreeMode", 1);
            ShowLevelScreen("FreeMode");
        }
        else
        {

            if (carSelectionPanel.activeInHierarchy)
                PlayOneShotAudio(carSelectAudioClip);
            else
                PlayOneShotAudio(BtnPressClip);
            customPanel.SetActive(false);
            modesPanel.SetActive(true);
            //if (slideInMode)
            modesPanel.GetComponent<DOTweenAnimation>().DORestart();
        }
    }

    public void Music()
    {
        if (PlayerPrefs.GetInt("Music") == 0)
        {
            PlayerPrefs.SetInt("Music", 1);
            musicOnBtn.SetActive(false);
            musicOffBtn.SetActive(true);
            musicSource.mute = true;
        }
        else
        {
            PlayerPrefs.SetInt("Music", 0);
            musicOnBtn.SetActive(true);
            musicOffBtn.SetActive(false);
            musicSource.mute = false;
        }
        PlayOneShotAudio(BtnPressClip);
    }

    public async void ReplayDotweenAnim(DOTweenAnimation dOTween)
    {
        var t = Time.time + 3f;
        while (t > Time.time)
            await Task.Yield();

        dOTween.DORestart();
    }

    void PlayOneShotAudio(AudioClip clip)
    {
        OneShotPlayer.PlayOneShot(clip);
    }


    #region Car Selection

    [Header("------------------------------     Car Selection     ------------------------------")]
    public Button prevCar;
    public Button nextCar;
    public Text carNameText;
    public GameObject buyCar;
    public GameObject playBtn;
    public GameObject price;
    public GameObject noCashObj;
    public GameObject unlockAllCarsBtn;
    public AudioClip noCash;
    public AudioClip carUnlock;
    public AudioClip carSelectAudioClip;
    public ParticleSystem CarUnlockEffect;

    [Space(10)]
    public GameObject[] cars;
    public string[] carNames;
    public Image[] specs;
    public int[] carsPrices;
    public float[] carsPositions;
    public Image[] carsImages;
    public Sprite[] cars_Sprites;
    public GameObject[] purchaseCarBtns;

    public int displayCarNum;
    public static int currentCarNum;

    #region info
    [Serializable]
    class specsInfoClass
    {
        //[SerializeField]
        public float speedVal;
        //[SerializeField]
        public float accelerationVal;
        //[SerializeField]
        public float engineVal;
        //[SerializeField]
        public float brakesVal;
    }
    [SerializeField]
    specsInfoClass[] specsInfoClassObj;
    #endregion


    public void ChangeCar(int val)
    {
        cars[displayCarNum].transform.position = new Vector3(cars[displayCarNum].transform.position.x,
             carsPositions[displayCarNum], cars[displayCarNum].transform.position.z);

        if (val == -1)
        {
            displayCarNum--;
        }
        else if (val == 1)
        {
            displayCarNum++;
        }

        if (displayCarNum == 0)
            prevCar.interactable = false;
        else if (displayCarNum == cars.Length - 1)
            nextCar.interactable = false;
        else
        {
            prevCar.interactable = true;
            nextCar.interactable = true;
        }

        for (int i = 0; i < cars.Length; i++)
        {
            if (i.Equals(displayCarNum))
            {
                cars[i].SetActive(true);
                //carNameText.text = string.Format(carNames[i]).ToUpper();
                carNameText.DOText(carNames[i], 0.5f, false);
            }
            else
                cars[i].SetActive(false);
        }

        ShowSpecs();
        CheckCarLockStatus(displayCarNum);
        PlayOneShotAudio(BtnPressClip);
    }

    async void ShowSpecs()
    {
        var t = Time.time + 1;

        while (t > Time.time)
        {
            //for (int i = 0; i < specs.Length; i++)
            //{
            //    specs[i].fillAmount=specsInfoClassObj[].s
            //}

            specs[0].fillAmount = Mathf.Lerp(specs[0].fillAmount, specsInfoClassObj[displayCarNum].speedVal, Time.deltaTime);
            specs[1].fillAmount = Mathf.Lerp(specs[1].fillAmount, specsInfoClassObj[displayCarNum].accelerationVal, Time.deltaTime);
            specs[2].fillAmount = Mathf.Lerp(specs[2].fillAmount, specsInfoClassObj[displayCarNum].engineVal, Time.deltaTime);
            specs[3].fillAmount = Mathf.Lerp(specs[3].fillAmount, specsInfoClassObj[displayCarNum].brakesVal, Time.deltaTime);
            await Task.Yield();
        }
    }

    int spriteNum;
    async void UnlockAllCars()
    {
        spriteNum += 3;

        for (int i = 0; i < carsImages.Length; i++)
        {
            carsImages[i].GetComponent<DOTweenAnimation>().DORestart();
        }

        var t = Time.time + 5;

        while (t > Time.time)
        {
            for (int i = 0; i < carsImages.Length; i++)
            {
                carsImages[i].sprite = cars_Sprites[spriteNum - (i + 1)];
            }
            await Task.Yield();
        }

        if (spriteNum == 12)
            spriteNum = 0;

        //if (SceneManager.GetActiveScene().name.Equals("Menu"))
        //    UnlockAllCars();
    }

    bool animatePrice;
    public void CheckCarLockStatus(int vehicleNum)
    {
        if (vehicleNum.Equals(0))
        {
            animatePrice = false;
            buyCar.SetActive(false);
            price.SetActive(false);
            playBtn.SetActive(true);
            currentCarNum = displayCarNum;
            foreach (GameObject b in purchaseCarBtns)
                b.SetActive(false);
        }
        else
        {
            for (int i = 1; i < cars.Length; i++)
            {
                if (vehicleNum.Equals(i))
                {
                    if (PlayerPrefs.GetInt("UnlockedCar" + i).Equals(0))
                    {
                        buyCar.SetActive(true);
                        price.SetActive(true);
                        playBtn.SetActive(false);
                        price.GetComponentInChildren<Text>().text = carsPrices[i].ToString();
                        buyCar.GetComponent<DOTweenAnimation>().DORestart();

                        for (int j = 0; j < purchaseCarBtns.Length; j++)
                        {
                            if (j == displayCarNum)
                            {
                                purchaseCarBtns[j].SetActive(true);
                                purchaseCarBtns[j].GetComponent<DOTweenAnimation>().DORestart();
                            }
                            else
                                purchaseCarBtns[j].SetActive(false);
                        }



                        if (!animatePrice)
                        {
                            animatePrice = true;
                            price.GetComponent<DOTweenAnimation>().DORestart();
                        }
                    }
                    else
                    {
                        animatePrice = false;
                        buyCar.SetActive(false);
                        price.SetActive(false);
                        playBtn.SetActive(true);
                        currentCarNum = displayCarNum;
                        foreach (GameObject b in purchaseCarBtns)
                            b.SetActive(false);
                    }
                    break;
                }

            }
        }

        if (PlayerPrefs.GetInt("UnlockedCar1") == 0 || PlayerPrefs.GetInt("UnlockedCar2") == 0 || PlayerPrefs.GetInt("UnlockedCar3") == 0 ||
            PlayerPrefs.GetInt("UnlockedCar4") == 0 || PlayerPrefs.GetInt("UnlockedCar5") == 0 || PlayerPrefs.GetInt("UnlockedCar6") == 0 ||
            PlayerPrefs.GetInt("UnlockedCar7") == 0 || PlayerPrefs.GetInt("UnlockedCar8") == 0 || PlayerPrefs.GetInt("UnlockedCar9") == 0 ||
            PlayerPrefs.GetInt("UnlockedCar10") == 0 || PlayerPrefs.GetInt("UnlockedCar11") == 0)
        {
            unlockAllCarsBtn.SetActive(true);
        }
        else
        {
            unlockAllCarsBtn.SetActive(false);
        }
    }

    bool popingUp;
    public void BuyCarFunc()
    {
        for (int i = 0; i < cars.Length; i++)
        {
            if (displayCarNum.Equals(i))
            {
                if (PlayerPrefs.GetInt("coins") >= carsPrices[i])
                {
                    PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins") - carsPrices[i]);
                    PlayerPrefs.SetInt("UnlockedCar" + i, 1);
                    CheckCarLockStatus(displayCarNum);
                    SetCoinsText();
                    OneShotPlayer.PlayOneShot(carUnlock);
                    CarUnlockEffect.Play();
                }
                else
                {
                    if (!popingUp)
                        NoCashPopUp();
                }
                break;
            }
        }
    }

    async void NoCashPopUp()
    {
        popingUp = true;
        OneShotPlayer.PlayOneShot(noCash);
        Handheld.Vibrate();
        noCashObj.GetComponent<DOTweenAnimation>().DORestartById("0");

        var t = Time.time + 3;
        while (t > Time.time)
        {
            await Task.Yield();
        }

        noCashObj.GetComponent<DOTweenAnimation>().DORestartById("1");
        popingUp = false;
    }

    #endregion


    #region Level Selection

    [Header("------------------------------     Level Selection     ------------------------------")]
    public GameObject[] drivingModeLocks;
    public GameObject[] Ot_APModeLocks;
    public GameObject[] careerShiners;
    public GameObject[] careerHighlighters;
    public GameObject[] advanceShiners;
    public GameObject[] advanceHighlighters;
    public GameObject loadingPanel;
    public Image loadingBar;
    public GameObject unlockAllModesInapp;
    public GameObject unlockDrivingLevelsInapp;
    public GameObject unlockAdvancedLevelsInapp;
    public GameObject unlockOneTouchLevelsInapp;


    string currentMode;
    public static string modeLoaded;
    public static int currentLevelNum;

    public void ShowLevelScreen(string mode)
    {
        levelsPanel.SetActive(true);
        modesPanel.SetActive(false);
        levelsPanel.GetComponent<DOTweenAnimation>().DORestart();
        currentMode = mode;
        if (mode.Equals("FreeMode"))
        {
            LoadtheLevel("Free Mode New Env");
        }
        else
        {
            if (mode.Equals("Career"))
            {
                drivingLevels.SetActive(true);
                OT_APLevels.SetActive(false);
            }
            else
            {
                OT_APLevels.SetActive(true);
                drivingLevels.SetActive(false);
            }
        }

        CheckLevelsLockStatus();
        PlayOneShotAudio(BtnPressClip);
    }

    public void CheckLevelsLockStatus()
    {
        if (currentMode == "Career")
        {
            //PlayerPrefs.SetInt("DrivingLevelsUnlocked", 2);
            for (int i = 0; i < drivingModeLocks.Length; i++)
            {
                if (i <= PlayerPrefs.GetInt("DrivingLevelsUnlocked", 0))
                    drivingModeLocks[i].SetActive(false);
                else
                    drivingModeLocks[i].SetActive(true);

                if (PlayerPrefs.GetInt("DrivingLevelsUnlocked") == careerShiners.Length)
                {
                    careerShiners[9].SetActive(true);
                    careerHighlighters[9].SetActive(true);

                    for (int j = 0; j < careerShiners.Length - 1; j++)
                    {
                        careerShiners[j].SetActive(false);
                        careerHighlighters[j].SetActive(false);
                    }
                }
                else if (PlayerPrefs.GetInt("DrivingLevelsUnlocked") == i)
                {
                    careerShiners[i].SetActive(true);
                    careerHighlighters[i].SetActive(true);
                }
            }
        }
        else if (currentMode == "One Touch")
        {
            //PlayerPrefs.SetInt("OneTouchLevelsUnlocked", 9);
            for (int i = 0; i < Ot_APModeLocks.Length; i++)
            {
                if (i <= PlayerPrefs.GetInt("OneTouchLevelsUnlocked", 0))
                    Ot_APModeLocks[i].SetActive(false);
                else
                    Ot_APModeLocks[i].SetActive(true);

                if (PlayerPrefs.GetInt("OneTouchLevelsUnlocked") == advanceShiners.Length)
                {

                    advanceShiners[19].SetActive(true);
                    advanceHighlighters[19].SetActive(true);

                    for (int j = 0; j < advanceShiners.Length - 1; j++)
                    {
                        advanceShiners[j].SetActive(false);
                        advanceHighlighters[j].SetActive(false);
                    }
                }
                else
                {
                    if (PlayerPrefs.GetInt("OneTouchLevelsUnlocked") == i)
                    {
                        advanceShiners[i].SetActive(true);
                        advanceHighlighters[i].SetActive(true);
                    }
                    else
                    {
                        advanceShiners[i].SetActive(false);
                        advanceHighlighters[i].SetActive(false);
                    }
                }

            }
        }
        else
        {
            //PlayerPrefs.SetInt("AdvancedLevelsUnlocked", 2);
            for (int i = 0; i < Ot_APModeLocks.Length; i++)
            {
                if (i <= PlayerPrefs.GetInt("AdvancedLevelsUnlocked", 0))
                    Ot_APModeLocks[i].SetActive(false);
                else
                    Ot_APModeLocks[i].SetActive(true);


                if (PlayerPrefs.GetInt("AdvancedLevelsUnlocked") == advanceShiners.Length)
                {

                    advanceShiners[19].SetActive(true);
                    advanceHighlighters[19].SetActive(true);

                    for (int j = 0; j < advanceShiners.Length - 1; j++)
                    {
                        advanceShiners[j].SetActive(false);
                        advanceHighlighters[j].SetActive(false);
                    }
                }
                else
                {
                    if (PlayerPrefs.GetInt("AdvancedLevelsUnlocked") == i)
                    {
                        advanceShiners[i].SetActive(true);
                        advanceHighlighters[i].SetActive(true);
                    }
                    else
                    {
                        advanceShiners[i].SetActive(false);
                        advanceHighlighters[i].SetActive(false);
                    }
                }
            }
        }

    }


    public void SelectLevel(int levelNum)
    {
        if (currentMode == "Career")
        {
            modeLoaded = currentMode;
            currentLevelNum = levelNum;
            LoadtheLevel(currentMode);
            //StartCoroutine(LoadScene());
        }
        else if (currentMode == "Advanced")
        {
            modeLoaded = currentMode;
            currentLevelNum = levelNum;
            LoadtheLevel(currentMode);
        }
        else
        {
            modeLoaded = currentMode;
            currentLevelNum = levelNum;
            LoadtheLevel(currentMode);
        }
        PlayOneShotAudio(BtnPressClip);
    }

    async void LoadtheLevel(string sceneToLoad)
    {
        //if (AdsDisplayHelper.instance)//Ads Comment
        //    AdsDisplayHelper.instance.DisplayAd(true);

        loadingPanel.SetActive(true);
        //var t = Time.time + 3f;//Ads Comment
        //while (t > Time.time)//Ads Comment
        //    await Task.Yield();//Ads Comment

        //asyncOperation = SceneManager.LoadSceneAsync(sceneToLoad);
        //AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("Career_Basic");
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneToLoad);
        asyncOperation.allowSceneActivation = false;

        while (!asyncOperation.isDone)
        {
            loadingBar.fillAmount += asyncOperation.progress;

            if (asyncOperation.progress >= 0.9f)
                asyncOperation.allowSceneActivation = true;

            await Task.Yield();
        }

    }

    public void CheckModesInAppButton()
    {
        if (PlayerPrefs.GetInt("AdvancedLevelsUnlocked") < 19 || PlayerPrefs.GetInt("OneTouchLevelsUnlocked") < 19
            || PlayerPrefs.GetInt("DrivingLevelsUnlocked") < 9)
        {
            unlockAllModesInapp.SetActive(true);
        }
        else
            unlockAllModesInapp.SetActive(false);
    }

    public void CheckDrivingLevelsInappButton()
    {
        if (PlayerPrefs.GetInt("DrivingLevelsUnlocked") < 9)
        {
            unlockDrivingLevelsInapp.SetActive(true);
            unlockAdvancedLevelsInapp.SetActive(false);
            unlockOneTouchLevelsInapp.SetActive(false);
        }
        else
        {
            unlockDrivingLevelsInapp.SetActive(false);
            unlockAdvancedLevelsInapp.SetActive(false);
            unlockOneTouchLevelsInapp.SetActive(false);
        }
    }

    public void CheckAdvancedLevelsInappButton()
    {
        if (PlayerPrefs.GetInt("AdvancedLevelsUnlocked") < 19)
        {
            unlockAdvancedLevelsInapp.SetActive(true);
            unlockDrivingLevelsInapp.SetActive(false);
            unlockOneTouchLevelsInapp.SetActive(false);
        }
        else
        {
            unlockAdvancedLevelsInapp.SetActive(false);
            unlockDrivingLevelsInapp.SetActive(false);
            unlockOneTouchLevelsInapp.SetActive(false);
        }
    }

    public void CheckOneTouchLevelsInappButton()
    {
        if (PlayerPrefs.GetInt("OneTouchLevelsUnlocked") < 19)
        {
            unlockOneTouchLevelsInapp.SetActive(true);
            unlockDrivingLevelsInapp.SetActive(false);
            unlockAdvancedLevelsInapp.SetActive(false);
        }
        else
        {
            unlockOneTouchLevelsInapp.SetActive(false);
            unlockDrivingLevelsInapp.SetActive(false);
            unlockAdvancedLevelsInapp.SetActive(false);
        }
    }

    //IEnumerator LoadScene()
    //{
    //    yield return null;

    //    //Begin to load the Scene you specify
    //    AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(1);
    //    //Don't let the Scene activate until you allow it to
    //    asyncOperation.allowSceneActivation = false;
    //    Debug.Log("Pro :" + asyncOperation.progress);
    //    //When the load is still in progress, output the Text and progress bar
    //    while (!asyncOperation.isDone)
    //    {

    //        // Check if the load has finished
    //        if (asyncOperation.progress >= 0.9f)
    //        {
    //            if (Input.GetKeyDown(KeyCode.Space))
    //                //Activate the Scene
    //                asyncOperation.allowSceneActivation = true;
    //        }

    //        yield return null;
    //    }
    //}

    #endregion

    public void QuitAd()
    {
        //if (AdsDisplayHelper.instance)//Ads Comment
        //    AdsDisplayHelper.instance.DisplayAd(false);
    }

    public void FreeCoins()
    {
        PlayOneShotAudio(BtnPressClip);

        //if (AdsDisplayHelper.instance)//Ads Comment
        //    AdsDisplayHelper.instance.DisplayRewardedAd();
    }

    [ContextMenu("Set Coins")]
    public void SetTempCoins()
    {
        PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins") + 5000);
        foreach (Text t in coinsText)
            t.text = PlayerPrefs.GetInt("coins").ToString();
    }
    [SerializeField] bool IsShowDeviceInfo;
    [SerializeField] Text ProcessorTypeText;
    [SerializeField] Text ProcessorBitText;
    [SerializeField] Text ProcessorCountText;
    [SerializeField] Text ProcessorFrequencyText;
    [SerializeField] Text OSBitText;
    void ShowDeviceInfo()
    {
        ProcessorTypeText.text = "Processor Type:  " + SystemInfo.processorType;
        ProcessorBitText.text = "Is 64 Bit processor:  " + Environment.Is64BitOperatingSystem.ToString();
        ProcessorCountText.text = "Total Processors:  " + Environment.ProcessorCount;
        ProcessorFrequencyText.text = "Processor Frequency:  " + SystemInfo.processorFrequency;
        OSBitText.text = "Operating System:  " + SystemInfo.operatingSystem;
    }

    public void LoadFreeMode()
    {
        LoadtheLevel("Free Mode New Env");
    }

    [SerializeField] GameObject adLoading;
    public void ShowAd(bool showRec)
    {
        if (Application.internetReachability == NetworkReachability.NotReachable) return;
        StartCoroutine(ShowAdRoutine());
        IEnumerator ShowAdRoutine()
        {
            adLoading.SetActive(true);
            yield return new WaitForSeconds(2f);
            adLoading.SetActive(false);
            if (AdsHandler.instance)
            {
                AdsHandler.instance.ShowInterstitial();
                if (showRec)
                    AdsHandler.instance.ShowRec();
            }
        }
    }

    public void HideRecBanner()
    {
        if (AdsHandler.instance)
            AdsHandler.instance.HideRec();
    }

    // checking app state (foreground,background)
    //private void Update()
    //{
    //    print(IsAndroidAppInBackground());
    //}
    //private bool IsAndroidAppInBackground()
    //{
    //    //#if UNITY_ANDROID && !UNITY_EDITOR
    //    // Call an Android Java method to check if the app is in the background
    //    using (AndroidJavaClass unityPlayerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
    //    {
    //        AndroidJavaObject currentActivity = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity");
    //        AndroidJavaObject appState = currentActivity.Call<AndroidJavaObject>("getApplicationContext").Call<AndroidJavaObject>("getSystemService", "activity");

    //        bool isInBackground = appState.Call<bool>("moveTaskToBack", true);

    //        return isInBackground;
    //    }
    //    //#else
    //    // If not running on Android, always return false (app is considered in the foreground)
    //    return false;
    //    //#endif
    //}

}
