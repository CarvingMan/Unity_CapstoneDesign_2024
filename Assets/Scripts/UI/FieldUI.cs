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
    TextMeshProUGUI m_tmpMoney = null;

    [SerializeField]
    TextMeshProUGUI m_tmpStage = null;

    [SerializeField]
    Button m_btnMenu = null; //�޴� ��ư
    [SerializeField]
    GameObject m_objMenuPanel = null; //�޴� panel

    Sequence m_seqStage = null;
    Sequence m_seqMoney = null;

    // Start is called before the first frame update
    void Start()
    {

        GameManager.Instance.TakeObject(gameObject);
        SetStageText(GameManager.Instance.CurrentStage);
        SetMoneyText(0, GameManager.Instance.CurrentMoney,false);
        if(m_objMenuPanel == null)
        {
            Debug.LogError("m_objMenuPanel�� �����ϴ�."+gameObject.name);
        }

        if(m_btnMenu != null)
        {
            m_btnMenu.onClick.AddListener(ClickMenuBtn);
        }
        else
        {
            Debug.LogError("m_btnMenu�� �����ϴ�." + gameObject.name);
        }
    }

    //m_btnMenu onClick AddListener�� �߰��� �Լ�
    //timeScale = 0, m_objMenuPanel Active
    void ClickMenuBtn()
    {
        if (m_objMenuPanel != null) 
        {
            m_objMenuPanel.SetActive(true);
            if (Time.timeScale != 0)
            {
                Time.timeScale = 0;
            }
        }
    }

    //Stage Text�� Tweening�Ͽ� ǥ��
    public void SetStageText(int nStage)
    {
        if(m_tmpStage != null)
        {
            string strText = "Stage" + nStage.ToString();
            Tween FadeOut = DOTween.To(() => 1.0f, (alpha) =>
            {
                Color color = m_tmpStage.color;
                color.a = alpha;
                m_tmpStage.color = color;
            }, 0, 0.5f); ;
            Tween FadeIn = DOTween.To(() => 0.0f, alpha =>
            {
                Color color = m_tmpStage.color;
                color.a = alpha;
                m_tmpStage.color = color;
            }, 1, 2);
            //DOTween ���� ������ DoText �Լ��� textMeshPro�� �������� ������ �Ʒ�ó��, DOTween.To()�� ���� ��밡���ϴ�.
            Tween tweenText = DOTween.To(() => "", (str) => m_tmpStage.text = str, strText, 0.5f);
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
        if(m_tmpMoney != null)
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

            if (m_tmpMoney != null)
            {

                float fTweenTime = 0.5f / GameManager.Instance.MoveSpeed;

                //DOTween.To()�� ����Ͽ� ���� text���� ���ε��� text�� �� ����
                Tween tween = DOTween.To(() => "", (str) => m_tmpMoney.text = str, strMoney, fTweenTime);
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
