using System.Collections;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine;


public class FreeModeLM : MonoBehaviour
{

    //[Header("-----Fire Stunt-----")]
    [SerializeField] GameObject Level;
    [SerializeField] GameObject[] LevelObjectsToActivate;
    [SerializeField] GameObject[] LevelObjectsToDeactivate;
    [SerializeField] Transform PositionRef;
    [SerializeField] GameObject InfoPanel;
    [SerializeField] DOTweenAnimation InfoPanelAnim;
    [SerializeField] string Info;
    [SerializeField] string MissionHeading;
    [SerializeField] string rewardValue;
    [SerializeField] Text InfoHeadingText;
    [SerializeField] Text InfoText;
    [SerializeField] Text rewardValueText;
    [SerializeField] GameObject WarningPopUp;

    VehicleCamera VehicleCameraObj;
    //public static FreeModeLevelManager instance;
    //private void Awake()
    //{
    //    if (instance == null)
    //    {
    //        instance = this;
    //    }
    //    else
    //    {
    //        Destroy(instance);
    //    }
    //}

    private void Start()
    {
        VehicleCameraObj = FindObjectOfType<VehicleCamera>();
    }
    public bool rotateNow;
    private void Update()
    {
        if (rotateNow)
        {
            VehicleCameraObj.target.rotation = PositionRef.transform.rotation;
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        //if (!IsSkateBoarding)
        //{
            if (other.CompareTag("Vehicle"))
            {
                if (other.transform.root.TryGetComponent(out AIVehicle aIVehicle))
                {
                    if (aIVehicle.vehicleStatus == VehicleStatus.Player)
                    {
                        if (!GameControl.driving)
                        {
                            WarningPopUp.SetActive(true);
                            return;
                        }
                        if (other.GetComponentInParent<AIVehicle>().vehicleStatus == VehicleStatus.Player)
                        {
                            InfoPanel.SetActive(true);
                            InfoPanelAnim.DORestart();
                            InfoHeadingText.text = MissionHeading;
                            InfoText.text = Info;
                            rewardValueText.text = "$ " + rewardValue;

                            if (FreeModeGM.instance)
                            FreeModeGM.instance.FreeModeLevelManagerObj = this;
                        }
                    }
                }

            }
            else
            {
                if (other.CompareTag("Player"))
                {
                    if (!GameControl.driving && !IsSkateBoarding)
                    {
                        WarningPopUp.SetActive(true);
                        return;
                    }

                InfoPanel.SetActive(true);
                InfoPanelAnim.DORestart();
                InfoHeadingText.text = MissionHeading;
                InfoText.text = Info;
                rewardValueText.text = "$ " + rewardValue;

                if (FreeModeGM.instance)
                    FreeModeGM.instance.FreeModeLevelManagerObj = this;
            }
            }
        //}
        //else
        //{
        //    if (other.CompareTag("Player"))
        //    {

        //        InfoPanel.SetActive(true);
        //        InfoPanelAnim.DORestart();
        //        InfoHeadingText.text = MissionHeading;
        //        InfoText.text = Info;
        //        rewardValueText.text = "$ " + rewardValue;

        //        if (FreeModeGameManager.instance)
        //            FreeModeGameManager.instance.FreeModeLevelManagerObj = this;
        //    }
        //}
    }
    bool inRoutine;
    private void OnTriggerExit(Collider other)
    {
        if (inRoutine) return;

        if (InfoPanel.activeInHierarchy)
            StartCoroutine(CloseInfoPanel());
    }

    IEnumerator CloseInfoPanel()
    {
        inRoutine = true;
        yield return new WaitForSeconds(1f);
        inRoutine = false;
        InfoPanel.SetActive(false);
        InfoText.text = string.Empty;
        if (FreeModeGM.instance)
            FreeModeGM.instance.FreeModeLevelManagerObj = null;
        yield return null;
    }

    [SerializeField] bool IsSkateBoarding;
    public void StartLevel()
    {
        //if (!GameControl.driving)
        //{
        //    WarningPopUp.SetActive(true);
        //    return;
        //}
        //gameObject.SetActive(false);

        if (VehicleCameraObj.target)
        {
            if (VehicleCameraObj.target.TryGetComponent(out Rigidbody rigidbody))
            {
                rigidbody.velocity = Vector3.zero;
            }
        }

        if (!IsSkateBoarding)
        {
            if (PositionRef)
            {

                VehicleCameraObj.target.position = PositionRef.position;
                VehicleCameraObj.target.rotation = PositionRef.rotation;
            }

            Level.SetActive(true);

            if (LevelObjectsToActivate.Length > 0)
            {
                for (int i = 0; i < LevelObjectsToActivate.Length; i++)
                {
                    LevelObjectsToActivate[i].SetActive(true);
                }
            }

            if (LevelObjectsToDeactivate.Length > 0)
            {
                for (int i = 0; i < LevelObjectsToDeactivate.Length; i++)
                {
                    LevelObjectsToDeactivate[i].SetActive(false);
                }
            }

        }
        else
        {
            FreeModeGM.instance.LoadSkateboard();
        }

        FreeModeGM.rewardValue = int.Parse(rewardValue);
        
    }


    public void EnableProps()
    {
        if (LevelObjectsToDeactivate.Length > 0)
        {
            for (int i = 0; i < LevelObjectsToDeactivate.Length; i++)
            {
                LevelObjectsToDeactivate[i].SetActive(true);
            }
        }
    }
}
