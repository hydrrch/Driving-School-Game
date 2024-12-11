using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarCollisionDetector : MonoBehaviour
{

    AudioSource HitSfxPlayer;
    

    private void OnCollisionEnter(Collision collision)
    {
        Renderer _objRenderer = /*collision.gameObject.*/GetComponent<Renderer>();
        if (_objRenderer.material.color != Color.red)
            StartCoroutine(BlinkerRoutine(_objRenderer));
        
        HitSfxPlayer = GameObject.FindWithTag("HitSfxPlayer").GetComponent<AudioSource>();
        HitSfxPlayer.PlayOneShot(HitSfxPlayer.clip);

    }

    IEnumerator BlinkerRoutine(Renderer _renderer)
    {
        yield return null;

        var localTime = Time.time + 3.5f;
        while (localTime > Time.time)
        {
            if (_renderer.material.color == Color.red)
                _renderer.material.color = Color.white;
            else
                _renderer.material.color = Color.red;
            yield return new WaitForSeconds(0.5f);
            //    _renderer.material.color = Color.white;
            //yield return new WaitForSeconds(0.5f);
            //_renderer.material.color = Color.red;
            //yield return new WaitForSeconds(0.5f);
        }

    }
}
