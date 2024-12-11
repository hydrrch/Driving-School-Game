using System.Collections;
using UnityEngine;

public class StuntCamEnabler : MonoBehaviour
{
    [SerializeField] Transform camRef;
    [SerializeField] VehicleCamera VehicleCameraObj;
    [SerializeField] float StuntWait;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Vehicle") && other.GetComponentInParent<VehicleControl>().speed >= 50)
        {
            //if (Mathf.Abs(other.attachedRigidbody.velocity.z) >= 0.1f)
            StartCoroutine(ShowStunt());
            other.GetComponentInParent<CarResetCheckPoint>().RotateCar();
        }

    }

    IEnumerator ShowStunt()
    {
        VehicleCamera.CamRef = camRef;
        VehicleCameraObj.isStunt = true;
        yield return new WaitForSeconds(StuntWait);
        VehicleCameraObj.isStunt = false;
        yield return null;
    }

    //IEnumerator
}
