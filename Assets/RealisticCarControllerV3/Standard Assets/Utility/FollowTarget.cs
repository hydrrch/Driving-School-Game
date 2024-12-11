using System;
using UnityEngine;


namespace UnityStandardAssets.Utility
{
    public class FollowTarget : MonoBehaviour
    {
        public Transform target;
        public Vector3 offset = new Vector3(0f, 7.5f, 0f);
        public bool tiger;


        private void LateUpdate()
        {
            transform.position = target.position + offset;
            if (tiger)
            {
                transform.LookAt(target);
            }
        }
    }
}
