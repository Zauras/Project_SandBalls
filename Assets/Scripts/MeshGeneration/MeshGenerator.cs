using System.Collections.Generic;
using UnityEngine;

namespace MeshGeneration
{
	public class MeshGenerator : MonoBehaviour
	{
		public SquareGrid squareGrid;
		public MeshFilter destructibleMeshWall;
		public float meshWallHeight = 0.5f;
		public PolygonCollider2D meshWallCollider;
		
		private readonly Dictionary<int, List<Triangle>> trianglesDic = new Dictionary<int, List<Triangle>>();
		private readonly List<List<int>> contourMap = new List<List<int>>();
		private readonly HashSet<int> checkedVertices = new HashSet<int>();
		
		private List<Vector3> vertices;
		private List<int> triangles;

		private int GetConnectedOutlineVertex(int vertexIndex)
		{
			List<Triangle> list = trianglesDic[vertexIndex];
			for (int i = 0; i < list.Count; i++)
			{
				Triangle triangle = list[i];
				
				for (int j = 0; j < 3; j++)
				{
					int num = triangle[j];
					if (num != vertexIndex && !checkedVertices.Contains(num) &&
					    IsOutlineEdge(vertexIndex, num))
					{
						return num;
					}
				}
			}
			return -1;
		}

		private void FollowOutline(int vertexIndex, int outlineIndex)
		{
			contourMap[outlineIndex].Add(vertexIndex);
			checkedVertices.Add(vertexIndex);
			int connectedOutlineVertex = GetConnectedOutlineVertex(vertexIndex);
			
			if (connectedOutlineVertex != -1)
			{
				FollowOutline(connectedOutlineVertex, outlineIndex);
			}
		}

		private void CalculateMeshOutlines()
		{
			for (int i = 0; i < vertices.Count; i++)
			{
				if (!checkedVertices.Contains(i))
				{
					int connectedOutlineVertex = GetConnectedOutlineVertex(i);
					if (connectedOutlineVertex != -1)
					{
						checkedVertices.Add(i);
						var list = new List<int> {i};
						contourMap.Add(list);
						FollowOutline(connectedOutlineVertex, contourMap.Count - 1);
						contourMap[contourMap.Count - 1].Add(i);
					}
				}
			}
		}

		private void CreateWallMesh()
		{
			CalculateMeshOutlines();
			
			List<Vector3> meshVertices = new List<Vector3>();
			List<int> meshMap = new List<int>();
			Mesh mesh = new Mesh();
			List<Vector2> meshRegions = new List<Vector2>();
			
			meshWallCollider.pathCount = contourMap.Count;
			
			for (int i = 0; i < contourMap.Count; i++)
			{
				meshRegions.Clear();
				
				for (int j = 0; j < contourMap[i].Count - 1; j++)
				{
					int count = meshVertices.Count;
					meshVertices.Add(vertices[contourMap[i][j]]);
					meshVertices.Add(vertices[contourMap[i][j + 1]]);
					meshVertices.Add(vertices[contourMap[i][j]] + Vector3.forward * meshWallHeight);
					meshVertices.Add(vertices[contourMap[i][j + 1]] + Vector3.forward * meshWallHeight);
					
					meshMap.Add(count);
					meshMap.Add(count + 2);
					meshMap.Add(count + 3);
					meshMap.Add(count + 3);
					meshMap.Add(count + 1);
					meshMap.Add(count);
					
					meshRegions.Add(vertices[contourMap[i][j]]);
				}
				meshWallCollider.SetPath(i, meshRegions.ToArray());
			}
			mesh.vertices = meshVertices.ToArray();
			mesh.triangles = meshMap.ToArray();
			mesh.RecalculateNormals();
			destructibleMeshWall.mesh = mesh;
		}
		
				private void MeshFromPoints(params Point[] points)
		{
			AssignVertices(points);
			
			if (points.Length >= 3) { CreateTriangle(points[0], points[1], points[2]); }
			if (points.Length >= 4) { CreateTriangle(points[0], points[2], points[3]); }
			if (points.Length >= 5) { CreateTriangle(points[0], points[3], points[4]); }
			if (points.Length >= 6) { CreateTriangle(points[0], points[4], points[5]); }
		}

		private void TriangulateSquare(Square square)
		{
			switch (square.configuration) {
			case 0:
				break;
			case 1:
				MeshFromPoints(square.centreLeft, square.centreBottom, square.bottomLeft);
				break;
			case 2:
				MeshFromPoints(square.bottomRight, square.centreBottom, square.centreRight);
				break;
			case 4:
				MeshFromPoints(square.topRight, square.centreRight, square.centreTop);
				break;
			case 8:
				MeshFromPoints(square.topLeft, square.centreTop, square.centreLeft);
				break;
			case 3:
				MeshFromPoints(square.centreRight, square.bottomRight, square.bottomLeft, square.centreLeft);
				break;
			case 6:
				MeshFromPoints(square.centreTop, square.topRight, square.bottomRight, square.centreBottom);
				break;
			case 9:
				MeshFromPoints(square.topLeft, square.centreTop, square.centreBottom, square.bottomLeft);
				break;
			case 12:
				MeshFromPoints(square.topLeft, square.topRight, square.centreRight, square.centreLeft);
				break;
			case 5:
				MeshFromPoints(square.centreTop, square.topRight, square.centreRight, square.centreBottom, square.bottomLeft, square.centreLeft);
				break;
			case 10:
				MeshFromPoints(square.topLeft, square.centreTop, square.centreRight, square.bottomRight, square.centreBottom, square.centreLeft);
				break;
			case 7:
				MeshFromPoints(square.centreTop, square.topRight, square.bottomRight, square.bottomLeft, square.centreLeft);
				break;
			case 11:
				MeshFromPoints(square.topLeft, square.centreTop, square.centreRight, square.bottomRight, square.bottomLeft);
				break;
			case 13:
				MeshFromPoints(square.topLeft, square.topRight, square.centreRight, square.centreBottom, square.bottomLeft);
				break;
			case 14:
				MeshFromPoints(square.topLeft, square.topRight, square.bottomRight, square.centreBottom, square.centreLeft);
				break;
			case 15:
				MeshFromPoints(square.topLeft, square.topRight, square.bottomRight, square.bottomLeft);
				checkedVertices.Add(square.topLeft.vertexIndex);
				checkedVertices.Add(square.topRight.vertexIndex);
				checkedVertices.Add(square.bottomRight.vertexIndex);
				checkedVertices.Add(square.bottomLeft.vertexIndex);
				break;
			}
		}

		private void AssignVertices(Point[] points)
		{
			for (int i = 0; i < points.Length; i++)
			{
				if (points[i].vertexIndex == -1)
				{
					points[i].vertexIndex = vertices.Count;
					vertices.Add(points[i].position);
				}
			}
		}

		private void AddTriangleToDictionary(int vertexIndexKey, Triangle triangle)
		{
			if (trianglesDic.ContainsKey(vertexIndexKey))
			{
				trianglesDic[vertexIndexKey].Add(triangle);
				return;
			}

			var list = new List<Triangle> {triangle};
			trianglesDic.Add(vertexIndexKey, list);
		}

		private void CreateTriangle(Point a, Point b, Point c)
		{
			triangles.Add(a.vertexIndex);
			triangles.Add(b.vertexIndex);
			triangles.Add(c.vertexIndex);

			Triangle triangle = new Triangle(a.vertexIndex, b.vertexIndex, c.vertexIndex);

			AddTriangleToDictionary(triangle.vertexIndexA, triangle);
			AddTriangleToDictionary(triangle.vertexIndexB, triangle);
			AddTriangleToDictionary(triangle.vertexIndexC, triangle);
		}

		private bool IsOutlineEdge(int vertexA, int vertexB)
		{
			List<Triangle> list = trianglesDic[vertexA];
			int num = 0;
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].Contains(vertexB))
				{
					num++;
					if (num > 1)
					{
						break;
					}
				}
			}

			return num == 1;
		}

		public void GenerateMesh(float[,] map, float squareSize)
		{
			trianglesDic.Clear();
			contourMap.Clear();
			checkedVertices.Clear();

			squareGrid = new SquareGrid(map, squareSize);
			vertices = new List<Vector3>();
			triangles = new List<int>();

			for (int i = 0; i < squareGrid.squares.GetLength(0); i++)
			{
				for (int j = 0; j < squareGrid.squares.GetLength(1); j++)
				{
					TriangulateSquare(squareGrid.squares[i, j]);
				}
			}

			Mesh mesh = new Mesh();
			GetComponent<MeshFilter>().mesh = mesh;
			mesh.vertices = vertices.ToArray();
			mesh.triangles = triangles.ToArray();
			mesh.RecalculateNormals();

			CreateWallMesh();
		}

	}
}
