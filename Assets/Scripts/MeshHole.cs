using UnityEngine;
using MeshGeneration;

public class MeshHole : MonoBehaviour
{

    public float radius = 1.5f;

    public void CutHole(RegionGenerator region)
    {
        region.CutMesh(transform.position, radius);
    }

}
