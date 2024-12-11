using UnityEngine;
using System.Collections;

public class DestroyGameObject : MonoBehaviour
{

    public float clearDistance = 150.0f;
    public GameObject myRoot;
    public Renderer myBody;

    public bool human, vehicle;

    [SerializeField] float CurrentDist;
    [SerializeField] bool isVisible;
    void Update()
    {
        //CurrentDist = Vector3.Distance(transform.position, AIContoller.manager.player.transform.position);
        //isVisible=myBody.isVisible;
        if (!AIContoller.manager.player) return;

        if (Vector3.Distance(transform.position, AIContoller.manager.player.transform.position) > clearDistance
            /*&& !myBody.isVisible*/)
        {
            //DestroyVehicle();
            if (!destroyCalled)
            {
                destroyCalled = true;
                StartCoroutine(DestroyVehicle());
            }

        }

    }

    //float timerCount;

    //void DestroyVehicle()
    //{
    //    timerCount = Time.time + 1f;
    //    // display time

    //    if (timerCount >= 3)
    //    {
    //        Destroy(myRoot);

    //        if (human) AIContoller.manager.currentHumans--;
    //        if (vehicle) AIContoller.manager.currentVehicles--;
    //    }
    //}

    bool destroyCalled;
    IEnumerator DestroyVehicle()
    {
        yield return new WaitForSeconds(2f);
        if (Vector3.Distance(transform.position, AIContoller.manager.player.transform.position) > clearDistance)
        {
            Destroy(myRoot);

            if (human) AIContoller.manager.currentHumans--;
            if (vehicle) AIContoller.manager.currentVehicles--;
        }
        else
            destroyCalled = false;
        yield return null;
    }
}
