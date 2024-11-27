using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using System;

public class DamageText : MonoBehaviour
{
    /*
     * DOTween을 사용하여 GamaManager.cs의 FieldBattle()에서 Player가 공격할때 FieldMob의 Head Position(UI좌표로 바꾸어)
     * 해당 텍스트 프리팹을 Generater.cs에서 생성해준다.
     * 생성후 SetDamageText()를 호출하여 Player의 공격력 만큼 텍스트 애니메이션 효과를 준다.
     * 1. FieldMod의 머리 위치 에서 y축으로 0.5f만큼 이동(UI 좌표로 바꾸어 이동)
     * 2. 동시에 a값을 1로 만들어 준다.(서서히)
     * 3. 폰트 크기를 현재에서(60) 100으로 올린다.
     */


    const float m_fTime = 0.9f; //player_attack 클립의 기본 프레임 시간인 1초이므로 해당 값 보다는 작게 하고 공속을 곱해준다.
    TextMeshProUGUI m_TMPDamage = null;

    //CoinTween과 마찬가지로 Generator.cs에서 해당 프리팹을 Instantiate()하고 SetDamageText()를 바로 호출하게된다.
    //Awake()는 프리팹이 인스턴트화 된 즉시 호출되기에 Start()가 아닌 Awake()에서 멤버변수를 할당해 주어야 한다.
    private void Awake()
    {
        //투명도 0으로 설정
        m_TMPDamage = GetComponent<TextMeshProUGUI>();
        Color color = m_TMPDamage.color;
        color.a = 0;
        m_TMPDamage.color = color;
  
    }


    // DamageText Tweening
    public void SetDamageText(Vector2 vecFrom, float fDamage, float fAttackSpeed, bool isCritical)
    {
        //치명타일 시 red색상으로 변경
        if (isCritical)
        {
            Color color = Color.red;
            color.a = 0;
            m_TMPDamage.color = color;
        }

        //(수정)vecFrom이 FieldMob의 Head, World Position이기에 스크린좌표로 변경해야한다.
        //(수정)transform.position = Camera.main.WorldToScreenPoint(vecFrom);

        // 위 주석 코드에서 아래로 변경 : CameraResolution.cs와 함께
        // Canvas Render mode를 screen space- camera 에서 Main camera로 지정하여 실행 시 UI좌표가 월드 좌표로 변경
        transform.position = vecFrom;

        //목적지 좌표 설정
        Vector2 vecTo = vecFrom;
        vecTo.y += 0.5f;

        //fDamage에 따른 Text설정 (1000 = "1K", 1000000 = "1M", 1000000000 = "1B")
        m_TMPDamage.text = DamageToText(fDamage);
        
        //기본시간에서 AttackSpeed(배속)을 나눈다. 
        float fTime = m_fTime / fAttackSpeed;
        float fFadeInTime = (fTime / 10) * 5; //fTime의 50프로 사용
        float fFadeOutTime = fTime - fFadeInTime;

        //투명도를 0에서 1로 fFadeInTime동안 변화
        Tween fadeIn = DOTween.To(() => 0.0f,
            (a) =>
            {
                Color color = m_TMPDamage.color;
                color.a = a;
                m_TMPDamage.color = color;
            }, 1, fFadeInTime);

        //투명도를 0으로 fFadeOutTime동안 변화
        Tween fadeOut = DOTween.To(() => m_TMPDamage.color.a,
            (a) =>
            {
                Color color = m_TMPDamage.color;
                color.a = a;
                m_TMPDamage.color = color;
            }, 0, fFadeOutTime);

        //폰트 사이즈를 100에서 60으로 변화
        Tween fontSize = DOTween.To(() => 100, (fontSize) => { m_TMPDamage.fontSize = fontSize; }, 60, fFadeOutTime);

        //(수정)Tween move = transform.DOMove(Camera.main.WorldToScreenPoint(vecTo),fFadeOutTime);
        // 위 주석 코드에서 아래로 변경 : CameraResolution.cs와 함께
        // Canvas Render mode를 screen space- camera 에서 Main camera로 지정하여 실행 시 UI좌표가 월드 좌표로 변경
        Tween move = transform.DOMove(vecTo, fFadeOutTime);
        Sequence sequence = DOTween.Sequence()
            .Append(fadeIn).SetEase(Ease.Linear)
            .Append(fadeOut)
            .Join(move).SetEase(Ease.InSine)
            .Join(fontSize)
            .OnComplete(()=>Destroy(gameObject));
    }
    
    //fDamage 단위에 따른 Text 변환
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
