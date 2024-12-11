using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class SkateboardControllerCarType : MonoBehaviour
{

    [SerializeField] RCC_UIController RCC_UIControllerObj;
    [SerializeField] RCC_CarControllerV3 RCC_CarControllerV3Obj;
    [SerializeField] Animator PlayerAnimator;
    [SerializeField] Animator FinalDance;
    [SerializeField] float DefaultMaxVehicleSpeed;
    [SerializeField] GameObject LevelStartCountPanel;
    [SerializeField] GameObject[] LevelStartCounts;
    [SerializeField] DOTweenAnimation HopButtonAnim;
    [SerializeField] AudioSource BgMusic;
    [SerializeField] GameObject GoodJobPanel;
    [SerializeField] GameObject ClearedPanel;
    [SerializeField] AudioSource HopSfx;
    [SerializeField] GameObject[] Tut;
    [SerializeField] GameObject SkateSlider;
    [SerializeField] ParticleSystem SkateDust;

    Rigidbody rb;

    public static SkateboardControllerCarType instance;
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        //RCC_CarControllerV3Obj.gasInput = 1;
        RCC_Settings.instance.useAccelerometerForSteering = false;
        RCC_Settings.instance.useSteeringWheelForSteering = false;
        RCC_CarControllerV3Obj.maxspeed = DefaultMaxVehicleSpeed;
        rb = GetComponent<Rigidbody>();
        StartCoroutine(StartGame());
        if (Tut[0])
            StartCoroutine(Tutorial());
    }

    IEnumerator Tutorial()
    {
        yield return new WaitForSeconds(5f);
        Tut[0].SetActive(false);
        Tut[1].SetActive(false);
    }


    bool hopped;
    public float timeToResetVelocity;
    public float tempSpeed;


    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.R))
        //    StartCoroutine(RotateRoutine(1));

        //if (Input.GetKeyDown(KeyCode.F))
        //    RCC_Camera.isFollow = true;

        //if (Input.GetKeyDown(KeyCode.W))
        //{
        //    RCC_CarControllerV3Obj.maxspeed += 5;
        //    hopped = true;
        //    timeToResetVelocity = Time.time + 10;
        //}

        if (hopped)
        {
            if (timeToResetVelocity < Time.time)
            {
                RCC_CarControllerV3Obj.maxspeed = DefaultMaxVehicleSpeed;
                hopped = false;
            }
        }

        //tempSpeed = RCC_CarControllerV3Obj.speed;

    }



    public void UnloadScene()
    {

        FindObjectOfType<FreeModeGM>().LoadSkateboard();
    }


    #region Skating System
    public void Hop()
    {
        timeToResetVelocity = 0;
        if (RCC_CarControllerV3Obj.maxspeed < 100)
        {
            RCC_CarControllerV3Obj.maxspeed += 10;
            hopped = true;
            timeToResetVelocity = Time.time + 10;
            //RCC_CarControllerV3Obj.maxspeed = 60;
            StartCoroutine(PlayHopAnimation());
        }

    }

    IEnumerator PlayHopAnimation()
    {
        PlayerAnimator.SetBool("hop", true);
        yield return new WaitForSeconds(0.5f);
        PlayerAnimator.SetBool("hop", false);
        HopSfx.Play();
        yield return new WaitForSeconds(0.5f);
        SkateDust.Play();
    }


    [SerializeField] float ForceToForward;
    [SerializeField] Vector3 velocity;
    public void Skate()
    {
        Hop();
        Vector3 _velocity = rb.transform.forward * ForceToForward * Time.deltaTime;
        //if (_velocity.z > -15)
        //    _velocity = new Vector3(0, 0, -15f);
        rb.velocity += _velocity;
        velocity = rb.velocity;
    }
    #endregion

    #region Game Starting Scenario
    IEnumerator StartGame()
    {
        rb.isKinematic = true;
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
        rb.isKinematic = false;
        StartCoroutine(PlayHopAnimation());
        BgMusic.Play();
        yield return new WaitForSeconds(2f);
        LevelStartCounts[3].SetActive(false);
        LevelStartCountPanel.SetActive(false);
        //HopButtonAnim.gameObject.SetActive(true);
        //HopButtonAnim.DOPlayById("2");

    }
    #endregion

    #region Level Completion
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "CompleteCube")
        {
            StartCoroutine(LevelCompleteRoutine());
        }
    }

    IEnumerator LevelCompleteRoutine()
    {
        //PlayerAnimator.Play("hop stop");
        RCC_CarControllerV3Obj.maxspeed = 0f;
        SkateSlider.SetActive(false);
        yield return new WaitForSeconds(1f);

        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime / 1f;
            rb.drag = Mathf.Lerp(0.05f, 50f, t);
            yield return new WaitForEndOfFrame();
        }

        HopButtonAnim.DOPlayById("1");
        yield return new WaitForSeconds(1f);

        FinalDance.gameObject.SetActive(true);
        PlayerAnimator.gameObject.SetActive(false);
        FinalDance.Play("FinalDance");
        yield return new WaitForSeconds(3f);

        GoodJobPanel.SetActive(true);
        yield return new WaitForSeconds(2f);

        GoodJobPanel.SetActive(false);
        ClearedPanel.SetActive(true);
        PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins") + 1500);

        if (FreeModeGM.instance)
        {
            //FreeModeGameManager.instance.LevelCleared();
            FreeModeGM.instance.EnableMissions();
        }

        StartCoroutine(ShowAds(false));
    }
    [SerializeField] GameObject AdLoading;
    public IEnumerator ShowAds(bool recBanner)
    {
        yield return null;
        AdLoading.SetActive(true);
        yield return new WaitForSeconds(2f);
        AdLoading.SetActive(false);
        if (AdsHandler.instance)
            AdsHandler.instance.ShowInterstitial();
        yield return null;
    }

    #endregion

    #region Stunt Dialogue System
    [SerializeField] GameObject Dialogue;
    [SerializeField] Text DialogueText;
    [SerializeField] DOTweenAnimation DialogueAnim;
    [SerializeField] string[] Dialogues;
    public IEnumerator ShowDialogue()
    {
        //Dialogue.SetActive(true);
        DialogueText.text = string.Empty;
        DialogueText.text = Dialogues[Random.Range(0, Dialogues.Length)];
        DialogueAnim.DORestartById("1");
        yield return new WaitForSeconds(1.5f);
        //Dialogue.SetActive(false);
        DialogueAnim.DORestartById("2");
    }
    #endregion



    //IEnumerator RotateRoutine(float lerpTime)
    //{
    //    //GameControl.AllowLerp = false;
    //    float angularDrag = 0;
    //    if (rb)
    //    {
    //        angularDrag = rb.angularDrag;
    //        rb.angularDrag = 100;
    //    }

    //    //if (mainCam)
    //    //    mainCam.fieldOfView = 80;
    //    float t = 0;
    //    Vector3 rot = transform.eulerAngles;

    //    while (t < 1)
    //    {
    //        t += Time.deltaTime / lerpTime;
    //        float zRot = Mathf.Lerp(0, 350, t);
    //        transform.eulerAngles = new Vector3(transform.eulerAngles.x, zRot, transform.eulerAngles.z);

    //        yield return new WaitForEndOfFrame();
    //    }
    //    rb.angularDrag = angularDrag;
    //    //mainCam.fieldOfView = 60;
    //    //GameControl.AllowLerp = true;
    //    yield return null;
    //}


}
