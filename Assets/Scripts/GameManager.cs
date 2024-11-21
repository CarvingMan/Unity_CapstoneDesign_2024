using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    //�̱��� ������ ���� ���
    //���ʸ� Ŭ���� Singleton�� ��ӹ޾� GameManager �̱��� �ν��Ͻ� ����

    //***��������(Field) ����***//
    bool m_isInField = false; //���� Field�� �ִ���(�÷��̾ Field Stage�� �ִ���)
    int m_nStage = 1; //���� Stage
    public int CurrentStage {  get { return m_nStage; } }
    const int m_nMaxFieldMob = 7; //Stage �� ���� ��
    int m_nCurrentMobNo = 0; //���� ��ġ���� field mob ��ȣ -> 8������ ���� �� Stage++;
    public int CurrentMonNo { get { return m_nCurrentMobNo; } }


    //***�� ����***//
    long m_nCurrentMoney = 0;
    public long CurrentMoney { get { return m_nCurrentMoney; } }
    float m_fGetCoinTime = 0.8f; //������ �����ǰ� CoinUI���� ���� �ð�
    public float GetCoinTime { get { return m_fGetCoinTime; } }
    const int m_nCoinPrice = 10; //���� �ϳ��� �� -> m_fMoneyInc�� ���Ͽ� ����
    const float m_fMoneyInc = 1.8f; //Stage���� Coin�� �� ������

    //*** �ɷ�ġ ���� ***//
    //float m_fMoveLv = 0;
    //float m_fAttackSpeedLv = 0;
    //float m_fAttackDamegeLv = 0;
    

    //*** �ɷ�ġ �� ***//
    private float m_fMoveStatus = 1; //�̵��ӵ� �ɷ�ġ 1�̸� 100%, �������� ����
    private float m_fAttackSpeed = 1; //���ݼӵ� ���� ����
    private float m_fAttackDamage = 10; //�������� ���� �������� 5% �� ���� 
    private float m_fCriticalProb = 0; //ġ��Ÿ Ȯ�� ������ �� 0.01�� ���� �ִ�1
    private float m_fCriticalRatio = 1; //ġ��Ÿ ������ ��� -> ġ��Ÿ�� ���� �� m_fAttackDamage�� ���Ͽ� ��� ������ �� 0.01f����

    //**�÷��̾� ���� ����**//
    bool m_isFieldBattle = false; //�������� �� true
    public bool IsFieldBattle { get { return m_isFieldBattle; } }
    bool m_isPlayerAttack = false; //true�� �÷��̾ attack �ִϸ��̼��� play�ϰ� animation Ŭ�� �̺�Ʈ���� ���ݰ��� �Լ� ȣ��
    public bool IsPlayerAttack { get { return m_isPlayerAttack; } } 
    public float AttackSpeed { get { return m_fAttackSpeed; } }
    GameObject m_objCurrentMob = null; //���� player�� ��ġ���� monster
    public GameObject CurrentMob { get { return m_objCurrentMob; } }


    //**�ʵ� ���� ����**//
    const float m_fBasicFieldMobHp = 50; //�⺻ �ʵ�� hp -> ���⼭ �������� 0~20% �����Ͽ� ���
    const float m_fBasicFieldBossHp = 100; //�⺻ �ʵ庸�� hp
    const float m_fFieldMobHpInc = 1.2f; //Hp ������ �������� ���� �� ������ BasicHp�� ���Ͽ� ����


    //�̵����� ���� ��� ���� MoveBackGround,PlayerControl,MonsterControl��� ���
    private bool m_isMove = false;
    public bool IsMove { get { return m_isMove; }} //m_isMove ���ٿ� read only�� get�� ����

    public float MoveStatus //m_fMoveStatus ���ٿ� ������Ƽ
    { get { return m_fMoveStatus; } }



    //***GameManager���� �Ѱܹ��� �ν��Ͻ���***//
    //������ Start()���� �Ѱ��ش�.
    GameObject m_objPlayer = null;
    GameObject m_objGrounds = null;
    GameObject m_objFieldUI = null;

    //*** ���� ��ü����***//
    Generator m_csGenerator = new Generator();


    //***Scene***//����
    Scene m_currentScene; //���� ��


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

        m_isInField = false;

        m_currentScene = SceneManager.GetActiveScene();
    }
 

    // Update is called once per frame
    void Update()
    {
        //���� GameScene�� ó�� ������ FieldStage�� ���� �� 
        //Field ���͸� ���� �� �־�� �Ѵ�.
        if(m_currentScene.name == "GameScene" && m_isInField == false)
        {
            m_isInField = true; //���߿� ���� ������ false�� ����
            SetFieldStage(false); //ó�� Ȥ�� �ٽ� ���� ������ �� �̹Ƿ� �Ű������� false�� �Ͽ� m_nCurrentMobNo�� �״�� ����
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
    void EarnMoney(int nCoin)
    {
        if(m_nStage <= 1)
        {
            //���������� 1�� ��쿡�� CoinPrice�� Coin ����ŭ ���Ͽ� ���� ���� �����ش�.
            m_nCurrentMoney += nCoin * m_nCoinPrice;
        }
        else
        {
            //���������� 2 �̻��� ��� ���� ��������-1���� m_fMoneyInc�� ���� ���� m_nCoinPrice�� �����ش�.
            //FieldMob ü�������� ���� ���, Stage�� ���� ���� �ڵ����� ���� Ŀ����.
            float fPrice = ((m_nStage - 1) *m_fMoneyInc) * m_nCoinPrice;
            //m_nCurrentMoney �� long type�̹Ƿ� fPrice�� �ݿø���Int�� �ٲپ� nCoin �� ��ŭ �����ش�.
            m_nCurrentMoney += nCoin * Mathf.RoundToInt(fPrice);
        }

        if (m_objFieldUI != null) 
        {
            //FieldUI�� moneyText�� ���� ������ �������ش�.(Tween ��ȯ)
            m_objFieldUI.GetComponent<FieldUI>().SetMoneyText(m_fGetCoinTime, m_nCurrentMoney);   
        }
        else
        {
            Debug.LogError("m_objFieldUI�� �����ϴ�.");
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
                float fAttackDamage = 0;
                //ġ��Ÿ ���� Ȯ��
                bool isCritical = IsCritical();
                if (isCritical)
                {
                    //ġ��Ÿ�� �ɷ��� �� m_fAttackDamage�� ġ��Ÿ ���(m_fCriticalRatio)�� �����ش�.
                    fAttackDamage = m_fAttackDamage * m_fCriticalRatio;
                }
                else
                {
                    fAttackDamage = m_fAttackDamage;
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
        EarnMoney(nCoin);
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
