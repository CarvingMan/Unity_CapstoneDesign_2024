using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    /*
     * ���׸� Ŭ���� SingletonŬ������ ��ӹ޾� �̱��� �ν��Ͻ��� ���
     * ����� ���� �Լ� ����
     */

    //Player ���� Ŭ��
    [SerializeField]
    AudioClip m_adioPlayerAttack = null;

    //Field Mob ���� Ŭ��
    [SerializeField]
    AudioClip m_adioFieldMobDie = null;

    //coin Get,Spend ���� Ŭ�� 
    [SerializeField]
    AudioClip m_adioCoinGet = null;
    [SerializeField]
    AudioClip m_adioCoinSpend = null;

    // Start is called before the first frame update
    void Start()
    {
        if (m_adioPlayerAttack == null)
        {
            Debug.LogError("m_adioPlayerAttack ����� Ŭ���� �����ϴ�.");
        }   

        if(m_adioFieldMobDie == null)
        {
            Debug.LogError("m_adioFieldMobDie ����� Ŭ���� �����ϴ�.");
        }

        if (m_adioCoinGet == null) 
        {
            Debug.LogError("m_adioCoinGet ����� Ŭ���� �����ϴ�.");
        }

        if (m_adioCoinSpend == null) 
        {
            Debug.LogError("m_adioCoinSpend ����� Ŭ���� �����ϴ�.");
        }
    }

    //PlayerControl.cs���� player_attack�ִϸ��̼� Ŭ���� Ư�� �κп� Attack()�Լ��� �̺�Ʈ�� ȣ��Ǵµ�
    //�׶� ���� �ش� ���� Ŭ�� ���
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

    //FieldMobContorl.cs���� �ʵ� ���Ͱ� ����� �� ȣ���Ͽ� ���
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

    //Coin Get Spend ����-> FieldUI.cs���� ���
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
