using UnityEngine;

public class lerpPoints : MonoBehaviour
{
    public GameObject spherePrefab;
    public GameObject BLeftOrigin;
    public GameObject BRightOrigin; 
    public GameObject bottomLeftSphere;
    public GameObject bottomRightSphere;
   // private float lerpSpeed
    private GameObject lerpObj1;
    private Vector3 lerpObj1Origin;
    private GameObject lerpObj2;
    private Vector3 lerpObj2Origin;
    private GameObject follow;
    private Vector3 followOrigin;
    private Vector3 bottomLeftSphereOrigin;
    private Vector3 bottomRightSphereOrigin;
    //public GameObject targetSphere;
    public float yOffset = -50f;
    public float lerpSpeed = 2f;
    float randomFloat = 0f;

    private GameObject middleSphere;
    private Transform targetSphere;
    private bool isLerping = false;
    private float timer = 0f;
    private float interval = 5f; 
    
    private void Start() {
        bottomLeftSphere = Instantiate(spherePrefab, new Vector3(BLeftOrigin.transform.position.x, BLeftOrigin.transform.position.y +10f, BLeftOrigin.transform.position.z), Quaternion.identity);
        bottomRightSphere = Instantiate(spherePrefab, new Vector3(BRightOrigin.transform.position.x, BRightOrigin.transform.position.y+10f, BRightOrigin.transform.position.z), Quaternion.identity);
        //
        bottomRightSphereOrigin = new Vector3(bottomRightSphere.transform.position.x, BRightOrigin.transform.position.y+10f, bottomRightSphere.transform.position.z);
        bottomLeftSphereOrigin = new Vector3(bottomLeftSphere.transform.position.x, BLeftOrigin.transform.position.y +10f, bottomLeftSphere.transform.position.z);
        //
        float middleY = ((bottomRightSphere.transform.position.y + bottomLeftSphere.transform.position.y) / 2) + yOffset;
       // middleY = Mathf.Clamp(middleY, Mathf.Min(bottomRightSphere.transform.position.y, bottomLeftSphere.transform.position.y) + yOffset/2, Mathf.Max(bottomRightSphere.transform.position.y, bottomLeftSphere.transform.position.y) + yOffset/2) - yOffset;
        middleSphere = Instantiate(spherePrefab, new Vector3((bottomRightSphere.transform.position.x+bottomLeftSphere.transform.position.x)/2,  middleY, (bottomRightSphere.transform.position.z+bottomLeftSphere.transform.position.z)/2), Quaternion.identity);

        //
        lerpObj1 = Instantiate(spherePrefab, new Vector3((bottomLeftSphereOrigin.x+middleSphere.transform.position.x)/2, (bottomLeftSphereOrigin.y+middleSphere.transform.position.y)/2, (bottomLeftSphereOrigin.z+middleSphere.transform.position.z)/2), Quaternion.identity);
        lerpObj1Origin = lerpObj1.transform.position;
        lerpObj2 = Instantiate(spherePrefab, new Vector3((bottomRightSphereOrigin.x+middleSphere.transform.position.x)/2, (bottomRightSphereOrigin.y+middleSphere.transform.position.y)/2, (bottomRightSphereOrigin.z+middleSphere.transform.position.z)/2), Quaternion.identity);
        lerpObj2Origin = lerpObj2.transform.position;
        //
follow = Instantiate(spherePrefab, new Vector3((lerpObj1.transform.position.x+bottomLeftSphere.transform.position.x)/2, (lerpObj2.transform.position.y+bottomLeftSphere.transform.position.y)/2, (bottomLeftSphere.transform.position.z+bottomRightSphere.transform.position.z)/2), Quaternion.identity);
followOrigin = follow.transform.position;

MeshRenderer meshRenderer = follow.GetComponent<MeshRenderer>();
meshRenderer.material.color = Color.red;
MeshRenderer lerp1 = lerpObj1.GetComponent<MeshRenderer>();
lerp1.material.color = Color.green;
MeshRenderer lerp2 = lerpObj2.GetComponent<MeshRenderer>();
lerp2.material.color = Color.green;

      //  Debug.Log("log " + ((bottomRightSphere.transform.position.y-bottomLeftSphere.transform.position.y)/2)+yOffset);
      //  Debug.Log(Mathf.Clamp(((bottomRightSphere.transform.position.y-bottomLeftSphere.transform.position.y)/2)+yOffset, bottomRightSphere.transform.position.y+yOffset, bottomLeftSphere.transform.position.y+yOffset));
    }
private void Update() {
    timer += Time.deltaTime * lerpSpeed;
    float pingPongedTime = Mathf.PingPong(timer, interval);
    if (timer > interval) {
        if (!isLerping) {
            isLerping = true;
            if (randomFloat == 0){
            randomFloat = Random.value;
            } else if (randomFloat > 0.5) {
             randomFloat = 0.3f;
            } else {
                randomFloat = 0.6f;
            }
        }
        
        Vector3 startPoint = middleSphere.transform.position;
        Vector3 endPoint;
        if (randomFloat > 0.5f) {
            endPoint = new Vector3(bottomLeftSphere.transform.position.x, startPoint.y, bottomLeftSphere.transform.position.z);
            lerpObj1.transform.position = Vector3.Lerp(lerpObj1.transform.position, bottomLeftSphere.transform.position, (Time.deltaTime * pingPongedTime) / lerpSpeed);
            lerpObj2.transform.position = Vector3.Lerp(lerpObj2.transform.position, middleSphere.transform.position, (Time.deltaTime * pingPongedTime) / lerpSpeed);

            follow.transform.position = Vector3.Lerp(follow.transform.position, lerpObj1.transform.position, (Time.deltaTime * pingPongedTime) / lerpSpeed);

            bottomRightSphere.transform.position = Vector3.Lerp(bottomRightSphere.transform.position, new Vector3(bottomRightSphere.transform.position.x, middleSphere.transform.position.y, bottomRightSphere.transform.position.z), (Time.deltaTime * pingPongedTime) / lerpSpeed);
        } else {
            endPoint = new Vector3(bottomRightSphere.transform.position.x, startPoint.y, bottomRightSphere.transform.position.z);
            lerpObj1.transform.position = Vector3.Lerp(lerpObj1.transform.position, middleSphere.transform.position, (Time.deltaTime * pingPongedTime) / lerpSpeed);
            lerpObj2.transform.position = Vector3.Lerp(lerpObj2.transform.position, bottomRightSphere.transform.position, (Time.deltaTime * pingPongedTime) / lerpSpeed);
            follow.transform.position = Vector3.Lerp(follow.transform.position, lerpObj2.transform.position, (Time.deltaTime * pingPongedTime) / lerpSpeed);
            bottomLeftSphere.transform.position = Vector3.Lerp(bottomLeftSphere.transform.position, new Vector3(bottomLeftSphere.transform.position.x, middleSphere.transform.position.y, bottomLeftSphere.transform.position.z), (Time.deltaTime * pingPongedTime) / lerpSpeed);
        }
        middleSphere.transform.position = Vector3.Lerp(startPoint, endPoint, (Time.deltaTime * pingPongedTime) / lerpSpeed);
        if (pingPongedTime >= interval - 0.1f) {
           if (randomFloat < 0.5f){
            bottomRightSphere.transform.position = bottomRightSphereOrigin;
           } else {
            bottomLeftSphere.transform.position = bottomLeftSphereOrigin;
           }
           lerpObj1.transform.position = lerpObj1Origin;
           lerpObj2.transform.position = lerpObj2Origin;
           follow.transform.position = followOrigin;
            isLerping = false;
        }
    }
}
}
