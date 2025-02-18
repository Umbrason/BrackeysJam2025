using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ProceduralMeshes;
using UnityEngine;

[ExecuteInEditMode, RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
public class CardboardSpriteRenderer : MonoBehaviour
{
    public float thickness = .2f;
    [SerializeField] private Sprite sprite;
    public Sprite Sprite
    {
        get { return sprite; }
        set
        {
            sprite = value; m_Mesh = null; cached_propertyBlock = null; MeshFilter.sharedMesh = Mesh;
            MeshRenderer.sharedMaterial = material;
            MeshRenderer.SetPropertyBlock(PropertyBlock);
        }
    }
    public Material material;

    [SerializeField, HideInInspector] private Mesh m_Mesh = null;
    public Mesh Mesh { get { return m_Mesh ? m_Mesh : m_Mesh = GenerateMesh(); } }

    [SerializeField, HideInInspector] private MeshFilter cached_MeshFilter = null;
    private MeshFilter MeshFilter { get { return cached_MeshFilter ? cached_MeshFilter : cached_MeshFilter = GetComponent<MeshFilter>(); } }

    [SerializeField, HideInInspector] private MeshRenderer cached_MeshRenderer = null;
    private MeshRenderer MeshRenderer { get { return cached_MeshRenderer ? cached_MeshRenderer : cached_MeshRenderer = GetComponent<MeshRenderer>(); } }

    private MaterialPropertyBlock cached_propertyBlock = null;
    public MaterialPropertyBlock PropertyBlock
    {
        get
        {
            if (cached_propertyBlock != null)
                return cached_propertyBlock;
            cached_propertyBlock = new MaterialPropertyBlock();
            if (sprite && sprite.texture) cached_propertyBlock.SetTexture("_MainTexture", sprite.texture);
            return cached_propertyBlock;
        }
    }

    void Awake()
    {
        MeshRenderer.sharedMaterial = material;
        MeshRenderer.SetPropertyBlock(PropertyBlock);
        Sprite = Sprite; //TODO: remove later
    }

    private Mesh GenerateMesh()
    {
        if (!sprite)
            return null;

        var verts = sprite.vertices.Select(x => (Vector3)x).ToList();
        var tris = sprite.triangles.Select(x => (int)x).ToList();
        var singleEdges = GetSingleEdges(tris);

        var frontMesh = new DynamicMesh("sprite front");
        frontMesh.Vertices.AddRange(verts);
        frontMesh.Indices.AddRange(tris);
        frontMesh.Normals.AddRange(from i in Enumerable.Range(0, verts.Count) select Vector3.back);
        frontMesh.Colors.AddRange(from i in Enumerable.Range(0, verts.Count) select Color.white);
        frontMesh.UV0.AddRange(sprite.uv);

        var backMesh = new DynamicMesh("sprite back");
        backMesh.Vertices.AddRange(verts);
        backMesh.Indices.AddRange(FlipTriangles(tris));
        backMesh.Normals.AddRange(from i in Enumerable.Range(0, verts.Count) select Vector3.forward);
        backMesh.Colors.AddRange(from i in Enumerable.Range(0, verts.Count) select Color.white);
        backMesh.UV0.AddRange(sprite.uv);

        var borderMesh = GenerateBorderMesh(frontMesh);

        var combinedMesh = new DynamicMesh(sprite.name, new Vector3[0], new int[0], new Vector3[0]);
        combinedMesh.AppendMesh(frontMesh, Vector3.back * thickness / 2f, Quaternion.identity, Vector3.one);
        combinedMesh.AppendMesh(backMesh, Vector3.forward * thickness / 2f, Quaternion.identity, Vector3.one);
        combinedMesh.AppendMesh(borderMesh, null);

        return combinedMesh.ToMesh();
    }

    private DynamicMesh GenerateBorderMesh(DynamicMesh frontMesh)
    {
        var borderMesh = new DynamicMesh("sprite side");
        var edges = GetSingleEdges(frontMesh.Indices);
        var vertexCount = frontMesh.Vertices.Count;
        var offset = 0f;
        for (int i = 0; i < edges.Count; i++)
        {
            var edge = edges[i];
            var delta = frontMesh.Vertices[edge.a] - frontMesh.Vertices[edge.b];
            var length = delta.magnitude;
            var normal = Vector3.Cross(Vector3.back, delta);
            borderMesh.Vertices.Add(frontMesh.Vertices[edge.a] + Vector3.forward * thickness / 2f);
            borderMesh.Normals.Add(normal);
            borderMesh.Colors.Add(Color.black);
            borderMesh.UV0.Add(Vector2.zero + Vector2.right * offset);

            borderMesh.Vertices.Add(frontMesh.Vertices[edge.b] + Vector3.forward * thickness / 2f);
            borderMesh.Normals.Add(normal);
            borderMesh.Colors.Add(Color.black);
            borderMesh.UV0.Add(Vector2.right * length / thickness + Vector2.right * offset);

            borderMesh.Vertices.Add(frontMesh.Vertices[edge.a] + Vector3.back * thickness / 2f);
            borderMesh.Normals.Add(normal);
            borderMesh.Colors.Add(Color.black);
            borderMesh.UV0.Add(Vector2.up + Vector2.right * offset);

            borderMesh.Vertices.Add(frontMesh.Vertices[edge.b] + Vector3.back * thickness / 2f);
            borderMesh.Normals.Add(normal);
            borderMesh.Colors.Add(Color.black);
            borderMesh.UV0.Add(Vector2.up + Vector2.right * length / thickness + Vector2.right * offset);

            borderMesh.Indices.Add(i * 4 + 0);
            borderMesh.Indices.Add(i * 4 + 1);
            borderMesh.Indices.Add(i * 4 + 2);
            borderMesh.Indices.Add(i * 4 + 3);
            borderMesh.Indices.Add(i * 4 + 2);
            borderMesh.Indices.Add(i * 4 + 1);
            offset = (offset + length / thickness) % 1;
        }
        return borderMesh;
    }

    private List<int> GenerateSideTriangles(List<int> triangles, int vertexCount)
    {

        var edges = GetSingleEdges(triangles);
        var edgeTriangles = new List<int>();
        for (int i = 0; i < edges.Count; i++)
        {
            var edge = edges[i];
            edgeTriangles.Add(edge.a);
            edgeTriangles.Add(edge.b);
            edgeTriangles.Add(edge.b + vertexCount);
            edgeTriangles.Add(edge.a);
            edgeTriangles.Add(edge.b + vertexCount);
            edgeTriangles.Add(edge.a + vertexCount);
        }
        return edgeTriangles;
    }

    private List<Edge> GetSingleEdges(List<int> triangles)
    {
        var list = new List<Edge>();
        for (int i = 0; i < triangles.Count / 3; i++)
        {
            list.Add(new Edge(triangles[i * 3 + 0], triangles[i * 3 + 1]));
            list.Add(new Edge(triangles[i * 3 + 1], triangles[i * 3 + 2]));
            list.Add(new Edge(triangles[i * 3 + 2], triangles[i * 3 + 0]));
        }
        var singleEdges = new List<Edge>();
        for (int i = 0; i < list.Count; i++)
        {
            if (list.Where(x => x.Equals(list[i])).Count() == 1)
                singleEdges.Add(list[i]);
        }
        for (int i = 1; i < singleEdges.Count; i++)
        {
            var lastEdge = singleEdges[i - 1];
            for (int j = i; j < singleEdges.Count; j++)
            {
                if (singleEdges[j].a == lastEdge.b)
                {
                    var temp = singleEdges[i];
                    singleEdges[i] = singleEdges[j];
                    singleEdges[j] = temp;
                }
            }
        }
        return singleEdges;
    }

    private struct Edge
    {
        public int a, b;
        public Edge(int a, int b) { this.a = a; this.b = b; }
        public static bool operator ==(Edge a, Edge b) => a.Equals(b);
        public static bool operator !=(Edge a, Edge b) => !a.Equals(b);

        public override bool Equals(object obj)
        {
            if (!(obj is Edge))
                return false;
            var edge = (Edge)obj;
            return ((this.a == edge.a) && (this.b == edge.b)) || ((this.a == edge.b) && (this.b == edge.a));
        }
        public override int GetHashCode()
        {
            return a * b + a + b;
        }
    }

    private List<int> FlipTriangles(List<int> tris)
    {
        for (int i = 0; i < tris.Count / 3; i++)
        {
            int index = i * 3;
            var temp = tris[index + 1];
            tris[index + 1] = tris[index + 0];
            tris[index + 0] = temp;
        }
        return tris;
    }
}
