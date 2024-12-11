using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
public enum ControlMode { simple = 1, touch = 2 }
public class GameControl : MonoBehaviour
{

    public ControlMode controlMode = ControlMode.simple;

    public GameObject getInVehicle;
    [SerializeField] ParticleSystem NitroEffect;
    [SerializeField] Camera CameraObj;
    [SerializeField] GameUI GameUIObj;
    [SerializeField] AudioSource BackgroundMusic;



    public static GameControl manager;

    public static float accelFwd, accelBack;
    public static float steerAmount;

    public static bool shift;
    public static bool brake;
    public static bool driving;
    public static bool jump;




    private VehicleCamera vehicleCamera;
    private float drivingTimer = 0.0f;
    public void VehicleAccelForward(float amount) { accelFwd = amount; }
    public void VehicleAccelBack(float amount) { accelBack = amount; }
    public void VehicleSteer(float amount) { steerAmount = amount; }
    public void VehicleHandBrake(bool HBrakeing) { brake = HBrakeing; }

    public bool lerpFieldOfView;
    public void VehicleShift(bool Shifting)
    {
        shift = Shifting;
        if (shift)
        {
            NitroEffect.Play();
            lerpFieldOfView = true;

            if (NosYahooSfx())
                YahooSfx.Play();
        }
        else
        {
            NitroEffect.Stop();
            lerpFieldOfView = false;
        }

    }

    int yahooSfxCounter;
    [SerializeField] AudioSource YahooSfx;
    bool NosYahooSfx()
    {
        yahooSfxCounter++;

        if (yahooSfxCounter == 4)
        {
            yahooSfxCounter = 0;
            return true;
        }
        else
        {
            return false;
        }
    }


    [SerializeField] GameObject[] TutorialThings;
    public void GetInVehicle()
    {
        if (drivingTimer == 0)
        {
            driving = true;
            drivingTimer = 3.0f;
            if (PlayerCamera.lerpRef != null)
            {
                PlayerCamera.IsLerp = true;
            }

            if (TutorialThings[0].activeInHierarchy)
            {
                for (int i = 0; i < TutorialThings.Length; i++)
                {
                    TutorialThings[i].SetActive(false);
                }
            }
            GameUIObj.ShowControls();
            BackgroundMusic.Stop();
        }
    }
    public void GetOutVehicle()
    {
        if (drivingTimer == 0)
        {
            driving = false;
            drivingTimer = 3.0f;
            PlayerCamera.IsLerp = false;
            GameUIObj.ShowControls();
            BackgroundMusic.Play();
        }
    }
    public void Jumping() { jump = true; }

    void Awake()
    {
        manager = this;
    }
    void Start()
    {
        vehicleCamera = AIContoller.manager.vehicleCamera;

        //#if UNITY_EDITOR
        //        controlMode = ControlMode.simple;
        //#else 
        // controlMode = ControlMode.touch;
        //#endif
    }
    public static bool AllowLerp;
    void Update()
    {
        drivingTimer = Mathf.MoveTowards(drivingTimer, 0.0f, Time.deltaTime);

        if (lerpFieldOfView)
        {
            if (CameraObj.fieldOfView < 70f /*&& AllowLerp*/)
                CameraObj.fieldOfView = Mathf.Lerp(CameraObj.fieldOfView, 71, 5 * Time.deltaTime);
        }
        else
        {
            if (CameraObj.fieldOfView > 60f /*&& AllowLerp*/)
                CameraObj.fieldOfView = Mathf.Lerp(CameraObj.fieldOfView, 60, 5 * Time.deltaTime);
        }
        //if (Input.GetKeyDown(KeyCode.K))
        //    StartCoroutine(LerpCam(65, 1));

#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.T))
            controlMode = ControlMode.touch;
        else if (Input.GetKeyDown(KeyCode.U))
            controlMode = ControlMode.simple;
#endif
    }
    public void CameraSwitch()
    {
        vehicleCamera.Switch++;
        if (vehicleCamera.Switch > vehicleCamera.cameraSwitchView.Count) { vehicleCamera.Switch = 0; }
    }

}
