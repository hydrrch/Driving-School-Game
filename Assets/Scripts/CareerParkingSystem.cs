using System.Collections;
using UnityEngine;
using DG.Tweening;


public class CareerParkingSystem : MonoBehaviour
{

    public GameObject gameControls;
    public GameObject[] confettis;
    Animation cameraAnim;

    public bool isLevel6EndAnim;
    public bool isLevel7EndAnim;
    public bool isLevel9EndAnim;

    AudioSource successSFX;

    private void Start()
    {
        cameraAnim = GameObject.Find("Camera").GetComponent<Animation>();
        successSFX = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.attachedRigidbody.constraints = RigidbodyConstraints.FreezePosition;
            cameraAnim.Play();
            foreach (GameObject g in confettis)
                g.SetActive(true);

            if (isLevel6EndAnim)
            {
                FindObjectOfType<Level6CutScene>().EndingAnim();
                StartCoroutine(DisableControls(0, false));
            }
            else if (isLevel7EndAnim)
            {
                FindObjectOfType<Level7CutScene>().EndingAnim();
                StartCoroutine(DisableControls(0, false));
            }
            else if (isLevel9EndAnim)
            {
                FindObjectOfType<Level9CutScene>().EndingAnim();
                StartCoroutine(DisableControls(0, false));
            }
            else
            {
                StartCoroutine(DisableControls(1f, true));
            }

            successSFX.PlayOneShot(successSFX.clip);
            GameManager.instance.PlayWellDoneSfx();
            
        }
    }

    IEnumerator DisableControls(float levelClearWait, bool showCleared)
    {

        if (MainMenuManager.currentLevelNum >= PlayerPrefs.GetInt("DrivingLevelsUnlocked"))
            PlayerPrefs.SetInt("DrivingLevelsUnlocked", PlayerPrefs.GetInt("DrivingLevelsUnlocked") + 1);

        int tempCoins = (MainMenuManager.currentLevelNum + 1) * 100;
        PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins") + tempCoins);
        GraphicalUIManager.levelcoins = tempCoins;

        yield return new WaitForSeconds(1f);
        gameControls.GetComponent<DOTweenAnimation>().DOPlay();
        yield return new WaitForSeconds(levelClearWait);
        if (showCleared)
        {
            GraphicalUIManager.instance.LevelCleared();
            yield return new WaitForSeconds(1f);
            cameraAnim.GetComponent<Camera>().farClipPlane = 1;
        }
        yield return null;
    }
}
