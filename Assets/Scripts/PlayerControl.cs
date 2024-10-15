using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    //**�ִϸ��̼� ����**//
    Animator m_PlayerAnimator = null;

   

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.TakeObject(this.gameObject); //GameManager�� �ڽ��� �Ѱ��ش�.
        m_PlayerAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckEnemy();
        SetAnimation();
    }

    //Raycast�� �տ� ���� ���� �� GameManager�� �˷��ְ� ���� �غ�
    void CheckEnemy()
    {
        //�����̰� ���� �ÿ� ����������raycast�� ��� field���Ϳ� ����ĥ�� �����غ�
        if (GameManager.Instance.IsMove)
        {
            RaycastHit2D hitEnemy;
            //Monster ���̾ �˻��� �� �ֵ��� ���̾��ũ ����
            int nLayerMask = 1 << LayerMask.NameToLayer("Monster");
            float fDistance = 1;
            //Player�� ��ġ���� ���������� 1��ŭ Monster���̾ ���� �ݶ��̴��� �˻�
            hitEnemy = Physics2D.Raycast(transform.position, Vector2.right, fDistance, nLayerMask);
            //Debug.DrawRay(transform.position, Vector2.right *1, Color.red);
            if (hitEnemy.collider != null)
            {
                //���� �浹�� ����Ǿ��� �� �±װ� FieldMob�̶��
                if (hitEnemy.collider.CompareTag("FieldMob"))
                {
                    //�����غ� ���� GameManager���� m_isPlayerAttack�� true�� �Ǿ� SetAnimation���� attack 
                    GameManager.Instance.SetFieldBattle(hitEnemy.collider.gameObject);
                }
            }
        }

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

        //GameManager���� 
        if (GameManager.Instance.IsPlayerAttack)
        {
            //player_attack�ִϸ��̼� �ӵ��� GameManager�� m_fAttackSpeed�� �°� �Ķ���� ����
            m_PlayerAnimator.SetFloat("fAttackSpeed",GameManager.Instance.AttackSpeed);
            //player_attack �ִϸ��̼� ����� Ŭ������ Į���ϴ� ����
            //�ִϸ��̼� �̺�Ʈ�� �÷��̾��� Attack()�Լ��� ȣ��ȴ�.
            m_PlayerAnimator.Play("player_attack");
        }
    }


    //Player�� player_attack�� ����ɶ� Į���ϴ� ���� ���ϸ��̼� �̺�Ʈ�� ȣ�� 
    void Attack()
    {
        //���� ������ FieldBattle�� ���
        if (GameManager.Instance.IsFieldBattle)
        {
            GameManager.Instance.FieldBattle();
        }
    }
    
}
