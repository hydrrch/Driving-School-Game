using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiftAnimationHandler : MonoBehaviour
{

    public float animSpeed;
    new Animation animation;

    void OnEnable()
    {
        animation = GetComponent<Animation>();
        animation["giftbox"].speed = animSpeed;
    }

}
