using UnityEngine;

namespace MeshGeneration
{
    // RegionGenerator -> MeshGenerator
    public class RegionGenerator : MonoBehaviour
    {
        // Scale of tile
        public float scale = 1f;
        // Width of tilesMap
        public int width = 25;
        // Height of tilesMap
        public int height = 25;

        private MeshGenerator meshGenerator;
        private float[,] tileMap;
        private Vector3 offset;
        
        private void Awake()
        {
            meshGenerator = GetComponent<MeshGenerator>();
            
            offset = new Vector3(width * scale / 2f + scale / 2f,
                                 height * scale / 2f + scale / 2f,
                                 0f)
                     - Vector3.one * scale;
        }

        private void InitTileMap()
        {
            GenerateTiles();
            CutHoles();
        }

        private void Start() => InitTileMap();
        
        
        private void FillTileMap()
        {
            // edge points - 0, inner point - 1
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (i == 0 || i == width - 1 || j == 0 || j == height - 1)
                    {
                        tileMap[i, j] = 0f;
                    }
                    else
                    {
                        tileMap[i, j] = 1f;
                    }
                }
            }
        }
        private void GenerateTiles()
        {
            tileMap = new float[width, height];
            FillTileMap();
            meshGenerator.GenerateMesh(tileMap, scale);
        }

        private void CutHoles()
        {
            MeshHole[] componentsInChildren = GetComponentsInChildren<MeshHole>();
            for (int i = 0; i < componentsInChildren.Length; i++)
            {
                componentsInChildren[i].CutHole(this);
            }
        }

        public void CutMesh(Vector3 position, float radius)
        {
            position += offset - transform.position;
        
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    Vector3 v = new Vector3(i, j) * scale;
                    float distance = Vector2.Distance(position, v);
                    
                    if (distance < radius)
                    {
                        float distanceThreshold = distance / radius;
                        if (distanceThreshold < tileMap[i, j])
                        {
                            tileMap[i, j] = distanceThreshold;
                        }
                    }
                }
            }

            meshGenerator.GenerateMesh(tileMap, scale);
        }
    }
}