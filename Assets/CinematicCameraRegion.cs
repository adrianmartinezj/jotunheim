using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicCameraRegion : MonoBehaviour
{
    [SerializeField]
    private BezierSpline spline;
    private void Start()
    {
        Debug.Assert(spline != null, name + " must have a spline attached.");
    }
    private void OnTriggerEnter(Collider other)
    {
        GameObject cameraObj = GameObject.FindGameObjectWithTag("MainCamera");
        var mgc = cameraObj.GetComponent<MixedGameCamera>();
        if (mgc != null)
        {
            mgc.SwitchToCinematicCamera(spline);
        }
    }

}
