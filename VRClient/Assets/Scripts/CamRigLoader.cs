using UnityEngine;
using System.Collections;

public class CamRigLoader : MonoBehaviour
{
    private void Start()
    {
        SteamVR_ControllerManager steamManager = FindObjectOfType<SteamVR_ControllerManager>();
        Vector3 _vrCamPos = steamManager.transform.position;
        FindObjectOfType<SteamVR_ControllerManager>().transform.position = CamRigDate.curCamRigPos;
    }
}
