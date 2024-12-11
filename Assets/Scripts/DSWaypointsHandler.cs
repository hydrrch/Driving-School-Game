using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DSWaypointsHandler : MonoBehaviour
{
    public bool fillHealthBar;

    GameObject currentWP;
    GameObject destArrow;

    AudioClip cpSfx;
    AudioSource cpSfxPlayer;



    void Start()
    {
        cpSfxPlayer = GameObject.FindWithTag("CpPlayer").GetComponent<AudioSource>();
        cpSfx = cpSfxPlayer.clip;

        if (transform.GetSiblingIndex() != transform.parent.childCount - 1)
            currentWP = transform.parent.GetChild(transform.GetSiblingIndex() + 1).gameObject;

        
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            if (currentWP != null)
                currentWP.SetActive(true);

            if (transform.GetSiblingIndex() == transform.parent.childCount - 3)
            {
                destArrow = transform.parent.GetChild(transform.GetSiblingIndex() + 2).gameObject;
                destArrow.SetActive(true);
                DirectionArrow.instance._target = destArrow.transform;
            }

            cpSfxPlayer.PlayOneShot(cpSfx);
            gameObject.SetActive(false);

            if (fillHealthBar)
                GameManager.instance.FillTheBar2();
                //GameManager.instance.FillTheBar();
        }

    }
}
