using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trackPosition : MonoBehaviour
{
    private GameObject tracker;
    private Material grassMat;

    void Start()
    {
        grassMat = GetComponent<Renderer>().material;
        tracker = GameObject.Find("tracker");
    }

    void Update()
    {
        Vector3 trackerPos = tracker.GetComponent<Transform>().position;
        grassMat.SetVector("_trackerPosition", trackerPos);
    }
}