using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class conquest : MonoBehaviour
{
    /* 
     *  ��ȣ��
        ���� �ý���
        A -> B -> C
        �÷��̾� �� ���� ���� ��ġ ����ؼ� ���� ���̸� ���� ������ �ö󰡰�
        �����ϱ� ���� ���� �Ѿ�� �ٽ� ������ �������� �ϰ� (UI)
        ���� �Ϸ��ϸ� ���� ���� ��������

        UI 
        �����ؾ��ϴ� �� ǥ�� : A -> B -> C ����
        ���� ������ ǥ�� : �����̴� ���
        
        ���ɱ��� ǥ��
        ������ġ ������ ������ ���� ǥ�� + �ش� �ǹ� ���� �� ������ ǥ�� (�ָ����� ���̰�) (lineRendererTest)
        �����ؾ� �ϴ� �� : ���� �� ǥ�� -> ���� ��(�Ϸ�) : �Ķ� �� ǥ��
        ���� �ƴ� ���� : ȸ�� ǥ��
     */

    // �÷��̾� ��ġ, ������ ��ġ 3��
    private GameObject targetBase; // ���� Ÿ�� ������
    public GameObject baseA;
    public GameObject baseB;
    public GameObject baseC;
    public GameObject player;


    // ���� ����
    public int n_conquest = 0; // ������ �ľ�
    private bool GameOver = false; // 3�� ���ɽ� GameOver

    // �Ÿ� �� ��
    private float xlen;
    private float zlen;
    private float square_r; // r^2

    // ���� ������
    float count = 0;

    //UI
    public GameObject conquestGauge; // ���� ������
    public Text notice;              // 
    public Button buttonA;           // A ���� �̹���
    public Button buttonB;           // B
    public Button buttonC;           // C

    // ���� ���� Ÿ�̸� SH
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
                notice.text = "B������ �����Ͻʽÿ�";

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
                notice.text = "C������ �����Ͻʽÿ�";

                baseC.transform.GetChild(1).GetComponent<LineRenderer>().startColor = Color.red;
                baseC.transform.GetChild(1).GetComponent<LineRenderer>().endColor = Color.red;
                buttonC.GetComponent<Image>().color = Color.red;

                targetBase = baseC;
                break;
            case 3:
                //base_before color change
                notice.text = "���� ���� �Ϸ�";
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

        // �Ÿ�
        xlen = targetBase.transform.position.x - player.transform.position.x;
        zlen = targetBase.transform.position.z - player.transform.position.z;
        square_r = xlen * xlen + zlen * zlen; // r^2 = x^2 +z^2

        // ������
        if (count < 100) // 100 �� ä�� ����
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
                // 100�ۼ�Ʈ �Ǳ����� ������ �ٽ� �پ��
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
