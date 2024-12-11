using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class Level5CutScene : MonoBehaviour
{
    public Image SteeringImage;
    public string instructions;
    public Text instrucionsText;
    public GameObject waypoints;
    public GameObject gameplay;
    public GameObject instructionPanel;
    public GameObject cutScene;
    public GameObject cutScenePanel;
    public Camera rccCamera;
    public GameObject carToTakeWorkshop;
    Rigidbody carRigidbody;
    GameObject chain;
    RCC_Camera rCC_Camera;

    private void Start()
    {
        carRigidbody = GameObject.FindWithTag("Player").GetComponentInParent<Rigidbody>();
        chain = GameObject.FindWithTag("ChainedCarParent");
        rCC_Camera = rccCamera.GetComponentInParent<RCC_Camera>();
        chain.SetActive(true);
        StartCoroutine(ShowCutScene());
        SteeringImage.enabled = false;
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    StartCoroutine(ShowCutScene());
    //}

    IEnumerator ShowCutScene()
    {
        gameplay.SetActive(false);
        rccCamera.enabled = false;
        cutScene.SetActive(true);
        yield return new WaitForSeconds(3f);
        instrucionsText.text = instructions.ToString();
        instructionPanel.SetActive(true);
        yield return new WaitForSeconds(5f);
        rccCamera.enabled = true;
        cutScene.SetActive(false);
        instructionPanel.SetActive(false);
        cutScenePanel.SetActive(false);
        carToTakeWorkshop.SetActive(true);
        carToTakeWorkshop.transform.SetParent(chain.transform);
        carToTakeWorkshop.transform.localPosition = Vector3.zero;
        carToTakeWorkshop.transform.localRotation = Quaternion.identity;
        carToTakeWorkshop.transform.localScale = Vector3.one;
        carRigidbody.gameObject.SetActive(true);
        gameplay.SetActive(true);
        waypoints.SetActive(true);
        rCC_Camera.TPSDistance = 3;
        rCC_Camera.TPSHeight = 2;
        SteeringImage.enabled = true;
        yield return null;
    }
}
