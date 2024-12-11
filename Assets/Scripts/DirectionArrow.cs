using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionArrow : MonoBehaviour
{
    public Transform _target;
    public float speed;
    public bool isTimeBased;
    public static DirectionArrow instance = null;

    //Quaternion targetRotation;

    private void Start()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

    }

    private void FixedUpdate()
    {
        if (_target != null)
        {
            var targetRotation = Quaternion.LookRotation(_target.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, speed * Time.deltaTime);
        }

    }
}
