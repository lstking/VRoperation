using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Unit.Trigger
{
    public enum TriggerType
    {
        Animation,
        Color,
        Position,
        Rotation,
        MoveTo,
        RotateTo,
        Render,
        LoadScene,
        Other
    }

    public class TriggerEventArgs : EventArgs
    {
        public TriggerEventArgs()
        {

        }

        public TriggerEventArgs(object obj)
        {
            
        }

        public TriggerEventArgs(List<string> infos)
        {

        }
    }

    public class Trigger : MonoBehaviour
    {
        public TriggerType type;

        public bool enableCollision;

        public bool isDelay;

        public bool limitTriggerCount = false;
        //public bool limitEnterTriggerTime = false;
        //public bool limitExitTriggerTime = false;

        public bool limitTriggerLayer = false;

        public int enterTriggerTimeLimit = 0;
        public int exitTriggerTimeLimit = 0;

        public float delayLength = 1.0f;
        
        public List<string> limitLayerNames = new List<string>();

        public List<GameObject> triggeringList = new List<GameObject>();

        #region   ***   Animation Trigger Part   ***
        public Animation enterAnim;
        public Animation exitAnim;
        
        public string enterAnimName = "";
        public string exitAnimName = "";
        
        public float enterAnimSpeed = 0;
        public float exitAnimSpeed = 0;

        public float enterAnimStartTime = 0;
        public float exitAnimStartTime = 0;
        
        public event EventHandler<TriggerEventArgs> OnEnterAnimBegin;
        public event EventHandler<TriggerEventArgs> OnExitAnimBegin;

        #endregion

        #region   ***   Color Trigger Part   ***

        public GameObject colorTarget;

        public Color enterColor;
        public Color exitColor;

        #endregion

        #region   ***   Position Trigger Part   ***

        public GameObject posTarget;

        public Vector3 enterPos;
        public Vector3 exitPos;

        #endregion

        #region   ***   Rotation Trigger Part   ***

        public GameObject rotationTarget;

        public Vector3 enterRot;
        public Vector3 exitRot;

        #endregion

        #region   ***   Move To   ***

        public GameObject moveTarget;

        public float enterMoveTime;
        public float exitMoveTime;

        public Vector3 enterMoveToPos;
        public Vector3 exitMoveToPos;

        #endregion

        #region   ***   Rotate To   ***

        public GameObject rotateTarget;

        public float enterRotateTime;
        public float exitRotateTime;

        public Vector3 enterRotateToRotation;
        public Vector3 exitRotateToRotation;

        #endregion

        #region   ***   Render   ***

        public Renderer renderTarget;

        public bool enterRenderEnable;
        public bool exitRenderEnable;

        #endregion

        #region   ***   LoadScene   ***

        public string enterLoadSceneName;
        public string exitLoadSceneName;

        #endregion

        public event EventHandler<TriggerEventArgs> OnTriggerStart;
        public event EventHandler<TriggerEventArgs> OnTriggering;
        public event EventHandler<TriggerEventArgs> OnTriggerEnd;

        protected int enterTriggerTime = 0;
        protected int exitTriggerTime = 0;

        private bool startTiming = false;

        private float curDelayTime = 0;

        private Collider _collider = new Collider();

        private void Start()
        {
            _collider = GetComponent<Collider>();

            _collider.isTrigger = !enableCollision;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (limitTriggerLayer)
                if (!limitLayerNames.Contains(LayerMask.LayerToName(other.gameObject.layer)))
                    return;
            
            triggeringList.Add(other.gameObject);

            if (limitTriggerCount)
            {
                ++enterTriggerTime;

                int enterCount = enterTriggerTime - exitTriggerTime;
                
                if (enterCount >= enterTriggerTimeLimit)
                {
                    enterTriggerTime = 0;
                    exitTriggerTime = 0;
                }
                else
                    return;
            }
            if (isDelay && !startTiming)
            {
                startTiming = true;
                return;
            }

            if (OnTriggerStart != null)
            {
                OnTriggerStart(this, new TriggerEventArgs());
            }

            #region   ***   Use Function   ***
            /*
            switch (type)
            {
                case TriggerType.Animation:
                    if(OnTriggerEnd != null)
                    {
                        OnTriggerEnd(this, new TriggerEventArgs());
                    }
                    PlayAnim(enterAnim, enterAnimName, enterAnimSpeed, enterAnimStartTime);
                    break;
                case TriggerType.Color:
                    SetColor(colorTarget, enterColor);
                    break;
                case TriggerType.Position:
                    SetPos(posTarget, exitPos);
                    break;
                case TriggerType.Rotation:
                    SetRot(rotateTarget, exitRot);
                    break;
                case TriggerType.MoveTo:
                    MoveTo(moveTarget, enterMoveToPos, enterMoveTime);
                    break;
                case TriggerType.RotateTo:
                    RotateTo(rotateTarget, enterRotateToRotation, enterRotateTime);
                    break;
                case TriggerType.Render:
                    SetRender(renderTarget, enterRenderEnable);
                    break;
                case TriggerType.Other:
                    break;
            }
            */
            #endregion
        }

        private void OnTriggerStay(Collider other)
        {
            if (OnTriggering != null)
            {
                OnTriggering(this, new TriggerEventArgs());
            }

            if (isDelay && startTiming)
            {
                curDelayTime += Time.deltaTime;

                if (curDelayTime >= delayLength)
                {
                    curDelayTime = 0;
                    startTiming = false;
                    if (OnTriggerStart != null)
                    {
                        OnTriggerStart(this, new TriggerEventArgs());
                    }

                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (limitTriggerLayer)
                if (!limitLayerNames.Contains(LayerMask.LayerToName(other.gameObject.layer)))
                    return;

            if (isDelay && startTiming)
            {
                curDelayTime = 0;
                startTiming = false;
            }

            if (triggeringList.Contains(other.gameObject))
                triggeringList.Remove(other.gameObject);

            if (limitTriggerCount)
            {
                ++exitTriggerTime;

                int exitCount = exitTriggerTime - enterTriggerTime;

                if (exitCount <= 0)
                {
                    enterTriggerTime = 0;
                    exitTriggerTime = 0;
                }
                else
                    return;
            }

            if (OnTriggerEnd != null)
            {
                OnTriggerEnd(this, new TriggerEventArgs());
            }

            #region   ***   Use Function   ***
            /*
            switch (type)
            {
                case TriggerType.Animation:
                    if (OnEnterAnimBegin != null)
                    {
                        OnEnterAnimBegin(this, new TriggerEventArgs());
                    }
                    PlayAnim(exitAnim, exitAnimName, exitAnimSpeed, exitAnimStartTime);
                    break;
                case TriggerType.Color:
                    SetColor(colorTarget, exitColor);
                    break;
                case TriggerType.Position:
                    SetPos(posTarget, exitPos);
                    break;
                case TriggerType.Rotation:
                    SetRot(rotateTarget, exitRot);
                    break;
                case TriggerType.MoveTo:
                    MoveTo(moveTarget, exitMoveToPos, exitMoveTime);
                    break;
                case TriggerType.RotateTo:
                    RotateTo(rotateTarget, exitRotateToRotation, exitRotateTime);
                    break;
                case TriggerType.Render:
                    SetRender(renderTarget, exitRenderEnable);
                    break;
                case TriggerType.Other:
                    break;
            }
            */
            #endregion

        }

        #region   ***   Functions   ***
        private void PlayAnim(Animation _anim, string _animName, float speed, float startTime)
        {
            if (!_anim)
                return;

            _anim.Play(_animName);
            _anim[_animName].speed = speed;
            if(startTime >= 0)
                _anim[_animName].time = startTime;
        }

        private void SetColor(GameObject obj, Color color)
        {
            obj.GetComponent<Renderer>().material.color = color;
        }

        private void SetPos(GameObject obj, Vector3 _pos)
        {
            obj.transform.position = _pos;
        }

        private void SetRot(GameObject obj, Vector3 _rot)
        {
            obj.transform.eulerAngles = _rot;
        }

        private void MoveTo(GameObject obj, Vector3 _pos, float time)
        {
            iTween.MoveTo(obj, _pos, time);
        }

        private void RotateTo(GameObject obj, Vector3 _rot, float time)
        {
            iTween.RotateTo(obj, _rot, time);
        }

        private void SetRender(Renderer _render, bool _enable)
        {
            _render.enabled = _enable;
        }
        #endregion
    }
}