using UnityEngine;
using UnityEngine.EventSystems;

public class SkateboardDragController : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public float dragValue;
    public bool dragging;
    [SerializeField] Rigidbody RB;
    [SerializeField] float ForceToForward;
    [SerializeField] bool push;
    [SerializeField] SkateboardControllerCarType SkateboardControllerCarTypeObj;
    public void OnDrag(PointerEventData eventData)
    {
        dragging = true;
        dragValue = eventData.delta.y;

        if (dragValue >= 4f)
        {
            //RB.AddForce(RB.transform.forward * ForceToForward * Time.deltaTime);
            //RB.velocity += RB.transform.forward * ForceToForward * Time.deltaTime;
            //print(ForceToForward * Time.deltaTime);
            //print("Forced");
            SkateboardControllerCarTypeObj.Skate();
        }
    }
    private void Update()
    {
        if (/*dragValue > 2f*/push)
        {
            push = !push;
            RB.AddForce(RB.transform.forward * ForceToForward * Time.deltaTime, ForceMode.VelocityChange);
            print(ForceToForward * Time.deltaTime);
            print("Forced");
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        dragging = false;
        dragValue = 0f;
    }
}
