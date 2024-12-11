using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SkateboardSceneStunt : MonoBehaviour
{

    [SerializeField] bool enableCamera;
    [SerializeField] AnimationClip[] AnimationClipName;
    [SerializeField] Animator AnimatorObj;
    [SerializeField] Animation AnimationObj;
    [SerializeField] GameObject StuntCamera;
    [SerializeField] Camera RccCamera;
    [SerializeField] Canvas PlayerCanvas;
    [SerializeField] RCC_CarControllerV3 RCC_CarControllerV3Obj;
    [SerializeField] Button HopButton;
    [SerializeField] GameObject SkateSlider;

    private void OnTriggerEnter(Collider other)
    {
        //print(RCC_CarControllerV3Obj.speed);
        //if (RCC_CarControllerV3Obj.speed >= 55)
        //{

        if (enableCamera)
        {
            StuntCamera.SetActive(true);
            //RccCamera.SetActive(false);
            RccCamera.enabled = false;
            PlayerCanvas.enabled = false;

        }
        else
            StartCoroutine(StuntRoutine());
        //}
        //else
        //{

        //}
    }


    IEnumerator StuntRoutine()
    {

        //if (AnimationObj)
        //    AnimationObj.Play(AnimationClipName[Random.Range(0, AnimationClipName.Length)].name);
        //else
        AnimatorObj.Play(AnimationClipName[Random.Range(0, AnimationClipName.Length)].name);
        GetComponent<AudioSource>().Play();
        HopButton.interactable = false;
        SkateSlider.SetActive(false);
        //RCC_Camera.isFollow = false;
        //StuntCamera.SetActive(true);
        //RccCamera.SetActive(false);
        yield return new WaitForSeconds(1f);
        HopButton.interactable = true;
        SkateSlider.SetActive(true);
        StartCoroutine(SkateboardControllerCarType.instance.ShowDialogue());
        //RccCamera.SetActive(true);
        //RccCamera.enabled = true;
        //StuntCamera.SetActive(false);
        //PlayerCanvas.enabled = true;
        //RCC_Camera.isFollow = true;
    }
}
