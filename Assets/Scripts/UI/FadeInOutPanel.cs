using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;


public class FadeInOutPanel : MonoBehaviour
{
    /*
        Scene이 전환될 시 Fade In/Out 효과를 줄 panel에 부착
     */
    [Tooltip("시작 시 FadeIn?")]
    [SerializeField] bool m_isStart = true; //Scene시작시 FadeIn 효과를 줄지 Inspacter에서 설정
    Image m_image;
    Sequence m_seqFade = null;
    // Start is called before the first frame update
    void Start()
    {
        if (m_isStart) //시작 true 설정 시 FadeIn함수 호출
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
        //이미 시퀀스가 실행중일시 Kill()
        if(m_seqFade != null && m_seqFade.IsActive())
        {
            m_seqFade.Kill();
        }

        //TO함수를 사용해 alpha값 조정
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
        //이미 시퀀스가 실행중일 시 Kill()
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
