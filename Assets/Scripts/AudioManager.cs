using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    /*
     * 제네릭 클래스 Singleton클래스를 상속받아 싱글톤 인스턴스로 사용
     * 오디오 관련 함수 관리
     */

    //Player 관련 클립
    [SerializeField]
    AudioClip m_adioPlayerAttack = null;

    //Field Mob 관련 클립
    [SerializeField]
    AudioClip m_adioFieldMobDie = null;

    //coin Get,Spend 관련 클립 
    [SerializeField]
    AudioClip m_adioCoinGet = null;
    [SerializeField]
    AudioClip m_adioCoinSpend = null;

    // Start is called before the first frame update
    void Start()
    {
        if (m_adioPlayerAttack == null)
        {
            Debug.LogError("m_adioPlayerAttack 오디오 클립이 없습니다.");
        }   

        if(m_adioFieldMobDie == null)
        {
            Debug.LogError("m_adioFieldMobDie 오디오 클립이 없습니다.");
        }

        if (m_adioCoinGet == null) 
        {
            Debug.LogError("m_adioCoinGet 오디오 클립이 없습니다.");
        }

        if (m_adioCoinSpend == null) 
        {
            Debug.LogError("m_adioCoinSpend 오디오 클립이 없습니다.");
        }
    }

    //PlayerControl.cs에서 player_attack애니메이션 클립의 특정 부분에 Attack()함수가 이벤트로 호출되는데
    //그때 같이 해당 사운드 클립 재생
    public void PlayerAttackSound(AudioSource audioSource)
    {

        if (m_adioPlayerAttack != null && audioSource != null) 
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
                audioSource.clip = null;
            }
            audioSource.clip = m_adioPlayerAttack;
            audioSource.loop = false;
            audioSource.Play();
        }
    }

    //FieldMobContorl.cs에서 필드 몬스터가 사망할 시 호출하여 재생
    public void FieldMobDieSound(AudioSource audioSource)
    {
        if(m_adioFieldMobDie != null && audioSource != null)
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
                audioSource.clip = null;
            }
            audioSource.clip = m_adioFieldMobDie;
            audioSource.loop = false;
            audioSource.Play();
        }
    }

    //Coin Get Spend 관련-> FieldUI.cs에서 사용
    public void CoinGetSound(AudioSource audioSource) 
    {
        if(m_adioCoinGet != null && audioSource != null)
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
                audioSource.clip = null;
            }
            audioSource.clip= m_adioCoinGet;
            audioSource.loop= false;
            audioSource.Play();
        }
    }

    public void CoinSpendSound(AudioSource audioSource)
    {
        if(m_adioCoinSpend != null && audioSource != null)
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
                audioSource.clip = null;
            }
            audioSource.clip = m_adioCoinSpend;
            audioSource.loop = false;
            audioSource.Play();
        }
    }
}
