using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyMoveForwardController : MonoBehaviour
{
    private float seconds = 2;
    public float RayRange = 30f;
    private bool isRay = false;
    public Transform rocket;
     [SerializeField] private float verticalSpeed = 10.0f;
     [SerializeField] private float moveSpeed = 3.0f;
    private float rollSpeed = 90.0f;
    private float amplitude = 0.2f;
    private float rotateamount = 0;
    private float distance;
    public float maxdistance = 100f;
    public Slider slider;

    // for explosion
    [SerializeField] private float explosionRadius,explosionForce;
    [SerializeField] private float delay;
    private int cubesInRow = 5;
    private float timer = -1;
    // Start is called before the first frame update
    void Start()
    {
        distance = 0;
        //상훈 1127비활
        //slider.value = 1;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ObjectHit();
        Moveforward();
        if(timer < seconds && isRay){ // timer starts.
            timer += Time.deltaTime;
            float moveup = Mathf.Sin(Time.deltaTime * verticalSpeed) * amplitude;
            transform.position += new Vector3(0,moveup,0);
            transform.Rotate(-moveup * Time.deltaTime * rollSpeed,0,0,Space.Self);
            rotateamount += moveup * Time.deltaTime * rollSpeed;
        }

        if(timer >= seconds){
            isRay = false;
            transform.Rotate(rotateamount,0,0,Space.Self);
            rotateamount = 0;
        }
    }
    void ObjectHit(){
        // Draw Ray
        RaycastHit hit;

        // If Ray hit the object.
        if(Physics.Raycast(rocket.position, rocket.forward, out hit, RayRange)){
            if(isRay == false){
                timer = 0;
                isRay = true;
            }
        }
    }
    private float count = 0;
    void Moveforward(){
        if(distance > maxdistance){
            transform.Rotate(0,10f,0,Space.Self);
            count += 10f;
            if(count >= 180){
                count = 0;
                distance = 0;
            }
        }
        else {
            transform.position += transform.forward * Time.deltaTime * moveSpeed;
            distance += Time.deltaTime * moveSpeed;
        }
    }
    // code for explode 
    private void OnTriggerEnter(Collider other){
        if (other.gameObject.CompareTag("FireWork"))
        {
            explode();
        }
    }
    public void explode(){
        gameObject.SetActive(false);
        //상훈 1127비활
        //slider.value = 0;
        for(int x = 0; x<cubesInRow; x++){
            for(int y = 0; y<cubesInRow; y++){
                for(int z = 0; z<cubesInRow; z++){
                    createPiece(x,y,z);
                }
            }
        }
        Destroy(gameObject);
    }
    
    // 이거 작동을 안하네;;
    IEnumerator waitAndDestroy(GameObject goj){
        yield return new WaitForSeconds(delay);
        Destroy(goj);
    }
    private void createPiece(int x, int y, int z){
        GameObject piece = GameObject.CreatePrimitive(PrimitiveType.Cube);
        GameObject child = transform.GetChild(0).gameObject;
        Renderer rd = piece.GetComponent<Renderer>();
        Material[] mat = child.GetComponent<Renderer>().materials;
        rd.material = mat[0];

        rd.transform.localScale = transform.localScale / cubesInRow;
        Vector3 firstPiece = transform.position - transform.localScale / 2 + piece.transform.localScale / 2;
        piece.transform.position = firstPiece + Vector3.Scale(new Vector3(x,y,z), piece.transform.localScale);
        Rigidbody rb = piece.AddComponent<Rigidbody>();
        rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
        waitAndDestroy(piece);
    }

}
