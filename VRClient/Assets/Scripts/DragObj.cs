using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(FixedJoint))]
public class DragObj : MonoBehaviour
{
    public bool canDrag = true;

    public bool isTransparentObjs = false;

    public List<TransparentObj> transparentObjs = new List<TransparentObj>();

    public bool disableColliderWhenDrag = true;

    public List<Collider> disableColliderList = new List<Collider>();

    private bool isDragging = false;

    private Transform initParent;
    private Vector3 initPos;
    private Vector3 initRot;

    private Transform trackingObj;
    
    private ViveHand touchHand;
    
    private void Awake()
    {
        GetComponent<Rigidbody>().useGravity = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (touchHand)
            return;

        if (isDragging)
            return;

        if (other.gameObject.layer == LayerMask.NameToLayer("Hand"))
            touchHand = other.GetComponent<ViveHand>();
        
        GetInitDate();
    }
    private void OnTriggerStay(Collider other)
    {
        if (touchHand)
        {
            var device = SteamVR_Controller.Input((int)touchHand.trackedObj.index);

            if (!isDragging && device.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger))
            {
                if (touchHand.hoverObj.gameObject != gameObject)
                    return;

                if (!gameObject.GetComponent<FixedJoint>())
                    touchHand.draggingObj = gameObject.AddComponent<FixedJoint>();
                else
                    touchHand.draggingObj = gameObject.GetComponent<FixedJoint>();
                gameObject.GetComponent<Rigidbody>().useGravity = false;
                touchHand.draggingObj.connectedBody = touchHand.GetComponent<Rigidbody>();

                StartDrag(touchHand.dragObjParent.transform);

                StartCoroutine(TransparentObjs(true));

                touchHand.isDragging = true;

            }
            else if (isDragging && device.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger))
            {
                StartCoroutine(TransparentObjs(false));

                if (touchHand)
                {
                    if (touchHand.draggingObj)
                    {
                        touchHand.draggingObj.connectedBody = null;
                        touchHand.draggingObj = null;
                    }
                    EndDrag();
                    touchHand.isDragging = false;
                    touchHand = null;
                }
                //Destroy(touchHand.draggingObj);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        //if (touchHand)
        //    touchHand = null;
    }

    private void GetInitDate()
    {
        initParent = transform.parent;
        initPos = transform.position;
        initRot = transform.rotation.eulerAngles;
    }

    public void StartDrag(Transform handTrack)
    {
        if (canDrag && !isDragging)
        {
            touchHand.SetViveHandRender(false);
            trackingObj = handTrack;
            transform.SetParent(handTrack);
            if (disableColliderWhenDrag)
                SetColliders(false);
            isDragging = true;
        }
    }

    public void EndDrag()
    {
        if (isDragging)
        {
            transform.SetParent(initParent);
            transform.position = initPos;
            transform.eulerAngles = initRot;
            if (disableColliderWhenDrag)
                SetColliders(true);
            if (touchHand)
                touchHand.SetViveHandRender(true);
            isDragging = false;
        }
    }
    
    private IEnumerator TransparentObjs(bool isTransparent)
    {
        yield return new WaitForSeconds(0.02f);

        if (isTransparentObjs)
        {
            foreach (TransparentObj t in transparentObjs)
            {
                t.SetTransparent(isTransparent);
            }
        }
    }

    private void SetColliders(bool _enable)
    {
        foreach (Collider c in disableColliderList)
        {
            c.enabled = _enable;
        }
    }
}
