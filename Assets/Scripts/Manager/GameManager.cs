using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    //�̱��� ������ ���� ���
    //���ʸ� Ŭ���� Singleton�� ��ӹ޾� GameManager �̱��� �ν��Ͻ� ����


    //�� ��ȯ �� �����
    bool m_isInGameScene = false; //���� �� ���� �� false, ���� �� true�� ������ FieldMob����

    //���� ������ �ҷ��Դ��� Ȯ�ο� -> �α׾ƿ� �� �ʱ�ȭ
    bool m_isInitUserData = false; //�ε����̰� �ش� ������ false�̸� �ڳ��������� �����͸� �����´�.
    public bool IsInitUserData { get { return m_isInitUserData; } }

    //***��������(Field) ����***//
    int m_nStage = 1; //���� Stage -> LoadingScene���� �������� �޾ƿ�
    public int Stage {  get { return m_nStage; } }
    public int CurrentStage {  get { return m_nStage; } }
    const int m_nMaxFieldMob = 7; //Stage �� ���� ��
    int m_nCurrentMobNo = 0; //���� ��ġ���� field mob ��ȣ -> 8������ ���� �� Stage++;
    public int CurrentMonNo { get { return m_nCurrentMobNo; } }


    //***�� ����***//
    long m_lCurrentMoney = 0; //-> LoadingScene���� �������� �޾ƿ�
    public long CurrentMoney { get { return m_lCurrentMoney; } }
    float m_fCoinMoveTime = 1f; //������ �����ǰ� CoinUI���� ���� �ð� �ش� ���� m_fMoveSpeed���� ���Ͽ� ���
    public float CoinMoveTime { get { return m_fCoinMoveTime; } }
    const int m_nCoinPrice = 10; //���� �ϳ��� �� -> m_fMoneyInc�� ���Ͽ� ����
    const float m_fMoneyInc = 1.8f; //Stage���� Coin�� �� ������


    //*** �ɷ�ġ ���� ***//
    //������ ��� Ÿ��
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

    //* �ɷ�ġ ���� *// -> LoadingScene���� �ڳ� �������� �޾ƿ�
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

    const int m_nMaxDamageLv = 500; //AttackDamage�� �ִ� ����
    const int m_nMaxSpeedLv = 201; //Speed�迭(move,attack) �ִ� ���� 201�̸� �ɷ�ġ 300%(���� 1 : 100%)
    const int m_nMaxCriticalLv = 101; //Critical�迭 �ִ� ���� 101�̸� �ɷ�ġ prob : 100% ,Ratio : 200%
    public int MaxDamageLv { get { return m_nMaxDamageLv; } }
    public int MaxSpeedLv { get { return m_nMaxSpeedLv; } }
    public int MaxCriticalLv { get { return m_nMaxCriticalLv; } }

    //* �ɷ�ġ �� *// -> LoadingScene���� �ڳ� �������� �޾ƿ�
    private float m_fMoveSpeed = 1; //�̵��ӵ� �ɷ�ġ 1�̸� 100%, �������� ����
    private float m_fAttackSpeed = 1; //���ݼӵ� ���� ����
    private double m_dAttackDamage = 10; //�������� ���� �������� 5% �� ���� 
    private float m_fCriticalProb = 0; //ġ��Ÿ Ȯ�� ������ �� 0.01�� ���� �ִ�1
    private float m_fCriticalRatio = 1; //ġ��Ÿ ������ ��� -> ġ��Ÿ�� ���� �� m_fAttackDamage�� ���Ͽ� ��� ������ �� 0.01f����
    public double AttackDamage { get { return m_dAttackDamage; } }
    public float AttackSpeed { get { return m_fAttackSpeed; } }
    public float MoveSpeed { get { return m_fMoveSpeed; } }
    public float CriticalProb { get{ return m_fCriticalProb; } }
    public float CriticalRatio { get { return m_fCriticalRatio; } }

    //**������ ����**//
    //Status������//
    const float m_fDamageStatusInc = 1.05f; //AttackDamage ������ ���� Dagamea�� ���Ͽ� ���
    const float m_fStatusInc = 0.01f; //AttackDamage�� ���� �ϰ�� Percentage�̱⿡ 0.01 �� 1% �� +=

    //������ ���� (������ �� ���� �λ���� ���� ����)
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

    //���� �λ��
    const float m_fDamagePriceInc = 1.04f; //m_fAttackDamage���� �λ��
    const float m_fSpeedPriceInc = 1.1f; //Speed�迭(����,�̼�) ���� �λ��
    const float m_fCriticalPriceInc = 1.25f; //Critical(ġ��Ÿ)�迭 ���� �λ��





    //**�÷��̾� ���� ����**//
    bool m_isFieldBattle = false; //�������� �� true
    public bool IsFieldBattle { get { return m_isFieldBattle; } }
    bool m_isPlayerAttack = false; //true�� �÷��̾ attack �ִϸ��̼��� play�ϰ� animation Ŭ�� �̺�Ʈ���� ���ݰ��� �Լ� ȣ��
    public bool IsPlayerAttack { get { return m_isPlayerAttack; } } 
    GameObject m_objCurrentMob = null; //���� player�� ��ġ���� monster
    public GameObject CurrentMob { get { return m_objCurrentMob; } }


    //**�ʵ� ���� ����**//
    const float m_fBasicFieldMobHp = 50; //�⺻ �ʵ�� hp -> ���⼭ �������� 0~20% �����Ͽ� ���
    const float m_fBasicFieldBossHp = 100; //�⺻ �ʵ庸�� hp
    const float m_fFieldMobHpInc = 1.4f; //Hp ������ �������� ���� �� ������ BasicHp�� ���Ͽ� ����


    //�̵����� ���� ��� ���� MoveBackGround,PlayerControl,MonsterControl��� ���
    private bool m_isMove = false;
    public bool IsMove { get { return m_isMove; }} //m_isMove ���ٿ� read only�� get�� ����


    //�ڷ�ƾ ���� ����
    bool m_isCorLoadScene = false;
    

    //***GameManager���� �Ѱܹ��� �ν��Ͻ���***//
    //������ Start()���� �Ѱ��ش�.
    GameObject m_objPlayer = null;
    GameObject m_objGrounds = null;
    GameObject m_objFieldUI = null;

    //*** ���� ��ü����***//
    Generator m_csGenerator = new Generator();

    

    // �θ� Singleton�� Awake()�� �����ϱ⿡ �ش� Awake()�� �������� �ʰ� GameManager�� Awake()�� 
    // ���� �Ϸ��� �������̵� �� �� base.Awake() ��, �θ� Awake()�� ȣ���ؾ��Ѵ�. 
    public override void Awake()
    {
        base.Awake();

    }

    // Start is called before the first frame update
    void Start()
    {
        //new�� ������ ��ü��
        //MonoBehavior�� ��ӹ��� �ʾұ⿡ �ش� ��ũ��Ʈ��Init()�� �� �־�� �Ѵ�.
        m_csGenerator.Init();

        m_isInGameScene = false;

    }
 

    // Update is called once per frame
    void Update()
    {
        //���� GameScene�� ó�� ���� �� 
        //Field ���͸� ���� �� �־�� �Ѵ�.
        if(SceneManager.GetActiveScene().name == "GameScene" && m_isInGameScene == false)
        {
            m_isInGameScene = true; //���߿� ���� ������ false�� ����
            SetFieldStage(false); //ó�� Ȥ�� �ٽ� ���� ������ �� �̹Ƿ� �Ű������� false�� �Ͽ� m_nCurrentMobNo�� �״�� ����
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

    }

    //Scene�̵��� �񵿱� ���̵� LoadingScene�� �����ϰ�� 
    //�Ʒ��� �Լ��� ���� ���ϴ� �ʸ� �����Ͽ� �̵�
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
        if(SceneManager.GetActiveScene().name == "GameScene") //���� ���� ���̶��
        {
            //�� �̵��ϸ� m_isInGameScene�� false�� �� �־�� �Ѵ�. 
            //���� �ٽ� GameScene���� ���� �� update()���� SetFieldStage(false);
            m_isInGameScene = false;
        }

        //���� Ÿ��Ʋ ������ �̵��Ѵٸ� �α׾ƿ� �Ǿ��ٴ� ���̹Ƿ� m_isInitUserData�� false�� �ʱ�ȭ
        if(strSceneName == "TitleScene")
        {
            m_isInitUserData = false;
        }

        //���� �� �̵��� timeScale�� 0�̶�� �ٽ� 1�� �ٲپ��ش�.
        //���� �ٷ� �� ���ǹ����� GameScene -> TitleScene�̵��� 
        //�޴� ����� ����(timeScale = 0�̵ȴ�.) �α׾ƿ� ��ư�� ���� ��Ȳ���� timeScale�� 1�� �ٲپ� �־�� �Ѵ�. 
        if(Time.timeScale == 0) 
        {
            Time.timeScale = 1;
        }

        yield return new WaitForSeconds(fTime);
        m_isCorLoadScene = false;
        SceneManager.LoadScene(strSceneName);
    }


    //LoadingScene���� BackendManaer.Instance�� LoadUserData()�� ȣ���ϸ� �񵿱�� USER_DATA�� ���̺���
    //������ �Ϸ�� �� �ش� �Լ��� ȣ���Ѵ�. �Ѱܹ��� JsonData �� �ش� ��������� �־��ش�
    public void SetUserData(LitJson.JsonData jsonData)
    {
        if (jsonData.Count > 0)
        {
            //�������� �� �� ����
            m_nStage = int.Parse(jsonData[0]["Stage"].ToString());
            m_lCurrentMoney = long.Parse(jsonData[0]["Money"].ToString());

            //�ɷ�ġ �� ����
            m_dAttackDamage = double.Parse(jsonData[0]["AttackDamage"].ToString());
            m_fAttackSpeed = float.Parse(jsonData[0]["AttackSpeed"].ToString());
            m_fMoveSpeed = float.Parse(jsonData[0]["MoveSpeed"].ToString());
            m_fCriticalProb = float.Parse(jsonData[0]["CriticalProb"].ToString());
            m_fCriticalRatio = float.Parse(jsonData[0]["CriticalRatio"].ToString());

            //�ɷ�ġ ���� ����
            m_nAttackDamageLv = int.Parse(jsonData[0]["AttackDamageLv"].ToString());
            m_nAttackSpeedLv = int.Parse(jsonData[0]["AttackSpeedLv"].ToString());
            m_nMoveSpeedLv = int.Parse(jsonData[0]["MoveSpeedLv"].ToString());
            m_nCriticalProbLv = int.Parse(jsonData[0]["CriticalProbLv"].ToString());
            m_nCriticalRatioLv = int.Parse(jsonData[0]["CriticalRatioLv"].ToString());

            //������ ���� ����
            m_lAttackDamagePrice = long.Parse(jsonData[0]["AttackDamagePrice"].ToString());
            m_lAttackSpeedPrice = long.Parse(jsonData[0]["AttackSpeedPrice"].ToString());
            m_lMoveSpeedPrice = long.Parse(jsonData[0]["MoveSpeedPrice"].ToString());
            m_lCriticalProbPrice = long.Parse(jsonData[0]["CriticalProbPrice"].ToString());
            m_lCriticalRatioPrice = long.Parse(jsonData[0]["CriticalRatioPrice"].ToString());

            m_isInitUserData = true;
        }
    }

    //GameManager���� �ʿ��� ������Ʈ���� �Ѱ��ִ� �Լ� -> ������ Start()���� �Ѱ��ش�.
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
            Debug.LogError(obj.name.ToString()+"�� GameManager���� �ʿ�� ���� �ʽ��ϴ�.");
        }
    }


    //***Money���� �޼ҵ�***//

    //�� ����(������)
    void MoneyReward(int nCoin)
    {
        if(m_nStage <= 1)
        {
            //���������� 1�� ��쿡�� CoinPrice�� Coin ����ŭ ���Ͽ� ���� ���� �����ش�.
            m_lCurrentMoney += nCoin * m_nCoinPrice;
        }
        else
        {
            //���������� 2 �̻��� ��� ���� ��������-1���� m_fMoneyInc�� ���� ���� m_nCoinPrice�� �����ش�.
            //FieldMob ü�������� ���� ���, Stage�� ���� ���� �ڵ����� ���� Ŀ����.
            float fPrice = ((m_nStage - 1) *m_fMoneyInc) * m_nCoinPrice;
            //m_nCurrentMoney �� long type�̹Ƿ� fPrice�� �ݿø���Int�� �ٲپ� nCoin �� ��ŭ �����ش�.
            m_lCurrentMoney += nCoin * Mathf.RoundToInt(fPrice);
        }

        if (m_objFieldUI != null) 
        {
            float fWaitTime = m_fCoinMoveTime / m_fMoveSpeed;
            //FieldUI�� moneyText�� ���� ������ �������ش�.(Tween ��ȯ)
            m_objFieldUI.GetComponent<FieldUI>().SetMoneyText(fWaitTime, m_lCurrentMoney,true,true);   
        }
        else
        {
            Debug.LogError("m_objFieldUI�� �����ϴ�.");
        }
    }

    //�� MoneyReward()�� �� ����� ���� ���� �� �����̸� ���� tween���� ��� ����(���Ӻ����)private
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
            Debug.LogError("�� ���� SpendMoney ȣ�� ���� Ȯ�� �ٶ�");
            return;
        }

        if (m_objFieldUI != null)
        {
            m_objFieldUI.GetComponent<FieldUI>().SetMoneyText(0, m_lCurrentMoney,true,false);
        }
        else
        {
            Debug.LogError("m_objFieldUI�� �����ϴ�.");
        }
    }


    //���� ��(Scroll View�� ��ư Ŭ�� ��)//
    // E_Status_Type ���� �������� �����ϸ� ���� �� true��ȯ, �Ұ��� �� �� false ��ȯ
    public bool LevelUp(E_Status_Type eStatusType)
    {
        if(eStatusType == E_Status_Type.AttackDamage)
        {
            if(m_lAttackDamagePrice <= m_lCurrentMoney && m_nAttackDamageLv < m_nMaxDamageLv)
            {
                //�� �Һ�
                SpendMoney(m_lAttackDamagePrice);
                //������
                m_nAttackDamageLv++;
                //������ ����(�Ҽ��� �ø�) �������� m_fADStausInc�� ���� �������� ���Ѵ�.
                m_dAttackDamage = System.Math.Ceiling(m_dAttackDamage * m_fDamageStatusInc);
                //���� �λ�
                m_lAttackDamagePrice = (long)Mathf.Ceil(m_lAttackDamagePrice * m_fDamagePriceInc);
                return true;
            }
            else
            {
                //���� �����ϰų� �ִ� ������ ���
                return false;
            }
        }
        else if (eStatusType == E_Status_Type.AttackSpeed)
        {
            if (m_lAttackSpeedPrice <= m_lCurrentMoney && m_nAttackSpeedLv < m_nMaxSpeedLv)
            {
                //�� �Һ�
                SpendMoney(m_lAttackSpeedPrice);
                //������
                m_nAttackSpeedLv++;
                //���� 1% ����.
                m_fAttackSpeed += m_fStatusInc;
                //���� �λ�
                m_lAttackSpeedPrice = (long)Mathf.Ceil(m_lAttackSpeedPrice * m_fSpeedPriceInc);
                return true;
            }
            else
            {
                //���� �����ϰų� �ִ� ������ ���
                return false;
            }
        }
        else if (eStatusType == E_Status_Type.MoveSpeed)
        {
            if (m_lMoveSpeedPrice <= m_lCurrentMoney && m_nMoveSpeedLv < m_nMaxSpeedLv)
            {
                //�� �Һ�
                SpendMoney(m_lMoveSpeedPrice);
                //������
                m_nMoveSpeedLv++;
                //�̼� 1% ����.
                m_fMoveSpeed += m_fStatusInc;
                //���� �λ�
                m_lMoveSpeedPrice = (long)Mathf.Ceil(m_lMoveSpeedPrice * m_fSpeedPriceInc);
                return true;
            }
            else
            {
                //���� �����ϰų� �ִ� ������ ���
                return false;
            }
        }
        else if (eStatusType == E_Status_Type.CriticalProb)
        {
            if (m_lCriticalProbPrice <= m_lCurrentMoney && m_nCriticalProbLv < m_nMaxCriticalLv)
            {
                //�� �Һ�
                SpendMoney(m_lCriticalProbPrice);
                //������
                m_nCriticalProbLv++;
                //ġ��Ÿ Ȯ�� 1% ����.
                m_fCriticalProb += m_fStatusInc;
                //���� �λ�
                m_lCriticalProbPrice = (long)Mathf.Ceil(m_lCriticalProbPrice * m_fCriticalPriceInc);
                return true;
            }
            else
            {
                //���� �����ϰų� �ִ� ������ ���
                return false;
            }
        }
        else if (eStatusType == E_Status_Type.CriticalRatio)
        {
            if (m_lCriticalRatioPrice <= m_lCurrentMoney && m_nCriticalRatioLv < m_nMaxCriticalLv)
            {
                //�� �Һ�
                SpendMoney(m_lCriticalRatioPrice);
                //������
                m_nCriticalRatioLv++;
                //ġ��Ÿ ��� 1% ����.
                m_fCriticalRatio += m_fStatusInc;
                //���� �λ�
                m_lCriticalRatioPrice = (long)Mathf.Ceil(m_lCriticalRatioPrice * m_fCriticalPriceInc);
                return true;
            }
            else
            {
                //���� �����ϰų� �ִ� ������ ���
                return false;
            }
        }
        else
        {
            Debug.LogError("E_Status_Type Ȯ�� �ٶ�");
            return false;
        }
    }



    //***Field Stage ������� �޼ҵ�**//

    //���� �ʵ彺������ ������
    void SetFieldStage(bool isNext)
    {
        //���� ���� FieldStage��� �Ӽ����� ���� ������� �Ѵ�.
        if (isNext)
        {
            if (m_nCurrentMobNo == m_nMaxFieldMob) //���� �ֱٿ� ������ FiledMob�� ������(�ʵ庸��)���
            {
                m_nCurrentMobNo = 0; //Mob��ȣ �ʱ�ȭ
                m_nStage++; //�������� ����
                //�������� ������ �ڵ� USER_DATA �ڳ� ������ Update(�񵿱�)
                BackendManager.Instance.SaveUserData(false); //false�� �α׾ƿ��� ���ϰڴٴ� �Ű�����

                //StageText ����(Tweening)
                if (m_objFieldUI != null)
                {
                    m_objFieldUI.GetComponent<FieldUI>().SetStageText(m_nStage);
                }
                else
                {
                    Debug.LogError("m_objFieldUI�� �����ϴ�.");
                }
            }
            else
            {
                m_nCurrentMobNo++; //Mob��ȣ ����
            }
        }

        //FieldMob ����
        if (m_objGrounds != null && m_objPlayer != null)
        {
            //m_objGround���� ������ Field���� �θ� �� Ÿ�ϸ��� Tranform�� �޾ƿ´�.
            Transform trMobParent = m_objGrounds.GetComponent<MoveMap>().GetFieldMobGround();
            if (trMobParent != null)
            {
                bool isFieldBoss = false;
                if(m_nCurrentMobNo == m_nMaxFieldMob)
                {
                    isFieldBoss = true;
                }

                //Generate.cs�� ���� Field Mob ����
                if (m_csGenerator != null)
                {
                    if(m_objFieldUI != null)
                    {
                        //FieldMob�� ������ �� EnemyHpBar�����յ� ���ÿ� �����ؾ� �Ѵ�. �� �� GameScene���� �����ǹǷ� m_objFieldUI�� �θ� Canvas�� �Ѱ��ش�.
                        Canvas mainCanvas = m_objFieldUI.GetComponentInParent<Canvas>();
                        m_csGenerator.GenerateFieldMob(isFieldBoss, trMobParent, m_objPlayer.transform.position.y,mainCanvas);
                    }
                    else 
                    {
                    
                    }
                }
                else
                {
                    Debug.LogError("m_csGenerator�� �����ϴ�.");
                }
            }
            else
            {
                Debug.LogError("MoveMap���� ���� trMobParent�� �Ѱܹ��� ���մϴ�.");
            }
        }
        else
        {
            Debug.LogError("m_objGrounds Ȥ�� m_objPlayer�� null���� Ȯ�� �ϼ���");
        }
        Debug.Log(string.Format("�������� : {0}, ��ȣ : {1}", m_nStage, m_nCurrentMobNo));
        m_isMove = true;
    }


    //***Player ���� �޼ҵ�***//

    //�÷��̾ ���������� ġ��Ÿ Ȯ���� ���� bool���·� ��ȯ
    bool IsCritical()
    {
        
        float fCriticalProb = m_fCriticalProb * 100;
        //1~100 ������ ���� ���� m_fCriticalProb�� 100�� ���� �� ���� ������ ġ��Ÿ ����, Ŭ �� ����
        //���� ġ��Ÿ Ȯ���� 0�̶��, ������ false�̴�.
        if (Random.Range(1,101) <= fCriticalProb)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //Player�� raycast���� �ʵ���� ����� �����غ�
    public void SetFieldBattle(GameObject objEnemy)
    {
        m_isMove = false; // ���̻� �������� �ʵ��� ����
        m_isPlayerAttack = true; //�÷��̾ �����ϵ��� ����
        m_isFieldBattle = true;
        m_objCurrentMob = objEnemy;
    }
    // �ʵ� ���Ϳ��� ���� (��ǻ� Field ���ʹ� ������ ���ϹǷ� �÷��̾ Attack�ϴ� ��)
    public void FieldBattle()
    {
        if(m_objCurrentMob != null)
        {
            //���� �÷��̾�� ��ġ���� Field Monster�� ����ִ��� Ȯ��
            bool isMobAlive = m_objCurrentMob.GetComponent<FieldMobControl>().GetIsAlive();

            if (isMobAlive && m_isFieldBattle)
            {
                double fAttackDamage = 0;
                //ġ��Ÿ ���� Ȯ��
                bool isCritical = IsCritical();
                if (isCritical)
                {
                    //ġ��Ÿ�� �ɷ��� �� m_dAttackDamage�� ġ��Ÿ ���(m_fCriticalRatio)�� �����ش�.
                    fAttackDamage = m_dAttackDamage * m_fCriticalRatio;
                }
                else
                {
                    fAttackDamage = m_dAttackDamage;
                }

                //��ġ���� ���Ϳ��� �÷��̾��� ���ݷ°� ������ �Ѱ��� �������� �ش�.
                m_objCurrentMob.GetComponent<FieldMobControl>().SetDamege(fAttackDamage, m_fAttackSpeed);
                
                //DamageText������(Tweening) ����
                if(m_csGenerator != null)
                {
                    Vector2 vecMobHead = Vector2.zero;
                    if(m_objCurrentMob != null && m_objFieldUI)
                    {
                        vecMobHead = m_objCurrentMob.GetComponent<FieldMobControl>().GetHeadPos();
                        Canvas canvas = null;
                        
                        //DamageText�������� �θ�� �� main canvas�� �ʿ��ϴ�. �ٸ� ���� ������ Find�Լ��� �����ϱ�����
                        //DamageText�� GameScene���� �����ǹǷ� m_objFieldUI(cavas�ڽ� panel)�� �����ϹǷ� GetComponetInParent�� cavas�� �����´�.
                        if(m_objFieldUI != null)
                        {
                            canvas = m_objFieldUI.GetComponentInParent<Canvas>();
                            //Generator.cs �� GenerateDamageText()�� ���� Tweening�ϴ� DamageText ������ ����
                            m_csGenerator.GenerateDamageText(canvas,vecMobHead, fAttackDamage, m_fAttackSpeed, isCritical);
                        }
                        else
                        {
                            Debug.LogError("m_objFieldUI�� �����ϴ�.");
                        }

                    }
                    else
                    {
                        Debug.LogError("m_objCurrentMob�� �����ϴ�.");
                    }
                }
                else
                {

                }

            }
            else
            {
                //Field���� �׾�����
                m_isPlayerAttack = false; //�÷��̾��� ������ �����.
                m_isFieldBattle=false; //��������
                //���� FieldMobControl.cs���� GameManager�� FieldMobDie()�Լ��� ȣ���ϸ�
                //GameManager�� SetNextField()�� ȣ���Ͽ� ������ �Ѵ�.
                
            }
        }
        else
        {
            Debug.LogError("GameManager�� m_objCurrentMob�� �����ϴ�.");
        }
    }

    //***Field Monster���� �޼ҵ�***//
    
    //Field ���Ͱ� ����� Reward�� ���� Field������
    public void FieldMobDie()
    {
        //��� FieldBattle()���� ���� falseó�� ������ Ȥ�ø� ��Ȳ�� �ٽ� �ʱ�ȭ
        if(m_isFieldBattle && m_isPlayerAttack)
        {
            m_isFieldBattle = false;
            m_isPlayerAttack = false;
        }

        //Coin ����
        int nCoin = 0;// ������ ���� ��
        //�ʵ庸������ �ƴ����� ���� �ٸ��� �����Ѵ�.��
        if(m_nCurrentMobNo == m_nMaxFieldMob)
        {
            nCoin = Random.Range(9, 13);
        }
        else
        {
            nCoin = Random.Range(4, 7);
        }

        //FieldUI�� coinUI RectTransform�� �ʿ��ϹǷ� �˻�
        if (m_objFieldUI != null)
        {
            RectTransform recTarget = m_objFieldUI.GetComponent<FieldUI>().GetCoinUIRect();
            if (m_objCurrentMob != null) 
            {
                //���� ���� FieldMob�� ��ġ�� nCoin��ŭ ������ �����Ͽ� recTarget���� �̵���Ų��.
                m_csGenerator.GenerateCoin(m_objCurrentMob.transform.position,recTarget,nCoin);
            }
            else
            {
                Debug.LogError("m_objCurrentMob�� null�� �Ǿ����ϴ�. Ȯ���ϼ���");
            }
        }
        else
        {
            Debug.LogError("m_objFieldUI �� �����ϴ�.");
        }

        //nCoin��ŭ m_nCrrentMoney�� ���� �����ش�.
        MoneyReward(nCoin);
        m_objCurrentMob = null; //�ʱ�ȭ



        //���� �ʵ彺������ ������
        // ���⼭ �߿��� ���� ���� Coin Generate�� EarnMoney���� m_nCurrentMobNo �Ǵ� m_nStage�� �ʿ��ϹǷ�
        //SetFieldStage�� �������� ȣ���Ͽ� �ʿ��� �������� m_nCurrentMobNo �Ǵ� m_nStage�� ����������� �Ѵ�.
        SetFieldStage(true); 
    }

    //Monster Hp ����(FieldMob)
    public float SetEnemyHp(GameObject objEnemy)
    {
        if (objEnemy.CompareTag("FieldMob"))
        {
            if(m_nCurrentMobNo == m_nMaxFieldMob)  //���� Stage������ ���� �� �� field boss�̹Ƿ�Hp�� ���� �� ����.
            {
                if(m_nStage == 1) //stage�� 1�� ������ �������� ������ �ʴ´�.
                {
                    return m_fBasicFieldBossHp;
                }
                else //stage�� 2�̻��� ��
                {
                    //���� stage-1 �� �������� ���� ���� �⺻ ü�¿� ���Ͽ� ü���� �ø���.
                    // -> stage�� ������ ���� ������ hp�� �ڵ����� �����Ѵ�.
                    float fIncrease = (m_nStage - 1) * m_fFieldMobHpInc;
                    float fHp = m_fBasicFieldBossHp * fIncrease;
                    return fHp;
                }
            }
            else //fieldBoss�� �ƴ� �� 
            {
                //�⺻ field mobü�¿� 100% ~ 120%(�̸�)�� �������� ���Ͽ� ����
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
            Debug.LogError(objEnemy.name.ToString() + "�� enemy�±װ� �ƴմϴ�.");
            return 0;
        }
    }
}
