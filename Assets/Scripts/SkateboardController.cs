using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkateboardController : MonoBehaviour
{

    [SerializeField] bool Go;
    //[SerializeField] float SkateSpeed;
    [SerializeField] Animator PlayerAnimator;
    //[SerializeField] AnimationClip SkateClipGeneric;
    //[SerializeField] int Turns;
    [SerializeField] bool isVelocity;
    [SerializeField] float DefaultForce = 15;
    [SerializeField] RCC_CarControllerV3 RCC_CarControllerV3Obj;
    [SerializeField] float SkateBoardSpeed;

    new Rigidbody rigidbody;
    Transform thisTransform;


    private void Start()
    {
        thisTransform = transform;
        //speedRoutineRef = FloatSpeedRoutine();
        rigidbody = GetComponent<Rigidbody>();

    }

    float timeToResetVelocity;
    private void FixedUpdate()
    {
        //if (Input.GetKeyDown(KeyCode.S))
        //    StartCoroutine(PlayGenericAnims());

        //if (Input.GetKeyDown(KeyCode.Backspace))
        //    StopCoroutine(speedRoutineRef);


        if (Go)
        {
            thisTransform.position += thisTransform.forward * SkateBoardSpeed * Time.deltaTime;
            //rigidbody.velocity = thisTransform.forward.normalized * DefaultForce;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
            thisTransform.eulerAngles += thisTransform.right * 5;

        if (hopped)
        {
            if (timeToResetVelocity < Time.time)
            {
                if (DefaultForce > 5.05f)
                {
                    DefaultForce /*= Mathf.Lerp(DefaultForce, 5, Time.deltaTime)*/-= Time.deltaTime;
                }
                else hopped = false;
            }
        }

        //while (/*SkateSpeed > 1*/t < 1)
        //{
        //    t += Time.deltaTime / LerpTime;
        //    SkateSpeed = Mathf.Lerp(lerpTarget, /*_defaultSpeed*/10, t);
        //    yield return new WaitForEndOfFrame();
        //}
    }

    //public void Hop()
    //{
    //    StartCoroutine(PlayGenericAnims());
    //}

    //bool firstHop;
    bool hopped;
    public void HopVelocity()
    {
        //if (firstHop)
        //{
        if (!Go) Go = true;

        DefaultForce += 5;

        if (DefaultForce >= 20f)
            DefaultForce = 20f;
        //else
        //{
        hopped = true;
        timeToResetVelocity = Time.time + 5;
        StartCoroutine(PlayHopAnimation());
        //}
        //}
        //else
        //    firstHop = true;


        //if (isVelocity)
        //    rigidbody.velocity = transform.right.normalized * DefaultForce;
        //else
        //    rigidbody.AddForce(transform.right.normalized * DefaultForce /** Time.deltaTime*/, ForceMode.Acceleration);
        //magnitude = rigidbody.velocity.normalized.magnitude;
    }

    public void HopWithRCC()
    {
        RCC_CarControllerV3Obj.speed = SkateBoardSpeed;
    }

    IEnumerator PlayHopAnimation()
    {
        PlayerAnimator.SetBool("hop", true);
        yield return new WaitForSeconds(0.5f);
        PlayerAnimator.SetBool("hop", false);
    }


    //IEnumerator AddVelocity()
    //{
    //    float _defaultForce = DefaultForce;
    //    DefaultForce += DefaultForce;
    //    if (DefaultForce > DefaultForce * 2)
    //        DefaultForce -= DefaultForce;

    //}



    #region Static Position Translation
    //IEnumerator speedRoutineRef;
    //IEnumerator PlayGenericAnims()
    //{
    //    yield return null;
    //    speedRoutineRef = FloatSpeedRoutine();
    //    if (!Go)
    //    {
    //        PlayerAnimator.SetBool("hop", true);
    //        yield return new WaitForSeconds(0.5f);
    //        PlayerAnimator.SetBool("hop", false);
    //        Go = true;
    //    }
    //    else
    //    {
    //        PlayerAnimator.SetBool("hop", true);
    //        StopCoroutine(speedRoutineRef);
    //        yield return new WaitForSeconds(SkateClipGeneric.length / 2);
    //        StartCoroutine(speedRoutineRef);
    //        PlayerAnimator.SetBool("hop", false);
    //    }

    //    yield return null;
    //}

    ///// <summary>
    ///// make float speed scenario in update func
    ///// </summary>
    //public float t;
    //public float LerpTime;
    //float lerpTarget;
    //IEnumerator FloatSpeedRoutine()
    //{
    //    yield return null;
    //    //float _defaultSpeed = 10;
    //    SkateSpeed = SkateSpeed + 5;
    //    lerpTarget = SkateSpeed;
    //    t = 0;
    //    //yield return new WaitForSeconds(1f);
    //    while (/*SkateSpeed > 1*/t < 1)
    //    {
    //        t += Time.deltaTime / LerpTime;
    //        SkateSpeed = Mathf.Lerp(lerpTarget, /*_defaultSpeed*/10, t);
    //        yield return new WaitForEndOfFrame();
    //    }
    //    yield return null;

    //}
    #endregion
}
