using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCustomAnimationsPlayer : MonoBehaviour
{

    [SerializeField] ParticleSystem kickEffect;
    [SerializeField] ParticleSystem punchEffect;
    [SerializeField] ParticleSystem comboLeftEffect;
    [SerializeField] ParticleSystem jumpEffect;
    [SerializeField] AudioClip PunchSfx;
    [SerializeField] AudioClip ComboPunchSfx;
    [SerializeField] AudioClip KickSfx;
    [SerializeField] AudioClip JumpSfx;
    [SerializeField] AudioClip ComeOutSfx;


    Animator AnimatorObj;
    AudioSource AudioSourceObj;
    bool inAction;

    private void Start()
    {
        AnimatorObj = GetComponent<Animator>();
        AudioSourceObj = GetComponent<AudioSource>();
    }
    public void Hook(string trigger)
    {
        if (inAction) return;

        AnimatorObj.SetTrigger(trigger);
    }

    public void SetActionState()
    {
        inAction = !inAction;
    }

    public void PlayKickEffect()
    {
        kickEffect.Play();
        AudioSourceObj.PlayOneShot(KickSfx);
    }

    public void PlayPunchEffect()
    {
        punchEffect.Play();
        AudioSourceObj.PlayOneShot(PunchSfx);
    }

    public void PlayComboLeftEffect()
    {
        comboLeftEffect.Play();
        AudioSourceObj.PlayOneShot(ComboPunchSfx);
    }

    public void PlayComboRightEffect()
    {
        punchEffect.Play();
        AudioSourceObj.PlayOneShot(ComboPunchSfx);
    }

    public void JumpEffect()
    {
        jumpEffect.Play();
        AudioSourceObj.PlayOneShot(JumpSfx);
    }

    [SerializeField] Transform destination;
    [SerializeField] bool go;
    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Home))
        //{
        if (go)
            transform.position = Vector3.Lerp(transform.position, destination.position, 0.5f);
        //}
    }

    public void ComeOutSFX()
    {
        AudioSourceObj.PlayOneShot(ComeOutSfx);
    }
}
