using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EntryRwardPanel : MonoBehaviour
{
    /*
     *  ���� ������ ���� USER_DATA ������Ʈ �ð�(���� ���� �ð�) updateAt�÷��� �ð����� ���� �ð�����
     *  ���̰� 1�ð� �̻� ���̰� ���ԵǸ� �־����� �ȴ�. (�ִ� 24�ð�)
     *  LoadingScene����  GameManager SetUserDatar���� ������ �޾ƿ� updateAt������ ���� ���� ������� ���
     *  ���Ӻ��� = stage * �⺻ ���� ��(10) * (������ ����ð� - ����ð�)(�ִ�24�ð�)  
     */
    [Tooltip("�����ð�")]
    [SerializeField] TextMeshProUGUI m_tmpSpanHour = null;

    [Tooltip("�����")]
    [SerializeField] TextMeshProUGUI m_tmpEntryReward = null;

    [Tooltip("����ޱ�button")]
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
                Debug.LogError("m_tmpSpanHour�� �����ϴ�." + gameObject.name);
            }

            if (m_tmpEntryReward != null) 
            {
                m_tmpEntryReward.text = GameManager.Instance.EntryReward.ToString("N0") + "G";
            }
            else
            {
                Debug.LogError("m_tmpEntryReward�� �����ϴ�." + gameObject.name);
            }

            if (m_btnGetReward != null) 
            {
                m_btnGetReward.onClick.AddListener(ClickGetRewardBtn);
            }
            else
            {
                Debug.LogError("m_btnGetReward�� �����ϴ�.");
            }
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    //m_btnGetReward onClick AddListener�� �߰��� �Լ�
    void ClickGetRewardBtn()
    {
        GameManager.Instance.EarnEntryReward();
        gameObject.SetActive(false);
    }
}
