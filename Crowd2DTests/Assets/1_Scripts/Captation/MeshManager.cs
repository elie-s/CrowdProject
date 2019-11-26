using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CrowdProject
{
    public class MeshManager : MonoBehaviour
    {
        [SerializeField] public Texture2D texture = default;
        [SerializeField] private WebCamManager webcam = default;
        [SerializeField] private MeshFilter meshFilter = default;
        [SerializeField] private MeshCollider meshCollider = default;
        [SerializeField] private float size = 1.0f;
        public SquareGrid squareGrid;
        public bool gizmos = false;

        private List<Vector3> vertices = new List<Vector3>();
        private List<int> triangles = new List<int>();

        private PolygonCollider2D test;

        private void Update()
        {
            GenerateMesh(SetMapping(webcam.texture), size);

        }

        private void OnDrawGizmos()
        {
            if (!gizmos) return;

            if (squareGrid != null)
            {
                for (int x = 0; x < squareGrid.squares.GetLength(0); x++)
                {
                    for (int y = 0; y < squareGrid.squares.GetLength(1); y++)
                    {
                        Gizmos.color = squareGrid.squares[x, y].topLeft.active ? Color.black : Color.white;
                        Gizmos.DrawCube(squareGrid.squares[x, y].topLeft.position, Vector3.one * 4.0f);

                        Gizmos.color = squareGrid.squares[x, y].topRight.active ? Color.black : Color.white;
                        Gizmos.DrawCube(squareGrid.squares[x, y].topRight.position, Vector3.one * 4.0f);

                        Gizmos.color = squareGrid.squares[x, y].bottomRight.active ? Color.black : Color.white;
                        Gizmos.DrawCube(squareGrid.squares[x, y].bottomRight.position, Vector3.one * 4.0f);

                        Gizmos.color = squareGrid.squares[x, y].bottomLeft.active ? Color.black : Color.white;
                        Gizmos.DrawCube(squareGrid.squares[x, y].bottomLeft.position, Vector3.one * 4.0f);

                        Gizmos.color = Color.grey;
                        Gizmos.DrawCube(squareGrid.squares[x, y].centerTop.position, Vector3.one * 1.5f);
                        Gizmos.DrawCube(squareGrid.squares[x, y].centerRight.position, Vector3.one * 1.5f);
                        Gizmos.DrawCube(squareGrid.squares[x, y].centerBottom.position, Vector3.one * 1.5f);
                        Gizmos.DrawCube(squareGrid.squares[x, y].centerLeft.position, Vector3.one * 1.5f);
                    }
                }
            }
        }

        public void GenerateMesh(int[,] _map, float _squareSize)
        {
            squareGrid = new SquareGrid(_map, _squareSize);
            vertices = new List<Vector3>();
            triangles = new List<int>();

            for (int x = 0; x < squareGrid.squares.GetLength(0); x++)
            {
                for (int y = 0; y < squareGrid.squares.GetLength(1); y++)
                {
                    TriangulateSquare(squareGrid.squares[x, y]);
                }
            }

            Mesh mesh = new Mesh();
            meshFilter.mesh = mesh;
            meshCollider.sharedMesh = mesh; 

            mesh.vertices = vertices.ToArray();
            mesh.triangles = triangles.ToArray();
            mesh.RecalculateNormals();
        }


        private int[,] SetMapping(Texture2D _texture)
        {
            Color[] pixels = _texture.GetPixels();

            Debug.Log(pixels.Length);
            Debug.Log(_texture.width + " x " + _texture.height);

            int[,] result = new int[_texture.width, _texture.height];

            for (int x = 0; x < _texture.width; x++)
            {
                for (int y = 0; y < _texture.height; y++)
                {
                    result[x, y] = Mathf.RoundToInt(pixels[x + y * _texture.width].r);
                }
            }

            return result;
        }

        private void TriangulateSquare(Square _square)
        {
            switch (_square.configuration)
            {
                case 0:
                    break;

                // 1 points:
                case 1:
                    MeshFromPoints(_square.centerBottom, _square.bottomLeft, _square.centerLeft);
                    break;
                case 2:
                    MeshFromPoints(_square.centerRight, _square.bottomRight, _square.centerBottom);
                    break;
                case 4:
                    MeshFromPoints(_square.centerTop, _square.topRight, _square.centerRight);
                    break;
                case 8:
                    MeshFromPoints(_square.topLeft, _square.centerTop, _square.centerLeft);
                    break;

                // 2 points:
                case 3:
                    MeshFromPoints(_square.centerRight, _square.bottomRight, _square.bottomLeft, _square.centerLeft);
                    break;
                case 6:
                    MeshFromPoints(_square.centerTop, _square.topRight, _square.bottomRight, _square.centerBottom);
                    break;
                case 9:
                    MeshFromPoints(_square.topLeft, _square.centerTop, _square.centerBottom, _square.bottomLeft);
                    break;
                case 12:
                    MeshFromPoints(_square.topLeft, _square.topRight, _square.centerRight, _square.centerLeft);
                    break;
                case 5:
                    MeshFromPoints(_square.centerTop, _square.topRight, _square.centerRight, _square.centerBottom, _square.bottomLeft, _square.centerLeft);
                    break;
                case 10:
                    MeshFromPoints(_square.topLeft, _square.centerTop, _square.centerRight, _square.bottomRight, _square.centerBottom, _square.centerLeft);
                    break;

                // 3 point:
                case 7:
                    MeshFromPoints(_square.centerTop, _square.topRight, _square.bottomRight, _square.bottomLeft, _square.centerLeft);
                    break;
                case 11:
                    MeshFromPoints(_square.topLeft, _square.centerTop, _square.centerRight, _square.bottomRight, _square.bottomLeft);
                    break;
                case 13:
                    MeshFromPoints(_square.topLeft, _square.topRight, _square.centerRight, _square.centerBottom, _square.bottomLeft);
                    break;
                case 14:
                    MeshFromPoints(_square.topLeft, _square.topRight, _square.bottomRight, _square.centerBottom, _square.centerLeft);
                    break;

                // 4 point:
                case 15:
                    MeshFromPoints(_square.topLeft, _square.topRight, _square.bottomRight, _square.bottomLeft);
                    break;
            }
        }

        private void MeshFromPoints(params Node[] _points)
        {
            AssignVertices(_points);

            if (_points.Length >= 3) CreateTriangle(_points[0], _points[1], _points[2]);
            if (_points.Length >= 4) CreateTriangle(_points[0], _points[2], _points[3]);
            if (_points.Length >= 5) CreateTriangle(_points[0], _points[3], _points[4]);
            if (_points.Length >= 6) CreateTriangle(_points[0], _points[4], _points[5]);
        }

        private void AssignVertices(Node[] _points)
        {
            for (int i = 0; i < _points.Length; i++)
            {
                if (_points[i].vertexIndex == -1) _points[i].vertexIndex = vertices.Count;

                vertices.Add(_points[i].position);
            }
        }

        private void CreateTriangle(Node a, Node b, Node c)
        {
            triangles.Add(a.vertexIndex);
            triangles.Add(b.vertexIndex);
            triangles.Add(c.vertexIndex);
        }


        public class Node
        {
            public Vector3 position;
            public int vertexIndex = -1;

            public Node(Vector3 _pos)
            {
                position = _pos;
            }
        }

        public class ControlNode : Node
        {
            public bool active;
            public Node above;
            public Node right;

            public ControlNode(Vector3 _pos, bool _active, float _squareSize) : base(_pos)
            {
                active = _active;
                above = new Node(position + Vector3.forward * _squareSize / 2.0f);
                right = new Node(position + Vector3.right * _squareSize / 2.0f);
            }
        }

        public class Square
        {
            public ControlNode topLeft;
            public ControlNode topRight;
            public ControlNode bottomRight;
            public ControlNode bottomLeft;

            public Node centerTop;
            public Node centerRight;
            public Node centerBottom;
            public Node centerLeft;

            public int configuration;

            public Square(ControlNode topLeft, ControlNode topRight, ControlNode bottomRight, ControlNode bottomLeft)
            {
                this.topLeft = topLeft;
                this.topRight = topRight;
                this.bottomRight = bottomRight;
                this.bottomLeft = bottomLeft;

                centerTop = topLeft.right;
                centerRight = bottomRight.above;
                centerBottom = bottomLeft.right;
                centerLeft = bottomLeft.above;

                if (topLeft.active) configuration += 8;
                if (topRight.active) configuration += 4;
                if (bottomRight.active) configuration += 2;
                if (bottomLeft.active) configuration += 1;

            }
        }

        public class SquareGrid
        {
            public Square[,] squares;

            public SquareGrid(int[,] _map, float _squareSize)
            {
                int nodeCountX = _map.GetLength(0);
                int nodeCountY = _map.GetLength(1);
                float mapWidth = nodeCountX * _squareSize;
                float mapHeight = nodeCountY * _squareSize;

                ControlNode[,] controlNodes = new ControlNode[nodeCountX, nodeCountY];

                for (int x = 0; x < nodeCountX; x++)
                {
                    for (int y = 0; y < nodeCountY; y++)
                    {
                        Vector3 pos = new Vector3(-mapWidth / 2.0f + x * _squareSize + _squareSize / 2.0f, 0, -mapHeight / 2.0f + y * _squareSize + _squareSize / 2.0f);
                        controlNodes[x, y] = new ControlNode(pos, _map[x, y] == 0, _squareSize);
                    }
                }

                squares = new Square[nodeCountX - 1, nodeCountY - 1];

                for (int x = 0; x < nodeCountX - 1; x++)
                {
                    for (int y = 0; y < nodeCountY - 1; y++)
                    {
                        squares[x, y] = new Square(controlNodes[x, y + 1], controlNodes[x + 1, y + 1], controlNodes[x + 1, y], controlNodes[x, y]);
                    }
                }


            }
        }
    }
}