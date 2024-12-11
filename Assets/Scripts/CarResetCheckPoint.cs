using UnityEngine;
using System.Collections;

public class CarResetCheckPoint : MonoBehaviour
{
    [SerializeField] Transform CurrentCP;
    [SerializeField] AudioClip[] CollisionSounds;
    public GameObject[] Indicators;


    AIVehicle AIVehicleObj;
    AudioSource AudioSourceObj;
    Rigidbody rb;
    //Camera mainCam;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        //mainCam = Camera.main;
        AudioSourceObj = GetComponent<AudioSource>();
        AIVehicleObj = GetComponent<AIVehicle>();
    }

    #region Stunt Code
    public void RotateCar()
    {
        StartCoroutine(RotateRoutine(1));
    }


    IEnumerator RotateRoutine(float lerpTime)
    {
        //GameControl.AllowLerp = false;
        float angularDrag = 0;
        if (rb)
        {
            angularDrag = rb.angularDrag;
            rb.angularDrag = 100;
        }
        //if (mainCam)
        //    mainCam.fieldOfView = 80;
        float t = 0;
        Vector3 rot = transform.eulerAngles;

        while (t < 1)
        {
            t += Time.deltaTime / lerpTime;
            float zRot = Mathf.Lerp(0, 350, t);
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, zRot);

            yield return new WaitForEndOfFrame();
        }
        rb.angularDrag = angularDrag;
        //mainCam.fieldOfView = 60;
        //GameControl.AllowLerp = true;
        yield return null;
    }
    #endregion

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("resetcarcp"/*"Vehicle"*/))
        {
            CurrentCP = other.transform;
            FreeModeGM.instance.CarResetCheckPointInstance = this;
        }
    }

    public void ResetCar()
    {
        if (!CurrentCP) return;

        transform.position = new Vector3(CurrentCP.position.x, CurrentCP.position.y + 1, CurrentCP.position.z);
        transform.rotation = CurrentCP.rotation;

        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = new Vector3(0, 0, rb.velocity.z);
    }

    public void ResetCheckPoint()
    {
        CurrentCP = null;
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Vehicle") || collision.gameObject.CompareTag("Human"))
        {
            if (AIVehicleObj)
            {
                if (AIVehicleObj.vehicleStatus.Equals(VehicleStatus.Player))

                    if (!isAudioPlaying)
                        StartCoroutine(CrashSoundRoutine());

            }
        }
        //else
        //{
        //    if (!collision.gameObject.CompareTag("Human") && !collision.gameObject.name.Equals("Road")
        //        && !collision.gameObject.CompareTag("stunt") && !collision.gameObject.CompareTag("Player"))
        //    {
        //        if (AIVehicleObj)
        //            if (AIVehicleObj.vehicleStatus.Equals(VehicleStatus.Player))
        //            {
        //                if (!isAudioPlaying)
        //                    StartCoroutine(CrashSoundRoutine(1));
        //            }
        //    }
        //}
    }

    bool isAudioPlaying;
    
    IEnumerator CrashSoundRoutine()
    {

        isAudioPlaying = true;
        //AudioSourceObj.PlayOneShot(CollisionSounds[Random.Range(0, CollisionSounds.Length)]);
        FreeModeGM.instance.ShowDialogue();
        yield return new WaitForSeconds(3f);
        FreeModeGM.instance.HideDalogue();
        isAudioPlaying = false;
        yield return null;
    }

    public void Signal(bool status, int _num)
    {
        Indicators[_num].SetActive(status);
    }

    public void ClearCPs()
    {
        CurrentCP = null;
    }
}
