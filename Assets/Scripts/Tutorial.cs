using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Tutorial : MonoBehaviour
{

    public GameObject CongratsImage;
    public GameObject gameplay;
    public Scrollbar gearBar;
    public RCC_UIController gas;
    public RCC_CarControllerV3 rCC_CarController;
    public GameObject instructionsPanel;
    public Text instructionsText;
    public string instructions;
    public GameObject TutorialScreen;
    public Image[] buttons;
    public GameObject[] pointers;
    public GameObject[] gears;

    float carMaxSpeed;
    bool gasPressed;
    bool stopTheCar;
    new Rigidbody rigidbody;

    void Start()
    {
        if (PlayerPrefs.GetInt("Tut") == 0)
        {
            GameManager.instance.isTutorial = true;
            foreach (Image b in buttons)
                b.enabled = false;
            StartCoroutine(ShowInstructions());
            carMaxSpeed = rCC_CarController.maxspeed;
            rigidbody = rCC_CarController.GetComponent<Rigidbody>();
            RCC_Settings.instance.useSteeringWheelForSteering = false;
            RCC_Settings.instance.useAccelerometerForSteering = false;
        }
        else
            gameplay.SetActive(true);

    }

    public void ChangeGear(Scrollbar gear)
    {
        if (gear.value < 0.5f)
            gear.value = 1f;
        else
            gear.value = 0f;

        //for (int i = 0; i < gears.Length; i++)
        //{
        //    if (gears[i].activeInHierarchy)
        //        gears[i].SetActive(false);
        //    else
        //        gears[i].SetActive(true);
        //}
        gears[1].SetActive(true);
    }

    private void LateUpdate()
    {
        if (gasPressed)
            gas.pressing = true;

        if (stopTheCar)
            rigidbody.velocity = Vector3.Lerp(rigidbody.velocity, Vector3.zero, Time.deltaTime);
    }


    IEnumerator ShowInstructions()
    {
        foreach (GameObject g in gears)
            g.SetActive(false);
        yield return new WaitForSeconds(1f);
        instructionsPanel.SetActive(true);
        instructionsText.text = instructions;
        yield return new WaitForSeconds(5f);
        instructionsPanel.SetActive(false);
        StartCoroutine(ButtonsHandlerRoutine(0, 1f));
        yield return new WaitForSeconds(1f);
        gameplay.SetActive(true);
        gameplay.GetComponent<DOTweenAnimation>().DOPlayById("0");
        ChangeGear(gearBar);
        yield return null;
    }

    [SerializeField] bool calledRoutine;
    public void EnableNextButton(int number)
    {

        if (GameManager.instance.isTutorial && !calledRoutine)
        {
            calledRoutine = true;

            if (number.Equals(1))
            {
                gears[0].SetActive(true);
                gears[1].SetActive(false);
            }
            else if (number.Equals(4))
            {
                gasPressed = true;
                rCC_CarController.maxspeed = 20f;
            }
            else if (number.Equals(-1))
            {
                gasPressed = false;
                gas.pressing = false;
                StartCoroutine(ButtonsHandlerRoutine(number, 2f));
            }

            if (number > -1)
                StartCoroutine(ButtonsHandlerRoutine(number, 2f));
            //else
            //{
            //    foreach (Image b in buttons)
            //        b.enabled = true;
            //    isTutorial = false;
            //}
        }


    }

    IEnumerator ButtonsHandlerRoutine(int buttonNumber, float wait)
    {
        yield return new WaitForSeconds(wait);
        if (buttonNumber != -1)
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                if (i.Equals(buttonNumber))
                {
                    buttons[i].enabled = true;
                    buttons[i].transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                    iTween.ScaleTo(buttons[i].gameObject, iTween.Hash("scale", Vector3.one, "time", 0.5f, "easetype",
                        iTween.EaseType.easeOutQuad));

                }
                else
                {
                    buttons[i].enabled = false;
                }
            }

            for (int i = 0; i < pointers.Length; i++)
            {
                if (i.Equals(buttonNumber))
                {
                    pointers[i].SetActive(true);
                }
                else
                {
                    pointers[i].SetActive(false);
                }
            }

            if (buttonNumber == 1)
            {
                gears[0].SetActive(false);
                gearBar.value = 0;
            }
            else if (buttonNumber.Equals(2))
            {
                gas.pressing = false;
                stopTheCar = true;
            }
            else
            {
                stopTheCar = false;
            }
        }
        else
        {
            //foreach (Image b in buttons)
            //    b.enabled = true;
            buttons[4].enabled = false;
            GameManager.instance.isTutorial = false;
            pointers[4].SetActive(false);
            rCC_CarController.maxspeed = carMaxSpeed;
            PlayerPrefs.SetInt("Tut", 1);
            GetComponent<AudioSource>().Play();
            CongratsImage.SetActive(true);
        }
        calledRoutine = false;
        yield return new WaitForSeconds(2f);
        if (PlayerPrefs.GetInt("Tut") == 1)
        {
            gears[0].SetActive(true);
            foreach (Image b in buttons)
                b.enabled = true;
            CongratsImage.SetActive(false);
        }
        yield return null;
    }
}
