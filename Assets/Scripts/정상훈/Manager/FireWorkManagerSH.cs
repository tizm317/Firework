using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWorkManagerSH : MonoBehaviour
{
    //상훈 : 싱글톤, FireWorkParent,Child에서 불러와야 하기 때문에 이렇게 정의. 굉장히 위험하다. 어디서든 접근가능.
    public static FireWorkManagerSH singleTon;
    [SerializeField]
    Transform playerObject;

    [SerializeField]
    GameObject fireWorkParentPrefab;
    [SerializeField]
    GameObject fireWorkChildPrefab;
    [SerializeField]
    Transform fireWorkInstsPool;

    [SerializeField]
    Light mainLight;

    float coolTime;

    //상훈 : 오브젝트 풀링 라운드로빈써서 할거임
    List<GameObject> fireWorkParentPool;
    int fireWorkParentIndex;
    List<GameObject> fireWorkChildPool;
    int fireWorkChildIndex;

    //상훈 : FireWork에서 setactiveFalse 될 때 카운트 세어줌.
    public int fireWorkCount;

    const int childCount = 150;

    //상훈 : 단일씬이기 때문에 DontDestroyOnLoad를 사용하지 않는다.
    private void Awake()
    {
        singleTon = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        //상훈 : 풀 초기화 및 풀에 오브젝트 넣어주기.
        fireWorkParentPool = new List<GameObject>();
        fireWorkChildPool = new List<GameObject>();
        fireWorkChildIndex = 0;
        fireWorkParentIndex = 0;

        for(int i = 0; i < 10; i++)
        {
            GameObject obj = Instantiate(fireWorkParentPrefab,fireWorkInstsPool);
            obj.SetActive(false);
            fireWorkParentPool.Add(obj);
        }
        for (int i = 0; i < 400; i++)
        {
            GameObject obj = Instantiate(fireWorkChildPrefab,fireWorkInstsPool);
            obj.SetActive(false);
            fireWorkChildPool.Add(obj);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //상훈 : 게임 시작 전이라면 비활성화.
        if (GameManager.singleTon.isGameStarted == false || GameManager.singleTon.isGameOver == true)
        {
            return;
        }
        //상훈 : 쿨타임이 1이 되기 전에는 발사 불가.
        if (Input.GetMouseButtonDown(0) && coolTime > 1)
        {
            coolTime = 0;
            FireParent();
        }
        coolTime += Time.deltaTime;
    }

    //상훈 : 클릭 인풋을 하면 Update에서 호출.
    void FireParent()
    {
        FireWorkParentSH fireWorkParent;
        GameObject parentObject = ParentPooling();

        //Character Controller의 static변수 aim을 받아온다.
        Vector3 pointingVector = Character_Controller.aim;

        CountFirework(true);

        pointingVector.Normalize();
        parentObject.SetActive(true);
        fireWorkParent = parentObject.GetComponent<FireWorkParentSH>();
        //상훈 : Activate시 현재 플레이어 포지션에서 aim을 향해 발사.
        fireWorkParent.Activate(pointingVector,playerObject.transform.position);
        SoundManager.singleTon.FireWorkSoundPlay();
    }

    //상훈 : FireworkParent에서 불러옴. parentObject의 위치에서 시작해야 하기 때문에 매개변수로 parentObj받아옴.
    public void FireChild(GameObject parentObj)
    {
        //상훈 : parentPos에서 소환.
        Vector3 parentPos = parentObj.transform.position;

        for(int i = 0; i < childCount; i++)
        {
            FireWorkChildSH fireWorkChild = null;
            GameObject childObject = null;
            Vector3 pointingVector = new Vector3(Random.Range(-1f,1f),Random.Range(-1f,1f),
                Random.Range(-1f,1f));
            //속도는 일정해야하기 때문에 normalize를 해준다.
            pointingVector.Normalize();
            childObject = ChildPooling();
            childObject.SetActive(true);
            fireWorkChild = childObject.GetComponent<FireWorkChildSH>();
            //상훈 : fireWorkChild를 GetComponent로 받아오고 매개변수로 시작위치, 속도벡터를 넣어준다.
            fireWorkChild.Activate(parentPos, pointingVector);
            //상훈 : fireWork개수에 맞게 라이트닝 조정을 위해 CountFireWork
            CountFirework(true);
        }
    }

    //상훈 : 패런트 및 차일드 오브젝트 풀링.
    GameObject ParentPooling()
    {
        GameObject returningObject;
        //상훈 : fireWorkParentIndex는 라운드로빈으로 돌아간다. 라운드로빈 중 Pool의 카운트에 도달하면 0으로 초기화
        if(fireWorkParentIndex >= fireWorkParentPool.Count)
        {
            fireWorkParentIndex = 0;
        }

        //상훈 : 만약 풀링하려는 인덱스가 이미 active하다면, 사용 가능한 Object가 없다고 판단, 새로 Instantiate
        if(fireWorkParentPool[fireWorkParentIndex].activeSelf == true)
        {
            GameObject obj = Instantiate(fireWorkParentPrefab,fireWorkInstsPool);
            fireWorkParentPool.Add(obj);
            //상훈 : Add를 하게되면 마지막 인덱스로 보내지니 인덱스를 마지막인덱스로 조정.
            fireWorkParentIndex = fireWorkParentPool.Count - 1;

        }
        returningObject = fireWorkParentPool[fireWorkParentIndex];

        fireWorkParentIndex++;
        return returningObject;
    }

    //상훈 : 위와 동일.
    GameObject ChildPooling()
    {
        GameObject returningObject;

        if (fireWorkChildIndex >= fireWorkChildPool.Count)
        {
            fireWorkChildIndex = 0;
        }

        if (fireWorkChildPool[fireWorkChildIndex].activeSelf == true)
        {
            GameObject obj = Instantiate(fireWorkChildPrefab,fireWorkInstsPool);
            fireWorkChildPool.Add(obj);
            fireWorkChildIndex = fireWorkChildPool.Count - 1;
        }
        returningObject = fireWorkChildPool[fireWorkChildIndex];

        fireWorkChildIndex++;
        return returningObject;
    }

    //상훈 : Firework child, parent둘다 불러올거. 폭죽개수에 맞게 light의 intensity조정.
    public void ModifyLight(bool shine, float intensity = 0.0001f)
    {
        if(shine == true)
        {
            mainLight.intensity += intensity;
        }
        else
        {
            mainLight.intensity -= intensity;
            if (mainLight.intensity < 0.1f)
            {
                mainLight.intensity = 0.1f;
            }
        }
    }

    //상훈 : FireWork child,Parent에서 소멸될 때 불러옴.
    public void CountFirework(bool plus)
    {
        if (plus)
        {
            fireWorkCount++;
        }
        else
        {
            fireWorkCount--;
        }
        if (fireWorkCount == 0)
        {
            mainLight.intensity = 0.1f;
        }
    }
}
