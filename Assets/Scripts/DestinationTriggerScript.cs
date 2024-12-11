using System.Collections;
using UnityEngine;


public class DestinationTriggerScript : MonoBehaviour
{
    public bool isAdvancedParking;
    public AnimationClip parkingClip;
    public AudioClip winSfx;
    public GameObject[] winParticlesEffect;

    //iTween
    public float sizeIncrement;
    public float scaleUpTime;
    public float delay;

    public Color completeColor;
    public Animation camera;
    GameObject chain;
    //GameObject parkingSignParent;
    //Image parkingSign;
    //Transform trainer;
    //Animator TrainerAnim;

    void Start()
    {

        iTween.ScaleTo(this.gameObject, iTween.Hash("scale", new Vector3(this.transform.localScale.x + sizeIncrement,
                    this.transform.localScale.y + sizeIncrement, this.transform.localScale.z + sizeIncrement),
                    "time", scaleUpTime, "easetype", iTween.EaseType.easeInOutBack, "looptype", iTween.LoopType.pingPong,
                    "delay", delay));

        chain = GameObject.FindWithTag("ChainedCarParent");
        chain.SetActive(false);

        //parkingSign = GameObject.FindWithTag("parkingsign").GetComponent<Image>();
        //parkingSignParent = parkingSign.transform.parent.gameObject;
        //parkingSignParent.SetActive(false);

        ////trainer = GameObject.FindWithTag("Trainer").transform;
        ////playerCar = GameObject.FindWithTag("Player").transform;
        //if (isBasicParking)
        //{
        //    TrainerAnim = GameObject.FindWithTag("Trainer").GetComponent<Animator>();
        //    TrainerAnim.SetBool("waving", true);
        //}
    }

    //void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.P))
    //    {
    //        //DoColorChange();
    //        //iTween.ColorTo(gameObject, Color.green, 1);
    //    }
    //    //trainer.LookAt(playerCar);
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //if (!isAdvancedParking)
            //{
            other.attachedRigidbody.isKinematic = true;
            GetComponent<Collider>().enabled = false;
            StartCoroutine(CompleteRoutine());
            //}
            //else
            //{
            //    other.attachedRigidbody.isKinematic = true;
            //    GetComponent<Collider>().enabled = false;
            //    StartCoroutine(CompleteRoutineBasicParking());
            //}


            if (MainMenuManager.modeLoaded == "Advanced")
            {
                LevelCompleteAP();
                DoColorChange();
            }
            else /*if (MainMenuUpdated.modeName == "One Touch Parking")*/
                LevelCompleteOT();

            GraphicalUIManager.instance.PlayGameplayAnim();
            GameManager.instance.PlayWellDoneSfx();
        }
    }


    void LevelCompleteAP()
    {
        if (MainMenuManager.currentLevelNum >= PlayerPrefs.GetInt("AdvancedLevelsUnlocked"))
            PlayerPrefs.SetInt("AdvancedLevelsUnlocked", MainMenuManager.currentLevelNum + 1);

        int tempCoins = (MainMenuManager.currentLevelNum + 1) * 100;
        PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins") + tempCoins);
        GraphicalUIManager.levelcoins = tempCoins;

    }

    void LevelCompleteOT()
    {
        if (MainMenuManager.currentLevelNum >= PlayerPrefs.GetInt("OneTouchLevelsUnlocked"))
            PlayerPrefs.SetInt("OneTouchLevelsUnlocked", MainMenuManager.currentLevelNum + 1);

        int tempCoins = (MainMenuManager.currentLevelNum + 1) * 100;
        PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins") + tempCoins);
        GraphicalUIManager.levelcoins = tempCoins;

    }

    public int timer = 0;
    IEnumerator CompleteRoutine()
    {

        camera.Play();
        while (timer < 2.5f)
        {
            foreach (var item in winParticlesEffect)
            {
                //item.SetActive(true);
                item.GetComponent<ParticleSystem>().Emit(100);
                //GetComponent<AudioSource>().PlayOneShot(winSfx);
            }
            yield return new WaitForSeconds(1f);
            if (!GetComponent<AudioSource>().isPlaying)
                GetComponent<AudioSource>().PlayOneShot(winSfx);
            timer++;
        }

        GraphicalUIManager.instance.LevelCleared();
        yield return null;
    }


    void DoColorChange()
    {
        iTween.ColorTo(gameObject, Color.green, 0.5f);
    }
}
