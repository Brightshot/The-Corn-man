using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GizmoRenderer : MonoBehaviour
{
    public enum ShapeType { Box,Sphere}
    public ShapeType Shape;

    [Header("Box Renderer"),Space(10)]
    public Vector3 size;

    [Header("Sphere")]
    public float radius;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        if(Shape == ShapeType.Box)
        {
            Gizmos.DrawWireCube(transform.position, size);
        }else if
        (Shape == ShapeType.Sphere)
         {
            Gizmos.DrawWireSphere(transform.position,radius);
         }
    }
}
