using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    //싱글톤 디자인 패턴 사용
    //제너릭 클래스 Singleton을 상속받아 GameManager 싱글톤 인스턴스 생성


    //씬 변환 시 제어용
    bool m_isInGameScene = false; //게임 씬 나갈 시 false, 들어올 시 true로 설정후 FieldMob생성

    //유저 데이터 불러왔는지 확인용 -> 로그아웃 시 초기화
    bool m_isInitUserData = false; //로딩씬이고 해당 변수가 false이면 뒤끝서버에서 데이터를 가져온다.
    public bool IsInitUserData { get { return m_isInitUserData; } }

    //***스테이지(Field) 관련***//
    int m_nStage = 1; //현재 Stage -> LoadingScene에서 서버에서 받아옴
    public int Stage {  get { return m_nStage; } }
    public int CurrentStage {  get { return m_nStage; } }
    const int m_nMaxFieldMob = 7; //Stage 당 몬스터 수
    int m_nCurrentMobNo = 0; //현재 대치중인 field mob 번호 -> 8마리를 잡을 시 Stage++;
    public int CurrentMonNo { get { return m_nCurrentMobNo; } }


    //***돈 관련***//
    long m_lCurrentMoney = 0; //-> LoadingScene에서 서버에서 받아옴
    public long CurrentMoney { get { return m_lCurrentMoney; } }
    float m_fCoinMoveTime = 1f; //동전이 생성되고 CoinUI까지 가는 시간 해당 값에 m_fMoveSpeed값을 곱하여 사용
    public float CoinMoveTime { get { return m_fCoinMoveTime; } }
    const int m_nCoinPrice = 10; //동전 하나당 값 -> m_fMoneyInc를 곱하여 제공
    const float m_fMoneyInc = 1.8f; //Stage별로 Coin당 값 증가량


    //*** 능력치 관련 ***//
    //열거형 상수 타입
    public enum E_Status_Type
    {
        None,
        AttackDamage,
        AttackSpeed,
        MoveSpeed,
        CriticalProb,
        CriticalRatio,
        Max
    }

    //* 능력치 레벨 *// -> LoadingScene에서 뒤끝 서버에서 받아옴
    int m_nAttackDamageLv = 1; 
    int m_nAttackSpeedLv = 1;
    int m_nMoveSpeedLv = 1;
    int m_nCriticalProbLv = 1;
    int m_nCriticalRatioLv = 1;
    public int AttackDamageLv { get { return m_nAttackDamageLv; } }
    public int AttackSpeedLv { get { return m_nAttackSpeedLv; } }
    public int MoveSpeedLv { get {  return m_nMoveSpeedLv; } }
    public int CriticalProbLv { get { return m_nCriticalProbLv; } }
    public int CriticalRatioLv { get { return m_nCriticalRatioLv; } }

    const int m_nMaxDamageLv = 500; //AttackDamage의 최대 레벨
    const int m_nMaxSpeedLv = 201; //Speed계열(move,attack) 최대 레벨 201이면 능력치 300%(레벨 1 : 100%)
    const int m_nMaxCriticalLv = 101; //Critical계열 최대 레벨 101이면 능력치 prob : 100% ,Ratio : 200%
    public int MaxDamageLv { get { return m_nMaxDamageLv; } }
    public int MaxSpeedLv { get { return m_nMaxSpeedLv; } }
    public int MaxCriticalLv { get { return m_nMaxCriticalLv; } }

    //* 능력치 값 *// -> LoadingScene에서 뒤끝 서버에서 받아옴
    private float m_fMoveSpeed = 1; //이동속도 능력치 1이면 100%, 레벨업시 증가
    private float m_fAttackSpeed = 1; //공격속도 위와 동일
    private double m_dAttackDamage = 10; //레벨업시 현재 데미지의 5% 씩 증가 
    private float m_fCriticalProb = 0; //치명타 확률 레벨업 시 0.01씩 증가 최대1
    private float m_fCriticalRatio = 1; //치명타 데미지 배수 -> 치명타가 나올 시 m_fAttackDamage에 곱하여 사용 레벨업 시 0.01f증가
    public double AttackDamage { get { return m_dAttackDamage; } }
    public float AttackSpeed { get { return m_fAttackSpeed; } }
    public float MoveSpeed { get { return m_fMoveSpeed; } }
    public float CriticalProb { get{ return m_fCriticalProb; } }
    public float CriticalRatio { get { return m_fCriticalRatio; } }

    //**레벨업 관련**//
    //Status증가율//
    const float m_fDamageStatusInc = 1.05f; //AttackDamage 증가율 기존 Dagamea에 곱하여 사용
    const float m_fStatusInc = 0.01f; //AttackDamage를 제외 하고는 Percentage이기에 0.01 즉 1% 씩 +=

    //레벨업 가격 (레벨업 시 가격 인상률에 따라 증가)
    long m_lAttackDamagePrice = 50;
    long m_lAttackSpeedPrice = 50;
    long m_lMoveSpeedPrice = 50;
    long m_lCriticalProbPrice = 100;
    long m_lCriticalRatioPrice = 100;
    public long AttackDamagePrice { get { return m_lAttackDamagePrice; } }
    public long AttackSpeedPrice { get {return m_lAttackSpeedPrice; } }
    public long MoveSpeedPrice { get {return m_lMoveSpeedPrice; } }
    public long CriticalProbPrice { get {return m_lCriticalProbPrice; } }
    public long CriticalRatioPrice { get {return m_lCriticalRatioPrice; } }

    //가격 인상률
    const float m_fDamagePriceInc = 1.04f; //m_fAttackDamage가격 인상률
    const float m_fSpeedPriceInc = 1.1f; //Speed계열(공속,이속) 가격 인상률
    const float m_fCriticalPriceInc = 1.25f; //Critical(치명타)계열 가격 인상률





    //**플레이어 전투 관련**//
    bool m_isFieldBattle = false; //전투중일 시 true
    public bool IsFieldBattle { get { return m_isFieldBattle; } }
    bool m_isPlayerAttack = false; //true시 플레이어가 attack 애니메이션을 play하고 animation 클립 이벤트에서 공격관련 함수 호출
    public bool IsPlayerAttack { get { return m_isPlayerAttack; } } 
    GameObject m_objCurrentMob = null; //현재 player와 대치중인 monster
    public GameObject CurrentMob { get { return m_objCurrentMob; } }


    //**필드 몬스터 관련**//
    const float m_fBasicFieldMobHp = 50; //기본 필드몹 hp -> 여기서 랜덤으로 0~20% 증가하여 사용
    const float m_fBasicFieldBossHp = 100; //기본 필드보스 hp
    const float m_fFieldMobHpInc = 1.4f; //Hp 증가율 스테이지 증가 할 때마다 BasicHp에 곱하여 제공


    //이동관련 제어 멤버 변수 MoveBackGround,PlayerControl,MonsterControl등에서 사용
    private bool m_isMove = false;
    public bool IsMove { get { return m_isMove; }} //m_isMove 접근용 read only로 get만 가능


    //코루틴 제어 변수
    bool m_isCorLoadScene = false;
    

    //***GameManager에게 넘겨받을 인스턴스들***//
    //각각의 Start()에서 넘겨준다.
    GameObject m_objPlayer = null;
    GameObject m_objGrounds = null;
    GameObject m_objFieldUI = null;

    //*** 직접 객체생성***//
    Generator m_csGenerator = new Generator();

    

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

        m_isInGameScene = false;

    }
 

    // Update is called once per frame
    void Update()
    {
        //만약 GameScene에 처음 들어올 때 
        //Field 몬스터를 설정 해 주어야 한다.
        if(SceneManager.GetActiveScene().name == "GameScene" && m_isInGameScene == false)
        {
            m_isInGameScene = true; //나중에 씬을 나갈때 false로 설정
            SetFieldStage(false); //처음 혹은 다시 씬에 들어왔을 때 이므로 매개변수는 false로 하여 m_nCurrentMobNo는 그대로 유지
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

    }

    //Scene이동은 비동기 씬이동 LoadingScene을 제외하고는 
    //아래의 함수를 통해 원하는 초를 지정하여 이동
    public void LoadSceneWithTime(string strSceneName, float fTime = 0)
    {
        if(strSceneName != null && strSceneName.Length > 0)
        {
            if (!m_isCorLoadScene && SceneManager.GetActiveScene().name != strSceneName)
            {
                StartCoroutine(CorLoadScene(strSceneName, fTime));
            }
        }
    }
    
    IEnumerator CorLoadScene(string strSceneName, float fTime)
    {
        m_isCorLoadScene = true;
        if(SceneManager.GetActiveScene().name == "GameScene") //만약 게임 씬이라면
        {
            //씬 이동하며 m_isInGameScene을 false로 해 주어야 한다. 
            //이후 다시 GameScene으로 들어올 시 update()에서 SetFieldStage(false);
            m_isInGameScene = false;
        }

        //만약 타이틀 씬으로 이동한다면 로그아웃 되었다는 뜻이므로 m_isInitUserData를 false로 초기화
        if(strSceneName == "TitleScene")
        {
            m_isInitUserData = false;
        }

        //만약 씬 이동시 timeScale이 0이라면 다시 1로 바꾸어준다.
        //예로 바로 위 조건문에서 GameScene -> TitleScene이동시 
        //메뉴 페널을 열어(timeScale = 0이된다.) 로그아웃 버튼을 누른 상황에서 timeScale을 1로 바꾸어 주어야 한다. 
        if(Time.timeScale == 0) 
        {
            Time.timeScale = 1;
        }

        yield return new WaitForSeconds(fTime);
        m_isCorLoadScene = false;
        SceneManager.LoadScene(strSceneName);
    }


    //LoadingScene에서 BackendManaer.Instance의 LoadUserData()를 호출하면 비동기로 USER_DATA의 테이블을
    //가져와 완료될 시 해당 함수를 호출한다. 넘겨받은 JsonData 를 해당 멤버변수에 넣어준다
    public void SetUserData(LitJson.JsonData jsonData)
    {
        if (jsonData.Count > 0)
        {
            //스테이지 및 돈 저장
            m_nStage = int.Parse(jsonData[0]["Stage"].ToString());
            m_lCurrentMoney = long.Parse(jsonData[0]["Money"].ToString());

            //능력치 값 저장
            m_dAttackDamage = double.Parse(jsonData[0]["AttackDamage"].ToString());
            m_fAttackSpeed = float.Parse(jsonData[0]["AttackSpeed"].ToString());
            m_fMoveSpeed = float.Parse(jsonData[0]["MoveSpeed"].ToString());
            m_fCriticalProb = float.Parse(jsonData[0]["CriticalProb"].ToString());
            m_fCriticalRatio = float.Parse(jsonData[0]["CriticalRatio"].ToString());

            //능력치 레벨 저장
            m_nAttackDamageLv = int.Parse(jsonData[0]["AttackDamageLv"].ToString());
            m_nAttackSpeedLv = int.Parse(jsonData[0]["AttackSpeedLv"].ToString());
            m_nMoveSpeedLv = int.Parse(jsonData[0]["MoveSpeedLv"].ToString());
            m_nCriticalProbLv = int.Parse(jsonData[0]["CriticalProbLv"].ToString());
            m_nCriticalRatioLv = int.Parse(jsonData[0]["CriticalRatioLv"].ToString());

            //레벨업 가격 저장
            m_lAttackDamagePrice = long.Parse(jsonData[0]["AttackDamagePrice"].ToString());
            m_lAttackSpeedPrice = long.Parse(jsonData[0]["AttackSpeedPrice"].ToString());
            m_lMoveSpeedPrice = long.Parse(jsonData[0]["MoveSpeedPrice"].ToString());
            m_lCriticalProbPrice = long.Parse(jsonData[0]["CriticalProbPrice"].ToString());
            m_lCriticalRatioPrice = long.Parse(jsonData[0]["CriticalRatioPrice"].ToString());

            m_isInitUserData = true;
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
        else if (obj.CompareTag("FieldUI"))
        {
            m_objFieldUI = obj;
        }
        else
        {
            Debug.LogError(obj.name.ToString()+"를 GameManager에서 필요로 하지 않습니다.");
        }
    }


    //***Money관련 메소드***//

    //돈 증가(리워드)
    void MoneyReward(int nCoin)
    {
        if(m_nStage <= 1)
        {
            //스테이지가 1일 경우에는 CoinPrice에 Coin 수만큼 곱하여 기존 값에 더해준다.
            m_lCurrentMoney += nCoin * m_nCoinPrice;
        }
        else
        {
            //스테이지가 2 이상일 경우 현재 스테이지-1에서 m_fMoneyInc를 곱한 값을 m_nCoinPrice에 곱해준다.
            //FieldMob 체력증가와 같은 방식, Stage가 오를 수록 자동으로 보상도 커진다.
            float fPrice = ((m_nStage - 1) *m_fMoneyInc) * m_nCoinPrice;
            //m_nCurrentMoney 가 long type이므로 fPrice를 반올림한Int로 바꾸어 nCoin 수 만큼 곱해준다.
            m_lCurrentMoney += nCoin * Mathf.RoundToInt(fPrice);
        }

        if (m_objFieldUI != null) 
        {
            float fWaitTime = m_fCoinMoveTime / m_fMoveSpeed;
            //FieldUI의 moneyText를 현재 값으로 변경해준다.(Tween 변환)
            m_objFieldUI.GetComponent<FieldUI>().SetMoneyText(fWaitTime, m_lCurrentMoney,true,true);   
        }
        else
        {
            Debug.LogError("m_objFieldUI가 없습니다.");
        }
    }

    //위 MoneyReward()는 몹 사냥후 코인 얻을 때 로직이며 코인 tween없이 즉시 세팅(접속보상등)private
    private void SetMoney(long lMoney)
    {
        m_lCurrentMoney += lMoney;
        if(m_objFieldUI != null)
        {
            m_objFieldUI.GetComponent<FieldUI>().SetMoneyText(0, m_lCurrentMoney);
        }
    }

    void SpendMoney(long nMoney)
    {
        if (m_lCurrentMoney >= nMoney)
        {
            m_lCurrentMoney -= nMoney;
        }
        else
        {
            Debug.LogError("돈 부족 SpendMoney 호출 로직 확인 바람");
            return;
        }

        if (m_objFieldUI != null)
        {
            m_objFieldUI.GetComponent<FieldUI>().SetMoneyText(0, m_lCurrentMoney,true,false);
        }
        else
        {
            Debug.LogError("m_objFieldUI가 없습니다.");
        }
    }


    //레벨 업(Scroll View의 버튼 클릭 시)//
    // E_Status_Type 별로 레벨업이 가능하면 진행 후 true반환, 불가능 할 시 false 반환
    public bool LevelUp(E_Status_Type eStatusType)
    {
        if(eStatusType == E_Status_Type.AttackDamage)
        {
            if(m_lAttackDamagePrice <= m_lCurrentMoney && m_nAttackDamageLv < m_nMaxDamageLv)
            {
                //돈 소비
                SpendMoney(m_lAttackDamagePrice);
                //레벨업
                m_nAttackDamageLv++;
                //데미지 증가(소수점 올림) 데미지는 m_fADStausInc를 현제 데미지에 곱한다.
                m_dAttackDamage = System.Math.Ceiling(m_dAttackDamage * m_fDamageStatusInc);
                //가격 인상
                m_lAttackDamagePrice = (long)Mathf.Ceil(m_lAttackDamagePrice * m_fDamagePriceInc);
                return true;
            }
            else
            {
                //돈이 부족하거나 최대 레벨인 경우
                return false;
            }
        }
        else if (eStatusType == E_Status_Type.AttackSpeed)
        {
            if (m_lAttackSpeedPrice <= m_lCurrentMoney && m_nAttackSpeedLv < m_nMaxSpeedLv)
            {
                //돈 소비
                SpendMoney(m_lAttackSpeedPrice);
                //레벨업
                m_nAttackSpeedLv++;
                //공속 1% 증가.
                m_fAttackSpeed += m_fStatusInc;
                //가격 인상
                m_lAttackSpeedPrice = (long)Mathf.Ceil(m_lAttackSpeedPrice * m_fSpeedPriceInc);
                return true;
            }
            else
            {
                //돈이 부족하거나 최대 레벨인 경우
                return false;
            }
        }
        else if (eStatusType == E_Status_Type.MoveSpeed)
        {
            if (m_lMoveSpeedPrice <= m_lCurrentMoney && m_nMoveSpeedLv < m_nMaxSpeedLv)
            {
                //돈 소비
                SpendMoney(m_lMoveSpeedPrice);
                //레벨업
                m_nMoveSpeedLv++;
                //이속 1% 증가.
                m_fMoveSpeed += m_fStatusInc;
                //가격 인상
                m_lMoveSpeedPrice = (long)Mathf.Ceil(m_lMoveSpeedPrice * m_fSpeedPriceInc);
                return true;
            }
            else
            {
                //돈이 부족하거나 최대 레벨인 경우
                return false;
            }
        }
        else if (eStatusType == E_Status_Type.CriticalProb)
        {
            if (m_lCriticalProbPrice <= m_lCurrentMoney && m_nCriticalProbLv < m_nMaxCriticalLv)
            {
                //돈 소비
                SpendMoney(m_lCriticalProbPrice);
                //레벨업
                m_nCriticalProbLv++;
                //치명타 확률 1% 증가.
                m_fCriticalProb += m_fStatusInc;
                //가격 인상
                m_lCriticalProbPrice = (long)Mathf.Ceil(m_lCriticalProbPrice * m_fCriticalPriceInc);
                return true;
            }
            else
            {
                //돈이 부족하거나 최대 레벨인 경우
                return false;
            }
        }
        else if (eStatusType == E_Status_Type.CriticalRatio)
        {
            if (m_lCriticalRatioPrice <= m_lCurrentMoney && m_nCriticalRatioLv < m_nMaxCriticalLv)
            {
                //돈 소비
                SpendMoney(m_lCriticalRatioPrice);
                //레벨업
                m_nCriticalRatioLv++;
                //치명타 배수 1% 증가.
                m_fCriticalRatio += m_fStatusInc;
                //가격 인상
                m_lCriticalRatioPrice = (long)Mathf.Ceil(m_lCriticalRatioPrice * m_fCriticalPriceInc);
                return true;
            }
            else
            {
                //돈이 부족하거나 최대 레벨인 경우
                return false;
            }
        }
        else
        {
            Debug.LogError("E_Status_Type 확인 바람");
            return false;
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
                //스테이지 증가시 자동 USER_DATA 뒤끝 서버에 Update(비동기)
                BackendManager.Instance.SaveUserData(false); //false는 로그아웃을 안하겠다는 매개인자

                //StageText 세팅(Tweening)
                if (m_objFieldUI != null)
                {
                    m_objFieldUI.GetComponent<FieldUI>().SetStageText(m_nStage);
                }
                else
                {
                    Debug.LogError("m_objFieldUI가 없습니다.");
                }
            }
            else
            {
                m_nCurrentMobNo++; //Mob번호 증가
            }
        }

        //FieldMob 생성
        if (m_objGrounds != null && m_objPlayer != null)
        {
            //m_objGround에서 생성될 Field몹의 부모가 될 타일맵의 Tranform을 받아온다.
            Transform trMobParent = m_objGrounds.GetComponent<MoveMap>().GetFieldMobGround();
            if (trMobParent != null)
            {
                bool isFieldBoss = false;
                if(m_nCurrentMobNo == m_nMaxFieldMob)
                {
                    isFieldBoss = true;
                }

                //Generate.cs를 통해 Field Mob 생성
                if (m_csGenerator != null)
                {
                    if(m_objFieldUI != null)
                    {
                        //FieldMob을 생성할 때 EnemyHpBar프리팹도 동시에 생성해야 한다. 둘 다 GameScene에서 생성되므로 m_objFieldUI의 부모 Canvas를 넘겨준다.
                        Canvas mainCanvas = m_objFieldUI.GetComponentInParent<Canvas>();
                        m_csGenerator.GenerateFieldMob(isFieldBoss, trMobParent, m_objPlayer.transform.position.y,mainCanvas);
                    }
                    else 
                    {
                    
                    }
                }
                else
                {
                    Debug.LogError("m_csGenerator가 없습니다.");
                }
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

    //플레이어가 공격했을시 치명타 확률에 따라 bool형태로 반환
    bool IsCritical()
    {
        
        float fCriticalProb = m_fCriticalProb * 100;
        //1~100 정수중 랜덤 값이 m_fCriticalProb에 100을 곱한 값 보다 낮으면 치명타 성공, 클 시 실패
        //따라서 치명타 확률이 0이라면, 무조건 false이다.
        if (Random.Range(1,101) <= fCriticalProb)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

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
                double fAttackDamage = 0;
                //치명타 여부 확인
                bool isCritical = IsCritical();
                if (isCritical)
                {
                    //치명타에 걸렸을 시 m_dAttackDamage에 치명타 배수(m_fCriticalRatio)를 곱해준다.
                    fAttackDamage = m_dAttackDamage * m_fCriticalRatio;
                }
                else
                {
                    fAttackDamage = m_dAttackDamage;
                }

                //대치중인 몬스터에게 플레이어의 공격력과 공속을 넘겨줘 데미지를 준다.
                m_objCurrentMob.GetComponent<FieldMobControl>().SetDamege(fAttackDamage, m_fAttackSpeed);
                
                //DamageText프리팹(Tweening) 생성
                if(m_csGenerator != null)
                {
                    Vector2 vecMobHead = Vector2.zero;
                    if(m_objCurrentMob != null && m_objFieldUI)
                    {
                        vecMobHead = m_objCurrentMob.GetComponent<FieldMobControl>().GetHeadPos();
                        Canvas canvas = null;
                        
                        //DamageText프리팹의 부모로 들어갈 main canvas가 필요하다. 다만 성능 문제로 Find함수는 지양하기위해
                        //DamageText는 GameScene에서 생성되므로 m_objFieldUI(cavas자식 panel)가 존재하므로 GetComponetInParent로 cavas를 가져온다.
                        if(m_objFieldUI != null)
                        {
                            canvas = m_objFieldUI.GetComponentInParent<Canvas>();
                            //Generator.cs 의 GenerateDamageText()를 통해 Tweening하는 DamageText 프리팹 생성
                            m_csGenerator.GenerateDamageText(canvas,vecMobHead, fAttackDamage, m_fAttackSpeed, isCritical);
                        }
                        else
                        {
                            Debug.LogError("m_objFieldUI가 없습니다.");
                        }

                    }
                    else
                    {
                        Debug.LogError("m_objCurrentMob이 없습니다.");
                    }
                }
                else
                {

                }

            }
            else
            {
                //Field몹이 죽었으면
                m_isPlayerAttack = false; //플레이어의 공격을 멈춘다.
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

        //Coin 보상
        int nCoin = 0;// 보상할 코인 수
        //필드보스인지 아닌지에 따라 다르게 보상한다.ㄴ
        if(m_nCurrentMobNo == m_nMaxFieldMob)
        {
            nCoin = Random.Range(9, 13);
        }
        else
        {
            nCoin = Random.Range(4, 7);
        }

        //FieldUI의 coinUI RectTransform이 필요하므로 검사
        if (m_objFieldUI != null)
        {
            RectTransform recTarget = m_objFieldUI.GetComponent<FieldUI>().GetCoinUIRect();
            if (m_objCurrentMob != null) 
            {
                //현재 죽은 FieldMob의 위치에 nCoin만큼 코인을 생성하여 recTarget으로 이동시킨다.
                m_csGenerator.GenerateCoin(m_objCurrentMob.transform.position,recTarget,nCoin);
            }
            else
            {
                Debug.LogError("m_objCurrentMob이 null이 되었습니다. 확인하세요");
            }
        }
        else
        {
            Debug.LogError("m_objFieldUI 가 없습니다.");
        }

        //nCoin만큼 m_nCrrentMoney를 증가 시켜준다.
        MoneyReward(nCoin);
        m_objCurrentMob = null; //초기화



        //다음 필드스테이지 재정비
        // 여기서 중요한 점은 위에 Coin Generate와 EarnMoney에서 m_nCurrentMobNo 또는 m_nStage가 필요하므로
        //SetFieldStage를 마지막에 호출하여 필요한 로직이후 m_nCurrentMobNo 또는 m_nStage를 증가시켜줘야 한다.
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
                    // -> stage가 증가할 수록 몬스터의 hp가 자동으로 증가한다.
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
