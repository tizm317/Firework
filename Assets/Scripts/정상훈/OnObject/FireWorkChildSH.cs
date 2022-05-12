using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * ���� : �ɰ����� ���󰡴°� �������ִ� Ŭ����
 */ 
public class FireWorkChildSH : MonoBehaviour
{
    //���� : ���
    const float gravityScale = 1;
    const float startForce = 3;
    const float popTime = 3f;
    const float accelerateTime = 0.2f;
    float lightIntensity;


    //���� : ������ġ, �ٶ󺸴� ����.
    Vector3 startPos;
    Vector3 pointingVector;
    Vector3 nowScale;
    Vector3 nowVelocity;

    //���� : ������ ���� ���� �־������. �̰Ŵ� �Ŵ������� �ٰ��� �����������.
    bool onValueSet;

    

    float timer;
    
    void Start()
    {
        //�������� �����ſ���.


    }

    //���� : ����ȿ������ �Ƚ��������Ʈ. FireWorkParent�� ����.
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
            //���� : ���������� �־��༭ ���� ���Ŀ��� �ӵ��� �پ��� ��.
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
    //���� : fireWorkManager���� �ҷ���.
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

    //���� : �ٸ����� �ε����� �� ������� �ϰ�.
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
