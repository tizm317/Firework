using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lineRendererTest : MonoBehaviour
{
    // lineRenderer
    // ��ȣ��
    // ConquestManager �� ����
    // Line A,B,C
    // ���� ���� ǥ�� ���� ������ : ���� ������ �̿��ؼ� �׸�
    // ������, ���� color, width �����ϰ� lerp �̿��ؼ� �������ϰ� �׸�

    LineRenderer lineRenderer;
    float counter;
    float dist;
    
    // �׸��� �ӵ�
    public float lineDrawSpeed = 6f;

    conquest Conquest;

    // ������ A,B,C ������
    public LineRenderer lineA;
    public LineRenderer lineB;
    public LineRenderer lineC;

    float x;
    Vector3 pointAlongTime;

    // �ʱ�ȭ �ѹ����� �Ϸ��� ��ư
    bool toggle = false;

    // Use this for initialization
    void Start()
    {
        Conquest = GameObject.Find("ConquestManager").GetComponent<conquest>();

        //���η����� ����
        lineRenderer = lineA;
        
        // set color
        lineRenderer.startColor = new Color (1, 0, 0, 0.5f);
        lineRenderer.endColor = new Color (1, 0, 0, 0.5f);

        // set width
        lineRenderer.startWidth = 0.45f;
        lineRenderer.endWidth = 0.45f;


        //���η����� ó����ġ
        lineRenderer.SetPosition(0, new Vector3(0, 0, 0));

        dist = 50;
    }

    // Update is called once per frame
    void Update()
    {
        switch(Conquest.n_conquest) // Conquest�� ���� ���� ������
        {
            case 0:
                break;
            case 1:
                if(!toggle)
                {
                    //�ʱ�ȭ
                    lineRenderer.SetPosition(1, new Vector3(0, 0, 0));
                    lineRenderer = lineB;
                    lineRenderer.startColor = new Color(1, 0, 0, 0.5f);
                    lineRenderer.endColor = new Color(1, 0, 0, 0.5f);
                    lineRenderer.SetPosition(0, new Vector3(0, 0, 0));
                    lineRenderer.startWidth = 0.45f;
                    lineRenderer.endWidth = 0.45f;

                    counter = 0;
                    x = 0;
                    pointAlongTime = new Vector3(0, 0, 0);
                    toggle = true;
                }
                break;
            case 2:
                if(toggle)
                {
                    //�ʱ�ȭ
                    lineRenderer.SetPosition(1, new Vector3(0, 0, 0));
                    lineRenderer = lineC;
                    lineRenderer.startColor = new Color(1, 0, 0, 0.5f);
                    lineRenderer.endColor = new Color(1, 0, 0, 0.5f);
                    lineRenderer.SetPosition(0, new Vector3(0, 0, 0));
                    lineRenderer.startWidth = 0.45f;
                    lineRenderer.endWidth = 0.45f;

                    counter = 0;
                    x = 0;
                    pointAlongTime = new Vector3(0, 0, 0);
                    toggle = false;
                }
                break;
            case 3:
                // Game Over
                break;
        }

        // ���� �׸��� (����)
        if(counter < dist)
        {
            counter += .1f / lineDrawSpeed;

            x = Mathf.Lerp(0, 50, counter);

            Vector3 pointA = new Vector3(0, 0, 0);
            Vector3 pointB = new Vector3(0, 50, 0);

            pointAlongTime = x * Vector3.Normalize(pointB - pointA) + pointA;

            lineRenderer.SetPosition(1, pointAlongTime);
        }
    }

}
