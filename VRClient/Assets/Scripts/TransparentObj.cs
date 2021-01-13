using UnityEngine;
using System.Collections.Generic;

public class TransparentObj : MonoBehaviour
{
    public Material transparentMat;

    public List<MeshRenderer> exceptMesh = new List<MeshRenderer>();

    private List<MeshRenderer> renderObjs = new List<MeshRenderer>();
    private List<Material> opaqueMats = new List<Material>();
    private Dictionary<MeshRenderer, List<Material>> multiMatDic = new Dictionary<MeshRenderer, List<Material>>();
    
    private void SetRenderObjs()
    {
        renderObjs.Clear();
        opaqueMats.Clear();
        multiMatDic.Clear();
        foreach (MeshRenderer mr in GetComponentsInChildren<MeshRenderer>())
        {
            renderObjs.Add(mr);
            if (mr.materials.Length > 1)
            {
                List<Material> _mats = new List<Material>(mr.materials);
                multiMatDic.Add(mr, _mats);
            }
        }

        foreach (MeshRenderer mr in renderObjs)
        {
            foreach (Material m in mr.materials)
            {
                opaqueMats.Add(m);
            }
        }
    }

    public void SetTransparent(bool isTransparent)
    {
        #region   ***   Transparent   ***
        if (isTransparent)
        {
            SetRenderObjs();
            foreach (MeshRenderer mr in renderObjs)
            {
                if (mr.materials.Length > 1)
                {
                    List<Material> _mats = new List<Material>();
                    for (int id = 0; id < mr.materials.Length; ++id)
                    {
                        _mats.Add(transparentMat);
                    }
                    mr.materials = _mats.ToArray();
                }
                else
                {
                    mr.material = transparentMat;
                }
            }
        }
        #endregion
        #region   ***   Opaque   ***
        else
        {
            int curMatIndex = 0;
            foreach (MeshRenderer mr in renderObjs)
            {
                if(multiMatDic.ContainsKey(mr))
                {
                    mr.materials = multiMatDic[mr].ToArray();
                    curMatIndex += multiMatDic[mr].Count;
                }
                else
                {
                    mr.material = opaqueMats[curMatIndex];
                    ++curMatIndex;
                }
            }
            #endregion
        }
    }
}
