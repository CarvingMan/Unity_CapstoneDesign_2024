using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldMobControl : MonoBehaviour
{
    /*
     * Field Monster(기본,fieldBoss) 제어 스크립트
     */
    Animator m_mobAnimator = null;

    float m_fMaxHp = 0f;
    float m_fCurrentHp = 0f;

    bool m_isAlive = true; //false일 시 GameManager에서 GetDamage함수를 호출하지 않는다.

    bool m_isCorDie = false; //CorDie() 코루틴 중복방지

    [SerializeField]
    Transform m_trHead;
    [SerializeField]
    Transform m_trHpBarPos;

    //Generator에서 넘겨받을 HpBar Panel
    GameObject m_objHpBar = null;


    private void Awake()
    {
        m_isAlive=true;
        m_fMaxHp = GameManager.Instance.SetEnemyHp(this.gameObject); //체력 설정
        Debug.Log("몬스터 체력 : " + m_fMaxHp.ToString("F2"));
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
            //넘겨받은 EnemyHpBar프리팹이 m_trHpBarPos.position을 따라올 수 있도록
            m_objHpBar.transform.position = Camera.main.WorldToScreenPoint(m_trHpBarPos.position);
        }
        else
        {
            Debug.LogError("m_objHpBar가 없습니다.");
        }
    }

    //DamageText가 생성될 머리 위의 Position을 가져오는 함수
    public Vector2 GetHeadPos()
    {
        if (m_trHead != null)
        {
            return m_trHead.position;
        }
        else
        {
            Debug.LogError(gameObject.name + "의 m_trHead가 없습니다.");
            return Vector2.zero;
        }
    }

    //Generator에서 넘겨받은 HpBar를 세팅
    public void InitHpBar(GameObject HpBar)
    {
        m_objHpBar = HpBar;
        if (m_trHpBarPos != null) 
        {
            m_objHpBar.transform.position = Camera.main.WorldToScreenPoint(m_trHpBarPos.position);
        }
        else
        {
            Debug.LogError("m_trHpPos가 없습니다.");
        }
        
    }


    public bool GetIsAlive()
    {
        return m_isAlive;
    }

    public void SetDamege(float fDamage, float fAnimeSpeed)
    {
        m_fCurrentHp -= fDamage;
        Mathf.Clamp(m_fCurrentHp, 0f, m_fMaxHp); //m_fCurrentHp가 0 밑으로 떨어지지 않게 고정

        //HpBar 세팅
        if (m_objHpBar != null)
        {
            float fHpRatio = m_fCurrentHp / m_fMaxHp;
            //현재 남은 Hp비율을 넘겨준다.
            m_objHpBar.GetComponent<EnemyHpBar>().SetHpBar(fHpRatio);
        }

        if(m_mobAnimator != null && m_isAlive)
        {
            // take_a_Hit 애니매이션의 speed를 설정 -> GameManager에서 Player의 공격속도를 전달
            // 해당함수는 Player의 attack애니메이션 이벤트에서 GameManager -> 해당메소드로 연결되기에 속도를 맞추기 위함
            m_mobAnimator.SetFloat("fHitSpeed", fAnimeSpeed);
            m_mobAnimator.Play("take_a_hit", -1, 0f);
            //take_a_hit(공격받는모션)을 재생한다.
            //두번째 매개인자는 Play함수 정의를 보면 
            //The layer index. If layer is -1, it plays the first state with the given state name or hash.
            //즉 레이어 인덱스 -1로 설정시 이미 재생중인 애니메이션도 처음부터 다시 재생이 가능해 진다.
            //세번 째 매개인자는 normalizedTime: The time offset between zero and one.
            //쉽게말해 애니메이션 offset 0(시작) ~ 1(끝) 0.5로 설정시 중간부터 시작
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
            Debug.LogError("m_MobAnimator가 없습니다.");
        }
    }

    IEnumerator CorDie()
    {
        if(m_isCorDie == false)
        {
            m_isCorDie = true;
        }

        yield return new WaitForEndOfFrame();
        //현재 애니메이터 0레이어(Base Layer)의 의 애니메이션이 take_a_hit인지 확인
        //공격받는 모션이 끝나고 death 클립을 재생하기 위함
        while (m_mobAnimator.GetCurrentAnimatorStateInfo(0).IsName("take_a_hit"))
        {
            //ture일 시 아직 take_a_hit이 재생중이므로 잠시 기다린다.
            yield return new WaitForEndOfFrame();
        }
        // take_a_hit 이 재생완료된 후 death 클립 재생
        if(m_mobAnimator != null)
        {
            m_mobAnimator.Play("death");
            AudioManager.Instance.FieldMobDieSound(GetComponent<AudioSource>()); //오디오 재생
            //death 애니메이션 클립을 재생하였으니, 레이어 0의 GetCurrentAnimatorState는
            //death 애니메이션이 재생중이다. 따라서 normalizedTime(0:시작~1:종료)가 1 보다 작을 때에는
            //아직 해당 클립이 재생중이므로 잠시 yeild return 해 준다. 
            //물론 위에서 사용한 것 처럼 IsName()으로 사용해도 정상작동한다.(특정 시간이 중요한게 아니면 사실 더 확실한 방법인듯 하다.)
            while (m_mobAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1) 
            {
                //Debug.Log(m_MobAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime.ToString());
                yield return new WaitForEndOfFrame();
            }
            //Debug.Log("완전히 사망");

        }
        else
        {
            Debug.LogError("m_MobAnimator가 없습니다.");
            yield break;
        }
        //모든 로직이 끝났을 시
        yield return new WaitForSeconds(0.3f);
        m_isCorDie = false;
        GameManager.Instance.FieldMobDie(); //GameManager에 사망을 알리고
        Destroy(m_objHpBar);
        Destroy(gameObject);
        //Debug.Log("삭제");
        yield break;
    }


}
