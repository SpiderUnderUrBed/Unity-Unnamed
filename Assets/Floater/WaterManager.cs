using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]


public class WaterManager : MonoBehaviour
{
    private MeshFilter meshFilter;
    public static WaterManager instance;
    public List<Wave> waves = new List<Wave>();
   // public float daDistance = 20f;
    public float amplitude = 0.01f;
    public float length = 0.5f;
    public float speed = 0.1f;
    public float offset = 0f;
    public float variation = 5f;
 
     // [... all other vertex data arrays you need]
 
     static List<int> indices;
     static Dictionary<uint,int> newVectices;
 
     static int GetNewVertex(int i1, int i2)
     {
         // We have to test both directions since the edge
         // could be reversed in another triangle
         uint t1 = ((uint)i1 << 16) | (uint)i2;
         uint t2 = ((uint)i2 << 16) | (uint)i1;
         if (newVectices.ContainsKey(t2))
             return newVectices[t2];
         if (newVectices.ContainsKey(t1))
             return newVectices[t1];
         // generate vertex:
         int newIndex = vertices.Count;
         newVectices.Add(t1,newIndex);
 
         // calculate new vertex
         vertices.Add((vertices[i1] + vertices[i2]) * 0.5f);
         normals.Add((normals[i1] + normals[i2]).normalized);
         // [... all other vertex data arrays]
 
         return newIndex;
     }
 
 
     public static void Subdivide(Mesh mesh)
     {
         newVectices = new Dictionary<uint,int>();
 
         vertices = new List<Vector3>(mesh.vertices);
         normals = new List<Vector3>(mesh.normals);
         // [... all other vertex data arrays]
         indices = new List<int>();
 
         int[] triangles = mesh.triangles;
         for (int i = 0; i < triangles.Length; i += 3)
         {
             int i1 = triangles[i + 0];
             int i2 = triangles[i + 1];
             int i3 = triangles[i + 2];
 
             int a = GetNewVertex(i1, i2);
             int b = GetNewVertex(i2, i3);
             int c = GetNewVertex(i3, i1);
             indices.Add(i1);   indices.Add(a);   indices.Add(c);
             indices.Add(i2);   indices.Add(b);   indices.Add(a);
             indices.Add(i3);   indices.Add(c);   indices.Add(b);
             indices.Add(a );   indices.Add(b);   indices.Add(c); // center triangle
         }
         mesh.vertices = vertices.ToArray();
         mesh.normals = normals.ToArray();
         // [... all other vertex data arrays]
         mesh.triangles = indices.ToArray();
 
         // since this is a static function and it uses static variables
         // we should erase the arrays to free them:
         newVectices = null;
         vertices = null;
         normals = null;
         // [... all other vertex data arrays]
 
         indices = null;
     }
  
public int newVertexCount = 1000;
     static List<Vector3> vertices;
     static List<Vector3> normals;
     // [... all other vertex data arrays you need]

private void Start()
{
    var mesh = GetComponent<MeshFilter>().mesh;
    //var vertices = mesh.vertices;
    //var triangles = mesh.triangles;
   // Subdivide(mesh);
 //  Subdivide(mesh);
   //Subdivide(mesh);
}

    private void Awake()
    {  
        //originVerticies = GetComponent<MeshFilter>().mesh.vertices.ToList();
       // renderer = gameObject.GetComponent<Renderer>();
        meshFilter = GetComponent<MeshFilter>();
        //newVertices = GetComponent<MeshFilter>().mesh.vertices.ToList();
        instance = this;
        

        //10 waves
        waves.Add(new Wave(0.01f, 0.1f, 0.8f, 0f));
        waves.Add(new Wave(-0.01f, 0.2f, 1.2f, 1.2f));
        waves.Add(new Wave(-0.01f, 0.3f, 0.8f, -2f));
        waves.Add(new Wave(-1.0f, 0.4f, 1.5f, 3f));
        waves.Add(new Wave(-0.05f, 0.5f, 0.3f, -1f));
        waves.Add(new Wave(0.06f, 0.1f, 1.0f, 0f));
        waves.Add(new Wave(-0.08f, 0.2f, 1.5f, 1.5f));
        waves.Add(new Wave(-0.1f, 0.3f, 1.0f, -3f));
        waves.Add(new Wave(-1.01f, 0.4f, 2.0f, 4f));
        waves.Add(new Wave(-0.09f, 0.5f, 0.5f, -1.5f));

     if (instance == null){
        instance = this;
      }   else if (instance != this){
         Debug.Log("Instance already exists, destroying object");
         Destroy(this);
      }
    }



private void Update()
{

}

   public void GetWaveHeight(float x, float z, out float waveHeight, out float pitch, out float roll)
{  
    float foundWaveHeight = 0f;
    float foundWavePitch = 0f;
    float foundWaveRoll = 0f;
   
    foreach (Wave wave in WaterManager.instance.waves)
    {
        float dx = x - wave.offset;
        float dz = z - wave.offset;
        float dist = Mathf.Sqrt(dx * dx + dz * dz);
        float wavePhase = dist / wave.length + Time.time * wave.speed + instance.offset;
        foundWaveHeight += Mathf.Sin(wavePhase * Mathf.PI * 2f) * wave.amplitude;

        float waveDx, waveDz;
        wave.GetWaveSin(x, z, out waveHeight, out waveDx, out waveDz);
        foundWavePitch += waveDz;
        foundWaveRoll += waveDx;
    }


    waveHeight = foundWaveHeight;
    pitch = foundWavePitch;
    roll = foundWaveRoll;


   // GameObject otherPlane = GameObject.Find("OtherPlane");
   // MeshFilter otherPlaneMeshFilter = otherPlane.GetComponent<MeshFilter>();
   // Mesh finalMesh = BezierCurveExtractor.instance.GetRadiusMesh(new Vector3(x, 0, z),2f);
  //  otherPlaneMeshFilter.mesh = finalMesh;
   
}


public class Wave
{
    public float amplitude;
    public float length;
    public float speed;
    public float offset;


    public Wave(float _amplitude, float _length, float _speed, float _offset)
    {
        amplitude = _amplitude;
        length = _length;
        speed = _speed;
        offset = _offset;
    }


public float GetWaveSin(float x, float z, out float waveHeight, out float waveDx, out float waveDz)
{
    float dx = x - offset;
    float dz = z - offset;
    float dist = Mathf.Sqrt(dx * dx + dz * dz);
    float wavePhase = dist / length + Time.time * speed;
    waveHeight = amplitude * Mathf.Sin(wavePhase * 2 * Mathf.PI);
    waveDx = -2 * Mathf.PI * amplitude * speed / length * Mathf.Cos(wavePhase * 2 * Mathf.PI) * dx / dist;
    waveDz = -2 * Mathf.PI * amplitude * speed / length * Mathf.Cos(wavePhase * 2 * Mathf.PI) * dz / dist;
    return waveHeight;
}
}
}