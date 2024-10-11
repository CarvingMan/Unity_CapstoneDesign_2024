using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldMobControl : MonoBehaviour
{
    /*
     * Field Monster(기본,fieldBoss) 제어 스크립트
     */
    Animator MobAnimator = null;

    float m_fMaxHp = 0f;
    float m_fCurrentHp = 0f;

    private void Awake()
    {
        m_fMaxHp = GameManager.Instance.SetEnemyHp(this.gameObject); //체력 설정
        m_fCurrentHp = m_fMaxHp;
    }

    // Start is called before the first frame update
    void Start()
    {
        if(MobAnimator == null)
        {
            MobAnimator = GetComponent<Animator>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetDamege(float fDamage, float fAnimeSpeed)
    {
        m_fCurrentHp -= fDamage;
        Mathf.Clamp(m_fCurrentHp, 0f, m_fMaxHp); //m_fCurrentHp가 0 밑으로 떨어지지 않게 고정

    }
}
