using DG.Tweening;
using UnityEngine;

public class DannyCustomization : MonoBehaviour
{

    public bool IsGameplay;

    [SerializeField] GameObject cap;
    [SerializeField] GameObject glasses;
    [SerializeField] GameObject bag;
    [SerializeField] Renderer[] clothMesh;
    [SerializeField] Material[] clothesMats;




    void Start()
    {
        if (IsGameplay)
            ApplyChanges();
    }

    void ApplyChanges()
    {

        if (PlayerPrefs.GetInt("cap") == 1)
            cap.SetActive(true);
        else
            cap.SetActive(false);

        if (PlayerPrefs.GetInt("bag") == 1)
            bag.SetActive(true);
        else
            bag.SetActive(false);

        if (PlayerPrefs.GetInt("glasses") == 1)
            glasses.SetActive(true);
        else
            glasses.SetActive(false);

        clothMesh[0].material = clothesMats[PlayerPrefs.GetInt("clothes")];
        clothMesh[1].material = clothesMats[PlayerPrefs.GetInt("clothes")];

     
    }

    public void Cap()
    {
        if (cap.activeInHierarchy)
        {
            cap.SetActive(false);
            PlayerPrefs.SetInt("cap", 0);
        }
        else
        {
            cap.SetActive(true);
            PlayerPrefs.SetInt("cap", 1);
        }

        PlayAnim(-40);
    }

    public void Bag()
    {
        if (bag.activeInHierarchy)
        {
            bag.SetActive(false);
            PlayerPrefs.SetInt("bag", 0);
        }
        else
        {
            bag.SetActive(true);
            PlayerPrefs.SetInt("bag", 1);
        }

        //PlayAnim("1");
        PlayAnim(120);
    }

    public void Glasses()
    {
        if (glasses.activeInHierarchy)
        {
            glasses.SetActive(false);
            PlayerPrefs.SetInt("glasses", 0);
        }
        else
        {
            glasses.SetActive(true);
            PlayerPrefs.SetInt("glasses", 1);
        }

        PlayAnim(-40);
    }


    int clothNum;
    public void ChangeClothes()
    {
        clothNum++;
        if (clothNum == clothesMats.Length)
            clothNum = 0;

        clothMesh[0].material = clothesMats[clothNum];
        clothMesh[1].material = clothesMats[clothNum];
        PlayerPrefs.SetInt("clothes", clothNum);

        PlayAnim(-40);
    }

    void PlayAnim(int val) => iTween.RotateTo(gameObject, iTween.Hash("y", val, "time", 1, "easetype", Ease.OutBack));
}
