using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EntryRwardPanel : MonoBehaviour
{
    /*
     *  접속 보상은 최종 USER_DATA 업데이트 시간(최종 저장 시간) updateAt컬럼의 시간과의 현재 시간과의
     *  차이가 1시간 이상 차이가 나게되면 주어지게 된다. (최대 24시간)
     *  LoadingScene에서  GameManager SetUserDatar에서 서버로 받아온 updateAt정보에 따라 접속 보상금을 계산
     *  접속보상 = stage * 기본 코인 값(10) * (마지막 저장시간 - 현재시간)(최대24시간)  
     */
    [Tooltip("누적시간")]
    [SerializeField] TextMeshProUGUI m_tmpSpanHour = null;

    [Tooltip("보상금")]
    [SerializeField] TextMeshProUGUI m_tmpEntryReward = null;

    [Tooltip("보상받기button")]
    [SerializeField] Button m_btnGetReward;

    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.Instance.IsEntryReward)
        {
            if(m_tmpSpanHour != null)
            {
                m_tmpSpanHour.text = GameManager.Instance.EntryHourSpan.ToString();
            }
            else
            {
                Debug.LogError("m_tmpSpanHour가 없습니다." + gameObject.name);
            }

            if (m_tmpEntryReward != null) 
            {
                m_tmpEntryReward.text = GameManager.Instance.EntryReward.ToString("N0") + "G";
            }
            else
            {
                Debug.LogError("m_tmpEntryReward가 없습니다." + gameObject.name);
            }

            if (m_btnGetReward != null) 
            {
                m_btnGetReward.onClick.AddListener(ClickGetRewardBtn);
            }
            else
            {
                Debug.LogError("m_btnGetReward가 없습니다.");
            }
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    //m_btnGetReward onClick AddListener에 추가할 함수
    void ClickGetRewardBtn()
    {
        GameManager.Instance.EarnEntryReward();
        gameObject.SetActive(false);
    }
}
