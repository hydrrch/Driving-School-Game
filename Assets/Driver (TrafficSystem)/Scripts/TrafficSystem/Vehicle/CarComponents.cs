﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class CarComponents : MonoBehaviour
{

    public Transform handleTrigger;
    public Transform door;
    public Transform sitPoint;
    public Transform driver;

    public AudioClip[] deathSoundClips;

    public CameraViewSetting cameraViewSetting;

    [HideInInspector]
    public bool driving = true;

    [System.Serializable]
    public class CameraViewSetting
    {
        public List<Transform> cameraViews;

        public float distance = 5.0f;
        public float height = 1.0f;
        public float Angle = 20;
    }

    void Update()
    {

        if (!driver) return;

        if (driving)
        {
            driver.position = sitPoint.position;
            driver.rotation = sitPoint.rotation;
        }
        else
        {
            driver.position = handleTrigger.position;
            driver.rotation = handleTrigger.rotation;



            Component[] Rigidbodys = driver.GetComponentsInChildren(typeof(Rigidbody));

            

            foreach (Rigidbody rigidbody in Rigidbodys)
            {
                rigidbody.isKinematic = false;
            }

            Component[] Colliders = driver.GetComponentsInChildren(typeof(Collider));

            foreach (Collider collider in Colliders)
            {
                collider.enabled = true;
            }

            
            driver.GetComponent<AudioSource>().clip = deathSoundClips[Random.Range(0, deathSoundClips.Length)];
            driver.GetComponent<AudioSource>().Play();

            Destroy(driver.gameObject, 3.0f);
            driver.parent = null;
            driver = null;

        }
    }
}
