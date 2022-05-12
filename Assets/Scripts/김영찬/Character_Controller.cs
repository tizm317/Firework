using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Controller: MonoBehaviour
{
    // Start is called before the first frame update
    private Transform character;//플레이어 객체를 만들면 사용할 예정
    private Rigidbody player;
    private Transform cameraArm;
    private Transform cam;
    public static Vector3 aim;
    private float camera_location;
    private GameObject angle;
    public Animator animator;
    public GameObject Light;
    //상훈 1127
    int healthPoint;
    private bool jump = false;
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Rigidbody>();
        character = GameObject.Find("Jammo_Player").GetComponent<Transform>();
        cameraArm = GameObject.Find("Camera_Arm").GetComponent<Transform>();//카메라 암을 회전시켜주기위해 카메라 암 찾아주기
        cam = GameObject.Find("Camera").GetComponent<Transform>();

        healthPoint = 20;
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.singleTon.isGameStarted == false || GameManager.singleTon.isGameOver == true)
        {
            return;
        }

        Camera_Move();
        Character_Move();
    }

    void Camera_Move()
    { 
        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"),Input.GetAxis("Mouse Y"));
        Vector3 camAngle = cameraArm.rotation.eulerAngles;

        aim = new Vector3(cameraArm.forward.x,cameraArm.forward.y,cameraArm.forward.z);
        float limit = camAngle.x - mouseDelta.y;
        if(limit < 180f)
        {
            limit = Mathf.Clamp(limit, -1f, 70f);
        }
        else
        {
            limit = Mathf.Clamp(limit, 295f, 361f);
        }
       
        camera_location += Input.GetAxis("Mouse ScrollWheel")*5;
        camera_location = Mathf.Clamp(camera_location,-10.0f,0.2f);
        if(camera_location > -0.2)
            camera_location = 0.2f;
        cam.localPosition = new Vector3(0,0,camera_location);

        cameraArm.rotation = Quaternion.Euler(limit,camAngle.y + mouseDelta.x, camAngle.z);
    }

    void Character_Move()
    {
        Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")); 
        bool isMove = moveInput.magnitude != 0; 
        if(Input.GetKey(KeyCode.Space))
        {
            if(jump)
            {
                jump = false;
                player.AddForce(new Vector3(0,1,0)*5,ForceMode.Impulse);
                
            }
            
        }
        //Light.SetActive(false);
        
        if (isMove) 
        { 
            //상훈 : 1127 비활
            animator.SetBool("move",true);
            Vector3 lookForward = new Vector3(cameraArm.forward.x, 0f, cameraArm.forward.z).normalized; 
            Vector3 lookRight = new Vector3(cameraArm.right.x, 0f, cameraArm.right.z).normalized; 
            Vector3 moveDir = lookForward * moveInput.y + lookRight * moveInput.x; 
            character.forward = moveDir; 
            transform.position += moveDir * Time.deltaTime * 5f; 
        }
        else
            animator.SetBool("move",false);

    }


    void OnTriggerEnter(Collider other)
    {
        //상훈 : 총알 피격판정
        if (other.gameObject.CompareTag("Bullet"))
        {
            other.gameObject.SetActive(false);
            SoundManager.singleTon.ScreamSoundPlay();
            GameManager.singleTon.HealthChange(--healthPoint);
        }
        
    }

    void OnCollisionEnter(Collision ground)
    {
        if(ground.gameObject.CompareTag("ground"))
            jump = true;
    }
   
}
