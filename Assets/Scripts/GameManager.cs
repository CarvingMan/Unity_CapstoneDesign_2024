using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    //싱글톤 디자인 패턴 사용
    //제너릭 클래스 Singleton을 상속받아 GameManager 싱글톤 인스턴스 생성


    //***스테이지(Field) 관련***//
    bool m_isInField = false; //현재 Field에 있는지(플레이어가 Field Stage에 있는지)
    int m_nStage = 1; //현재 Stage
    const int m_nMaxFieldMob = 7; //Stage 당 몬스터 수
    int m_nCurrentMobNo = 0; //현재 대치중인 field mob 번호 -> 8마리를 잡을 시 Stage++;
    public int CurrentMonNo { get { return m_nCurrentMobNo; } }


    //*** 능력치 레벨 ***//
    //float m_fMoveLv = 0;
    //float m_fAttackSpeedLv = 0;
    //float m_fAttackDamegeLv = 0;
    

    //*** 능력치 값 ***//
    private float m_fMoveStatus = 1; //이동속도 능력치 1이면 100%, 레벨업시 증가
    private float m_fAttackSpeed = 1; //공격속도 위와 동일
    private float m_fAttackDamage = 10; //레벨업시 10% 씩 증가 


    //**플레이어 전투 관련**//
    bool m_isFieldBattle = false; //전투중일 시 true
    public bool IsFieldBattle { get { return m_isFieldBattle; } }
    bool m_isPlayerAttack = false; //true시 플레이어가 attack 애니메이션을 play하고 animation 클립 이벤트에서 공격관련 함수 호출
    public bool IsPlayerAttack { get { return m_isPlayerAttack; } } 
    public float AttackSpeed { get { return m_fAttackSpeed; } }
    GameObject m_objCurrentMob = null; //현재 player와 대치중인 monster
    public GameObject CurrentMob { get { return m_objCurrentMob; } }


    //**필드 몬스터 관련**//
    const float m_fBasicFieldMobHp = 50; //기본 필드몹 hp -> 여기서 랜덤으로 0~20% 증가하여 사용
    const float m_fBasicFieldBossHp = 100; //기본 필드보스 hp
    const float m_fFieldMobHpInc = 1.2f; //Hp 증가율 스테이지 증가 할 때마다 BasicHp에 곱하여 제공


    //이동관련 제어 멤버 변수 MoveBackGround,PlayerControl,MonsterControl등에서 사용
    private bool m_isMove = false;
    public bool IsMove { get { return m_isMove; }} //m_isMove 접근용 read only로 get만 가능

    public float MoveStatus //m_fMoveStatus 접근용 프로퍼티
    { get { return m_fMoveStatus; } }



    //***GameManager에게 넘겨받을 인스턴스들***//
    //각각의 Start()에서 넘겨준다.
    GameObject m_objPlayer = null;
    GameObject m_objGrounds = null;

    //*** 직접 객체생성***//
    Generator m_csGenerator = new Generator();


    //***Scene***//관련
    Scene m_currentScene; //현재 씬


    // 부모 Singleton의 Awake()가 존재하기에 해당 Awake()를 무시하지 않고 GameManager의 Awake()를 
    // 정의 하려면 오버라이드 한 후 base.Awake() 즉, 부모 Awake()를 호출해야한다. 
    public override void Awake()
    {
        base.Awake();

    }

    // Start is called before the first frame update
    void Start()
    {
        //new로 생성한 객체들
        //MonoBehavior를 상속받지 않았기에 해당 스크립트의Init()을 해 주어야 한다.
        m_csGenerator.Init();

        m_isInField = false;

        m_currentScene = SceneManager.GetActiveScene();
    }
 

    // Update is called once per frame
    void Update()
    {
        //만약 GameScene에 처음 들어오고 FieldStage에 있을 때 
        //Field 몬스터를 설정 해 주어야 한다.
        if(m_currentScene.name == "GameScene" && m_isInField == false)
        {
            m_isInField = true; //나중에 씬을 나갈때 false로 설정
            SetFieldStage(false); //처음 혹은 다시 씬에 들어왔을 때 이므로 매개변수는 false로 하여 m_nCurrentMobNo는 그대로 유지
        }
    }


    //GameManager에서 필요한 오브젝트들을 넘겨주는 함수 -> 각각의 Start()에서 넘겨준다.
    public void TakeObject(GameObject obj)
    {
        if (obj.CompareTag("Player"))
        {
            m_objPlayer = obj;
        }
        else if (obj.CompareTag("TileMap"))
        {
            m_objGrounds = obj;
            
        }
        else
        {
            Debug.LogError(obj.name.ToString()+"를 GameManager에서 필요로 하지 않습니다.");
        }
    }


    //***Field Stage 정비관련 메소드**//

    //다음 필드스테이지 재정비
    void SetFieldStage(bool isNext)
    {
        //만약 다음 FieldStage라면 속성들을 증가 시켜줘야 한다.
        if (isNext)
        {
            if (m_nCurrentMobNo == m_nMaxFieldMob) //만약 최근에 전투한 FiledMob이 마지막(필드보스)라면
            {
                m_nCurrentMobNo = 0; //Mob번호 초기화
                m_nStage++; //스테이지 증가
            }
            else
            {
                m_nCurrentMobNo++; //Mob번호 증가
            }
        }

        //FieldMob 생성
        if (m_objGrounds != null && m_objPlayer != null)
        {
            Transform trMobParent = m_objGrounds.GetComponent<MoveMap>().GetFieldMobGround();
            if (trMobParent != null)
            {
                bool isFieldBoss = false;
                if(m_nCurrentMobNo == m_nMaxFieldMob)
                {
                    isFieldBoss = true;
                }
                //Generate.cs를 통해 Field Mob 생성
                m_csGenerator.GenerateFieldMob(isFieldBoss, trMobParent, m_objPlayer.transform.position.y);
            }
            else
            {
                Debug.LogError("MoveMap으로 부터 trMobParent를 넘겨받지 못합니다.");
            }
        }
        else
        {
            Debug.LogError("m_objGrounds 혹은 m_objPlayer가 null인지 확인 하세요");
        }
        Debug.Log(string.Format("스테이지 : {0}, 번호 : {1}", m_nStage, m_nCurrentMobNo));
        m_isMove = true;
    }


    //***Player 관련 메소드***//

    //Player의 raycast에서 필드몬스터 검출시 전투준비
    public void SetFieldBattle(GameObject objEnemy)
    {
        m_isMove = false; // 더이상 움직이지 않도록 설정
        m_isPlayerAttack = true; //플레이어가 공격하도록 설정
        m_isFieldBattle = true;
        m_objCurrentMob = objEnemy;
    }
    // 필드 몬스터와의 전투 (사실상 Field 몬스터는 공격을 안하므로 플레이어가 Attack하는 것)
    public void FieldBattle()
    {
        if(m_objCurrentMob != null)
        {
            //현재 플레이어와 대치중인 Field Monster이 살아있는지 확인
            bool isMobAlive = m_objCurrentMob.GetComponent<FieldMobControl>().GetIsAlive();

            if (isMobAlive && m_isFieldBattle)
            {
                //대치중인 몬스터에게 플레이어의 공격력과 공속을 넘겨줘 데미지를 준다.
                m_objCurrentMob.GetComponent<FieldMobControl>().SetDamege(m_fAttackDamage, m_fAttackSpeed);
            }
            else
            {
                //Field몹이 죽었으면
                m_isPlayerAttack = false; //플레이어의 공격을 멈춘다.
                m_objCurrentMob = null;
                m_isFieldBattle=false; //전투종료
                //추후 FieldMobControl.cs에서 GameManager의 FieldMobDie()함수를 호출하면
                //GameManager의 SetNextField()를 호출하여 재정비 한다.
                
            }
        }
        else
        {
            Debug.LogError("GameManager의 m_objCurrentMob이 없습니다.");
        }
    }

    //***Field Monster관련 메소드***//
    
    //Field 몬스터가 사망시 Reward와 다음 Field재정비
    public void FieldMobDie()
    {
        //사실 FieldBattle()에서 전부 false처리 하지만 혹시모를 상황에 다시 초기화
        if(m_isFieldBattle && m_isPlayerAttack)
        {
            m_isFieldBattle = false;
            m_isPlayerAttack = false;

            
        }
        //다음 필드스테이지 재정비
        SetFieldStage(true);
    }

    //Monster Hp 세팅(FieldMob)
    public float SetEnemyHp(GameObject objEnemy)
    {
        if (objEnemy.CompareTag("FieldMob"))
        {
            if(m_nCurrentMobNo == m_nMaxFieldMob)  //만약 Stage마지막 몬스터 일 시 field boss이므로Hp가 조금 더 높다.
            {
                if(m_nStage == 1) //stage가 1일 때에는 증가율을 곱하지 않는다.
                {
                    return m_fBasicFieldBossHp;
                }
                else //stage가 2이상일 때
                {
                    //현재 stage-1 에 증가율을 곱한 값을 기본 체력에 곱하여 체력을 늘린다.
                    // -> stage가 증가할 수록 몬스터의 hp가 증가한다.
                    float fIncrease = (m_nStage - 1) * m_fFieldMobHpInc;
                    float fHp = m_fBasicFieldBossHp * fIncrease;
                    return fHp;
                }
            }
            else //fieldBoss가 아닐 시 
            {
                //기본 field mob체력에 100% ~ 120%(미만)을 랜덤으로 곱하여 설정
                float fHp = m_fBasicFieldMobHp * Random.Range(1, 1.2f);
                if(m_nStage == 1)
                {
                    return fHp;
                }
                else
                {
                    float fIncrease = (m_nStage - 1) * m_fFieldMobHpInc;
                    return fHp * fIncrease;
                }
            }
        }
        else
        {
            Debug.LogError(objEnemy.name.ToString() + "는 enemy태그가 아닙니다.");
            return 0;
        }
    }
}
