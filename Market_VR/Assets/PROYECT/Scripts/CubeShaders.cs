using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeShaders : MonoBehaviour
{
    public Material topMat;
    public Material bottomMat;
    public Material frontMat;
    public Material backMat;
    public Material leftMat;
    public Material rightMat;

    void Start()
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        var mats = new Material[6];

        mats[0] = rightMat;   // +X
        mats[1] = leftMat;    // -X
        mats[2] = topMat;     // +Y
        mats[3] = bottomMat;  // -Y
        mats[4] = frontMat;   // +Z
        mats[5] = backMat;    // -Z

        GetComponent<MeshRenderer>().materials = mats;

    }

}
