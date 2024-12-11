using UnityEngine;

public class StuntCameraPositionHandler : MonoBehaviour
{
    [SerializeField] Vector3 Offset;
    [SerializeField] Transform PositionRef;
    //[SerializeField] Camera StuntCamera;
    //[SerializeField] Camera RccCamera;
    Transform thisTransform;
    private void Start()
    {
        thisTransform = transform;
    }

    void Update()
    {
        thisTransform.position = PositionRef.position + Offset;
        //StuntCamera.fieldOfView = RccCamera.fieldOfView;
        //thisTransform.rotation = PositionRef.rotation;
    }
}
