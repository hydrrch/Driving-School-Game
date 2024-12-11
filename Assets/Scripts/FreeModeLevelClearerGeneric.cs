using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FreeModeLevelClearerGeneric : MonoBehaviour
{

    [SerializeField] bool IsSpiralStunt;
    [SerializeField] float clearedWaitTime;
    [SerializeField] GameObject ClearedPanel;
    [SerializeField] GameObject CurrentMission;
    [SerializeField] Transform PositionResetter;
    [SerializeField] int reward;
    [SerializeField] GameObject Instructions;
    [SerializeField] string InstructionsMsg;

    GameObject[] winEffects;
    private void Start()
    {
        winEffects = GameObject.FindGameObjectsWithTag("winparticle");
        if (Instructions)
            StartCoroutine(InstructionsRoutine(InstructionsMsg));
    }

    IEnumerator InstructionsRoutine(string instructionsMsg)
    {
        yield return null;
        Instructions.SetActive(true);
        Instructions.GetComponent<Text>().text = instructionsMsg;
        yield return new WaitForSeconds(3f);
        Instructions.SetActive(false);
        Instructions.GetComponent<Text>().text = string.Empty;
        yield return null;
    }

    bool isCleared;
    private void OnTriggerEnter(Collider other)
    {
        if (!isCleared)
        {
            isCleared = true;
            for (int i = 0; i < winEffects.Length; i++)
            {
                winEffects[i].GetComponent<ParticleSystem>().Play();
            }

            //StartCoroutine(LevelClearedRoutine(clearedWaitTime));
            FreeModeGM.instance.LevelCleared();
        }

    }

    IEnumerator LevelClearedRoutine(float waitingTime)
    {
        yield return new WaitForSeconds(waitingTime);
        if (IsSpiralStunt)
        {
            if (PositionResetter)
            {
                Camera.main.GetComponent<VehicleCamera>().target.position = PositionResetter.position;
                Camera.main.GetComponent<VehicleCamera>().target.rotation = PositionResetter.rotation;
            }
        }
        yield return new WaitForSeconds(1f);

        isCleared = false;
        PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins") + reward);
        FreeModeGM.rewardValue = reward;
        if (FreeModeGM.instance)
        {
            FreeModeGM.instance.LevelCleared();
            FreeModeGM.instance.EnableMissions();
        }
        if (FreeModeGM.instance.FreeModeLevelManagerObj)
        {
            FreeModeGM.instance.FreeModeLevelManagerObj.EnableProps();
        }
        CurrentMission.SetActive(false);

        yield return null;
    }

    public void LevelCleared()
    {
        isCleared = false;
        PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins") + reward);
        FreeModeGM.rewardValue = reward;

        if (IsSpiralStunt)
        {
            if (PositionResetter)
            {
                Camera.main.GetComponent<VehicleCamera>().target.position = PositionResetter.position;
                Camera.main.GetComponent<VehicleCamera>().target.rotation = PositionResetter.rotation;
            }
        }

        if (FreeModeGM.instance)
        {
            //FreeModeGameManager.instance.LevelCleared();
            FreeModeGM.instance.EnableMissions();
        }
        if (FreeModeGM.instance.FreeModeLevelManagerObj)
        {
            FreeModeGM.instance.FreeModeLevelManagerObj.EnableProps();
        }
        CurrentMission.SetActive(false);
    }

}
