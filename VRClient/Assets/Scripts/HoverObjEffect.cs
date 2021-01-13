using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using HighlightingSystem;
using UnityEngine.UI;

public class HoverObjEffect : MonoBehaviour
{
    public Color highLightColor;

    public List<HighlighterController> highLightObjs = new List<HighlighterController>();
    public List<GameObject> hideObjs = new List<GameObject>();
    public List<TransparentObj> transparentObjs = new List<TransparentObj>();

    public bool isLimitLayer;

    public string layerName;

    public bool isShowText = false;

    public Text text;
    public TextMesh textMesh;

    public string textContent;

    public float flashTime = 1.0f;

    private bool isHighLightEnable = false;
    private bool isHideEnable = false;
    private bool isTransparentEnable = false;

    private List<Material> transObjInitMaterials = new List<Material>();

    private Collider selfCollider;

    private ViveHand hand;

    private void Start()
    {
        selfCollider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isLimitLayer && other.gameObject.layer != LayerMask.NameToLayer(layerName))
            return;

        if (other.GetComponent<ViveHand>())
            hand = other.GetComponent<ViveHand>();

        if (!hand || hand.isDragging)
            return;

        if (hand && hand.hoverObj != null && hand.hoverObj != this)
        {
            hand.hoverObj.Exit();
        }

        Enter();
    }

    private void OnTriggerExit(Collider other)
    {
        Exit();
    }

    private void Update()
    {
        if (isHighLightEnable)
        {
            foreach (HighlighterController g in highLightObjs)
            {
                g.h.On(highLightColor);
            }
        }

        if (selfCollider && !selfCollider.enabled && isHighLightEnable)
        {
            isHighLightEnable = false;
            foreach (HighlighterController g in highLightObjs)
            {
                g.GetComponent<Highlighter>().Off();
            }
        }
    }

    public void Enter()
    {
        if (highLightObjs.Count > 0)
        {
            isHighLightEnable = true;
        }
        if (hideObjs.Count > 0)
        {
            isHideEnable = true;
            foreach (GameObject g in hideObjs)
            {
                g.SetActive(false);
            }
        }
        if (transparentObjs.Count > 0)
        {
            isTransparentEnable = true;
            foreach (TransparentObj t in transparentObjs)
            {
                t.SetTransparent(true);
            }
        }

        if (isShowText && text)
        {
            //text.text = textContent;
            text.gameObject.SetActive(true);
        }

        if (isShowText && textMesh)
        {
            //textMesh.text = textContent;
            textMesh.gameObject.SetActive(true);
        }
        if (hand)
            hand.hoverObj = this;
    }
    public void Exit()
    {
        if (isHighLightEnable)
        {
            isHighLightEnable = false;

            foreach (HighlighterController g in highLightObjs)
            {
                g.GetComponent<Highlighter>().Off();
            }
        }
        if (isTransparentEnable)
        {
            isTransparentEnable = false;
            if (transparentObjs.Count > 0)
            {
                foreach (TransparentObj t in transparentObjs)
                {
                    t.SetTransparent(false);
                }
            }
        }

        if (isShowText && text)
        {
            //text.text = "";
            text.gameObject.SetActive(false);
        }

        if (isShowText && textMesh)
        {
            //textMesh.text = "";
            textMesh.gameObject.SetActive(false);
        }
    }

    public void StartFlash()
    {
        isHighLightEnable = false;
        StartCoroutine(Flash(flashTime));
    }

    private IEnumerator Flash(float _time)
    {
        /*
        float _unit = _time / flashTime;
        for (int t = 0; t < flashTime; ++t)
        {
            if (t % 2 == 0)
            {
                highlightCtr.h.On(Color.blue);
                highlightCtr.h.FlashingOn();
            }
            else
            {
                highlightCtr.h.Off();
            }
            yield return new WaitForSeconds(_unit);
        }
        */
        foreach (HighlighterController highCtr in highLightObjs)
        {
            highCtr.h.FlashingOn();
        }
        yield return new WaitForSeconds(_time);
        foreach (HighlighterController highCtr in highLightObjs)
        {
            highCtr.h.FlashingOff();
        }
    }

    public enum RenderingMode
    {
        Opaque,
        Cutout,
        Fade,
        Transparent
    }

    //设置材质的渲染模式  
    private void setMaterialRenderingMode(Material material, RenderingMode renderingMode)
    {
        switch (renderingMode)
        {
            case RenderingMode.Opaque:
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                material.SetInt("_ZWrite", 1);
                material.DisableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = -1;
                break;
            case RenderingMode.Cutout:
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                material.SetInt("_ZWrite", 1);
                material.EnableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = 2450;
                break;
            case RenderingMode.Fade:
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                material.SetInt("_ZWrite", 0);
                material.DisableKeyword("_ALPHATEST_ON");
                material.EnableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = 3000;
                break;
            case RenderingMode.Transparent:
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                material.SetInt("_ZWrite", 0);
                material.DisableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHABLEND_ON");
                material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = 3000;
                break;
        }
    }
}
