//----------------------------------------------
//            Realistic Car Controller
//
// Copyright © 2016 BoneCracker Games
// http://www.bonecrackergames.com
//
//----------------------------------------------

using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

/// <summary>
/// UI input (float) receiver from UI Button. 
/// </summary>
[AddComponentMenu("BoneCracker Games/Realistic Car Controller/UI/Mobile/Button")]
public class RCC_UIController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    // Getting an Instance of Main Shared RCC Settings.
    #region RCC Settings Instance

    private RCC_Settings RCCSettingsInstance;
    private RCC_Settings RCCSettings
    {
        get
        {
            if (RCCSettingsInstance == null)
            {
                RCCSettingsInstance = RCC_Settings.Instance;
            }
            return RCCSettingsInstance;
        }
    }

    #endregion

    internal float input;
    private float sensitivity { get { return RCCSettings.UIButtonSensitivity; } }
    private float gravity { get { return RCCSettings.UIButtonGravity; } }
    public bool pressing;

    Rigidbody carRigidbody;


    private void Start()
    {
        if (this.name.Equals("Gas"))
            carRigidbody = GameObject.FindWithTag("Player").GetComponentInParent<Rigidbody>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {

        pressing = true;

    }

    public void OnPointerUp(PointerEventData eventData)
    {

        pressing = false;

    }

    void OnPress(bool isPressed)
    {

        if (isPressed)
            pressing = true;
        else
            pressing = false;

    }

    void Update()
    {

        if (pressing)
        {

            input += Time.deltaTime * sensitivity;
        }
        else
        {
            if (this.name.Equals("Gas"))
            {
                if (GameManager.instance)
                {
                    if (!GameManager.instance.isTutorial)
                        carRigidbody.velocity = Vector3.Lerp(carRigidbody.velocity, Vector3.zero,  /*1.5f**/Time.deltaTime);
                }
            }

            input -= Time.deltaTime * gravity;
        }

        if (input < 0f)
            input = 0f;

        if (input > 1f)
            input = 1f;

    }

    void OnDisable()
    {

        input = 0f;
        pressing = false;

    }

    public void Accelerate(int force)
    {
        carRigidbody.velocity = Vector3.forward * force;
    }

}
