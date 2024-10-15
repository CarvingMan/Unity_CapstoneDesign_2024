using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    //**애니메이션 관련**//
    Animator m_PlayerAnimator = null;

   

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.TakeObject(this.gameObject); //GameManager에 자신을 넘겨준다.
        m_PlayerAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckEnemy();
        SetAnimation();
    }

    //Raycast로 앞에 적이 있을 시 GameManager에 알려주고 전투 준비
    void CheckEnemy()
    {
        //움직이고 있을 시에 오른쪽으로raycast를 쏘아 field몬스터와 마주칠시 전투준비
        if (GameManager.Instance.IsMove)
        {
            RaycastHit2D hitEnemy;
            //Monster 레이어만 검사할 수 있도록 레이어마스크 설정
            int nLayerMask = 1 << LayerMask.NameToLayer("Monster");
            float fDistance = 1;
            //Player의 위치에서 오른쪽으로 1만큼 Monster레이어를 가진 콜라이더만 검사
            hitEnemy = Physics2D.Raycast(transform.position, Vector2.right, fDistance, nLayerMask);
            //Debug.DrawRay(transform.position, Vector2.right *1, Color.red);
            if (hitEnemy.collider != null)
            {
                //만약 충돌이 검출되었을 때 태그가 FieldMob이라면
                if (hitEnemy.collider.CompareTag("FieldMob"))
                {
                    //전투준비 이후 GameManager에서 m_isPlayerAttack이 true가 되어 SetAnimation에서 attack 
                    GameManager.Instance.SetFieldBattle(hitEnemy.collider.gameObject);
                }
            }
        }

    }

    //플레이어의 애니메이션 설정
    void SetAnimation()
    {
        //Singleton GameManager인스턴스의 IsMove가 true일 때 애니메이션 재생
        if (GameManager.Instance.IsMove)
        {
            //player_run 애니메이션을 재생하기 전에
            //애니메이션 재생속도 파라미터로 지정해 놓은 fMoveSpeed를 GameManager의 m_fMoveStatus에 맞게 설정 
            if(m_PlayerAnimator.GetFloat("fMoveSpeed")!= GameManager.Instance.MoveStatus)
            {
                m_PlayerAnimator.SetFloat("fMoveSpeed", GameManager.Instance.MoveStatus);
            }
         
            m_PlayerAnimator.SetBool("isRun", true);
        }
        else
        {
            m_PlayerAnimator.SetBool("isRun", false);
        }

        //GameManager에서 
        if (GameManager.Instance.IsPlayerAttack)
        {
            //player_attack애니메이션 속도를 GameManager의 m_fAttackSpeed에 맞게 파라미터 설정
            m_PlayerAnimator.SetFloat("fAttackSpeed",GameManager.Instance.AttackSpeed);
            //player_attack 애니메이션 재생시 클립에서 칼질하는 순간
            //애니메이션 이벤트로 플레이어의 Attack()함수가 호출된다.
            m_PlayerAnimator.Play("player_attack");
        }
    }


    //Player의 player_attack이 재생될때 칼질하는 순간 에니메이션 이벤트로 호출 
    void Attack()
    {
        //현재 전투가 FieldBattle인 경우
        if (GameManager.Instance.IsFieldBattle)
        {
            GameManager.Instance.FieldBattle();
        }
    }
    
}
