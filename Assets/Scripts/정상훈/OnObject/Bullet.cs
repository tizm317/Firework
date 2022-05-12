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
        //���� : timer�� speed������ ������ ������ ������ 1�ʾȿ� �����ϱ� ������, 
        //�Ÿ��� ����ؼ� timer���� �������ֱ� ���� speed������ ����.
        timer +=10000*Time.deltaTime / speed;

        //���� : ������ Lerp�� ����Ͽ� ������������ ���������� ����.
        transform.position = Vector3.Lerp(startPos, playerPos, timer);
        if (timer>1.0f)
        {
            gameObject.SetActive(false);
        }
        
    }

    //���� : �ٸ����� ������ �������.
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
        //���� : speed�� start�� end������ sqrMagnitude
        speed = (start - end).sqrMagnitude;
    }
}
