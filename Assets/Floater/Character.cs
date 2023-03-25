using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    private float playerSpeed = 10.0f;
    private float jumpForce = 10.0f;
    public float depthBeforeSubmerged = 1f;
    public float displacementAmount = 100f;
    public int floaterCount = 1;
    public float waterDrag = 0.99f;
    public float waterAngularDrag = 5f;
    public float minPitch = -45f;
    public float maxPitch = 45f;
    public float minYaw = -45f;
    public float maxYaw = 45f;
    public float minRoll = -45;
    public float maxRoll = 45f;
    public float TorqueLessenerYaw = 1000;
    public float TorqueLessenerRoll = 1000;
    public float TorqueLessenerPitch = 1000;
   float TorqueAmount = 1f;
  // public float MaxTorque = 1f;
    float originalPitch;
    float originalRoll;
    float originalYaw;


    public Rigidbody rigidBody;

    private void Start() {
    originalPitch = transform.eulerAngles.x;
    originalYaw = transform.eulerAngles.y;
    originalRoll = transform.eulerAngles.z;
    }
    private void FixedUpdate()
    {
        
        Vector3 position = transform.position;
        float waveHeight = 0f;
        float pitch = 0f;
        float roll = 0f;
        float TorqueInput = Input.GetAxis("Horizontal");
        WaterManager.instance.GetWaveHeight(position.x, position.z, out waveHeight, out pitch, out roll);

        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        rigidBody.AddForce(movement * playerSpeed, ForceMode.Acceleration);
        //rigidBody.AddForceAtPosition(Physics.gravity / floaterCount, position, ForceMode.Acceleration);

        if (position.y < waveHeight - depthBeforeSubmerged) 
        {
       //     SubRotationLimiter(position, waveHeight);
           ApplyDisplacement(position, waveHeight);
           ApplyBuoyancy(position, waveHeight, TorqueInput);
           ApplyPitchAndRoll(pitch, roll);
           ApplyCounterTorque();
        }
    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            rigidBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
    private void SubRotationLimiter(Vector3 position, float waveHeight){
    // Limit the boat's roll and pitch when submerged
    //rigidBody.angularVelocity = Vector3.zero;
    
    float maxRoll = 40f;
    float rollAngle = Mathf.DeltaAngle(transform.eulerAngles.z, 0f);
    float rollTorque = Mathf.Clamp(-rollAngle, -maxRoll, maxRoll) * TorqueAmount;
    rigidBody.AddRelativeTorque(0f, 0f, rollTorque * Time.fixedDeltaTime, ForceMode.Impulse);
    
    float maxPitch = 40f;
    float pitchAngle = Mathf.DeltaAngle(transform.eulerAngles.x, 0f);
    float pitchTorque = Mathf.Clamp(-pitchAngle, -maxPitch, maxPitch) * TorqueAmount;
    rigidBody.AddRelativeTorque(pitchTorque * Time.fixedDeltaTime, 0f, 0f, ForceMode.Impulse);
}

    private void ApplyDisplacement(Vector3 position, float waveHeight)
    {
        float displacementMultiplier = Mathf.Clamp01((waveHeight - position.y) / depthBeforeSubmerged) * displacementAmount;
    rigidBody.AddForce(new Vector3(0f, Mathf.Abs(Physics.gravity.y) * displacementMultiplier, 0f), ForceMode.Acceleration);
        //float displacementMultiplier = Mathf.Clamp01((waveHeight - position.y) / depthBeforeSubmerged) * displacementAmount;
       // rigidBody.AddForceAtPosition(new Vector3(0f, Mathf.Abs(Physics.gravity.y) * displacementMultiplier, 0f), position, ForceMode.Acceleration);
        /*
        private void ApplyDisplacement(Vector3 position, float waveHeight)
{
    float displacementMultiplier = Mathf.Clamp01((waveHeight - position.y) / depthBeforeSubmerged) * displacementAmount;
    rigidBody.AddForceAtPosition(new Vector3(0f, -Mathf.Abs(Physics.gravity.y) * displacementMultiplier, 0f), position, ForceMode.Acceleration);
}
*/
    }

    private void ApplyBuoyancy(Vector3 position, float waveHeight, float TorqueInput)
    {
        float displacementMultiplier = Mathf.Clamp01((waveHeight - position.y) / depthBeforeSubmerged) * displacementAmount;
        rigidBody.AddForce(displacementMultiplier * -rigidBody.velocity * waterDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);

        float angularDamping = -waterAngularDrag * rigidBody.angularVelocity.magnitude;
        rigidBody.AddRelativeTorque(Vector3.Scale(angularDamping * rigidBody.angularVelocity, new Vector3(0f, TorqueInput * TorqueAmount, 0f)) * Time.fixedDeltaTime, ForceMode.Acceleration);
    }

    private void ApplyPitchAndRoll(float pitch, float roll)
{
    //It removes yaw, and no, I dont want to rename my function to ApplyPitchAndRollRemoveYaw or smthin long
   float yaw = transform.eulerAngles.y;
  //  Quaternion noyaw = Quaternion.Euler(targetPitch, 0f, 0f);
   // rigidBody.MoveRotation(Quaternion.RotateTowards(transform.rotation, noyaw, Time.fixedDeltaTime * 100f));
    //
    float threshold = 0.5f;
  //  Debug.Log(Mathf.Abs(pitch));
  //  Debug.Log(Mathf.Abs(roll));
  //Debug.Log(pitch + " " + roll);
  
  
    if (Mathf.Abs(pitch) > threshold || Mathf.Abs(pitch) < -threshold)
    {
        float targetPitch = Mathf.Clamp(pitch, minPitch, maxPitch);
     //   Debug.Log(targetPitch + " pitch");
        Quaternion targetRotation = Quaternion.Euler(targetPitch, transform.eulerAngles.y, transform.eulerAngles.z);
       // Debug.Log(transform.rotation + " " + targetRotation + " " + Time.fixedDeltaTime * 100f);
        rigidBody.MoveRotation(Quaternion.RotateTowards(transform.rotation, targetRotation, Time.fixedDeltaTime * 100f));
       // Debug.Log(rigidBody.angularVelocity);

    }
    /*
    Vector3 currentAngularVelocity = rigidBody.angularVelocity + new Vector3(TorqueLessenerRoll, TorqueLessenerYaw, TorqueLessenerPitch);
    counterTorque = -currentAngularVelocity;
    Debug.Log(counterTorque);
    
    */

    if (Mathf.Abs(roll) > threshold || Mathf.Abs(roll) < -threshold)
    {
        float targetRoll = Mathf.Clamp(roll, minRoll, maxRoll);
      //  Debug.Log(targetRoll + " roll");
        Quaternion targetRotation2 = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, targetRoll);
        //Debug.Log(transform.eulerAngles.x + " " + (transform.eulerAngles.y + targetRoll) + " " + 0f);
         //Debug.Log(transform.rotation + " " + targetRotation2 + " " + Time.fixedDeltaTime * 100f);
        rigidBody.MoveRotation(Quaternion.RotateTowards(transform.rotation, targetRotation2, Time.fixedDeltaTime * 100f));
        //Debug.Log(rigidBody.angularVelocity);
    }

    if (Mathf.Abs(yaw) < threshold){
    float targetPitch2 = Mathf.Clamp(yaw, minYaw, maxYaw);
    Quaternion targetRotation3 = Quaternion.Euler(targetPitch2, 0f, 0f);
    rigidBody.MoveRotation(Quaternion.RotateTowards(transform.rotation, targetRotation3, Time.fixedDeltaTime * 100f));
    }
}
private void ApplyCounterTorque()
{

 Vector3 counterTorque = Vector3.zero;
    Vector3 currentAngularVelocity = rigidBody.angularVelocity + new Vector3(TorqueLessenerRoll, TorqueLessenerYaw, TorqueLessenerPitch);
    counterTorque = -currentAngularVelocity;
  //  Debug.Log(counterTorque);
rigidBody.AddRelativeTorque(counterTorque, ForceMode.Acceleration);

//rigidBody.AddRelativeTorque(new Vector3(0,0,0), ForceMode.Acceleration);
}
}
    /*
    float yawDifference = transform.eulerAngles.y - originalYaw;
    float pitchDifference = transform.eulerAngles.x - originalPitch;
    float rollDifference = transform.eulerAngles.z - originalRoll;

    float yawTorque = (-yawDifference * TorqueAmount) + TorqueLessenerYaw;
    float pitchTorque = (-pitchDifference * TorqueAmount) + TorqueLessenerPitch;
    float rollTorque = (-rollDifference * TorqueAmount) + TorqueLessenerRoll;
    Vector3 sum = new Vector3(yawTorque, pitchTorque, rollTorque);
Debug.Log(sum);
    
    // Apply the torques to the rigidbody
rigidBody.AddRelativeTorque(sum, ForceMode.Acceleration);
Debug.Log("Current orientation: " + transform.eulerAngles);
*/
/*
yawDifference = transform.eulerAngles.y - originalYaw;
pitchDifference = transform.eulerAngles.x - originalPitch;
rollDifference = transform.eulerAngles.z - originalRoll;
// Clamp the torques to a maximum value to avoid over-spinning
yawTorque = Mathf.Clamp(yawTorque, -MaxTorque, MaxTorque);
pitchTorque = Mathf.Clamp(pitchTorque, -MaxTorque, MaxTorque);
rollTorque = Mathf.Clamp(rollTorque, -MaxTorque, MaxTorque);
sum = new Vector3(yawTorque, pitchTorque, rollTorque);
Debug.Log(sum);

// Apply the clamped torques to the rigidbody
rigidBody.AddRelativeTorque(pitchTorque, yawTorque, rollTorque, ForceMode.Acceleration);
Debug.Log("Current orientation: " + transform.eulerAngles);
*/
   // yawDifference = transform.eulerAngles.y - originalYaw;
   // pitchDifference = transform.eulerAngles.x - originalPitch;
   // rollDifference = transform.eulerAngles.z - originalRoll;

   // yawTorque = -yawDifference * TorqueAmount;
   // pitchTorque = -pitchDifference * TorqueAmount;
   // rollTorque = -rollDifference * TorqueAmount;
   // rigidBody.AddRelativeTorque(pitchTorque, yawTorque, rollTorque, ForceMode.Acceleration);
   /*
   Vector3 counterTorque = Vector3.zero;
    Vector3 currentAngularVelocity = rigidBody.angularVelocity;
    counterTorque = -currentAngularVelocity;
    //* TorqueAmount;
    Debug.Log(counterTorque);
    //Debug.Log(currentAngularVelocity);
    rigidBody.AddRelativeTorque(counterTorque, ForceMode.Acceleration);
    Debug.Log("Current orientation: " + transform.eulerAngles);
    */
   
   // Vector3 counterTorque = Vector3.zero;
   // Vector3 currentAngularVelocity = rigidBody.angularVelocity;
   // counterTorque = -currentAngularVelocity;
    //* TorqueAmount;
   // Debug.Log(counterTorque);
    //Debug.Log(currentAngularVelocity);
/*
    Debug.Log(-transform.eulerAngles);
    rigidBody.AddRelativeTorque(-transform.eulerAngles, ForceMode.Acceleration);
    Debug.Log("Current orientation: " + transform.eulerAngles);
*/
/*
Debug.Log(transform.eulerAngles);
rigidBody.AddRelativeTorque(transform.eulerAngles, ForceMode.Acceleration);
*/