using System.Collections;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using RosSharp.RosBridgeClient;

public class RGBDRender : MonoBehaviour
{
    public RGBDMerger subscriber;

    // Mesh stores the positions and colours of every point in the cloud
    // The renderer and filter are used to display it
    Mesh mesh;
    MeshRenderer meshRenderer;
    MeshFilter mf;

    // The size, positions and colours of each of the pointcloud
    public float pointSize = 1f;
    public float radius = 0.05f;

    [Header("MAKE SURE THESE LISTS ARE MINIMISED OR EDITOR WILL CRASH")]
    private Vector3[] positions = new Vector3[] {new Vector3(0, 0, 0)};
    private Color[] colours = new Color[] {new Color(1f, 1f, 1f)};
    private List<int> m_Triangles = new List<int>();
    private List<Vector3> m_UVRs = new List<Vector3>();

    int i, count;

    public Transform offset; // Put any gameobject that faciliatates adjusting the origin of the pointcloud in VR. 

    void Start()
    {
        // Give all the required components to the gameObject
        meshRenderer = gameObject.AddComponent<MeshRenderer>();
        mesh = new Mesh();
        mf = gameObject.AddComponent<MeshFilter>();
        meshRenderer.material = new Material(Shader.Find("Unlit/PointCloudCutout"));
            // Use 32 bit integer values for the mesh, allows for stupid amount of vertices (2,147,483,647 I think?)
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;

        transform.position = offset.position;
        transform.rotation = offset.rotation;
    }

    void UpdateMesh()
    {
        m_UVRs.Clear();
        m_Triangles.Clear();
        mesh.Clear();
        //positions = subscriber.pcl;
        positions = subscriber.GetPCL(3);
        colours = subscriber.GetPCLColor(3);
        if (positions == null || colours == null)
        {
            return;
        }
        
        mesh.vertices = positions;
        mesh.colors = colours;

        count = positions.Length / 4;

        for(i = 0;i < count;i++)
        {
            m_UVRs.Add(new Vector3(0, 0, radius));
            m_UVRs.Add(new Vector3(0, 1, radius));
            m_UVRs.Add(new Vector3(1, 1, radius));
            m_UVRs.Add(new Vector3(1, 0, radius));

            m_Triangles.Add(i + 0);
            m_Triangles.Add(i + 1);
            m_Triangles.Add(i + 2);
            m_Triangles.Add(i + 0);
            m_Triangles.Add(i + 2);
            m_Triangles.Add(i + 3);
        }

        mesh.SetUVs(0, m_UVRs.ToArray());
        mesh.triangles = m_Triangles.ToArray();

        // int[] indices = new int[positions.Length];

        // for (i = 0; i < positions.Length; i++)
        // {
        //     indices[i] = i;
        // }
        int[] indices = Enumerable.Range(0, positions.Length).ToArray();

        mesh.SetIndices(indices, MeshTopology.Quads, 0);
        mf.mesh = mesh;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = offset.position;
        transform.rotation = offset.rotation;
        // meshRenderer.material.SetFloat("_PointSize", pointSize);
        UpdateMesh();
        // Debug.Log(DateTime.Now.ToString("HH:mm:ss.ffffff"));
    }
}