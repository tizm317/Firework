using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 상훈 : 쪼가리들 날라가는거 구현해주는 클래스
 */ 
public class FireWorkChildSH : MonoBehaviour
{
    //상훈 : 상수
    const float gravityScale = 1;
    const float startForce = 3;
    const float popTime = 3f;
    const float accelerateTime = 0.2f;
    float lightIntensity;


    //상훈 : 시작위치, 바라보는 방향.
    Vector3 startPos;
    Vector3 pointingVector;
    Vector3 nowScale;
    Vector3 nowVelocity;

    //상훈 : 최초의 위의 값을 넣어줘야함. 이거는 매니저에서 다같이 조종해줘야함.
    bool onValueSet;

    

    float timer;
    
    void Start()
    {
        //스케일이 줄을거여서.


    }

    //상훈 : 물리효과여서 픽스드업데이트. FireWorkParent와 동일.
    void FixedUpdate()
    {
        Vector3 nowPos;
        if(onValueSet == false)
        {
            return;
        }
        timer += Time.deltaTime;


        nowScale -= Time.deltaTime * Vector3.one;
        transform.localScale = nowScale;

        if (timer > popTime / 2)
        {
            lightIntensity -= 0.0001f;
            FireWorkManagerSH.singleTon.ModifyLight(false);
        }
        else
        {
            lightIntensity += 0.0001f;
            FireWorkManagerSH.singleTon.ModifyLight(true);
        }

        nowPos = transform.position;
        nowVelocity += Vector3.down * Time.deltaTime * gravityScale;
        if (timer < accelerateTime)
        {
            nowVelocity += pointingVector * Time.deltaTime * startForce;
        }
        else
        {
            //상훈 : 공기저항을 넣어줘서 가속 이후에는 속도가 줄어들게 함.
                nowVelocity *= 0.95f;
        }

        nowPos += nowVelocity;

        transform.position = nowPos;

        if (timer > popTime)
        {
            onValueSet = false;
            FireWorkManagerSH.singleTon.CountFirework(false);
            gameObject.SetActive(false);
        }

    }
    //상훈 : fireWorkManager에서 불러옴.
    public void Activate(Vector3 _startPos, Vector3 _pointingVector)
    {
        transform.position = _startPos;
        startPos = _startPos;
        pointingVector = _pointingVector;
        onValueSet = true;
        nowVelocity = _pointingVector;
        transform.localScale = new Vector3(popTime, popTime, popTime);
        nowScale = transform.localScale;
        timer = 0;
        lightIntensity = 0;
    }

    //상훈 : 다른곳에 부딪혔을 때 사라지게 하게.
    private void OnTriggerEnter(Collider collision)
    {
        if (!(collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("FireWork")) || collision.gameObject.CompareTag("Rocket"))
        {
            onValueSet = false;
            FireWorkManagerSH.singleTon.ModifyLight(false, lightIntensity);
            FireWorkManagerSH.singleTon.CountFirework(false);
            gameObject.SetActive(false);
        }
    }
}
