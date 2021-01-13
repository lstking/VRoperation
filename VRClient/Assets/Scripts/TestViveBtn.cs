using UnityEngine;
using System.Collections;

public class TestViveBtn : MonoBehaviour
{
    public ViveHand touchHand;
    
    void Update()
    {
        var device = SteamVR_Controller.Input((int)touchHand.trackedObj.index);
        if (device.GetTouchDown(SteamVR_Controller.ButtonMask.Grip))
        {
            Debug.Log("Grip");
        }
    }
}
