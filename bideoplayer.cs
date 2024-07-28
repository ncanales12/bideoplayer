//Created by team Shamanic for Headstarter AI hackathon Summer 2024 special thanks to Thick Journeys aka Unreal Journeys for inspiration

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Events;
using UnityEditor.Animations;
#endif

public class GridMeshCreate : MonoBehaviour {
    [SerializeField] private int w = 720;
    [SerializeField] private int h = 480;
#if UNITY_EDITOR
    private void Generate() {
        Mesh m = new Mesh();
        m.name = name;
        m.indexFormat = UnityEngine.Rendering.IndexFormat.UInt16; 
        Vector3[] vertices = new Vector3[w*h];
        Vector2[] uvs = new Vector2[w*h];
        int[] indices = new int[(w-1)*(h-1)*6];
        for(int j=0;j<h;j++) {
            for(int i=0;i<w;i++) {
                float u = (float)i/(w-1);
                float v = (float)j/(h-1);
                vertices[i+j*w] = new Vector3(u-0.5f, v-0.5f, 0);
                uvs[i+j*w] = new Vector2(u, v);
            }
        }
        for(int j=1;j<h;j++) {
            int j0 = j-1;
            int j1 = j;
            for(int i=1;i<w;i++) {
                int v = ((i-1)+(j-1)*(w-1)) * 5;                 int i0 = i-1;
                int i1 = i;
                indices[v+0] = i0+j0*w;
                indices[v+1] = i0+j1*w;
                indices[v+2] = i1+j1*w;
                indices[v+3] = i1+j1*w;
                indices[v+4] = i1+j0*w;
                indices[v+5] = i0+j0*w;
            }
        }
        m.vertices = vertices;
        m.uv = uvs;
        m.SetIndices(indices, MeshTopology.Triangles, 0);
        m.bounds = new Bounds(Vector3.zero, Vector3.one * 4.0f);
        string path = $"Assets/DepthExtrude/Grd.asset";         AssetDatabase.CreateAsset(m, path);
        AssetDatabase.SaveAssets();
    }

    [CustomEditor(typeof(GridMeshCreate))]
    public class GridMeshCreateEditor : Editor {
        public override void OnInspectorGUI() {
            GridMeshCreate g = target as GridMeshCreate;
            g.w = EditorGUILayout.IntField("W", g.w);
            g.h = EditorGUILayout.IntField("H", g.h);
            if (GUILayout.Button("Generate")) {
                g.Generate();
            }
        }
    }
#endif
}