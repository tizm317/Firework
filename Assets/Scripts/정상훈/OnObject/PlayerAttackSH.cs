using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackSH : MonoBehaviour
{

    [SerializeField]
    GameObject bullet;
    [SerializeField]
    Transform bulletParent;
    //[SerializeField]
    //Transform player//1127영찬수정
    private Transform player;

    //상훈 : 오브젝트 풀링
    List<GameObject> bulletPool;
    int nowPoolIndex;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Transform>();

        //상훈 : 풀에 오브젝트들 Instantiate
        bulletPool = new List<GameObject>();
        for(int i = 0; i < 100; i++)
        {
            bulletPool.Add(Instantiate(bullet, bulletParent));
            bulletPool[i].SetActive(false);
        }
        nowPoolIndex = 0;
        //상훈 : 스타트에서 코루틴 시작.
        StartCoroutine(BulletSpawnCor());
    }

    //상훈 : FireWork와 동일한 라운드로빈.
    GameObject GetBullet()
    {
        if(nowPoolIndex>= bulletPool.Count)
        {
            nowPoolIndex = 0;
        }
        if(bulletPool[nowPoolIndex].activeSelf == true)
        {
            bulletPool.Add(Instantiate(bullet, bulletParent));
            nowPoolIndex = bulletPool.Count - 1;
        }

        return bulletPool[nowPoolIndex++];
    }

    //상훈 : Bullet을 스폰해주는 코루틴.
    IEnumerator BulletSpawnCor()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(0f, 2f));
            if (GameManager.singleTon.isGameStarted == true && GameManager.singleTon.isGameOver == false)
            {
                GameObject bulletObj = GetBullet();
                bulletObj.SetActive(true);
                bulletObj.GetComponent<Bullet>().Setting(transform.position, player.position);
            }
        }
        
    }

}
