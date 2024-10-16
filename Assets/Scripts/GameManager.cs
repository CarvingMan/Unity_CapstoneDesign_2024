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
    const int m_nMaxFieldMob = 7; //Stage �� ���� ��
    int m_nCurrentMobNo = 0; //���� ��ġ���� field mob ��ȣ -> 8������ ���� �� Stage++;
    public int CurrentMonNo { get { return m_nCurrentMobNo; } }


    //*** �ɷ�ġ ���� ***//
    //float m_fMoveLv = 0;
    //float m_fAttackSpeedLv = 0;
    //float m_fAttackDamegeLv = 0;
    

    //*** �ɷ�ġ �� ***//
    private float m_fMoveStatus = 1; //�̵��ӵ� �ɷ�ġ 1�̸� 100%, �������� ����
    private float m_fAttackSpeed = 1; //���ݼӵ� ���� ����
    private float m_fAttackDamage = 10; //�������� 10% �� ���� 


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
        else
        {
            Debug.LogError(obj.name.ToString()+"�� GameManager���� �ʿ�� ���� �ʽ��ϴ�.");
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
            }
            else
            {
                m_nCurrentMobNo++; //Mob��ȣ ����
            }
        }

        //FieldMob ����
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
                //Generate.cs�� ���� Field Mob ����
                m_csGenerator.GenerateFieldMob(isFieldBoss, trMobParent, m_objPlayer.transform.position.y);
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
                //��ġ���� ���Ϳ��� �÷��̾��� ���ݷ°� ������ �Ѱ��� �������� �ش�.
                m_objCurrentMob.GetComponent<FieldMobControl>().SetDamege(m_fAttackDamage, m_fAttackSpeed);
            }
            else
            {
                //Field���� �׾�����
                m_isPlayerAttack = false; //�÷��̾��� ������ �����.
                m_objCurrentMob = null;
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
        //���� �ʵ彺������ ������
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
                    // -> stage�� ������ ���� ������ hp�� �����Ѵ�.
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
