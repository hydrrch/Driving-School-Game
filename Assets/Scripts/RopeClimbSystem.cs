using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class RopeClimbSystem : MonoBehaviour
{


    [SerializeField] AudioSource WindSfx;
    [SerializeField] AudioSource FlyingSfx;
    [SerializeField] AudioSource RoofTopReachedSfx;
    [SerializeField] GameObject[] Canvases;
    [SerializeField] ParticleSystem WindEffect;
    [SerializeField] Image Crosshair;
    [SerializeField] Transform cam;
    [SerializeField] float TravelTime = 2;
    [SerializeField] LineRenderer LR;
    [SerializeField] Transform RopeStartPosition;
    [SerializeField] LayerMask Layer;
    [SerializeField] float RopeRaycastDistance;
    [SerializeField] Vector3 DestinationPoint;
    [SerializeField] Vector3 ActualPoint;
    [SerializeField] Vector3 offSet;
    [SerializeField] GameObject RopeThrowIndicator;


    float timetotravel = 0;
    bool travelNow;
    Vector3 startPosition;
    Rigidbody rb;
    new Collider collider;
    Animator animator;

    public static RopeClimbSystem instance;
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
        animator = GetComponent<Animator>();

        animator.Play("RoofClimb");
        //StartCoroutine(StartTheGame());
    }

    //IEnumerator StartTheGame()
    //{
    //    animator.Play("RoofClimb");
    //    yield return new WaitForSeconds(2.3f);
    //    Canvases[0].SetActive(true);
    //    //Canvases[1].SetActive(true);
    //}


    [SerializeField] Transform ClimbDetectionRayPosition;
    public static bool climbing;
    bool throwingRope;
    RaycastHit hit;
    bool climbedToRoofTop;


    void Update()
    {

        //animationCurve.AddKey(0.0f, 0.0f);
        //animationCurve.AddKey(1.0f, 1.0f);
        //LR.widthCurve = animationCurve;

        RaycastHit ClimbDetetionhit;
        //Debug.DrawRay(ClimbDetectionRayPosition.position, ClimbDetectionRayPosition.forward * 8, Color.green, 3);
        if (Physics.Raycast(ClimbDetectionRayPosition.position, ClimbDetectionRayPosition.forward, out ClimbDetetionhit, 8, Layer))
        {

            if (!climbedToRoofTop && rb.drag > 0 && !releasedClimbing)
                climbing = true;

        }
        else
        {
            if (climbing && !throwingRope)
            {
                climbing = false;
                StartCoroutine(ClimbToRoof());
            }
        }



        //Debug.DrawRay(cam.position, cam.forward* float.PositiveInfinity, Color.green, 3);
        if (Physics.Raycast(cam.position, cam.forward, out hit, /*float.PositiveInfinity*/RopeRaycastDistance, Layer))
        {
            if (hit.collider)
            {
                Crosshair.color = Color.green;
                RopeThrowBtn.enabled = true;

                if (!RopeThrowIndicator.activeInHierarchy)
                    RopeThrowIndicator.SetActive(true);
            }


            if (Input.GetKeyDown(KeyCode.K))
            {
                ActualPoint = hit.point;

                if (ActualPoint.x < 0)
                    offSet.x = -5;
                else
                    offSet.x = 5;

                if (ActualPoint.z < 0)
                    offSet.z = -5;
                else
                    offSet.z = 5;

                DestinationPoint = hit.point + offSet;
                transform.LookAt(DestinationPoint);
                StartCoroutine(ThrowRope());
            }
        }
        else
        {
            Crosshair.color = Color.red; 
            RopeThrowBtn.enabled = false;
            
            if (RopeThrowIndicator.activeInHierarchy)
                RopeThrowIndicator.SetActive(false);
        }

        if (travelNow)
        {
            timetotravel += Time.deltaTime / TravelTime;
            float magnitude = (transform.position - ActualPoint).magnitude;
            if (timetotravel < TravelTime/*magnitude > 2f*/)
            {
                transform.position = Vector3.Lerp(startPosition, ActualPoint, timetotravel);
                LR.SetPosition(0, RopeStartPosition.position);
                LR.SetPosition(1, ActualPoint);
            }
            else
            {
                travelNow = false;
                climbing = true;
                throwingRope = false;
                climbedToRoofTop = false;
                rb.drag = 5000;
                //collider.enabled = true;
                //rb.isKinematic = false;
                LR.SetPosition(0, Vector3.zero);
                LR.SetPosition(1, Vector3.zero);
                //StartCoroutine(StopTravel());
                animator.SetBool("Flying", false);
                animator.Play("StickToClimb");
                RopeThrowBtn.interactable = true;
                WindEffect.Stop();
                //WindSfx.Stop();
                ResetClimbPos(); 
            }

        }

    }


    private void ResetClimbPos()
    {
        RaycastHit ClimbDetetionhit;
        if (Physics.Raycast(ClimbDetectionRayPosition.position, ClimbDetectionRayPosition.forward, out ClimbDetetionhit, 8, Layer))
        {
            Vector3 _resetPos = ClimbDetetionhit.point;

            //print((transform.position - _resetPos).magnitude);
            float _distance = (transform.position - _resetPos).magnitude;
            if (_distance > 2f)
            {
                transform.position = _resetPos;
                //print("resettled position");
            }
        }
    }

    IEnumerator StopTravel()
    {
        travelNow = false;
        yield return new WaitForSeconds(0.1f);
        animator.SetBool("Flying", false);
        collider.enabled = true;
        //rb.isKinematic = false;
        LR.SetPosition(0, Vector3.zero);
        LR.SetPosition(1, Vector3.zero);
        rb.drag = 5000;
        //GetComponent<Rigidbody>().AddForce(Vector3.forward * force * Time.deltaTime, ForceMode.VelocityChange);
    }



    #region Manually Release Climbing

    bool releasedClimbing = true;

    public void ReleaseClimbing()
    {
        if (releasedClimbing)
            return;

        releasedClimbing = true;
        climbing = false;
        StartCoroutine(ClimbToRoof());
    }
    #endregion

    #region Climbing System
    [SerializeField] float forceToClimb;
    [SerializeField] ForceMode ForceModeType;
    IEnumerator ClimbToRoof()
    {
        //rb.drag = 0;
        RoofTopReachedSfx.Play();
        animator.SetTrigger("RoofClimb");
        float timeToClimb = 0;
        while (timeToClimb < 1f)
        {
            timeToClimb += Time.deltaTime;
            //rb.AddForce(transform.forward * forceToClimb * Time.deltaTime
            //    + transform.up * forceToClimb * 2 * Time.deltaTime, ForceModeType);
            transform.position += transform.forward * Time.deltaTime * 5 + transform.up * Time.deltaTime * 3;
            yield return new WaitForEndOfFrame();
        }

        //collider.enabled = true;
        climbedToRoofTop = true;
        animator.SetBool("WallClimbing", false);
        rb.drag = 0;
    }

    public static void ClimbAnimHandler(bool status)
    {
        if (status)
            instance.PlayClimbAnim();
        else
            instance.StopClimbAnim();
    }

    void PlayClimbAnim()
    {
        animator.Play("Climbing Up Wall");
        animator.SetBool("WallClimbing", true);
    }

    void StopClimbAnim()
    {
        animator.SetBool("WallClimbing", false);
    }
    #endregion

    #region Rope Throw System

    [SerializeField] AnimationClip ThrowAnimClip;
    [SerializeField] Button RopeThrowBtn;
    public Transform ropeAnimator;

    public void ThrowRopeFunc()
    {
        ActualPoint = hit.point;
        //if (ActualPoint.x < 0)
        //    offSet.x = -5;
        //else
        //    offSet.x = 5;
        //if (ActualPoint.z < 0)
        //    offSet.z = -5;
        //else
        //    offSet.z = 5;
        //DestinationPoint = hit.point + offSet;
        transform.LookAt(ActualPoint);
        StartCoroutine(ThrowRope());
    }
    IEnumerator ThrowRope()
    {
        throwingRope = true;
        RopeThrowBtn.interactable = false;
        animator.SetBool("Flying", true);
        yield return new WaitForSeconds(ThrowAnimClip.length / 2f);
        WindSfx.Play();


        float t = 0;
        while (t < 0.8f)
        {
            t += Time.deltaTime / 1;
            ropeAnimator.position = Vector3.Lerp(RopeStartPosition.position, ActualPoint, t);
            LR.SetPosition(0, RopeStartPosition.position);
            LR.SetPosition(1, ropeAnimator.position);
            
            yield return new WaitForEndOfFrame();
        }
        //yield return new WaitForSeconds(ThrowAnimClip.length / 2);

        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
        timetotravel = 0;
        travelNow = true;
        releasedClimbing = false;
        startPosition = transform.position;
        WindEffect.Play();
        FlyingSfx.Play();
        //collider.enabled = false;
        //rb.isKinematic = true;
    }
    #endregion


    //private void OnCollisionEnter(Collision collision)
    //{
    //    print("Colliding");
    //    if (collision.gameObject.layer == 13)
    //    {
    //        print("Colliding with building");
    //        travelNow = false;
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (!other.CompareTag("Player"))
    //    {
    //        print("exited from building");
    //    }
    //}


}
