using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class conquest : MonoBehaviour
{
    /* 
     *  이호영
        점령 시스템
        A -> B -> C
        플레이어 랑 점령 구역 위치 계산해서 범위 안이면 점령 게이지 올라가게
        점령하기 전에 범위 넘어가면 다시 게이지 내려가게 하고 (UI)
        점령 완료하면 다음 점령 구역으로

        UI 
        점령해야하는 곳 표시 : A -> B -> C 순서
        점령 게이지 표시 : 슬라이더 사용
        
        점령구역 표시
        오버워치 느낌의 반투명 구역 표시 + 해당 건물 수직 빛 레이저 표시 (멀리서도 보이게) (lineRendererTest)
        점령해야 하는 곳 : 빨간 색 표시 -> 점령 중(완료) : 파란 색 표시
        아직 아닌 구역 : 회색 표시
     */

    // 플레이어 위치, 점령지 위치 3개
    private GameObject targetBase; // 현재 타겟 점령지
    public GameObject baseA;
    public GameObject baseB;
    public GameObject baseC;
    public GameObject player;


    // 점령 여부
    public int n_conquest = 0; // 개수로 파악
    private bool GameOver = false; // 3개 점령시 GameOver

    // 거리 잴 때
    private float xlen;
    private float zlen;
    private float square_r; // r^2

    // 점령 게이지
    float count = 0;

    //UI
    public GameObject conquestGauge; // 점령 게이지
    public Text notice;              // 
    public Button buttonA;           // A 구역 이미지
    public Button buttonB;           // B
    public Button buttonC;           // C

    // 게임 오버 타이머 SH
    float gameOverTimer = 0;


    // Update is called once per frame
    void Update()
    {
        //Debug.Log(n_conquest);
        switch(n_conquest)
        {
            case 0:
                buttonA.GetComponent<Image>().color = Color.red;
                targetBase = baseA;
                break;
            case 1:
                

                //base_before color change
                baseB.transform.GetChild(9).GetComponent<LineRenderer>().startColor = Color.red;
                baseB.transform.GetChild(9).GetComponent<LineRenderer>().endColor = Color.red;
                buttonB.GetComponent<Image>().color = Color.red;

                // next_base
                notice.text = "B거점을 수비하십시오";

                baseA.transform.GetChild(3).GetComponent<LineRenderer>().startColor = Color.blue;
                baseA.transform.GetChild(3).GetComponent<LineRenderer>().endColor = Color.blue;
                buttonA.GetComponent<Image>().color = Color.blue;


                targetBase = baseB;
                break;
            case 2:
                //base_before color change
                baseB.transform.GetChild(9).GetComponent<LineRenderer>().startColor = Color.blue;
                baseB.transform.GetChild(9).GetComponent<LineRenderer>().endColor = Color.blue;
                buttonB.GetComponent<Image>().color = Color.blue;

                // next_base
                notice.text = "C거점을 수비하십시오";

                baseC.transform.GetChild(1).GetComponent<LineRenderer>().startColor = Color.red;
                baseC.transform.GetChild(1).GetComponent<LineRenderer>().endColor = Color.red;
                buttonC.GetComponent<Image>().color = Color.red;

                targetBase = baseC;
                break;
            case 3:
                //base_before color change
                notice.text = "거점 점령 완료";
                baseC.transform.GetChild(1).GetComponent<LineRenderer>().startColor = Color.blue;
                baseC.transform.GetChild(1).GetComponent<LineRenderer>().endColor = Color.blue;
                buttonC.GetComponent<Image>().color = Color.blue;
                conquestGauge.SetActive(false);
                GameOver = true;

                // gameOver SH
                gameOverTimer += Time.deltaTime;
                if (gameOverTimer > 1)
                {
                    GameManager.singleTon.GameClear();
                }
                break;
        }

        // 거리
        xlen = targetBase.transform.position.x - player.transform.position.x;
        zlen = targetBase.transform.position.z - player.transform.position.z;
        square_r = xlen * xlen + zlen * zlen; // r^2 = x^2 +z^2

        // 게이지
        if (count < 100) // 100 못 채운 상태
        {
            if (square_r <= 169 && n_conquest != 3) // r^2 <= 13^2
            {
                // in_bound
                conquestGauge.SetActive(true);
                count++; 
                conquestGauge.GetComponent<Slider>().value = (count / 100);
                //Debug.Log(count);
            }
            else
            {
                // 100퍼센트 되기전에 나가면 다시 줄어듦
                // out_of_boundary
                if (count > 0)
                {
                    count--;
                    conquestGauge.GetComponent<Slider>().value = (count / 100);
                }
                if(count <= 0)
                    conquestGauge.SetActive(false);
                //Debug.Log(count--);
            }
        }
        else
        {
            // next base
            // reset
            if(!GameOver)
            {
                // sound
                SoundManager.singleTon.ConquestSoundPlay();

                n_conquest++;
                count = 0;
            }
        }
    }
}
