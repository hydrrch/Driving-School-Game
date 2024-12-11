using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class Level9CutScene : MonoBehaviour
{

    public Image SteeringImage;
    public GameObject waypoints;
    public GameObject gameControls;
    public WPSystem waypointSystem;
    public Camera rccCam;
    public GameObject cutScene;
    public GameObject cutScene2;
    public GameObject instructionsBox;
    public string instructions;

    private void Start()
    {
        StartCoroutine(CutSceneRoutine());
    }

    IEnumerator CutSceneRoutine()
    {

        rccCam.enabled = false;
        gameControls.SetActive(false);
        SteeringImage.enabled = false;
        yield return new WaitForSeconds(1.8f);
        waypointSystem.GetComponent<Animation>().Play();
        waypointSystem.enabled = true;
        yield return new WaitForSeconds(3f);
        rccCam.enabled = true;
        cutScene.SetActive(false);
        waypoints.SetActive(true);
        instructionsBox.SetActive(true);
        instructionsBox.GetComponentInChildren<Text>().text = instructions;
        yield return new WaitForSeconds(5f);
        instructionsBox.SetActive(false);
        gameControls.SetActive(true);
        SteeringImage.enabled = true;
        yield return null;
    }

    public void EndingAnim()
    {
        StartCoroutine(EndingAnimRoutine());
    }

    public IEnumerator EndingAnimRoutine()
    {
        yield return new WaitForSeconds(2f);
        cutScene2.SetActive(true);
        rccCam.enabled = false;
        yield return new WaitForSeconds(3f);
        GraphicalUIManager.instance.LevelCleared();
        yield return new WaitForSeconds(1f);
        rccCam.enabled = true;
        rccCam.farClipPlane = 1;
        cutScene2.SetActive(false);
        yield return null;
    }
}
