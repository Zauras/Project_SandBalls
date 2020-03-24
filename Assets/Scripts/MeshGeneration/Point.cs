using UnityEngine;

namespace MeshGeneration
{
    public class Point
    {
        public Point(Vector3 position)
            {
                this.position = position;
            }

            public Vector3 position;
            public int vertexIndex = -1;
    }
}