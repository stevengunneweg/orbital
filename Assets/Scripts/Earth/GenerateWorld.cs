using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateWorld : MonoBehaviour {

    MeshFilter _meshfilter;
    public float roughness = 1;
    public float noisesScale = 1;

    public Vector3 offset;

    [SerializeField]
    private List<GameObject> _treesPrefabs;
    [SerializeField]
    private List<GameObject> _buildingPrefabs;
    [SerializeField]
    private GameObject _person;

    private List<Vector3> z_vertices = new List<Vector3>();
    private List<int> seaLevel_index =  new List<int>();
    private List<Vector3> cityPos = new List<Vector3>();

    private float chanceToSpawnCity = 0.5f;

    [ContextMenu("Randomize")]
    void RandomizeOffset()
    {
        offset = new Vector3(
            Random.value,
            Random.value,
            Random.value
            ) * 1000;
    }
    void Awake()
    {
        _meshfilter = CreateShereMesh();
        Generate();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.G))
            Generate();

    }
    void Generate()
    {
        Vector3[] vertices = _meshfilter.mesh.vertices;
        int zi = 0;
        for (int i = 0; i < vertices.Length;i++)
        {
            vertices[i] += _meshfilter.mesh.normals[i] * ((Perlin.Noise(offset+vertices[i]* noisesScale) /255f)* roughness);
            
            if(vertices[i].z<= 0.01&& vertices[i].z >= -0.01)
            {
                z_vertices.Add(vertices[i]);
                zi++;
                float height = CalculateHeight(vertices[i]);
                if (height >= 0.5f && height < 2f)
                    seaLevel_index.Add(zi);
            }
        }
        _meshfilter.mesh.vertices = vertices;
        _meshfilter.mesh.RecalculateBounds();
        SpawnBuilding();
        SpawnPerson();
    }

    void SpawnPerson()
    {
        foreach (Vector3 citypos in cityPos)
        {
            GameObject go = Instantiate(_person);
            Vector3 pos = citypos;
            go.transform.position = pos- Vector3.forward * 0.25f;
            go.transform.up = citypos;
        }
    }
    void SpawnBuilding()
    {
        foreach(int index in seaLevel_index)
        {
            if (Random.value >= chanceToSpawnCity)
            {
                GameObject go = Instantiate(_buildingPrefabs[(int)(Random.value*2)]);
                Vector3 pos = CalculatePos(z_vertices[index], go);
                go.transform.position = go.transform.position+Vector3.forward*0.5f;
                cityPos.Add(pos);

            }
        }

    }
    void SpawnTree()
    {

    }
    Vector3 CalculatePos(Vector3 vertex,GameObject go)
    {
        Vector3 pos = vertex;
        pos.x *= transform.localScale.x + (go.transform.localScale.x / 2) - 0.1f;
        pos.y *= transform.localScale.y + (go.transform.localScale.y / 2) - 0.1f;
        go.transform.position = pos;
        go.transform.up = pos;
        return pos;
    }

    float CalculateHeight(Vector3 vertex)
    {
        return (Vector3.Distance(Vector3.zero, vertex)-1)*100;
    }

    MeshFilter CreateShereMesh()
    {
        MeshFilter filter = gameObject.AddComponent<MeshFilter>();
        Mesh mesh = filter.mesh;
        mesh.Clear();

        float radius = 1f;
        // Longitude |||
        int nbLong = 24*3;
        // Latitude ---
        int nbLat = 16*3;

        #region Vertices
        Vector3[] vertices = new Vector3[(nbLong + 1) * nbLat + 2];
        float _pi = Mathf.PI;
        float _2pi = _pi * 2f;

        vertices[0] = Vector3.up * radius;
        for (int lat = 0; lat < nbLat; lat++)
        {
            float a1 = _pi * (float)(lat + 1) / (nbLat + 1);
            float sin1 = Mathf.Sin(a1);
            float cos1 = Mathf.Cos(a1);

            for (int lon = 0; lon <= nbLong; lon++)
            {
                float a2 = _2pi * (float)(lon == nbLong ? 0 : lon) / nbLong;
                float sin2 = Mathf.Sin(a2);
                float cos2 = Mathf.Cos(a2);

                vertices[lon + lat * (nbLong + 1) + 1] = new Vector3(sin1 * cos2, cos1, sin1 * sin2) * radius;
            }
        }
        vertices[vertices.Length - 1] = Vector3.up * -radius;
        #endregion

        #region Normales		
        Vector3[] normales = new Vector3[vertices.Length];
        for (int n = 0; n < vertices.Length; n++)
            normales[n] = vertices[n].normalized;
        #endregion

        #region UVs
        Vector2[] uvs = new Vector2[vertices.Length];
        uvs[0] = Vector2.up;
        uvs[uvs.Length - 1] = Vector2.zero;
        for (int lat = 0; lat < nbLat; lat++)
            for (int lon = 0; lon <= nbLong; lon++)
                uvs[lon + lat * (nbLong + 1) + 1] = new Vector2((float)lon / nbLong, 1f - (float)(lat + 1) / (nbLat + 1));
        #endregion

        #region Triangles
        int nbFaces = vertices.Length;
        int nbTriangles = nbFaces * 2;
        int nbIndexes = nbTriangles * 3;
        int[] triangles = new int[nbIndexes];

        //Top Cap
        int i = 0;
        for (int lon = 0; lon < nbLong; lon++)
        {
            triangles[i++] = lon + 2;
            triangles[i++] = lon + 1;
            triangles[i++] = 0;
        }

        //Middle
        for (int lat = 0; lat < nbLat - 1; lat++)
        {
            for (int lon = 0; lon < nbLong; lon++)
            {
                int current = lon + lat * (nbLong + 1) + 1;
                int next = current + nbLong + 1;

                triangles[i++] = current;
                triangles[i++] = current + 1;
                triangles[i++] = next + 1;

                triangles[i++] = current;
                triangles[i++] = next + 1;
                triangles[i++] = next;
            }
        }

        //Bottom Cap
        for (int lon = 0; lon < nbLong; lon++)
        {
            triangles[i++] = vertices.Length - 1;
            triangles[i++] = vertices.Length - (lon + 2) - 1;
            triangles[i++] = vertices.Length - (lon + 1) - 1;
        }
        #endregion

        mesh.vertices = vertices;
        mesh.normals = normales;
        mesh.uv = uvs;
        mesh.triangles = triangles;

        mesh.RecalculateBounds();
        return filter;
    }
}
