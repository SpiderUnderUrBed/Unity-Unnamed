using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
  /*
  public static WaveManager instance;
  public float amplitude = 1f;
  public float length = 2f;
  public float speed = 1f;
  public float offset = 0f;
  //private WaterManager waterManager;
  private float waves;
   
  private void Awake() {
    
    if (instance == null){
    instance = this;
   }   else if (instance != this){
    Debug.Log("Instance already exists, destroying object");
    Destroy(this);
   }
    }
   private void Start() {
    List<Wave> waves = WaterManager.instance.waves;
    //Debug.Log(waves);
   }
  private void Update() {
    offset += Time.deltaTime * speed;
  }

public void GetWaveHeight(float _x, float _z, out float waveHeight, out float pitch, out float roll)
{
    float x = (_x / length + offset) * Mathf.PI * 2f + Time.time * speed;
    float z = (_z / length + offset) * Mathf.PI * 2f + Time.time * speed;

    waveHeight = amplitude * Mathf.Sin(x) * Mathf.Sin(z);
    pitch = amplitude * Mathf.Cos(x) * Mathf.Cos(z);
    roll = amplitude * Mathf.Sin(x) * Mathf.Cos(z);
}
*/
}
