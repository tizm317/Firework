using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;   // For using NavMeshAgent.
using UnityEngine.UI;
public class EnemyController : MonoBehaviour
{
    public float amplitude; // 진폭.
    private float initYpos; // 너무 많이 안내려가게.
    public Vector3 tempPosition;
    private float verticalTimer, waitingtime;
    public float verticalSpeed;
    public float Speed = 2;
    public float rollSpeed = 90.0f;
    public float rotateSpeed = 10.0f;
    public Slider slider;
    [SerializeField] private float explosionRadius,explosionForce;
    [SerializeField] private float delay;
    private int cubesInRow = 5;
    void Start(){
        tempPosition = transform.position;
        initYpos = transform.position.y;
        verticalTimer = 0.0f;
        waitingtime = 1.0f;
        verticalSpeed = 1.0f;
        //상훈 1127 비활
        //slider.value = 1;
    }

    void FixedUpdate(){
        verticalTimer += Time.deltaTime;

        if(verticalTimer > waitingtime){    // 1초 마다 진폭 갱신.
            amplitude = Random.Range(-3.0f,3.0f);
            verticalTimer = 0;
        }

        CheckMaxAmp();        
        MoveCircle();
    }

    void CheckMaxAmp(){
        // -4 만큼 내려가면, 더이상 내려가지 않게 진폭을 조절해준다.
         if(transform.position.y  < initYpos - 4){
            amplitude = 2;
        }
        float moveup = Mathf.Sin(Time.deltaTime * verticalSpeed) * amplitude;
        transform.position += new Vector3(0,moveup,0);
        transform.Rotate(-moveup * Time.deltaTime * rollSpeed,0,0,Space.Self);
    }

    void MoveCircle(){
        transform.Rotate(0,rollSpeed * Time.deltaTime,0,Space.Self);
        transform.position += transform.forward * Time.deltaTime * rotateSpeed;
        transform.position += transform.right * Time.deltaTime * rotateSpeed;
    }

    // Explosion.
    void ExplodeAndDestroy(GameObject gameObject){
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other){
        if (other.gameObject.CompareTag("FireWork"))
        {
            explode();
        }
        
    }
    public void explode(){
        //상훈 : 사운드넣음
        SoundManager.singleTon.CrashSoundPlay();
        gameObject.SetActive(false);

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