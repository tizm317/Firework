using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * ���� : ������Ʈ Ǯ���Ἥ ����ſ������� activate�Լ� �����ؾ� ����. 
 * SetActive(true)�� �Ŵ������� ����� �ڵ尡 �����
*/
public class FireWorkParentSH : MonoBehaviour
{
    //���� : ���ʿ� ���󰡴� ����.
    Vector3 pointingVector;

    //���� : ���� ���ӵ��� ���� ���ӵ�.
    Vector3 nowVelocity;
    Vector3 startVelocity;
    Vector3 startScale;
    Vector3 nowScale;
    //���� : �߷��� 0.5f�� ����.���������
    const float gravityScale = 0.5f;
    //���� : ���ʿ� �޴� ��.
    const float startForce = 2f;
    //���� : ���󰡸鼭 ��� �޴� ��.
    //���� : ������ �ö󰡸鼭 ���� ��� �޾ƾ��ϴϱ�
    const float accelerateTime = 0.5f;
    //���� : �����ִ� ��������.
    const float popTime = 1f;
    //���� : ���� Ÿ�� ī�������ִ°�.
    float timer;
    bool isActivate;

    //���� : ������ Activate�Լ����� �ʱ�ȭ�� ���ֱ� ������ Start���� �ʱ�ȭ ������.



    // Update is called once per frame
    void FixedUpdate()
    {
        //���� : ������ǥ�� ������ǥ.
        Vector3 pos;
        //���� : ���ʿ� Active���°� false��� update�� ������� ������, Ȥ�ø� ����ó��.
        if(isActivate == false)
        {
            return;
        }
        //���� : activate�Ǿ��ٸ� ���� ��ǥ += �ӵ� �� ���ش�.
        pos = transform.position;
        timer += Time.deltaTime;
        //���� : �߷±���. 
        nowVelocity += Vector3.down * Time.deltaTime * gravityScale;
        //���� : �����ϴ� �ð�.
        if (timer < accelerateTime)
        {
            nowVelocity += pointingVector * Time.deltaTime * startForce;
        }
        //���� : ���ʿ� �ҿ� Ż ���� ���� �����. ���߿��� ������ ��ο���.
        if(timer > popTime / 2)
        {
            //���� : ������ ������ ���ٹ��.
            FireWorkManagerSH.singleTon.ModifyLight(false);
        }
        else
        {
            FireWorkManagerSH.singleTon.ModifyLight(true);
        }

        //���� : ������ Ż �� �۾����� �ϱ� ���� ������ ����
        nowScale -= Time.deltaTime * Vector3.one;
        transform.localScale = nowScale;

        //���� ���� position + Velocity
        pos += nowVelocity;

        transform.position = pos;
        //���� : 0�Ʒ��� �������� �������.
        if (timer> popTime)
        {
            isActivate = false;
            //��Ÿ����
            FireWorkManagerSH.singleTon.FireChild(gameObject);
            FireWorkManagerSH.singleTon.CountFirework(false);
            gameObject.SetActive(false);
        }
    }

    //FireWorkManager���� �ҷ���. ���ϴ� ���� �� ������ġ �Ű�����, �������� �ʱ�ȭ.
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

    //���� : ȯ�湰�� �ε����� �� ��� ������ �ؾ���.
    private void OnTriggerEnter(Collider collision)
    {
        //���� : ��� �� �ȿ��� �������� ������.
        if (collision.gameObject.name == "Lake")
        {
            isActivate = false;
            FireWorkManagerSH.singleTon.CountFirework(false);
            gameObject.SetActive(false);
            SoundManager.singleTon.FireWorkSoundStop();
            //�Ʒ��ڵ尡 ����Ǹ� �ȵ�.
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
