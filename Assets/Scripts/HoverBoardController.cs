using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverBoardController : MonoBehaviour
{

    [SerializeField] List<GameObject> Springs;
    [SerializeField] Rigidbody RB;
    [SerializeField] GameObject Prop;
    [SerializeField] GameObject CM;
    [SerializeField] float Speed;
    [SerializeField] float TurnSpeed;
    [SerializeField] Transform HoverBoard;

    float accelerate;
    float turn;
    bool brake;
    Transform thisTransform;
    void Start()
    {
        RB.centerOfMass = CM.transform.position;
        thisTransform = transform;

    }


    private void OnEnable()
    {
        RB.AddForceAtPosition(transform.TransformDirection(Vector3.up) * 150, Prop.transform.position);
    }

    void Update()
    {

        RB.AddForceAtPosition(Time.deltaTime * HoverBoard./*TransformDirection(Vector3.forward)*/forward *
            accelerate/*Input.GetAxis("Vertical")*/ * Speed, Prop.transform.position);

        //RB.AddTorque(Time.deltaTime * thisTransform.TransformDirection(Vector3.up)/*up*/ *
        //    /*turn*/Input.GetAxis("Horizontal") * TurnSpeed);

        thisTransform.eulerAngles += Time.deltaTime * TurnSpeed * turn/*Input.GetAxis("Horizontal")*/ * Vector3.up;

        //foreach (GameObject spring in Springs)
        //{
        RaycastHit hit;
        //Debug.DrawRay(spring.transform.position, -transform.up * 3f, Color.black, 2);
        if (Physics.Raycast(/*spring*/Prop.transform.position, -thisTransform.up/*TransformDirection(Vector3.down)*/, out hit, 2f))
        {
            float _force = Random.Range(2.0f, 5.0f);
            RB.AddForceAtPosition(Time.deltaTime * thisTransform.up/*TransformDirection(Vector3.up)*/
                * Mathf.Pow(_force - hit.distance, 2)
                / _force * 250f, Prop.transform.position);

            //RB.AddForceAtPosition(Time.deltaTime * transform.TransformDirection(Vector3.up) * Mathf.Pow(3f - hit.distance, 2)
            //        / 3f * 250f, spring.transform.position);

            //force = Mathf.Pow(3f - hit.distance, 2);

        }
        //}


        //RB.AddForce(-Time.deltaTime * transform.TransformVector(Vector3.right) *
        //    transform.InverseTransformVector(RB.velocity).x * 5f);

        if (brake)
        {
            RB.velocity = Vector3.Lerp(RB.velocity, Vector3.zero, Time.deltaTime*3);
        }
    }


    public void RacePressed()
    {
        accelerate = 1;
    }
    public void RaceReleased()
    {
        accelerate = 0;
    }

    public void BrakePressed()
    {
        brake = true;
    }
    public void BrakeReleased()
    {
        brake = false;
    }

    public void RightPressed()
    {
        turn = 1;
    }
    public void RightReleased()
    {
        turn = 0;
    }

    public void LeftPressed()
    {
        turn = -1;
    }
    public void LeftReleased()
    {
        turn = 0;
    }

}
