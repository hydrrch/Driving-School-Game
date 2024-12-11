using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

public class WPSystem : MonoBehaviour
{

    public GameObject[] Waypoints;
    public int num = 0;

    public float minDist;
    public float Speed;
    public float dist;

    public bool rand = false;
    public bool go = true;
    public bool sizeUp;
    public bool isDisable;
    public bool isAnimate;
    public AnimationClip anger;

    public bool halt;


    new Animation animation;
    bool sizeUpCalled;

    void Start()
    {
        if (isAnimate)
        {
            iTween.ScaleTo(this.gameObject, iTween.Hash("scale", new Vector3(0.6f, 0.6f, 0.6f),
                   "time", 0.5f, "easetype", iTween.EaseType.linear, "looptype", iTween.LoopType.pingPong));
        }

        animation = GetComponent<Animation>();
    }


    void Update()
    {
        dist = Vector3.Distance(transform.position, Waypoints[num].transform.position);

        if (num == 1 && (dist < 4.1f && dist > 4f) && !sizeUpCalled)
        {
            StartCoroutine(FaceUpAnger());
        }

        if (go)
        {
            if (dist > minDist)
            {
                Move();
            }
            else
            {
             
                if (halt && !animation.IsPlaying("Talking"))
                {
                    animation.CrossFade("Talking", 0.4f);
                }
                //else
                //{
                //    if (!rand)
                //    {
                //        if (num + 1 == Waypoints.Length)
                //        {
                //            num = 0;
                //        }
                //        else
                //        {
                //            num++;

                //        }
                //    }
                //    else
                //    {
                //        num = Random.Range(0, Waypoints.Length);
                //    }
                //}

            }
        }

        if (isDisable)
        {
            if (dist < minDist)
                gameObject.SetActive(false);
        }
    }

    void Move()
    {
        transform.LookAt(Waypoints[num].transform.position);
        transform.position += transform.forward * Speed * Time.deltaTime;
    }

    IEnumerator FaceUpAnger()
    {
        sizeUpCalled = true;
        go = false;
        GetComponentInChildren<Animation>().Play("idle");
        Transform baba = transform.GetChild(0);
        iTween.ScaleTo(baba.gameObject, new Vector3(baba.transform.localScale.x + 0.5f, baba.transform.localScale.y + 0.5f,
            baba.transform.localScale.z + 0.5f), 0.5f);
        yield return new WaitForSeconds(0.6f);

        GetComponentInChildren<Animation>().Play(anger.name);
        yield return new WaitForSeconds(anger.length + 1f);

        GetComponentInChildren<Animation>().Play("idle");
        iTween.ScaleTo(baba.gameObject, new Vector3(baba.transform.localScale.x - 0.5f, baba.transform.localScale.y - 0.5f,
            baba.transform.localScale.z - 0.5f), 0.5f);
        yield return new WaitForSeconds(0.6f);

        go = true;
        GetComponentInChildren<Animation>().Play("walk");
        yield return new WaitForSeconds(1f);
        sizeUpCalled = false;
        yield return null;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            gameObject.SetActive(false);
        }
    }

}
