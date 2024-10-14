using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldMobControl : MonoBehaviour
{
    /*
     * Field Monster(�⺻,fieldBoss) ���� ��ũ��Ʈ
     */
    Animator m_MobAnimator = null;

    float m_fMaxHp = 0f;
    float m_fCurrentHp = 0f;

    bool m_isAlive = true;



    // Start is called before the first frame update
    void Start()
    {
        m_fMaxHp = GameManager.Instance.SetEnemyHp(this.gameObject); //ü�� ����
        m_fCurrentHp = m_fMaxHp;
        if (m_MobAnimator == null)
        {
            m_MobAnimator = GetComponent<Animator>();
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
        
        if(m_MobAnimator != null)
        {
            // take_a_Hit �ִϸ��̼��� speed�� ���� -> GameManager���� Player�� ���ݼӵ��� ����
            // �ش��Լ��� Player�� attack�ִϸ��̼� �̺�Ʈ���� GameManager -> �ش�޼ҵ�� ����Ǳ⿡ �ӵ��� ���߱� ����
            m_MobAnimator.SetFloat("fHitSpeed", fAnimeSpeed);
            m_MobAnimator.Play("take_a_hit", -1, 0f);
            //take_a_hit(���ݹ޴¸��)�� ����Ѵ�.
            //�ι�° �Ű����ڴ� Play�Լ� ���Ǹ� ���� 
            //The layer index. If layer is -1, it plays the first state with the given state name or hash.
            //�� ���̾� �ε��� -1�� ������ �̹� ������� �ִϸ��̼ǵ� ó������ �ٽ� ����� ������ ����.
            //���� ° �Ű����ڴ� normalizedTime: The time offset between zero and one.
            //���Ը��� �ִϸ��̼� offset 0(����) ~ 1(��) 0.5�� ������ �߰����� ����
            if(m_fCurrentHp <= 0)
            {
                m_isAlive = false;
            }
        }
        else
        {
            Debug.LogError("m_MobAnimator�� �����ϴ�.");
        }



    }
}
