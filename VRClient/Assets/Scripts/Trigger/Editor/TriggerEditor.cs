using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace Unit.Trigger
{
    [CustomEditor(typeof(Trigger))]
    public class TriggerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            Trigger trigger = (Trigger)target;

            trigger.type = (TriggerType)EditorGUILayout.EnumPopup("Trigger Type", trigger.type);

            trigger.enableCollision = EditorGUILayout.Toggle("Collision", trigger.enableCollision);

            trigger.limitTriggerLayer = EditorGUILayout.Toggle("Limit Trigger Layer", trigger.limitTriggerLayer);
            
            if (trigger.limitTriggerLayer)
            {
                serializedObject.Update();
                EditorGUILayout.PropertyField(serializedObject.FindProperty("limitLayerNames"), true);
                serializedObject.ApplyModifiedProperties();
            }

            trigger.isDelay = EditorGUILayout.Toggle("Use Delay", trigger.isDelay);

            if (trigger.isDelay)
            {
                trigger.delayLength = EditorGUILayout.FloatField("Delay Length", trigger.delayLength);
            }
            trigger.limitTriggerCount = EditorGUILayout.Toggle("limitTriggerCount", trigger.limitTriggerCount);
            //trigger.limitEnterTriggerTime = EditorGUILayout.Toggle("Limit Enter Trigger Time", trigger.limitEnterTriggerTime);
            
            if (trigger.limitTriggerCount)
            {
                trigger.enterTriggerTimeLimit = EditorGUILayout.IntField("Enter Trigger Time", trigger.enterTriggerTimeLimit);
            }

            //trigger.limitExitTriggerTime = EditorGUILayout.Toggle("Limit Exit Trigger Time", trigger.limitExitTriggerTime);

            if (trigger.limitTriggerCount)
            {
                trigger.exitTriggerTimeLimit = EditorGUILayout.IntField("Exit Trigger Time", trigger.exitTriggerTimeLimit);
            }

            EditorGUILayout.Separator();

            if (trigger.type == TriggerType.Animation)
            {
                trigger.enterAnim = (Animation)EditorGUILayout.ObjectField("Enter Animation", trigger.enterAnim, typeof(Animation), true);
                trigger.exitAnim = (Animation)EditorGUILayout.ObjectField("Exit Animation", trigger.exitAnim, typeof(Animation), true);

                trigger.enterAnimName = EditorGUILayout.TextField("Enter Animation Name", trigger.enterAnimName);
                trigger.exitAnimName = EditorGUILayout.TextField("Exit Animation Name", trigger.exitAnimName);

                trigger.enterAnimSpeed = EditorGUILayout.FloatField("Enter Animation Speed", trigger.enterAnimSpeed);
                trigger.exitAnimSpeed = EditorGUILayout.FloatField("Exit Animation Speed", trigger.exitAnimSpeed);

                trigger.enterAnimStartTime = EditorGUILayout.FloatField("Enter Animation Start Time", trigger.enterAnimStartTime);
                trigger.exitAnimStartTime = EditorGUILayout.FloatField("Exit Animation Start Time", trigger.exitAnimStartTime);
            }
            else if (trigger.type == TriggerType.Color)
            {
                trigger.colorTarget = (GameObject)EditorGUILayout.ObjectField("Target", trigger.colorTarget, typeof(GameObject), true);

                trigger.enterColor = EditorGUILayout.ColorField("Enter Color", trigger.enterColor);
                trigger.exitColor = EditorGUILayout.ColorField("Exit Color", trigger.exitColor);
            }
            else if (trigger.type == TriggerType.Position)
            {
                trigger.posTarget = (GameObject)EditorGUILayout.ObjectField("Target", trigger.posTarget, typeof(GameObject), true);

                trigger.enterPos = EditorGUILayout.Vector3Field("Enter Position", trigger.enterPos);
                trigger.exitPos = EditorGUILayout.Vector3Field("Exit Position", trigger.exitPos);
            }
            else if (trigger.type == TriggerType.Rotation)
            {
                trigger.rotationTarget = (GameObject)EditorGUILayout.ObjectField("Target", trigger.rotationTarget, typeof(GameObject), true);

                trigger.enterRot = EditorGUILayout.Vector3Field("Enter Rotation", trigger.enterRot);
                trigger.exitRot = EditorGUILayout.Vector3Field("Exit Rotation", trigger.exitRot);
            }
            else if (trigger.type == TriggerType.MoveTo)
            {
                trigger.moveTarget = (GameObject)EditorGUILayout.ObjectField("Target", trigger.moveTarget, typeof(GameObject), true);
                
                trigger.enterMoveTime = EditorGUILayout.FloatField("Enter Time", trigger.enterMoveTime);
                trigger.exitMoveTime = EditorGUILayout.FloatField("Exit Time", trigger.exitMoveTime);
                
                trigger.enterMoveToPos = EditorGUILayout.Vector3Field("Enter Position", trigger.enterMoveToPos);
                trigger.exitMoveToPos = EditorGUILayout.Vector3Field("Exit Position", trigger.exitMoveToPos);
            }
            else if (trigger.type == TriggerType.RotateTo)
            {
                trigger.rotateTarget = (GameObject)EditorGUILayout.ObjectField("Target", trigger.rotateTarget, typeof(GameObject), true);

                trigger.enterRotateTime = EditorGUILayout.FloatField("Enter Time", trigger.enterRotateTime);
                trigger.exitRotateTime = EditorGUILayout.FloatField("Exit Time", trigger.exitRotateTime);

                trigger.enterRotateToRotation = EditorGUILayout.Vector3Field("Enter Rotation", trigger.enterRotateToRotation);
                trigger.exitRotateToRotation = EditorGUILayout.Vector3Field("Exit Rotation", trigger.exitRotateToRotation);
            }
            else if(trigger.type == TriggerType.LoadScene)
            { }
        }
    }
}