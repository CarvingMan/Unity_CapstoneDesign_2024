using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //�̱��� ������ ���� ���
    //���� �ν��Ͻ� ����
    // ������ ���� set ������Ƽ�� private�� �����Ͽ� 
    // �ٸ� Ŭ�������� get�� �����ϳ� �ν��Ͻ� �ʱ�ȭ�� ���� GameManager������ �����ϴ�.
    public static GameManager Instance { get; private set; }


    //***�������� ����***//
    int m_nStage = 1; //���� Stage
    const int m_nMaxFieldMob = 8; //Stage �� ���� ��
    int m_nCurrentMobNo = 1; //���� ��ġ���� field mob ��ȣ -> 8������ ���� �� Stage++;
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
    bool m_isBattle = false; //�������� �� true
    public bool IsBattle { get { return m_isBattle; } }
    bool m_isPlayerAttack = false; //true�� �÷��̾ attack �ִϸ��̼��� play�ϰ� animation Ŭ�� �̺�Ʈ���� ���ݰ��� �Լ� ȣ��
    public bool IsPlayerAttack { get { return m_isPlayerAttack; } } 
    GameObject m_objCurrentMob = null; //���� player�� ��ġ���� field monster
    GameObject CurrentMob { set { m_objCurrentMob = value; } } //Player Control���� raycast�� ��� ���� ���͸� �־��ش�.


    //**�ʵ� ���� ����**//
    bool m_isFieldMob = false; //���� �ʵ���Ͱ� ���� �� true
    //public bool IsFieldMob { get { return m_isFieldMob; } }
    GameObject m_objCurrentFieldMob = null;
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

    private void Awake()
    {
        if(Instance != null)
        {
            //���Ӱ� GameManager�� �����Ǿ��� �� ���� instance�� �����Ѵٸ� 
            //GameManager�� �ߺ��̱⿡ ����
            Destroy(gameObject);
        }
        else
        {
            //���� ���� instance�� null�̶��, �ش� ��ü�� �Ѱ��ְ� DontDestroy
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            m_isMove = true;
        }
        else
        {
            m_isMove = false;
        }
    }


    //GameManager���� �ʿ��� ������Ʈ���� �Ѱ��ִ� �Լ� -> ������ Start()���� �Ѱ��ش�.
    public void TakeObject(GameObject obj)
    {
        if (obj.CompareTag("Player"))
        {
            m_objPlayer = obj;
        }
        else
        {
            Debug.LogError(obj.name.ToString()+"�� GameManager���� �ʿ�� ���� �ʽ��ϴ�.");
        }
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
