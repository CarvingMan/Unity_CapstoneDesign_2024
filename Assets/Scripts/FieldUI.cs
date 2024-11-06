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

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.TakeObject(gameObject);
        SetMoneyText(0, GameManager.Instance.CurrentMoney);
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
    public void SetMoneyText(float fWaitTime, long nCurrentMoney)
    {
        if(m_textMoney != null)
        {
            double fMoney = nCurrentMoney;
            string strMoney = "";

            if(fMoney / 1000 < 1) // ���� ���� 1000Coin �̸��� ���
            {
                strMoney = nCurrentMoney.ToString();
            }
            else //���� ���� 1000coin�̻��� ���
            { 
                fMoney = fMoney / 1000;

                if (fMoney >= 1000000) //���� ���� Billion(10��)�̻��ΰ��
                {
                    fMoney = fMoney / 1000000;
                    strMoney = fMoney.ToString("F1") + "B";
                }
                else if(fMoney >= 1000) //���� ���� Million(100��) �̻��� ���
                {
                    fMoney = fMoney / 1000;
                    strMoney = fMoney.ToString("F1") + "M";
                }
                else// ���� ���� Million �̸��� ���
                {
                    strMoney = fMoney.ToString("F1") + "K";
                }
            }

            StartCoroutine(CorMoneyText(fWaitTime, strMoney));
        }
        else
        {
            Debug.LogError("m_textMoney�� �����ϴ�.");
        }
    }

    //DoTween�� ����Ͽ� text ��ȯ -> �ٸ� Generator���� coin�� �����Ǿ� �̵��ϴ� �ð�. GameManager�� GetCoinTime���� wait�Ѵ�.
    IEnumerator CorMoneyText(float fWaitTime, string strMoney)
    {
        yield return new WaitForSeconds(fWaitTime);
        if(m_textMoney != null)
        {
            //���� text�� �޾ƿ���
            string strCurrentMoeny = m_textMoney.text;
            //DOTween.To()�� ����Ͽ� ���� text���� ���ε��� text�� �� ����
            DOTween.To(() => strCurrentMoeny, (str) => m_textMoney.text = str, strMoney, 0.5f).SetEase(Ease.Linear);
            yield break;
        }
        else
        {
            Debug.LogError("m_textMoeny�� �����ϴ�.");
            yield break;
        }
    }

}
