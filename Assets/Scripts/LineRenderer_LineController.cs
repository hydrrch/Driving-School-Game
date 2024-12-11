//using System.Collections;
//using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class LineRenderer_LineController : MonoBehaviour
{

    LineRenderer lr;
    public Transform[] points;
    //List<Vector3> points2;


    private void Awake()
    {
        lr = GetComponent<LineRenderer>();

    }

    private void Start()
    {
        GetPoints();
    }

    async void GetPoints()
    {
        var t = Time.time + 1;
        while (t > Time.time)
        {
            await Task.Yield();
        }
        points[0] = GameObject.FindObjectOfType<RCC_CarControllerV3>().transform;
        points[1] = GameObject.FindObjectOfType<DestinationTriggerScript>().transform.GetChild(0);

        SetUpLine(points);
    }

    public void SetUpLine(Transform[] points)
    {
        lr.positionCount = points.Length;
        this.points = points;
    }

    void SetPosition(Transform[] points)
    {
        for (int i = 0; i < points.Length; i++)
            lr.SetPosition(i, points[i].position);
    }

    //public void SetUpLineVectors(List<Vector3> points)
    //{
    //    lr.positionCount = points.Capacity;
    //    this.points2 = points;

    //    for (int i = 0; i < points.Capacity; i++)
    //    {
    //        lr.SetPosition(i, points[i]);
    //    }
    //}

    private void Update()
    {
        if (points[1] != null)
            SetPosition(points);
    }
}
