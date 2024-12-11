using System.Collections;
using DG.Tweening;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GraphicalUIManager : MonoBehaviour
{

    public GameObject NextBtn;
    public GameObject NewLevelsComingSoon;
    public GameObject ParticlesRendererImage;
    public GameObject ParticlesRenderer;
    public GameObject clearedPanel;
    public GameObject pausePanel;
    public GameObject loadingPanel;
    public Image loadingBar;
    public RCC_Camera rCC_Camera;
    public AudioSource successSFX;
    public Text totalCoins;
    public Text levelCoins;
    public DOTweenAnimation GameplayAnim;
    public DOTweenAnimation SteeringAnim;
    //public GameObject controlSelection;


    [Header("----------------------------------Arrays----------------------------------")]
    public Image[] controls;
    public Sprite[] controlSprites;
    public DOTweenAnimation[] stars;
    public DOTweenAnimation[] clearedPanelBtns;
    public float[] cameraHeights;
    public float[] cameraDistances;

    //public bool disableControlSelection;

    public static int levelcoins;
    public static GraphicalUIManager instance;


    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    void Start()
    {
        if (MainMenuManager.modeLoaded == "One Touch")
            RCC_Settings.instance.useSteeringWheelForSteering = true;
        //else
        //    Controls(PlayerPrefs.GetInt("Controls"));

        //if (disableControlSelection)
        //    controlSelection.SetActive(false);
        levelcoins = 0;
        controlValue = PlayerPrefs.GetInt("Controls");
    }


    bool isPaused;
    public void GamePaused()
    {
        //Controls(PlayerPrefs.GetInt("Controls"));

        isPaused = true;
        iTween.ScaleTo(pausePanel, iTween.Hash("scale", Vector3.one, "easetype", iTween.EaseType.easeOutBack,
            "time", 0.5f));
        AudioListener.volume = 0.2f;

        //if (AdsDisplayHelper.instance)//Ads Comment
        //    AdsDisplayHelper.instance.DisplayAd(false);
    }

    public void GameResumed()
    {
        isPaused = false;
        iTween.ScaleTo(pausePanel, iTween.Hash("scale", Vector3.zero, "easetype", iTween.EaseType.easeInBack,
            "time", 0.5f));
        AudioListener.volume = 1f;

        //if (AdsDisplayHelper.instance)//Ads Comment
        //    AdsDisplayHelper.instance.HideRecBanner();
    }

    public void PlayGameplayAnim()
    {
        GameplayAnim.DOPlay();
    }

    bool levelCleared;
    public void LevelCleared()
    {
        levelCleared = true;
        iTween.ScaleTo(clearedPanel, iTween.Hash("scale", Vector3.one, "easetype", iTween.EaseType.easeOutBack,
            "time", 0.5f));

        successSFX.PlayOneShot(successSFX.clip);
        AnimateStars();
        GameManager.instance.musicSource.volume = 0.25f;
        GameManager.instance.cars[MainMenuManager.currentCarNum].GetComponent<RCC_CarControllerV3>().maxEngineSoundVolume = 0f;

        if (MainMenuManager.modeLoaded == "Career")
        {
            if (PlayerPrefs.GetInt("DrivingLevelsUnlocked") == 10 && MainMenuManager.currentLevelNum == 9)
            {
                NewLevelsComingSoon.SetActive(true);
                NextBtn.SetActive(false);
            }
        }
        else if (MainMenuManager.modeLoaded == "Advanced")
        {
            if (PlayerPrefs.GetInt("AdvancedLevelsUnlocked") == 20 && MainMenuManager.currentLevelNum == 19)
            {
                NewLevelsComingSoon.SetActive(true);
                NextBtn.SetActive(false);
            }
        }
        else
        {
            if (PlayerPrefs.GetInt("OneTouchLevelsUnlocked") == 20 && MainMenuManager.currentLevelNum == 19)
            {
                NewLevelsComingSoon.SetActive(true);
                NextBtn.SetActive(false);
            }
        }

        //if (AdsDisplayHelper.instance)//Ads Comment
        //    AdsDisplayHelper.instance.DisplayBanner();
    }

    IEnumerator CoinCollectionRoutine()
    {
        yield return new WaitForSeconds(1f);
        AudioSource coinSfx = levelCoins.GetComponent<AudioSource>();
        AudioSource totalCoinsSfx = totalCoins.GetComponent<AudioSource>();
        int coinsVal = 0;
        float t = 0;
        coinSfx.Play();

        while (coinsVal < levelcoins)
        {
            t += Time.deltaTime / 1;
            coinsVal = (int)Mathf.Lerp(0, levelcoins, t);
            levelCoins.text = coinsVal.ToString();
            yield return new WaitForEndOfFrame();
        }

        coinSfx.Stop();
        totalCoinsSfx.PlayOneShot(totalCoinsSfx.clip);
        totalCoins.GetComponent<DOTweenAnimation>().DOPlay();
        totalCoins.text = PlayerPrefs.GetInt("coins").ToString();
        yield return new WaitForSeconds(1f);

        for (int i = 0; i < clearedPanelBtns.Length; i++)
        {
            clearedPanelBtns[i].DOPlay();
        }
        yield return null;
    }

    async void AnimateStars()
    {
        var t = Time.time + 1;
        while (t > Time.time)
            await Task.Yield();
        foreach (DOTweenAnimation anim in stars)
        {
            anim.DOPlay();

        }

        ParticlesRendererImage.SetActive(true);
        ParticlesRenderer.SetActive(true);
        StartCoroutine(CoinCollectionRoutine());
    }

    public void RestartLevel()
    {
        LoadtheLevel(SceneManager.GetActiveScene().name);
    }

    public void Home()
    {
        LoadtheLevel("Menu");
    }

    public void Next()
    {
        MainMenuManager.currentLevelNum++;

        if (MainMenuManager.currentLevelNum == 10 && MainMenuManager.modeLoaded == "Career")
            MainMenuManager.currentLevelNum = 0;
        else if (MainMenuManager.currentLevelNum == 20)
            MainMenuManager.currentLevelNum = 0;

        LoadtheLevel(SceneManager.GetActiveScene().name);
    }


    int controlValue;
    public void Controls()
    {

        controlValue++;
        if (controlValue == 3)
            controlValue = 0;

        if (controlValue == 0)
        {
            controls[0].sprite = controlSprites[0];
            controls[1].sprite = controlSprites[3];
            controls[2].sprite = controlSprites[5];

            RCC_Settings.instance.useSteeringWheelForSteering = false;
            RCC_Settings.instance.useAccelerometerForSteering = false;

        }
        else if (controlValue == 1)
        {
            controls[1].sprite = controlSprites[2];
            controls[0].sprite = controlSprites[1];
            controls[2].sprite = controlSprites[5];

            RCC_Settings.instance.useSteeringWheelForSteering = true;
            RCC_Settings.instance.useAccelerometerForSteering = false;

        }
        else
        {
            controls[2].sprite = controlSprites[4];
            controls[0].sprite = controlSprites[1];
            controls[1].sprite = controlSprites[3];

            RCC_Settings.instance.useSteeringWheelForSteering = false;
            RCC_Settings.instance.useAccelerometerForSteering = true;

        }

        PlayerPrefs.SetInt("Controls", controlValue);
    }

    async void LoadtheLevel(string sceneToLoad)
    {
        //if (AdsDisplayHelper.instance)//Ads Comment
        //    AdsDisplayHelper.instance.HideRecBanner();

        if (isPaused)
        {
            loadingPanel.SetActive(true);
        }
        else
        {
            //if (AdsDisplayHelper.instance)//Ads Comment
            //    AdsDisplayHelper.instance.DisplayAd(true);

            loadingPanel.SetActive(true);
            var t = Time.time + 3f;
            while (t > Time.time)
                await Task.Yield();
        }

        //asyncOperation = SceneManager.LoadSceneAsync(sceneToLoad);
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

    int camNum;
    public void ChangeCamera()
    {
        camNum++;
        if (camNum == cameraHeights.Length)
            camNum = 0;

        for (int i = 0; i < cameraHeights.Length; i++)
        {
            if (camNum == i)
            {
                rCC_Camera.TPSHeight = cameraHeights[i];
                rCC_Camera.TPSDistance = cameraDistances[i];
                break;
            }
        }

        //if (camNum == 0)
        //{
        //    rCC_Camera.TPSHeight = 1.5f;
        //    rCC_Camera.TPSDistance = 1.5f;
        //}
        //else if (camNum == 1)
        //{
        //    rCC_Camera.TPSHeight = 1.8f;
        //    rCC_Camera.TPSDistance = 2f;
        //}
        //else
        //{

        //    rCC_Camera.TPSHeight = 2.1f;
        //    rCC_Camera.TPSDistance = 2.5f;
        //}
    }

}
