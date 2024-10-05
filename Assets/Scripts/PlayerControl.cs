using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{


    //**애니메이션 관련**//
    Animator m_PlayerAnimator = null;
    
    // Start is called before the first frame update
    void Start()
    {
        m_PlayerAnimator = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        SetAnimation();
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
