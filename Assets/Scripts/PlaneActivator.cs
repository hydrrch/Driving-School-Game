using UnityEngine;
using DG.Tweening;
using System.Collections;
public class PlaneActivator : MonoBehaviour
{

    [SerializeField] GameObject Plane;
    [SerializeField] DOTweenAnimation Animation;
    [SerializeField] GameObject Plane2;
    [SerializeField] DOTweenAnimation Animation2;


    bool isAnimatePlane = true;
    private void OnTriggerEnter(Collider other)
    {
        if (!this.isAnimatePlane) return;

        if (other.CompareTag("Player"))
        {
            Plane.SetActive(true);
            Animation.DORestart();
            Plane2.SetActive(true);
            Animation2.DORestart();
            StartCoroutine(ResetTime());
        }

        if (other.CompareTag("Vehicle"))
        {
            if (other.transform.root.TryGetComponent(out AIVehicle aIVehicle))
            {
                if (aIVehicle.vehicleStatus == VehicleStatus.Player)
                {
                    Plane.SetActive(true);
                    Animation.DORestart();
                    Plane2.SetActive(true);
                    Animation2.DORestart();
                    StartCoroutine(ResetTime());
                }
            }
        }
    }

    IEnumerator ResetTime()
    {
        this.isAnimatePlane = false;
        yield return new WaitForSeconds(30);
        this.isAnimatePlane = true;
    }
}
