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
     * GameScene�� ScrollView > ViewPort > Content > ���ο� �ִ� ������ ���� �ɷ�ġ �гο� ���� ����
     * E_Status_Type(enum)�� �ν����Ϳ��� ������ �־� �� �гο� �´� ������ �ϰ� �ȴ�.
     * ������ ��ư�� AddListener �� GameManager�� LevelUp() ȣ���Ͽ� Type�� �´� �ɷ�ġ�� ���� ��
     * ���� �� �� ��ũ��Ʈ�� ������ �г��� �ڽ� TextPanel�� text���� Tweeing�ϸ� ������Ʈ �� �ش�.
     */
    [SerializeField]
    GameManager.E_Status_Type m_eStatusType = GameManager.E_Status_Type.None;

    //TextPanel�� text��
    [SerializeField]
    TextMeshProUGUI m_tmpCurrentLv = null; //���� ���� �ؽ�Ʈ
    [SerializeField] 
    TextMeshProUGUI m_tmpCurrentStatus = null; //���� �ɷ�ġ �ؽ�Ʈ

    //��ư
    [SerializeField]
    Button m_btnLvUp = null; //������ ��ư
    [SerializeField]
    TextMeshProUGUI m_tmpPrice = null; //��ư���� ���� �ؽ�Ʈ

    //TextUpdate Sequence
    Sequence m_seqTextUpdate = null;
    const float m_fTweenTime = 0.5f; 

    // Start is called before the first frame update
    void Start()
    {
        UpdateText(false); //Tween ���� �ʱ� text�ʱ�ȭ
        m_btnLvUp.onClick.AddListener(ClickToLvUp);
    }



    //���� ����(K,M,B) �ؽ�Ʈ�� ��ȯ ���� ��ȯ 
    string ConvertFloatToText(double dNumber)
    {
        string strNumber = "";
        if(dNumber / 1000 < 1)
        {
            //1000 �̸��ΰ��
            strNumber = dNumber.ToString("F0");
        }
        else
        {
            dNumber = dNumber / 1000;
            //1000�̻��� ���
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

    //E_Status_Type���� text ���� isTween false�� �� �� Tween�۾� ����
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
                m_btnLvUp.interactable = false; //������ ��ư ��Ȱ��ȭ
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
                m_btnLvUp.interactable = false; //������ ��ư ��Ȱ��ȭ
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
                m_btnLvUp.interactable = false; //������ ��ư ��Ȱ��ȭ
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
                m_btnLvUp.interactable = false; //������ ��ư ��Ȱ��ȭ
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
                m_btnLvUp.interactable = false; //������ ��ư ��Ȱ��ȭ
            }
        }
        else
        {
            Debug.LogError("m_eStatusType�� �����ϼ���");
        }


        if (isTween)
        {
            //�ؽ�Ʈ Ʈ���� ����
            TweenText(strNewLv,strNewStatus,strNewPrice);
        }
        else
        {
            //isTween�� false�� ��� Start()���� �ʱ� text ���� �Ҷ����� tween�� ����� ������ ����.
            m_tmpCurrentLv.text = strNewLv;
            m_tmpCurrentStatus.text = strNewStatus;
            m_tmpPrice.text = strNewPrice;
        }

    }

    //TextTween
    void TweenText(string strNewLv, string strNewStatus, string strNewPrice)
    {
        //��ư�� �������� ������ ������ ��� �������� �� �� Ʈ���� Kill�ϰ� �ٽ� �����ؾ��Ѵ�.
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
    
   
    //m_btnLvUp �� OnClick()�� addListenner �� �Լ�
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
            Debug.LogError("m_eStatusType�� ������ �ּ���");
        }
    }
}
