using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldMobControl : MonoBehaviour
{
    /*
     * Field Monster(기본,fieldBoss) 제어 스크립트
     */
    Animator m_MobAnimator = null;

    float m_fMaxHp = 0f;
    float m_fCurrentHp = 0f;

    bool m_isAlive = true;



    // Start is called before the first frame update
    void Start()
    {
        m_fMaxHp = GameManager.Instance.SetEnemyHp(this.gameObject); //체력 설정
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
        Mathf.Clamp(m_fCurrentHp, 0f, m_fMaxHp); //m_fCurrentHp가 0 밑으로 떨어지지 않게 고정
        
        if(m_MobAnimator != null)
        {
            // take_a_Hit 애니매이션의 speed를 설정 -> GameManager에서 Player의 공격속도를 전달
            // 해당함수는 Player의 attack애니메이션 이벤트에서 GameManager -> 해당메소드로 연결되기에 속도를 맞추기 위함
            m_MobAnimator.SetFloat("fHitSpeed", fAnimeSpeed);
            m_MobAnimator.Play("take_a_hit", -1, 0f);
            //take_a_hit(공격받는모션)을 재생한다.
            //두번째 매개인자는 Play함수 정의를 보면 
            //The layer index. If layer is -1, it plays the first state with the given state name or hash.
            //즉 레이어 인덱스 -1로 설정시 이미 재생중인 애니메이션도 처음부터 다시 재생이 가능해 진다.
            //세번 째 매개인자는 normalizedTime: The time offset between zero and one.
            //쉽게말해 애니메이션 offset 0(시작) ~ 1(끝) 0.5로 설정시 중간부터 시작
            if(m_fCurrentHp <= 0)
            {
                m_isAlive = false;
            }
        }
        else
        {
            Debug.LogError("m_MobAnimator가 없습니다.");
        }



    }
}
