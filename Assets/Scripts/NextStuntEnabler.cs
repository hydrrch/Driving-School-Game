using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextStuntEnabler : MonoBehaviour
{
    [SerializeField] GameObject NextStunt;
    [SerializeField] GameObject prevStunt;
    IEnumerator OnTriggerEnter(Collider other)
    {
        yield return null;
        yield return new WaitForSeconds(2f);
        NextStunt.SetActive(true);
        prevStunt.SetActive(false);
        yield return null;
    }
}
