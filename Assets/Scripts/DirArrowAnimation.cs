using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirArrowAnimation : MonoBehaviour
{

    public Material[] mats;
    new Renderer renderer;
    public float wait;
    int mat;

    void Start()
    {
        renderer = GetComponent<Renderer>();
        StartCoroutine(Animate());
    }

    IEnumerator Animate()
    {
        while (true)
        {
            mat++;
            if (mat == 2)
                mat = 0;
            yield return new WaitForSeconds(wait);
            renderer.material = mats[mat];
        }
    }
}
