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
    TextMeshProUGUI m_tmpMoney = null;

    [SerializeField]
    TextMeshProUGUI m_tmpStage = null;

    [SerializeField]
    Button m_btnMenu = null; //메뉴 버튼
    [SerializeField]
    GameObject m_objMenuPanel = null; //메뉴 panel

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
            Debug.LogError("m_objMenuPanel이 없습니다."+gameObject.name);
        }

        if(m_btnMenu != null)
        {
            m_btnMenu.onClick.AddListener(ClickMenuBtn);
        }
        else
        {
            Debug.LogError("m_btnMenu가 없습니다." + gameObject.name);
        }
    }

    //m_btnMenu onClick AddListener에 추가할 함수
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

    //Stage Text를 Tweening하여 표시
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
            //DOTween 무료 버전은 DoText 함수를 textMeshPro를 지원하지 않지만 아래처럼, DOTween.To()를 통해 사용가능하다.
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
    public void SetMoneyText(float fWaitTime, long lCurrentMoney, bool isSound = true, bool isGet = true)
    {
        if(m_tmpMoney != null)
        {
            double dMoney = lCurrentMoney;
            string strMoney = "";

            if(dMoney / 1000 < 1) // 현재 돈이 1000Coin 미만일 경우
            {
                strMoney = lCurrentMoney.ToString();
            }
            else //현재 돈이 1000coin이상인 경우
            { 
                dMoney = dMoney / 1000;

                if (dMoney >= 1000000) //현재 돈이 Billion(10억)이상인경우
                {
                    dMoney = dMoney / 1000000;
                    strMoney = dMoney.ToString("F1") + "B";
                }
                else if(dMoney >= 1000) //현재 돈이 Million(100만) 이상인 경우
                {
                    dMoney = dMoney / 1000;
                    strMoney = dMoney.ToString("F1") + "M";
                }
                else// 현재 돈이 Million 미만일 경우
                {
                    strMoney = dMoney.ToString("F1") + "K";
                }
            }

            if (m_tmpMoney != null)
            {

                float fTweenTime = 0.5f / GameManager.Instance.MoveSpeed;

                //DOTween.To()를 사용하여 현재 text에서 새로들어온 text로 값 변경
                Tween tween = DOTween.To(() => "", (str) => m_tmpMoney.text = str, strMoney, fTweenTime);
                if (m_seqMoney != null && m_seqMoney.IsActive())
                {
                    m_seqMoney.Kill();
                }

                m_seqMoney = DOTween.Sequence();
                m_seqMoney.PrependInterval(fWaitTime); //fWaitTime만큼 초반에 기다리기
                m_seqMoney.Append(tween).SetEase(Ease.Linear);
                m_seqMoney.JoinCallback(() =>
                {
                    //오디오 출력
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
                Debug.LogError("m_textMoeny가 없습니다.");
            }
        }
        else
        {
            Debug.LogError("m_textMoney가 없습니다.");
        }
    }


}
