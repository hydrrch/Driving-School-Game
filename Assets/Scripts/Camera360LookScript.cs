using UnityEngine.EventSystems;
using UnityEngine;

public class Camera360LookScript : MonoBehaviour, IDragHandler, IEndDragHandler
{
    [SerializeField] float HoriValue;
    [SerializeField] float VertiValue;
    [SerializeField] GameObject TutorialImage;
    public float HorizontalVal => HoriValue;
    public float VerticalVal => VertiValue;

    public void OnDrag(PointerEventData eventData)
    {
        HoriValue = eventData.delta.x;
        VertiValue = eventData.delta.y;

        if (TutorialImage.activeInHierarchy)
            TutorialImage.SetActive(false);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        VertiValue = 0;
        HoriValue = 0;
    }
}
