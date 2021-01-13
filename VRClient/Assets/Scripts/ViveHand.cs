using UnityEngine;
using System.Collections.Generic;

public class ViveHand : MonoBehaviour
{
    public bool isDragging = false;

    public SteamVR_TrackedObject trackedObj;
    
    public GameObject dragObjParent;

    public FixedJoint draggingObj;

    public FixedJoint triggerObj;

    public HoverObjEffect hoverObj;

    private SteamVR_RenderModel renderModelControl;

    public List<MeshRenderer> ViveRenders
    {
        get { return new List<MeshRenderer>(renderModelControl.GetComponentsInChildren<MeshRenderer>()); }
    }
    private void Start()
    {
        renderModelControl = GetComponentInParent<SteamVR_RenderModel>();
    }

    public void SetViveHandRender(bool _enable)
    {
        foreach(MeshRenderer mr in ViveRenders)
        {
            mr.enabled = _enable;
        }
    }
}


