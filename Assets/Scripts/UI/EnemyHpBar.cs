using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHpBar : MonoBehaviour
{
    /*
     * Generator���� FieldMob���� �� ���ÿ� EnemyHpBar ���� ������ �ش� FieldMob���� �Ѱ��ش�.
     */



    [SerializeField]
    Image m_imgHpBG = null;
    [SerializeField]
    Image m_imgHpBar = null;

    float m_fMaxWidth = 0;

    private void Awake()
    {
        if(m_imgHpBG != null)
        {
            m_fMaxWidth = m_imgHpBG.GetComponent<RectTransform>().rect.width;
        }
        else
        {
            Debug.LogError("m_imgHpBG�� �����ϴ�.");
        }
    }

    public void SetHpBar(float fCurrentHpRatio)
    {
        if (m_imgHpBar != null) 
        {
            Vector2 vecRectSize = m_imgHpBar.GetComponent<RectTransform>().sizeDelta;
            vecRectSize.x = m_fMaxWidth * fCurrentHpRatio;
            m_imgHpBar.GetComponent<RectTransform>().sizeDelta = vecRectSize;
        }
        else
        {
            Debug.LogError("m_imgHpBar�� �����ϴ�.");
        }
    }
}
