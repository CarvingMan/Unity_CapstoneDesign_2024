using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
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



    // Start is called before the first frame update
    void Start()
    {
        m_fMaxHp = GameManager.Instance.SetEnemyHp(this.gameObject); //ü�� ����
        m_fCurrentHp = m_fMaxHp;
        if (m_mobAnimator == null)
        {
            m_mobAnimator = GetComponent<Animator>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool GetIsAlive()
    {
        return m_isAlive;
    }

    public void SetDamege(float fDamage, float fAnimeSpeed)
    {
        m_fCurrentHp -= fDamage;
        Mathf.Clamp(m_fCurrentHp, 0f, m_fMaxHp); //m_fCurrentHp�� 0 ������ �������� �ʰ� ����
        
        if(m_mobAnimator != null)
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
        m_isCorDie = false;
        GameManager.Instance.FieldMobDie(); //GameManager�� ����� �˸���
        Destroy(gameObject);
        //Debug.Log("����");
        yield break;
    }
}
