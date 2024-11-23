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

    [SerializeField]
    TextMeshProUGUI m_textStage = null;

    Sequence m_seqStage = null;
    Sequence m_seqMoney = null;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.TakeObject(gameObject);
        SetStageText(GameManager.Instance.CurrentStage);
        SetMoneyText(0, GameManager.Instance.CurrentMoney);
    }


    //Stage Text를 Tweening하여 표시
    public void SetStageText(int nStage)
    {
        if(m_textStage != null)
        {
            string strText = "Stage" + nStage.ToString();
            Tween FadeOut = m_textStage.DOFade(0, 0.5f);
            Tween FadeIn = m_textStage.DOFade(1, 2f);
            //DOTween 무료 버전은 textMeshPro를 지원하지 않지만 아래처럼, DOTween.To()를 통해 사용가능하다.
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
            Debug.LogError("m_textStage 가 없습니다.");
        }
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

            if (m_textMoney != null)
            {

                float fTweenTime = 0.5f / GameManager.Instance.MoveSpeed;

                //DOTween.To()를 사용하여 현재 text에서 새로들어온 text로 값 변경
                Tween tween = DOTween.To(() => "", (str) => m_textMoney.text = str, strMoney, fTweenTime);
                if (m_seqMoney != null && m_seqMoney.IsActive())
                {
                    m_seqMoney.Kill();
                }

                m_seqMoney = DOTween.Sequence();
                m_seqMoney.PrependInterval(fWaitTime); //fWaitTime만큼 초반에 기다리기
                m_seqMoney.Append(tween).SetEase(Ease.Linear); 
                
            }
            else
            {
                Debug.LogError("m_textMoeny가 없습니다.");
            }
        }
        else
        {
            Debug.LogError("m_textMoney가 없습니다.");
        }
    }


}
