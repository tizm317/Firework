using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    Vector3 startPos;
    Vector3 playerPos;
    float speed;
    float timer;
    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //상훈 : timer에 speed값으로 나누지 않으면 무조건 1초안에 도착하기 때문에, 
        //거리와 비례해서 timer값을 조정해주기 위해 speed값으로 나눔.
        timer +=10000*Time.deltaTime / speed;

        //상훈 : 간단히 Lerp를 사용하여 시작지점에서 끝지점으로 가게.
        transform.position = Vector3.Lerp(startPos, playerPos, timer);
        if (timer>1.0f)
        {
            gameObject.SetActive(false);
        }
        
    }

    //상훈 : 다른곳에 닿으면 사라지게.
    private void OnTriggerEnter(Collider collision)
    {
        if (!(collision.gameObject.CompareTag("Rocket") || collision.gameObject.CompareTag("Bullet")
            || collision.gameObject.name == "Lake"))
        {
            gameObject.SetActive(false);
        }
    }

    public void Setting(Vector3 start, Vector3 end)
    {
        startPos = start;
        playerPos = end;
        //상훈 : speed는 start와 end사이의 sqrMagnitude
        speed = (start - end).sqrMagnitude;
    }
}
