using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldMobControl : MonoBehaviour
{
    /*
     * Field Monster(�⺻,fieldBoss) ���� ��ũ��Ʈ
     */
    Animator m_mobAnimator = null;

    float m_fMaxHp = 0f;
    float m_fCurrentHp = 0f;

    bool m_isAlive = true; //false�� �� GameManager���� GetDamage�Լ��� ȣ������ �ʴ´�.

    bool m_isCorDie = false; //CorDie() �ڷ�ƾ �ߺ�����

    [SerializeField]
    Transform m_trHead;
    [SerializeField]
    Transform m_trHpBarPos;

    //Generator���� �Ѱܹ��� HpBar Panel
    GameObject m_objHpBar = null;


    private void Awake()
    {
        m_isAlive=true;
        m_fMaxHp = GameManager.Instance.SetEnemyHp(this.gameObject); //ü�� ����
        Debug.Log("���� ü�� : " + m_fMaxHp.ToString("F2"));
        m_fCurrentHp = m_fMaxHp;
        if (m_mobAnimator == null)
        {
            m_mobAnimator = GetComponent<Animator>();
        }
    }

    private void Update()
    {
        if (m_objHpBar != null) 
        {
            //�Ѱܹ��� EnemyHpBar�������� m_trHpBarPos.position�� ����� �� �ֵ���
            m_objHpBar.transform.position = Camera.main.WorldToScreenPoint(m_trHpBarPos.position);
        }
        else
        {
            Debug.LogError("m_objHpBar�� �����ϴ�.");
        }
    }

    //DamageText�� ������ �Ӹ� ���� Position�� �������� �Լ�
    public Vector2 GetHeadPos()
    {
        if (m_trHead != null)
        {
            return m_trHead.position;
        }
        else
        {
            Debug.LogError(gameObject.name + "�� m_trHead�� �����ϴ�.");
            return Vector2.zero;
        }
    }

    //Generator���� �Ѱܹ��� HpBar�� ����
    public void InitHpBar(GameObject HpBar)
    {
        m_objHpBar = HpBar;
        if (m_trHpBarPos != null) 
        {
            m_objHpBar.transform.position = Camera.main.WorldToScreenPoint(m_trHpBarPos.position);
        }
        else
        {
            Debug.LogError("m_trHpPos�� �����ϴ�.");
        }
        
    }


    public bool GetIsAlive()
    {
        return m_isAlive;
    }

    public void SetDamege(float fDamage, float fAnimeSpeed)
    {
        m_fCurrentHp -= fDamage;
        Mathf.Clamp(m_fCurrentHp, 0f, m_fMaxHp); //m_fCurrentHp�� 0 ������ �������� �ʰ� ����

        //HpBar ����
        if (m_objHpBar != null)
        {
            float fHpRatio = m_fCurrentHp / m_fMaxHp;
            //���� ���� Hp������ �Ѱ��ش�.
            m_objHpBar.GetComponent<EnemyHpBar>().SetHpBar(fHpRatio);
        }

        if(m_mobAnimator != null && m_isAlive)
        {
            // take_a_Hit �ִϸ��̼��� speed�� ���� -> GameManager���� Player�� ���ݼӵ��� ����
            // �ش��Լ��� Player�� attack�ִϸ��̼� �̺�Ʈ���� GameManager -> �ش�޼ҵ�� ����Ǳ⿡ �ӵ��� ���߱� ����
            m_mobAnimator.SetFloat("fHitSpeed", fAnimeSpeed);
            m_mobAnimator.Play("take_a_hit", -1, 0f);
            //take_a_hit(���ݹ޴¸��)�� ����Ѵ�.
            //�ι�° �Ű����ڴ� Play�Լ� ���Ǹ� ���� 
            //The layer index. If layer is -1, it plays the first state with the given state name or hash.
            //�� ���̾� �ε��� -1�� ������ �̹� ������� �ִϸ��̼ǵ� ó������ �ٽ� ����� ������ ����.
            //���� ° �Ű����ڴ� normalizedTime: The time offset between zero and one.
            //���Ը��� �ִϸ��̼� offset 0(����) ~ 1(��) 0.5�� ������ �߰����� ����
            if(m_fCurrentHp <= 0)
            {
                m_isAlive = false;
                if(m_isCorDie == false)
                {
                    StartCoroutine(CorDie());
                }
            }
        }
        else
        {
            Debug.LogError("m_MobAnimator�� �����ϴ�.");
        }
    }

    IEnumerator CorDie()
    {
        if(m_isCorDie == false)
        {
            m_isCorDie = true;
        }

        yield return new WaitForEndOfFrame();
        //���� �ִϸ����� 0���̾�(Base Layer)�� �� �ִϸ��̼��� take_a_hit���� Ȯ��
        //���ݹ޴� ����� ������ death Ŭ���� ����ϱ� ����
        while (m_mobAnimator.GetCurrentAnimatorStateInfo(0).IsName("take_a_hit"))
        {
            //ture�� �� ���� take_a_hit�� ������̹Ƿ� ��� ��ٸ���.
            yield return new WaitForEndOfFrame();
        }
        // take_a_hit �� ����Ϸ�� �� death Ŭ�� ���
        if(m_mobAnimator != null)
        {
            m_mobAnimator.Play("death");
            AudioManager.Instance.FieldMobDieSound(GetComponent<AudioSource>()); //����� ���
            //death �ִϸ��̼� Ŭ���� ����Ͽ�����, ���̾� 0�� GetCurrentAnimatorState��
            //death �ִϸ��̼��� ������̴�. ���� normalizedTime(0:����~1:����)�� 1 ���� ���� ������
            //���� �ش� Ŭ���� ������̹Ƿ� ��� yeild return �� �ش�. 
            //���� ������ ����� �� ó�� IsName()���� ����ص� �����۵��Ѵ�.(Ư�� �ð��� �߿��Ѱ� �ƴϸ� ��� �� Ȯ���� ����ε� �ϴ�.)
            while (m_mobAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1) 
            {
                //Debug.Log(m_MobAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime.ToString());
                yield return new WaitForEndOfFrame();
            }
            //Debug.Log("������ ���");

        }
        else
        {
            Debug.LogError("m_MobAnimator�� �����ϴ�.");
            yield break;
        }
        //��� ������ ������ ��
        yield return new WaitForSeconds(0.3f);
        m_isCorDie = false;
        GameManager.Instance.FieldMobDie(); //GameManager�� ����� �˸���
        Destroy(m_objHpBar);
        Destroy(gameObject);
        //Debug.Log("����");
        yield break;
    }


}
