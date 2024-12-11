using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class Level7CutScene : MonoBehaviour
{

    public Image SteeringImage;
    public GameObject waypoints;
    public GameObject cutScene;
    public GameObject gameControls;
    public Camera rccCamera;
    public GameObject walkCamera;
    public GameObject phoneCamera;
    public AnimationClip walk;
    public Animation mechanicAnim;
    public GameObject cutScene2;
    public GameObject toolBox;
    public GameObject instructionsBox;
    public string instructions;

    void Start()
    {
        StartCoroutine(CutSceneRoutine());
    }

    IEnumerator CutSceneRoutine()
    {
        gameControls.SetActive(false);
        rccCamera.enabled = false;
        waypoints.SetActive(false);
        SteeringImage.enabled = false;
        yield return new WaitForSeconds(5f);
        phoneCamera.SetActive(false);
        walkCamera.SetActive(true);
        mechanicAnim.Play(walk.name);
        toolBox.SetActive(true);
        mechanicAnim.GetComponent<WPSystem>().enabled = true;
        yield return new WaitForSeconds(3f);
        rccCamera.enabled = true;
        cutScene.SetActive(false);
        waypoints.SetActive(true);
        instructionsBox.SetActive(true);
        instructionsBox.GetComponentInChildren<Text>().text = instructions;
        yield return new WaitForSeconds(7f);
        instructionsBox.SetActive(false);
        gameControls.SetActive(true);
        SteeringImage.enabled = true;
        yield return null;
    }


    public void EndingAnim()
    {
        StartCoroutine(EndingAnimRoutine());
    }

    IEnumerator EndingAnimRoutine()
    {
        yield return new WaitForSeconds(2f);
        cutScene2.SetActive(true);
        rccCamera.enabled = false;
        yield return new WaitForSeconds(3f);
        GraphicalUIManager.instance.LevelCleared();
        yield return new WaitForSeconds(1f);
        rccCamera.enabled = true;
        rccCamera.farClipPlane = 1;
        cutScene2.SetActive(false);
        yield return null;
    }

}
