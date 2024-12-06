using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;


public class FadeInOutPanel : MonoBehaviour
{
    /*
        Scene�� ��ȯ�� �� Fade In/Out ȿ���� �� panel�� ����
     */
    [Tooltip("���� �� FadeIn?")]
    [SerializeField] bool m_isStart = true; //Scene���۽� FadeIn ȿ���� ���� Inspacter���� ����
    Image m_image;
    Sequence m_seqFade = null;
    // Start is called before the first frame update
    void Start()
    {
        if (m_isStart) //���� true ���� �� FadeIn�Լ� ȣ��
        {
            Color color = GetComponent<Image>().color;
            color.a = 1;
            GetComponent<Image>().color = color;  
            FadeInPanle();
        }
    }

    //FadeIn Tweening
    public void FadeInPanle()
    {
        //�̹� �������� �������Ͻ� Kill()
        if(m_seqFade != null && m_seqFade.IsActive())
        {
            m_seqFade.Kill();
        }

        //TO�Լ��� ����� alpha�� ����
        Tween tweenFadeIn = DOTween.To(() => 1f, alpha =>
        {
            Color color = GetComponent<Image>().color;
            color.a = alpha;
            GetComponent<Image>().color = color;
        }, 0, 1.5f);

        m_seqFade = DOTween.Sequence(tweenFadeIn);
    }

    //FadeOut Tweening
    public void FadeOutPanel()
    {
        //�̹� �������� �������� �� Kill()
        if(m_seqFade != null && m_seqFade.IsActive())
        {
            m_seqFade.Kill();
        }

        Tween tweenFadeOut = DOTween.To(() => 0.0f, alpha =>
        {
            Color color = GetComponent<Image>().color;
            color.a = alpha;
            GetComponent<Image>().color = color;
        }, 1, 1.5f);

        m_seqFade = DOTween.Sequence(tweenFadeOut);
    }
}
