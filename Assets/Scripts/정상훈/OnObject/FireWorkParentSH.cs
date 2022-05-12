using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 상훈 : 오브젝트 풀링써서 만들거여가지고 activate함수 실행해야 날라감. 
 * SetActive(true)를 매니저에서 해줘야 코드가 실행됨
*/
public class FireWorkParentSH : MonoBehaviour
{
    //상훈 : 최초에 날라가는 방향.
    Vector3 pointingVector;

    //상훈 : 현재 가속도랑 나중 가속도.
    Vector3 nowVelocity;
    Vector3 startVelocity;
    Vector3 startScale;
    Vector3 nowScale;
    //상훈 : 중력은 0.5f로 고정.양수여야함
    const float gravityScale = 0.5f;
    //상훈 : 최초에 받는 힘.
    const float startForce = 2f;
    //상훈 : 날라가면서 계속 받는 힘.
    //상훈 : 폭죽이 올라가면서 힘을 계속 받아야하니까
    const float accelerateTime = 0.5f;
    //상훈 : 몇초있다 터지는지.
    const float popTime = 1f;
    //상훈 : 위의 타임 카운팅해주는거.
    float timer;
    bool isActivate;

    //상훈 : 어차피 Activate함수에서 초기화를 해주기 때문에 Start에서 초기화 안해줌.



    // Update is called once per frame
    void FixedUpdate()
    {
        //상훈 : 나중좌표랑 이전좌표.
        Vector3 pos;
        //상훈 : 애초에 Active상태가 false라면 update가 실행되지 않지만, 혹시모를 예외처리.
        if(isActivate == false)
        {
            return;
        }
        //상훈 : activate되었다면 현재 좌표 += 속도 를 해준다.
        pos = transform.position;
        timer += Time.deltaTime;
        //상훈 : 중력구현. 
        nowVelocity += Vector3.down * Time.deltaTime * gravityScale;
        //상훈 : 가속하는 시간.
        if (timer < accelerateTime)
        {
            nowVelocity += pointingVector * Time.deltaTime * startForce;
        }
        //상훈 : 최초에 불에 탈 때만 빛이 밝아짐. 나중에는 서서히 어두워짐.
        if(timer > popTime / 2)
        {
            //상훈 : 굉장히 위험한 접근방식.
            FireWorkManagerSH.singleTon.ModifyLight(false);
        }
        else
        {
            FireWorkManagerSH.singleTon.ModifyLight(true);
        }

        //상훈 : 폭죽이 탈 때 작아지게 하기 위한 스케일 조정
        nowScale -= Time.deltaTime * Vector3.one;
        transform.localScale = nowScale;

        //상훈 현재 position + Velocity
        pos += nowVelocity;

        transform.position = pos;
        //상훈 : 0아래로 떨어지면 사라지게.
        if (timer> popTime)
        {
            isActivate = false;
            //팝타임이
            FireWorkManagerSH.singleTon.FireChild(gameObject);
            FireWorkManagerSH.singleTon.CountFirework(false);
            gameObject.SetActive(false);
        }
    }

    //FireWorkManager에서 불러옴. 향하는 지점 및 시작위치 매개변수, 전역변수 초기화.
    public void Activate(Vector3 _pointingVector,Vector3 pos)
    {
        transform.position = pos;
        pointingVector = _pointingVector;
        startScale = new Vector3(popTime, popTime, popTime);
        transform.localScale = startScale;
        timer = 0;
        isActivate = true;
        startVelocity = pointingVector * startForce;
        nowVelocity = startVelocity;
        FireWorkManagerSH.singleTon.ModifyLight(true);
        nowScale = transform.localScale;
 
    }

    //상훈 : 환경물에 부딪혔을 때 즉시 터지게 해야함.
    private void OnTriggerEnter(Collider collision)
    {
        //상훈 : 대신 물 안에서 쐈을때는 안터짐.
        if (collision.gameObject.name == "Lake")
        {
            isActivate = false;
            FireWorkManagerSH.singleTon.CountFirework(false);
            gameObject.SetActive(false);
            SoundManager.singleTon.FireWorkSoundStop();
            //아래코드가 실행되면 안됨.
            return;
        }

        if (!(collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("FireWork")))
        {
            isActivate = false;
            SoundManager.singleTon.FireWorkSoundStop();
            FireWorkManagerSH.singleTon.FireChild(gameObject);
            FireWorkManagerSH.singleTon.CountFirework(false);
            gameObject.SetActive(false);
        }
        
    }
}
