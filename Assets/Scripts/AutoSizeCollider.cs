using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoSizeCollider : MonoBehaviour
{
    public SkinnedMeshRenderer render;
    public BoxCollider col;

    // Update is called once per frame
    void Update()
    {
        col.center = render.bounds.center;
        col.size = render.bounds.extents;
    }
}
