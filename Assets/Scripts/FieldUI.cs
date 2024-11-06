using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FieldUI : MonoBehaviour
{
    /*
        GameScene Canvas의 FieldPanel에 부착 되어있는 스크립트 입니다. 
        GameScene에서 스크롤 뷰(사용자 상호작용 가능UI)제외한 Field에서 
        Stage TextUI, CoinPanel UI등을 관리한다.
     */

    [SerializeField]
    GameObject m_objCoinImage = null; //CoinPanel의 Coin이미지

    [SerializeField]
    TextMeshProUGUI m_textMoney = null;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.TakeObject(gameObject);
        SetMoneyText(0, GameManager.Instance.CurrentMoney);
    }


    //CoinTween.cs에서 이동할 목표인 CoinUI의 RectTransform을 전달할 함수 GameManager에서 호출
    public RectTransform GetCoinUIRect()
    {
        if(m_objCoinImage != null)
        {
            return m_objCoinImage.GetComponent<RectTransform>();
        }
        else
        {
            Debug.LogError("m_objCoinImage가 없습니다.");
            return null;
        }
    }

    //FieldUI의 money UI창의 money text 표시
    public void SetMoneyText(float fWaitTime, long nCurrentMoney)
    {
        if(m_textMoney != null)
        {
            double fMoney = nCurrentMoney;
            string strMoney = "";

            if(fMoney / 1000 < 1) // 현재 돈이 1000Coin 미만일 경우
            {
                strMoney = nCurrentMoney.ToString();
            }
            else //현재 돈이 1000coin이상인 경우
            { 
                fMoney = fMoney / 1000;

                if (fMoney >= 1000000) //현재 돈이 Billion(10억)이상인경우
                {
                    fMoney = fMoney / 1000000;
                    strMoney = fMoney.ToString("F1") + "B";
                }
                else if(fMoney >= 1000) //현재 돈이 Million(100만) 이상인 경우
                {
                    fMoney = fMoney / 1000;
                    strMoney = fMoney.ToString("F1") + "M";
                }
                else// 현재 돈이 Million 미만일 경우
                {
                    strMoney = fMoney.ToString("F1") + "K";
                }
            }

            StartCoroutine(CorMoneyText(fWaitTime, strMoney));
        }
        else
        {
            Debug.LogError("m_textMoney가 없습니다.");
        }
    }

    //DoTween을 사용하여 text 변환 -> 다만 Generator에서 coin이 생성되어 이동하는 시간. GameManager의 GetCoinTime동안 wait한다.
    IEnumerator CorMoneyText(float fWaitTime, string strMoney)
    {
        yield return new WaitForSeconds(fWaitTime);
        if(m_textMoney != null)
        {
            //현재 text값 받아오기
            string strCurrentMoeny = m_textMoney.text;
            //DOTween.To()를 사용하여 현재 text에서 새로들어온 text로 값 변경
            DOTween.To(() => strCurrentMoeny, (str) => m_textMoney.text = str, strMoney, 0.5f).SetEase(Ease.Linear);
            yield break;
        }
        else
        {
            Debug.LogError("m_textMoeny가 없습니다.");
            yield break;
        }
    }

}
