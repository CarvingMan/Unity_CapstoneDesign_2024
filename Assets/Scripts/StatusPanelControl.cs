using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.Rendering;

public class StatusPanelControl : MonoBehaviour
{
    /*
     * GameScene의 ScrollView > ViewPort > Content > 내부에 있는 레벨업 관련 능력치 패널에 각각 부착
     * E_Status_Type(enum)을 인스펙터에서 지정해 주어 각 패널에 맞는 동작을 하게 된다.
     * 레벨업 버튼의 AddListener 로 GameManager의 LevelUp() 호출하여 Type에 맞는 능력치를 레벨 업
     * 레벨 업 시 스크립트가 부착된 패널의 자식 TextPanel의 text들을 Tweeing하며 업데이트 해 준다.
     */
    [SerializeField]
    GameManager.E_Status_Type m_eStatusType = GameManager.E_Status_Type.None;

    //TextPanel의 text들
    [SerializeField]
    TextMeshProUGUI m_tmpCurrentLv = null; //현재 레벨 텍스트
    [SerializeField] 
    TextMeshProUGUI m_tmpCurrentStatus = null; //현재 능력치 텍스트

    //버튼
    [SerializeField]
    Button m_btnLvUp = null; //레벨업 버튼
    [SerializeField]
    TextMeshProUGUI m_tmpPrice = null; //버튼내부 가격 텍스트

    //TextUpdate Sequence
    Sequence m_seqTextUpdate = null;
    const float m_fTweenTime = 0.5f; 

    // Start is called before the first frame update
    void Start()
    {
        UpdateText(false); //Tween 없이 초기 text초기화
        m_btnLvUp.onClick.AddListener(ClickToLvUp);
    }



    //단위 별로(K,M,B) 텍스트로 변환 시켜 반환 
    string ConvertFloatToText(double dNumber)
    {
        string strNumber = "";
        if(dNumber / 1000 < 1)
        {
            //1000 미만인경우
            strNumber = dNumber.ToString("F0");
        }
        else
        {
            dNumber = dNumber / 1000;
            //1000이상인 경우
            if(dNumber >= 1000000)
            {
                dNumber = dNumber / 1000000;
                strNumber = dNumber.ToString("F1") + "B";
            }
            else if (dNumber >= 1000)
            {
                dNumber = dNumber / 1000;
                strNumber = dNumber.ToString("F1") + "M";
            }
            else
            {
                strNumber = dNumber.ToString("F1") + "K";
            }
        }
        return strNumber;
    }

    //E_Status_Type별로 text 설정 isTween false로 할 시 Tween작업 안함
    void UpdateText(bool isTween = true)
    {
        string strNewLv = "";
        string strNewStatus = "";
        string strNewPrice = "";

        if(m_eStatusType == GameManager.E_Status_Type.AttackDamage)
        {
            strNewStatus = ConvertFloatToText(GameManager.Instance.AttackDamage);
            double dPrice = GameManager.Instance.AttackDamagePrice;
            strNewPrice = ConvertFloatToText(dPrice) + " G";
            if (GameManager.Instance.AttackDamageLv != GameManager.Instance.MaxDamageLv)
            {
                strNewLv = "Lv " + GameManager.Instance.AttackDamageLv.ToString();
            }
            else
            {
                strNewLv = "Max";
                strNewPrice = "";
                m_btnLvUp.interactable = false; //레벨업 버튼 비활성화
            }
            
        }
        else if(m_eStatusType == GameManager.E_Status_Type.AttackSpeed)
        {
            strNewStatus = (GameManager.Instance.AttackSpeed * 100).ToString("F0") + "%";
            double dPrice = GameManager.Instance.AttackSpeedPrice;
            strNewPrice = ConvertFloatToText(dPrice) + " G";
            if (GameManager.Instance.AttackSpeedLv != GameManager.Instance.MaxSpeedLv)
            {
                strNewLv = "Lv " + GameManager.Instance.AttackSpeedLv.ToString();
            }
            else
            {
                strNewLv = "Max";
                strNewPrice = "";
                m_btnLvUp.interactable = false; //레벨업 버튼 비활성화
            }
        }
        else if (m_eStatusType == GameManager.E_Status_Type.MoveSpeed)
        {
            strNewStatus = (GameManager.Instance.MoveSpeed * 100).ToString("F0") + "%";
            double dPrice = GameManager.Instance.MoveSpeedPrice;
            strNewPrice = ConvertFloatToText(dPrice) + " G";
            if (GameManager.Instance.MoveSpeedLv != GameManager.Instance.MaxSpeedLv)
            {
                strNewLv = "Lv " + GameManager.Instance.MoveSpeedLv.ToString();
            }
            else
            {
                strNewLv = "Max";
                strNewPrice = "";
                m_btnLvUp.interactable = false; //레벨업 버튼 비활성화
            }
        }
        else if (m_eStatusType == GameManager.E_Status_Type.CriticalProb)
        {
            strNewStatus = (GameManager.Instance.CriticalProb * 100).ToString("F0") + "%";
            double dPrice = GameManager.Instance.CriticalProbPrice;
            strNewPrice = ConvertFloatToText(dPrice) + " G";
            if (GameManager.Instance.CriticalProbLv != GameManager.Instance.MaxCriticalLv)
            {
                strNewLv = "Lv " + GameManager.Instance.CriticalProbLv.ToString();
            }
            else
            {
                strNewLv = "Max";
                strNewPrice = "";
                m_btnLvUp.interactable = false; //레벨업 버튼 비활성화
            }
        }
        else if (m_eStatusType == GameManager.E_Status_Type.CriticalRatio)
        {
            strNewStatus = (GameManager.Instance.CriticalRatio * 100).ToString("F0") + "%";
            double dPrice = GameManager.Instance.CriticalRatioPrice;
            strNewPrice = ConvertFloatToText(dPrice) + " G";
            if (GameManager.Instance.CriticalRatioLv != GameManager.Instance.MaxCriticalLv)
            {
                strNewLv = "Lv " + GameManager.Instance.CriticalRatioLv.ToString();
            }
            else
            {
                strNewLv = "Max";
                strNewPrice = "";
                m_btnLvUp.interactable = false; //레벨업 버튼 비활성화
            }
        }
        else
        {
            Debug.LogError("m_eStatusType를 설정하세요");
        }


        if (isTween)
        {
            //텍스트 트위닝 시작
            TweenText(strNewLv,strNewStatus,strNewPrice);
        }
        else
        {
            //isTween이 false인 경우 Start()에서 초기 text 설정 할때에는 tween을 사용할 이유가 없다.
            m_tmpCurrentLv.text = strNewLv;
            m_tmpCurrentStatus.text = strNewStatus;
            m_tmpPrice.text = strNewPrice;
        }

    }

    //TextTween
    void TweenText(string strNewLv, string strNewStatus, string strNewPrice)
    {
        //버튼을 연속으로 여러번 눌러서 계속 레벨업이 될 시 트윈을 Kill하고 다시 시작해야한다.
        if(m_seqTextUpdate != null && m_seqTextUpdate.IsActive())
        {
            m_seqTextUpdate.Kill();
        }

        Tween tweenLv = DOTween.To(() => "", strLv => m_tmpCurrentLv.text = strLv , strNewLv, m_fTweenTime);
        Tween tweenStatus = DOTween.To(() => "", strStatus => m_tmpCurrentStatus.text = strStatus, strNewStatus, m_fTweenTime);
        Tween tweenPrice = DOTween.To(() => "", strPrice => m_tmpPrice.text = strPrice, strNewPrice, m_fTweenTime);
        m_seqTextUpdate = DOTween.Sequence();
        m_seqTextUpdate.Prepend(tweenLv).SetEase(Ease.Linear);
        m_seqTextUpdate.Join(tweenStatus).SetEase(Ease.Linear);
        m_seqTextUpdate.Join(tweenPrice).SetEase(Ease.Linear);
    }
    
   
    //m_btnLvUp 의 OnClick()에 addListenner 할 함수
    void ClickToLvUp()
    {
        if (m_eStatusType != GameManager.E_Status_Type.None || m_eStatusType != GameManager.E_Status_Type.Max)
        {
            bool isAbleLvUp = GameManager.Instance.LevelUp(m_eStatusType);
            if (isAbleLvUp)
            {
                UpdateText();
            }
        }
        else
        {
            Debug.LogError("m_eStatusType을 지정해 주세요");
        }
    }
}
