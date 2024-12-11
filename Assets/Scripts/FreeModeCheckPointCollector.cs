using DG.Tweening;
using System.Collections;
using UnityEngine;

public class FreeModeCheckPointCollector : MonoBehaviour
{

    [SerializeField] GameObject ClearedPanel;
    [SerializeField] ParticleSystem CoinBurstPS;
    [SerializeField] bool isAnimatedDisable;
    [SerializeField] int reward;
    [SerializeField] bool isLast;
    [SerializeField] bool isCoinBurst;

    GameObject[] winEffects;
    AudioSource AudioSourceObj;
    Animation animator;
    DOTweenAnimation DOTweenAnimationObj;
    public Transform parent;

    private void Start()
    {
        parent = transform.parent;
        AudioSourceObj = GetComponentInParent<AudioSource>();
        animator = GetComponent<Animation>();
        DOTweenAnimationObj = GetComponent<DOTweenAnimation>();
        winEffects = GameObject.FindGameObjectsWithTag("winparticle");

    }

    private void OnEnable()
    {

        if (isAnimatedDisable && transform.localScale == Vector3.zero)
            transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
    }
    //[SerializeField] float dist;
    //[SerializeField] Transform car;
    //private void Update()
    //{
    //    dist = (transform.position - car.position).magnitude;
    //    if(dist<6f)
    //        animator.Play("check point asad disable anim");
    //}

    private void OnTriggerEnter(Collider other)
    {

        #region Old Code
        //if (other.CompareTag("Vehicle"))
        //{
        //    if (other.transform.root.TryGetComponent(out AIVehicle aIVehicle))
        //    {
        //        if (aIVehicle.vehicleStatus == VehicleStatus.Player)
        //        {
        //            if (parent.childCount > 2)
        //            {
        //                parent.GetChild(1).gameObject.SetActive(true);
        //                AudioSourceObj.PlayOneShot(AudioSourceObj.clip);
        //            }
        //            else
        //            {

        //                FreeModeGameManager.instance.LevelCleared();
        //                PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins") + reward);
        //                FreeModeGameManager.instance.EnableMissions();
        //                AudioSourceObj.PlayOneShot(AudioSourceObj.clip);
        //                if (winEffects[0])
        //                {

        //                    for (int i = 0; i < winEffects.Length; i++)
        //                    {
        //                        winEffects[i].GetComponent<ParticleSystem>().Play();
        //                    }
        //                }
        //            }
        //            Destroy(this.gameObject);
        //        }
        //    }
        //}
        //else if (other.CompareTag("Player"))
        //{
        //    if (parent.childCount > 1)
        //    {
        //        parent.GetChild(1).gameObject.SetActive(true);
        //        AudioSourceObj.PlayOneShot(AudioSourceObj.clip);
        //    }
        //    else
        //    {

        //        FreeModeGameManager.instance.LevelCleared();
        //        PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins") + reward);
        //        FreeModeGameManager.instance.EnableMissions();
        //        AudioSourceObj.PlayOneShot(AudioSourceObj.clip);
        //        if (winEffects[0])
        //        {

        //            for (int i = 0; i < winEffects.Length; i++)
        //            {
        //                winEffects[i].GetComponent<ParticleSystem>().Play();
        //            }
        //        }
        //    }
        //    Destroy(this.gameObject);
        //}
        #endregion


        if (other.CompareTag("Vehicle"))
        {
            if (other.transform.root.TryGetComponent(out AIVehicle aIVehicle))
            {
                if (aIVehicle.vehicleStatus == VehicleStatus.Player)
                {
                    //if (parent.childCount > 2)
                    if (!isLast)
                    {

                        //parent.GetChild(1).gameObject.SetActive(true);
                        parent.GetChild(transform.GetSiblingIndex() + 1).gameObject.SetActive(true);
                        AudioSourceObj.PlayOneShot(AudioSourceObj.clip);
                        if (isAnimatedDisable)
                            StartCoroutine(Disable());
                        else
                            gameObject.SetActive(false);
                    }
                    else
                    {

                        FreeModeGM.instance.LevelCleared();
                        PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins") + reward);
                        FreeModeGM.instance.EnableMissions();
                        AudioSourceObj.PlayOneShot(AudioSourceObj.clip);
                        if (winEffects[0])
                        {

                            for (int i = 0; i < winEffects.Length; i++)
                            {
                                winEffects[i].GetComponent<ParticleSystem>().Play();
                            }
                        }
                        gameObject.SetActive(false);
                        parent.gameObject.SetActive(false);
                        //if (isAnimatedDisable)
                        //    StartCoroutine(Disable());
                        //else
                        //    gameObject.SetActive(false);
                        
                    }

                    if (isCoinBurst)
                        CoinBurstPS.Play();
                }
            }
        }
        else if (other.CompareTag("Player"))
        {
            if (!isLast)
            {

                //parent.GetChild(1).gameObject.SetActive(true);
                parent.GetChild(transform.GetSiblingIndex() + 1).gameObject.SetActive(true);
                AudioSourceObj.PlayOneShot(AudioSourceObj.clip);
                if (isAnimatedDisable)
                    StartCoroutine(Disable());
                else
                    gameObject.SetActive(false);
            }
            else
            {

                FreeModeGM.instance.LevelCleared();
                PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins") + reward);
                FreeModeGM.instance.EnableMissions();
                AudioSourceObj.PlayOneShot(AudioSourceObj.clip);
                if (winEffects[0])
                {

                    for (int i = 0; i < winEffects.Length; i++)
                    {
                        winEffects[i].GetComponent<ParticleSystem>().Play();
                    }
                }
                gameObject.SetActive(false);
                parent.gameObject.SetActive(false);
                //if (isAnimatedDisable)
                //    StartCoroutine(Disable());
                //else
                //    gameObject.SetActive(false);
                
            }

            if (isCoinBurst)
                CoinBurstPS.Play();

        }

    }

    IEnumerator Disable()
    {
        //animator.Play("check point asad disable anim");
        DOTweenAnimationObj.DORestart();
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
    }

}
