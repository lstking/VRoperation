using UnityEngine;
using System.Collections.Generic;

public class DragAnim : MonoBehaviour
{
    public Animation anim;

    public string animName;

    public float maxAngle;

    public List<Collider> enableCollider = new List<Collider>();

    public Transform head;

    private bool isTrigger = false;
    private bool isDragging = false;
    private bool isOpen = false;

    private float curAngle;

    private ViveHand touchHand;

    private Vector3 startPos;
    private Vector3 curPos;
    
	void Update()
    {
        if (isOpen)
            return;

        if (touchHand)
        {
            var device = SteamVR_Controller.Input((int)touchHand.trackedObj.index);

            if (device.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger))
            {
                isDragging = true;
                startPos = touchHand.transform.position - head.position;
                //startPos = touchHand.transform.position;
            }
            if (device.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger))
            {
                anim[animName].normalizedTime = 0f;
                anim[animName].speed = 0f;
                isDragging = false;
            }

            if(isDragging)
            {
                curPos = touchHand.transform.position - head.position;
                //curPos = touchHand.transform.position;
                float curAngle = GetAngle(startPos, curPos);

                if(curAngle > 0 && curAngle < maxAngle)
                {
                    float _percent = curAngle / 180f;
                    PlayAnim(_percent, 0);
                }
                else if(curAngle > maxAngle && curAngle < 180f)
                {
                    isOpen = true;
                    float _percent = curAngle / 90f;
                    PlayAnim(_percent, 1.0f);
                    GetComponent<Collider>().enabled = false;
                    foreach (Collider c in enableCollider)
                    {
                        c.enabled = true;
                    }
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Hand"))
        {
            touchHand = other.GetComponent<ViveHand>();
            isTrigger = true;
        }
    }
    
    private void PlayAnim(float _percent, float _speed)
    {
        anim.Play(animName);
        anim[animName].normalizedTime = _percent;
        anim[animName].speed = _speed;
    }

    /// <summary>
    /// 获取两个向量X轴与Z轴形成平面上的投影的夹角
    /// </summary>
    /// <param name="_start"></param>
    /// <param name="_end"></param>
    /// <returns></returns>
    private float GetAngle(Vector3 _start, Vector3 _end)
    {
        Vector3 start = new Vector3(_start.x, 0, _start.z);
        Vector3 end = new Vector3(_end.x, 0, _end.z);
        return Vector3.Angle(start, end);
    }
}
