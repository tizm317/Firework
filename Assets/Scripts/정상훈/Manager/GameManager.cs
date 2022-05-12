using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    //상훈 : 마찬가지로 singleTon을 이용하여 접근
    public static GameManager singleTon;

    [SerializeField]
    GameObject gamePlayCanvas;
    [SerializeField]
    GameObject clearCanvas;
    [SerializeField]
    GameObject gameOverCanvas;
    [SerializeField]
    GameObject startCanvas;

    [SerializeField]
    GameObject bulletParent;


    //상훈 : Conquest, FireWorkManager, Character_Controller에서 접근하여 현재 상태를 확인. 인스펙터에선 숨겨줌
    [HideInInspector]
    public bool isGameOver;
    [HideInInspector]
    public bool isGameStarted;
    public Image healthBar;

    //호영 : key 설명 ( 0,1,2 순 방향키 -> 점프 -> 공격)
    int num_tutorial = 0;

    // Start is called before the first frame update
    void Awake()
    {
        singleTon = this;
        isGameOver = false;
        isGameStarted = false;
    }

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(isGameStarted == false && isGameOver == false)
            {
                //게임 시작 상태가 아니라면 게임 시작.
                isGameStarted = true;
                gamePlayCanvas.SetActive(true);
                startCanvas.SetActive(false);
            }
            else
            {
                //게임 시작 상태에서 space 누를 시
                // key 설명 text 활성화되있다면
                // 다음으로
                // num_tutorial: 1
                if (gamePlayCanvas.transform.GetChild(9) && num_tutorial == 1)
                {
                    gamePlayCanvas.transform.GetChild(9).gameObject.GetComponent<Text>().text = "마우스 좌클릭 : 공격\n마우스 휠 : 시점 변환";
                    num_tutorial++;
                }
            }

        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (isGameOver == true)
            {
                //씬 리로드
                SceneManager.LoadScene(0);
            }
            
        }

        // 방향키 설명 : num_tutorial : 0
        if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
        {
            if(isGameStarted && num_tutorial == 0)
            {
                gamePlayCanvas.transform.GetChild(9).gameObject.GetComponent<Text>().text = "Space : 점프";
                num_tutorial++;
            }
        }

        // 마우스 우클릭(공격) 설명 : num_tutorial : 2
        if (Input.GetMouseButtonDown(0))
        {
            if (gamePlayCanvas.transform.GetChild(9) && num_tutorial == 2)
            {
                num_tutorial = 0;
                gamePlayCanvas.transform.GetChild(9).gameObject.SetActive(false);
            }
        }
    }

    //Bullet에서 불러옴.
    public void HealthChange(int hp)
    {
        healthBar.fillAmount = (float)hp / 20;
        if (hp < 0)
        {
            GameOver();
        }
    }

    //timer에서 불러옴
    public void GameOver()
    {
        isGameOver = true;
        isGameStarted = false;
        gameOverCanvas.SetActive(true);
        gamePlayCanvas.SetActive(false);
        bulletParent.SetActive(false);
    }

    //Conquest에서 불러옴.
    public void GameClear()
    {
        isGameOver = true;
        isGameStarted = false;
        gamePlayCanvas.SetActive(false);
        clearCanvas.SetActive(true);
        bulletParent.SetActive(false);
    }
}
