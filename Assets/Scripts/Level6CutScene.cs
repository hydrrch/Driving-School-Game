using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class Level6CutScene: MonoBehaviour
{

    public Image SteeringImage;
    public Camera rccCameraObj;
    public GameObject gameplay;
    public GameObject cutScene1;
    public GameObject cutScene2;
    public Animation boyAnim;
    public AnimationClip joyfullAnim;
    public GameObject instructionBox;
    public string instructions;



    void Start()
    {
        //animationObj = businessMan.GetComponent<Animation>();
        StartCoroutine(CutSceneRoutine());
    }


    IEnumerator CutSceneRoutine()
    {
        rccCameraObj.enabled = false;
        gameplay.SetActive(false);
        SteeringImage.enabled = false;
        yield return new WaitForSeconds(7f);
        cutScene1.SetActive(false);
        rccCameraObj.enabled = true;
        instructionBox.SetActive(true);
        instructionBox.GetComponentInChildren<Text>().text = instructions;
        yield return new WaitForSeconds(7f);
        instructionBox.SetActive(false);
        gameplay.SetActive(true);
        SteeringImage.enabled = true;
        yield return null;
    }

    public void EndingAnim()
    {
        StartCoroutine(EndingAnimRoutine());
    }

    IEnumerator EndingAnimRoutine()
    {
        gameplay.SetActive(false);
        yield return new WaitForSeconds(2f);
        cutScene2.SetActive(true);
        rccCameraObj.enabled = false;
        yield return new WaitForSeconds(joyfullAnim.length);
        boyAnim.CrossFade("Excited", 0.1f);
        yield return new WaitForSeconds(3f);
        GraphicalUIManager.instance.LevelCleared();
        yield return new WaitForSeconds(1f);
        rccCameraObj.enabled = true;
        rccCameraObj.farClipPlane = 1;
        cutScene2.SetActive(false);
        yield return null;
    }
}
