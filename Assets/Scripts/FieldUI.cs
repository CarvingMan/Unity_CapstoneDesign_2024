using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FieldUI : MonoBehaviour
{
    /*
        GameScene Canvas�� FieldPanel�� ���� �Ǿ��ִ� ��ũ��Ʈ �Դϴ�. 
        GameScene���� ��ũ�� ��(����� ��ȣ�ۿ� ����UI)������ Field���� 
        Stage TextUI, CoinPanel UI���� �����Ѵ�.
     */

    [SerializeField]
    GameObject m_objCoinImage = null; //CoinPanel�� Coin�̹���

    [SerializeField]
    TextMeshProUGUI m_textMoney = null;

    [SerializeField]
    TextMeshProUGUI m_textStage = null;

    Sequence m_seqStage = null;
    Sequence m_seqMoney = null;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.TakeObject(gameObject);
        SetStageText(GameManager.Instance.CurrentStage);
        SetMoneyText(0, GameManager.Instance.CurrentMoney,false);
    }


    //Stage Text�� Tweening�Ͽ� ǥ��
    public void SetStageText(int nStage)
    {
        if(m_textStage != null)
        {
            string strText = "Stage" + nStage.ToString();
            Tween FadeOut = m_textStage.DOFade(0, 0.5f);
            Tween FadeIn = m_textStage.DOFade(1, 2f);
            //DOTween ���� ������ textMeshPro�� �������� ������ �Ʒ�ó��, DOTween.To()�� ���� ��밡���ϴ�.
            Tween tweenText = DOTween.To(() => "", (str) => m_textStage.text = str, strText, 0.5f);
            if(m_seqStage != null && m_seqStage.IsActive())
            {
                m_seqStage.Kill();
            } 
            m_seqStage = DOTween.Sequence();
            m_seqStage.Prepend(FadeOut).SetEase(Ease.Linear);
            m_seqStage.Append(FadeIn).SetEase(Ease.Linear);
            m_seqStage.Join(tweenText);
        }
        else
        {
            Debug.LogError("m_textStage �� �����ϴ�.");
        }
    }


    //CoinTween.cs���� �̵��� ��ǥ�� CoinUI�� RectTransform�� ������ �Լ� GameManager���� ȣ��
    public RectTransform GetCoinUIRect()
    {
        if(m_objCoinImage != null)
        {
            return m_objCoinImage.GetComponent<RectTransform>();
        }
        else
        {
            Debug.LogError("m_objCoinImage�� �����ϴ�.");
            return null;
        }
    }

    //FieldUI�� money UIâ�� money text ǥ��
    public void SetMoneyText(float fWaitTime, long lCurrentMoney, bool isSound = true, bool isGet = true)
    {
        if(m_textMoney != null)
        {
            double dMoney = lCurrentMoney;
            string strMoney = "";

            if(dMoney / 1000 < 1) // ���� ���� 1000Coin �̸��� ���
            {
                strMoney = lCurrentMoney.ToString();
            }
            else //���� ���� 1000coin�̻��� ���
            { 
                dMoney = dMoney / 1000;

                if (dMoney >= 1000000) //���� ���� Billion(10��)�̻��ΰ��
                {
                    dMoney = dMoney / 1000000;
                    strMoney = dMoney.ToString("F1") + "B";
                }
                else if(dMoney >= 1000) //���� ���� Million(100��) �̻��� ���
                {
                    dMoney = dMoney / 1000;
                    strMoney = dMoney.ToString("F1") + "M";
                }
                else// ���� ���� Million �̸��� ���
                {
                    strMoney = dMoney.ToString("F1") + "K";
                }
            }

            if (m_textMoney != null)
            {

                float fTweenTime = 0.5f / GameManager.Instance.MoveSpeed;

                //DOTween.To()�� ����Ͽ� ���� text���� ���ε��� text�� �� ����
                Tween tween = DOTween.To(() => "", (str) => m_textMoney.text = str, strMoney, fTweenTime);
                if (m_seqMoney != null && m_seqMoney.IsActive())
                {
                    m_seqMoney.Kill();
                }

                m_seqMoney = DOTween.Sequence();
                m_seqMoney.PrependInterval(fWaitTime); //fWaitTime��ŭ �ʹݿ� ��ٸ���
                m_seqMoney.Append(tween).SetEase(Ease.Linear);
                m_seqMoney.JoinCallback(() =>
                {
                    //����� ���
                    if (isSound)
                    {
                        if (isGet)
                        {
                            AudioManager.Instance.CoinGetSound(GetComponent<AudioSource>());
                        }
                        else
                        {
                            AudioManager.Instance.CoinSpendSound(GetComponent<AudioSource>());
                        }
                    }
                });
                
            }
            else
            {
                Debug.LogError("m_textMoeny�� �����ϴ�.");
            }
        }
        else
        {
            Debug.LogError("m_textMoney�� �����ϴ�.");
        }
    }


}
