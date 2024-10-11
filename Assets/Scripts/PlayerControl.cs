using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    //**애니메이션 관련**//
    Animator m_PlayerAnimator = null;


    //**공격관련**//
    GameObject m_objCurrentMob = null; //현재 대치중인 몬스터

   

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.TakeObject(this.gameObject); //GameManager에 자신을 넘겨준다.
        m_PlayerAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        SetAnimation();
    }

    //Raycast로 앞에 적이 있을 시 GameManager에 알려주고 전투 준비
    void CheckEnemy()
    {
        RaycastHit2D hitEnemy;

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
    }
}
