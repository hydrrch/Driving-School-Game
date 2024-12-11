using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class DetectCollision : MonoBehaviour
{


    bool firstTime;





    //private void Start()
    //{
    //    colcamview = GameObject.FindGameObjectWithTag("colcamview");
    //    //print(colcamview.name);
    //    //this.colcamview.SetActive(false);
    //    if (colcamview.GetComponent<RawImage>().enabled)
    //        colcamview.GetComponent<RawImage>().enabled = false;
    //}

    private void OnTriggerEnter(Collider other)
    {
        //if (!other.CompareTag("Player"))
        //if (other.GetComponent<CarCrashDetector>() != null)
        //{
        //    ActivateCam();
        //    //if (PlayerPrefs.GetInt("firstTime").Equals(0))
        //    //{
        //    //    PlayerPrefs.SetInt("firstTime", 1);
        //    //    StartCoroutine(SlowMo());
        //    //}
        //}
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            DeactivateCam();
    }

    void ActivateCam()
    {
        //colcamview.GetComponent<RawImage>().enabled = true;
        //++   colcamview.SetActive(true);
        GameObject.FindGameObjectWithTag("colcamview").GetComponent<Image>().enabled = true;
        GameObject.FindGameObjectWithTag("colcamraw").GetComponent<RawImage>().enabled = true;

        GameObject.FindGameObjectWithTag("colcamtext").GetComponent<Text>().enabled = true;
        transform.GetChild(0).gameObject.SetActive(true);
    }

    void DeactivateCam()
    {
        //colcamview.GetComponent<RawImage>().enabled = false;
        //++ colcamview.SetActive(false);
        GameObject.FindGameObjectWithTag("colcamview").GetComponent<Image>().enabled = false;
        GameObject.FindGameObjectWithTag("colcamraw").GetComponent<RawImage>().enabled = false;

        GameObject.FindGameObjectWithTag("colcamtext").GetComponent<Text>().enabled = false;
        transform.GetChild(0).gameObject.SetActive(false);
    }

    IEnumerator SlowMo()
    {
        firstTime = true;
        //FindObjectOfType<GUImanager>().SkipBtn.SetActive(true);
        Time.timeScale = 0.2f;
        yield return new WaitForSeconds(0.2f);
        Time.timeScale = 0.001f;
        yield return null;
    }

}
