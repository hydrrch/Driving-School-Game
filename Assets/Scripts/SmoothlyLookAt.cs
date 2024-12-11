using UnityEngine;

public class SmoothlyLookAt : MonoBehaviour
{

    public Transform Target;
    private void LateUpdate()
    {
        if (!Target) return;

        transform.LookAt(Target);
    }
}
