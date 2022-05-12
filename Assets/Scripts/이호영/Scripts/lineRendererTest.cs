using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lineRendererTest : MonoBehaviour
{
    // lineRenderer
    // 이호영
    // ConquestManager 에 적용
    // Line A,B,C
    // 점령 구역 표시 수직 레이저 : 라인 렌더러 이용해서 그림
    // 시작점, 끝점 color, width 지정하고 lerp 이용해서 스무스하게 그림

    LineRenderer lineRenderer;
    float counter;
    float dist;
    
    // 그리는 속도
    public float lineDrawSpeed = 6f;

    conquest Conquest;

    // 점령지 A,B,C 레이저
    public LineRenderer lineA;
    public LineRenderer lineB;
    public LineRenderer lineC;

    float x;
    Vector3 pointAlongTime;

    // 초기화 한번씩만 하려고 버튼
    bool toggle = false;

    // Use this for initialization
    void Start()
    {
        Conquest = GameObject.Find("ConquestManager").GetComponent<conquest>();

        //라인렌더러 설정
        lineRenderer = lineA;
        
        // set color
        lineRenderer.startColor = new Color (1, 0, 0, 0.5f);
        lineRenderer.endColor = new Color (1, 0, 0, 0.5f);

        // set width
        lineRenderer.startWidth = 0.45f;
        lineRenderer.endWidth = 0.45f;


        //라인렌더러 처음위치
        lineRenderer.SetPosition(0, new Vector3(0, 0, 0));

        dist = 50;
    }

    // Update is called once per frame
    void Update()
    {
        switch(Conquest.n_conquest) // Conquest의 점령 갯수 가져옴
        {
            case 0:
                break;
            case 1:
                if(!toggle)
                {
                    //초기화
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
                    //초기화
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

        // 라인 그리기 (끝점)
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
