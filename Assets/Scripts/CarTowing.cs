using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarTowing : MonoBehaviour
{

    [SerializeField] string CarName;
    [SerializeField] Animation Animation;
    [SerializeField] AnimationClip AnimClip;
    [SerializeField] GameObject TowingCarAnimated;
    [SerializeField] GameObject TowingCarCamera;
    [SerializeField] GameObject PlayerCamera;
    [SerializeField] Transform DamagedCar;
    [SerializeField] Transform DamagedCarParent;
    [SerializeField] Canvas PlayerCanvas;
    [SerializeField] GameObject TowButton;
    [SerializeField] GameObject ParticleEffect;
    [SerializeField] Transform PlayerPosRef;
    [SerializeField] CarComponents CarComponentsObj;
    [SerializeField] Collider DamagedCarBodyCol;
    [SerializeField] GameObject CompleteCube;
    [SerializeField] Transform positionResetDamagedCarRef;
    [SerializeField] GameObject TowCarIndicator;
    [SerializeField] GameObject PickTheCarInst;


    private void Start()
    {
        StartCoroutine(PickTheCarInstRoutine());
    }

    IEnumerator PickTheCarInstRoutine()
    {
        yield return null;
        PickTheCarInst.SetActive(true);
        yield return new WaitForSeconds(3f);
        PickTheCarInst.SetActive(false);
        yield return null;
    }

    private void OnTriggerEnter(Collider other)
    {
        //print(other.name);
        if (other.name.Equals(CarName))
        {
            TowButton.SetActive(true);
        }

        if (other.TryGetComponent(out FreeModeLevelClearerGeneric freeModeGenericLevelClearer))
        {
            DamagedCar.parent = positionResetDamagedCarRef.parent;
            DamagedCar.position = positionResetDamagedCarRef.position;
            DamagedCar.rotation = positionResetDamagedCarRef.rotation;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        //print(other.name);
        if (other.name.Equals(CarName))
        {
            TowButton.SetActive(false);
        }
    }

    public void Tow()
    {
        StartCoroutine(CarTowingRoutine());
    }

    IEnumerator CarTowingRoutine()
    {
        yield return null;
        PlayerCanvas.enabled = false;
        Animation.Play();
        TowingCarCamera.SetActive(true);
        PlayerCamera.SetActive(false);
        ParticleEffect.SetActive(false);
        TowingCarAnimated.SetActive(true);
        transform.position = PlayerPosRef.position;
        transform.rotation = PlayerPosRef.rotation;
        yield return new WaitForSeconds(AnimClip.length + 1);
        CompleteCube.SetActive(true);
        DamagedCar.SetParent(DamagedCarParent);
        DamagedCar.localPosition = Vector3.zero;
        DamagedCar.localRotation = Quaternion.identity;
        PlayerCanvas.enabled = true;
        PlayerCamera.SetActive(true);
        TowingCarAnimated.SetActive(false);
        TowingCarCamera.SetActive(false);
        DamagedCarBodyCol.enabled = false;
        transform.position = TowingCarAnimated.transform.position;
        transform.rotation = TowingCarAnimated.transform.rotation;
        CarComponentsObj.cameraViewSetting.Angle = 15;
        CarComponentsObj.cameraViewSetting.height = 3;
        CarComponentsObj.cameraViewSetting.distance = 10;
        if (TowCarIndicator)
            TowCarIndicator.SetActive(false);
        yield return null;

    }
}
