using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public enum TriggerType
{
    Animation,
    Animator,
    Color,
    Hide,
    LoadScene,
    Transparent,
    Collider,
    DisableMeshRender,
    All
}

public class TriggerObj : MonoBehaviour
{
    public TriggerType triggerType;

    public Animation anim;

    public bool isSingleAnim;
    public bool isAnimPingPong;

    public string forwardAnimName;
    public string backwardAnimName;

    public float animSpeed = 1.0f;

    public Animator animator;

    public List<GameObject> hideObjs = new List<GameObject>();

    public List<Collider> enableColliders = new List<Collider>();

    public string sceneName;

    public bool isTransparentReversible = false;

    public List<TransparentObj> transparentObjs = new List<TransparentObj>();

    public bool disableSelfCollider = true;

    public bool isColliderEnable = false;

    public bool isColliderChangeReversible = false;
    
    public List<MeshRenderer> renders = new List<MeshRenderer>();

    private ViveHand touchHand;

    #region   ***   Current State   ***

    private bool isTrigger = false;

    private bool isAnimTrigger = false;

    private bool isAnimPlaying = false;

    private bool isTransparent = false;

    private bool isHide = false;

    private bool isRenderEnable = true;

    #endregion

    private void Start()
    {
        if (!anim)
            anim = GetComponent<Animation>();

        if (!animator)
            animator = GetComponent<Animator>();
    }

    private void updata()
    {
        if(tempdata.istriggered == true)
        {
            StartCoroutine(HideObjs());
            tempdata.istriggered = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (touchHand)
            return;
        if (other.gameObject.layer == LayerMask.NameToLayer("Hand"))
            touchHand = other.GetComponent<ViveHand>();
    }
    private void OnTriggerStay(Collider other)
    {
        if (isTrigger)
            return;

        if (!touchHand || touchHand.isDragging)
            return;

        var device = SteamVR_Controller.Input((int)touchHand.trackedObj.index);

        bool isTriggerTouchDown = device.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger);
        bool isTriggerTouchUp = device.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger);

        float enableColliderDelay = 0;

        #region   ***   AnimationType   ***
        if ((triggerType == TriggerType.Animation || triggerType == TriggerType.All) && anim)
        {
            if (!isAnimPlaying && isTriggerTouchDown)
            {
                if (isSingleAnim)
                {
                    if (isAnimPingPong)
                    {
                        if (isAnimTrigger)
                        {
                            StartCoroutine(PlayAnim(forwardAnimName, -1f * animSpeed));
                        }
                        else
                        {
                            StartCoroutine(PlayAnim(forwardAnimName, animSpeed));
                        }
                        isAnimTrigger = !isAnimTrigger;
                    }
                    else
                    {
                        StartCoroutine(PlayAnim(forwardAnimName, animSpeed));
                    }
                    enableColliderDelay = anim[forwardAnimName].length;
                }
                else
                {
                    if (isAnimTrigger)
                    {
                        StartCoroutine(PlayAnim(backwardAnimName, animSpeed));
                        enableColliderDelay = anim[backwardAnimName].length;
                    }
                    else
                    {
                        StartCoroutine(PlayAnim(forwardAnimName, animSpeed));
                        enableColliderDelay = anim[forwardAnimName].length;
                    }
                    isAnimTrigger = !isAnimTrigger;
                }
            }

            if (isTriggerTouchUp)
            {

            }
        }
        #endregion
        #region   ***   HideType   ***
        if ((triggerType == TriggerType.Hide || triggerType == TriggerType.All) && hideObjs.Count > 0)
        {
            if (isTriggerTouchDown)
            {
                StartCoroutine(HideObjs());
                isTrigger = true;
            }
        }
        #endregion
        #region   ***   LoadScene   ***
        if ((triggerType == TriggerType.LoadScene || triggerType == TriggerType.All)
            && !string.IsNullOrEmpty(sceneName))
        {
            if (isTriggerTouchDown)
            {
                if (anim)
                {
                    StartCoroutine(LoadScene(anim[forwardAnimName].length));
                }
                else
                {
                    CamRigDate.curCamRigPos = FindObjectOfType<SteamVR_ControllerManager>().transform.position;
                    SceneManager.LoadSceneAsync(sceneName);
                }
                isTrigger = true;
            }
        }
        #endregion
        #region   ***   Transparent   ***
        if ((triggerType == TriggerType.Transparent || triggerType == TriggerType.All) && transparentObjs.Count > 0)
        {
            if (isTriggerTouchDown)
            {
                foreach (TransparentObj t in transparentObjs)
                {
                    if (isTransparentReversible)
                    {
                        t.SetTransparent(!isTransparent);
                        isTransparent = !isTransparent;
                    }
                    else
                    {
                        t.SetTransparent(true);
                    }
                }
                isTrigger = true;
            }
        }
        #endregion
        #region   ***   AnimatorType   ***
        if ((triggerType == TriggerType.Animator || triggerType == TriggerType.All) && animator)
        {
            if (isTriggerTouchDown)
            {
                isTrigger = true;
            }
            if (isTriggerTouchUp)
            {

            }
        }
        #endregion
        #region   ***   DisableMeshRender   ***
        if (renders.Count > 0 && triggerType == TriggerType.DisableMeshRender || triggerType == TriggerType.All)
        {
            if (isTriggerTouchDown)
            {
                foreach (MeshRenderer mr in renders)
                {
                    isRenderEnable = !isRenderEnable;
                    mr.enabled = isRenderEnable;
                }
            }
        }
        #endregion
        #region   ***   ColliderType   ***

        if (disableSelfCollider && isTriggerTouchDown)
            GetComponent<Collider>().enabled = false;

        if (enableColliders.Count > 0)
        {
            if (isTriggerTouchDown)
            {
                StartCoroutine(SetCollider(!isColliderEnable, enableColliderDelay));
                if (isColliderChangeReversible)
                {
                    isColliderEnable = !isColliderEnable;
                }
                isTrigger = true;
            }

            if (isTriggerTouchUp)
            {

            }
        }
        #endregion
    }
    private void OnTriggerExit(Collider other)
    {
        isTrigger = false;
        touchHand = null;
    }

    private IEnumerator PlayAnim(string animName, float _speed)
    {
        isAnimPlaying = true;

        anim.Play(animName);
        if (_speed > 0)
        {
            anim[animName].time = 0;
        }
        else
        {
            anim[animName].time = anim[animName].length;
        }
        anim[animName].speed = _speed;
        isTrigger = true;
        yield return new WaitForSeconds(anim[animName].length + 0.02f);

        if (_speed > 0)
            anim[animName].time = anim[animName].length;
        else
            anim[animName].time = 0;
        anim[animName].speed = 0f;

        isAnimPlaying = false;
    }

    private IEnumerator HideObjs()
    {
        yield return new WaitForSeconds(0.1f);
        foreach (GameObject g in hideObjs)
        {
            g.SetActive(isHide);
        }
        isHide = !isHide;
    }

    public IEnumerator LoadScene(float _time)
    {
        anim.Play();

        yield return new WaitForSeconds(_time);
        CamRigDate.curCamRigPos = FindObjectOfType<SteamVR_ControllerManager>().transform.position;
        SceneManager.LoadSceneAsync(sceneName);
    }

    private IEnumerator SetCollider(bool _enable, float _delay)
    {
        yield return new WaitForSeconds(_delay + 0.05f);
        if (enableColliders.Count > 0)
        {
            foreach (Collider c in enableColliders)
            {
                c.enabled = _enable;
            }
        }
    }
}
