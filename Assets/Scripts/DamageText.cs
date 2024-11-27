using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using System;

public class DamageText : MonoBehaviour
{
    /*
     * DOTween�� ����Ͽ� GamaManager.cs�� FieldBattle()���� Player�� �����Ҷ� FieldMob�� Head Position(UI��ǥ�� �ٲپ�)
     * �ش� �ؽ�Ʈ �������� Generater.cs���� �������ش�.
     * ������ SetDamageText()�� ȣ���Ͽ� Player�� ���ݷ� ��ŭ �ؽ�Ʈ �ִϸ��̼� ȿ���� �ش�.
     * 1. FieldMod�� �Ӹ� ��ġ ���� y������ 0.5f��ŭ �̵�(UI ��ǥ�� �ٲپ� �̵�)
     * 2. ���ÿ� a���� 1�� ����� �ش�.(������)
     * 3. ��Ʈ ũ�⸦ ���翡��(60) 100���� �ø���.
     */


    const float m_fTime = 0.9f; //player_attack Ŭ���� �⺻ ������ �ð��� 1���̹Ƿ� �ش� �� ���ٴ� �۰� �ϰ� ������ �����ش�.
    TextMeshProUGUI m_TMPDamage = null;

    //CoinTween�� ���������� Generator.cs���� �ش� �������� Instantiate()�ϰ� SetDamageText()�� �ٷ� ȣ���ϰԵȴ�.
    //Awake()�� �������� �ν���Ʈȭ �� ��� ȣ��Ǳ⿡ Start()�� �ƴ� Awake()���� ��������� �Ҵ��� �־�� �Ѵ�.
    private void Awake()
    {
        //���� 0���� ����
        m_TMPDamage = GetComponent<TextMeshProUGUI>();
        Color color = m_TMPDamage.color;
        color.a = 0;
        m_TMPDamage.color = color;
  
    }


    // DamageText Tweening
    public void SetDamageText(Vector2 vecFrom, float fDamage, float fAttackSpeed, bool isCritical)
    {
        //ġ��Ÿ�� �� red�������� ����
        if (isCritical)
        {
            Color color = Color.red;
            color.a = 0;
            m_TMPDamage.color = color;
        }

        //(����)vecFrom�� FieldMob�� Head, World Position�̱⿡ ��ũ����ǥ�� �����ؾ��Ѵ�.
        //(����)transform.position = Camera.main.WorldToScreenPoint(vecFrom);

        // �� �ּ� �ڵ忡�� �Ʒ��� ���� : CameraResolution.cs�� �Բ�
        // Canvas Render mode�� screen space- camera ���� Main camera�� �����Ͽ� ���� �� UI��ǥ�� ���� ��ǥ�� ����
        transform.position = vecFrom;

        //������ ��ǥ ����
        Vector2 vecTo = vecFrom;
        vecTo.y += 0.5f;

        //fDamage�� ���� Text���� (1000 = "1K", 1000000 = "1M", 1000000000 = "1B")
        m_TMPDamage.text = DamageToText(fDamage);
        
        //�⺻�ð����� AttackSpeed(���)�� ������. 
        float fTime = m_fTime / fAttackSpeed;
        float fFadeInTime = (fTime / 10) * 5; //fTime�� 50���� ���
        float fFadeOutTime = fTime - fFadeInTime;

        //������ 0���� 1�� fFadeInTime���� ��ȭ
        Tween fadeIn = DOTween.To(() => 0.0f,
            (a) =>
            {
                Color color = m_TMPDamage.color;
                color.a = a;
                m_TMPDamage.color = color;
            }, 1, fFadeInTime);

        //������ 0���� fFadeOutTime���� ��ȭ
        Tween fadeOut = DOTween.To(() => m_TMPDamage.color.a,
            (a) =>
            {
                Color color = m_TMPDamage.color;
                color.a = a;
                m_TMPDamage.color = color;
            }, 0, fFadeOutTime);

        //��Ʈ ����� 100���� 60���� ��ȭ
        Tween fontSize = DOTween.To(() => 100, (fontSize) => { m_TMPDamage.fontSize = fontSize; }, 60, fFadeOutTime);

        //(����)Tween move = transform.DOMove(Camera.main.WorldToScreenPoint(vecTo),fFadeOutTime);
        // �� �ּ� �ڵ忡�� �Ʒ��� ���� : CameraResolution.cs�� �Բ�
        // Canvas Render mode�� screen space- camera ���� Main camera�� �����Ͽ� ���� �� UI��ǥ�� ���� ��ǥ�� ����
        Tween move = transform.DOMove(vecTo, fFadeOutTime);
        Sequence sequence = DOTween.Sequence()
            .Append(fadeIn).SetEase(Ease.Linear)
            .Append(fadeOut)
            .Join(move).SetEase(Ease.InSine)
            .Join(fontSize)
            .OnComplete(()=>Destroy(gameObject));
    }
    
    //fDamage ������ ���� Text ��ȯ
    string DamageToText(float fDamage)
    {
        string strDamage = "";
        if (fDamage / 1000 < 1)
        {
            strDamage = fDamage.ToString("F0");
        }
        else
        {
            fDamage = fDamage / 1000;
            if(fDamage >= 1000000)
            {
                fDamage = fDamage / 1000000;
                strDamage = fDamage.ToString("F2") + "B";
            }
            else if(fDamage >= 1000)
            {
                fDamage = fDamage / 1000;
                strDamage = fDamage.ToString("F2") + "M";
            }
            else
            {
                strDamage = fDamage.ToString("F2") + "K";
            }
        }

        return strDamage;
    }

}
