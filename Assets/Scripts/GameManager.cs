using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //싱글톤 디자인 패턴 사용
    //정적 인스턴스 생성
    // 보안을 위해 set 프로퍼티는 private로 설정하여 
    // 다른 클래스에서 get은 가능하나 인스턴스 초기화는 오직 GameManager에서만 가능하다.
    public static GameManager Instance { get; private set; }


    //***스테이지 관련***//
    int m_nStage = 1; //현재 Stage
    const int m_nMaxFieldMob = 8; //Stage 당 몬스터 수
    int m_nCurrentMobNo = 1; //현재 대치중인 field mob 번호 -> 8마리를 잡을 시 Stage++;
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

    private void Awake()
    {
        if(Instance != null)
        {
            //새롭게 GameManager가 생성되었을 때 정적 instance가 존재한다면 
            //GameManager가 중복이기에 제거
            Destroy(gameObject);
        }
        else
        {
            //만약 정적 instance가 null이라면, 해당 객체를 넘겨주고 DontDestroy
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        m_isMove=true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    //GameManager에서 필요한 오브젝트들을 넘겨주는 함수 -> 각각의 Start()에서 넘겨준다.
    public void TakeObject(GameObject obj)
    {
        if (obj.CompareTag("Player"))
        {
            m_objPlayer = obj;
        }
        else
        {
            Debug.LogError(obj.name.ToString()+"를 GameManager에서 필요로 하지 않습니다.");
        }
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

            if (isMobAlive)
            {
                //대치중인 몬스터에게 플레이어의 공격력과 공속을 넘겨줘 데미지를 준다.
                m_objCurrentMob.GetComponent<FieldMobControl>().SetDamege(m_fAttackDamage, m_fAttackSpeed);
            }
            else
            {
                //Field몹이 죽었으면
                m_isPlayerAttack = false; //플레이어의 공격을 멈춘다.
                m_objCurrentMob = null;
                //추후 
            }
        }
        else
        {
            Debug.LogError("GameManager의 m_objCurrentMob이 없습니다.");
        }
    }

    //***Field Monster관련 메소드***//


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
