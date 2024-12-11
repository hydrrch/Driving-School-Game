using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class CarShaker : MonoBehaviour
{
    public GameObject instructionsPanel;
    public Text instructionsText;
    public string instructions;
    public GameObject damageBar;
    public UnityEngine.UI.Image damageBarFiller;
    public float damageVal;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(ShowInstructions(other.gameObject));
        }
    }

    IEnumerator ShowInstructions(GameObject car)
    {
        car.GetComponentInParent<DOTweenAnimation>().DOPlay();
        yield return new WaitForSeconds(1.5f);
        instructionsPanel.SetActive(true);
        instructionsText.text = instructions.ToString();
        Time.timeScale = 0f;
        //instructionsText.DOText(instructions, 1, false);
        yield return new WaitForSecondsRealtime(5f);
        Time.timeScale = 1f;
        instructionsPanel.SetActive(false);
        GetComponent<Collider>().enabled = false;
        StartCoroutine(DamageBarRoutine());
        yield return null;
    }

    public IEnumerator DamageBarRoutine()
    {
        damageBar.SetActive(true);
        while (damageBarFiller.fillAmount > 0)
        {
            iTween.PunchScale(damageBar, iTween.Hash("x", 0.3f, "y", 0.3f, "z", 0.3f, "time", 1f,
                    "ease", iTween.EaseType.easeOutBounce));
            float decresingVal = damageBarFiller.fillAmount - damageVal;
            while (damageBarFiller.fillAmount > decresingVal)
            {
                yield return new WaitForEndOfFrame();
                damageBarFiller.fillAmount = Mathf.Lerp(damageBarFiller.fillAmount, damageBarFiller.fillAmount - damageVal, Time.deltaTime);
            }
            yield return new WaitForSeconds(10f);
        }
        yield return null;
    }
}
