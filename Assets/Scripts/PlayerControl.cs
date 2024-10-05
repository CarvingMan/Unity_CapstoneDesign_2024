using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{


    //**�ִϸ��̼� ����**//
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

    //�÷��̾��� �ִϸ��̼� ����
    void SetAnimation()
    {
        //Singleton GameManager�ν��Ͻ��� IsMove�� true�� �� �ִϸ��̼� ���
        if (GameManager.Instance.IsMove)
        {
            //player_run �ִϸ��̼��� ����ϱ� ����
            //�ִϸ��̼� ����ӵ� �Ķ���ͷ� ������ ���� fMoveSpeed�� GameManager�� m_fMoveStatus�� �°� ���� 
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
