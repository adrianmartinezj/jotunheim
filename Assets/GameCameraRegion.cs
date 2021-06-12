using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCameraRegion : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        GameObject cameraObj = GameObject.FindGameObjectWithTag("MainCamera");
        var mgc = cameraObj.GetComponent<MixedGameCamera>();
        if (mgc != null)
        {
            mgc.SwitchToGameCamera();
        }
    }
}
