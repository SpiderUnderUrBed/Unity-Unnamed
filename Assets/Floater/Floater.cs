using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floater : MonoBehaviour
{
    public Rigidbody rigidBody;
    public float depthBeforeSubmerged = 1f;
    public float displacementAmount = 3f;
    public int floaterCount = 1;
    public float waterDrag = 0.99f;
    public float waterAngularDrag = 0.5f;
    public float maxSphereHeight = 5.0f;
    

 //   Vector3 position = transform.position;
   // public float minPitch = -45f;
   // public float maxPitch = 45f;
   // public float minYaw = -45f;
   // public float maxYaw = 45f;
    //float waveHeight = 0f;
    //float pitch = 0f;
    //float roll = 0f;

private void FixedUpdate() {
    WaterManager.instance.GetWaveHeight(transform.position.x, transform.position.z, out float waveHeight, out float pitch, out float roll);

    Vector3 localXAxis = transform.TransformDirection(new Vector3(Mathf.Cos(pitch), 0, -Mathf.Sin(pitch)));
    Vector3 localZAxis = transform.TransformDirection(new Vector3(Mathf.Sin(roll), 0, Mathf.Cos(roll)));
    Vector3 upAxis = transform.up;
    Vector3 surfacePosition = new Vector3(transform.position.x, waveHeight, transform.position.z);

    rigidBody.AddForceAtPosition(Physics.gravity / floaterCount, transform.position, ForceMode.Acceleration);

    if (transform.position.y < waveHeight) {
        float displacementMultiplier = Mathf.Clamp01((waveHeight - transform.position.y) / depthBeforeSubmerged) * displacementAmount;
        Vector3 force = upAxis * Mathf.Abs(Physics.gravity.y) * displacementMultiplier;
        rigidBody.AddForceAtPosition(force, transform.position, ForceMode.Acceleration);
        rigidBody.AddForce(-rigidBody.velocity * displacementMultiplier * waterDrag, ForceMode.VelocityChange);
        rigidBody.AddTorque(-rigidBody.angularVelocity * displacementMultiplier * waterAngularDrag, ForceMode.VelocityChange);
        rigidBody.AddTorque(Vector3.Cross(transform.up, upAxis) * displacementMultiplier * waterAngularDrag, ForceMode.VelocityChange);
        rigidBody.AddTorque(localXAxis * roll * displacementMultiplier * waterAngularDrag, ForceMode.VelocityChange);
        rigidBody.AddTorque(localZAxis * pitch * displacementMultiplier * waterAngularDrag, ForceMode.VelocityChange);
        //Debug.Log(localXAxis * roll * displacementMultiplier * waterAngularDrag + " " + localZAxis * pitch * displacementMultiplier * waterAngularDrag);

    }
    /*
    if (sphereHeight > maxSphereHeight) {
    float excessHeight = sphereHeight - maxSphereHeight;
    float restoringForceMagnitude = excessHeight * Physics.gravity.magnitude * rigidBody.mass;
    float dampingFactor = Mathf.Clamp01((sphereHeight - maxSphereHeight) / (maxSphereHeight * excessHeight));
    Vector3 restoringForce = -restoringForceMagnitude * dampingFactor * transform.up;

    if (sphereHeight > maxSphereHeight + hardLimitMargin) {
        // Apply a strong restoring force to bring the sphere back to the hard limit height
        restoringForce *= Mathf.Clamp01((sphereHeight - maxSphereHeight - hardLimitMargin) / hardLimitMargin) * hardLimitFactor;
    }

    rigidBody.AddForce(restoringForce, ForceMode.Force);
}
*/

    /*
    float sphereHeight = transform.position.y;
        if (sphereHeight > maxSphereHeight) {
          float excessHeight = sphereHeight - maxSphereHeight;
          float restoringForceMagnitude = excessHeight * Physics.gravity.magnitude * rigidBody.mass;
          float dampingFactor = Mathf.Clamp01((sphereHeight - maxSphereHeight) / (maxSphereHeight * 1f));
          Vector3 restoringForce = -restoringForceMagnitude * dampingFactor * transform.up;
           rigidBody.AddForce(restoringForce, ForceMode.Force);
           Debug.Log(restoringForce);
        }
        */
    //float x = rigidBody.transform.eulerAngles.x;
    //float z = rigidBody.transform.eulerAngles.z;
    //transform.eulerAngles = new Vector3(Mathf.Clamp(x, x, x), rigidBody.transform.eulerAngles.y, Mathf.Clamp(z, z, z));

/*
    float x = rigidBody.transform.eulerAngles.x;
    float z = rigidBody.transform.eulerAngles.z;
    if (x > 80 && x < 270) {
        rigidBody.transform.eulerAngles = new Vector3(Mathf.Clamp(x, 80, 180), transform.eulerAngles.y, z);
    }
    if (z > 80 && z < 270) {
        rigidBody.transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, Mathf.Clamp(z, 180, 270));
    }
    */
     
}



    void Start()
    {
       
    }

    void Update()
    {

    }
}
