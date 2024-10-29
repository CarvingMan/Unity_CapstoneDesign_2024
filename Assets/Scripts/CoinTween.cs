using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinTween : MonoBehaviour
{
    /*
     * Dotween 을 사용하여 Generator.cs에서 죽은 FieldMob위치에서 생성된 해당 coin프리팹이 coin UI로 이동하고 사라지도록 설정
     */

    //GameManager의 현재 Stage, FieldBoss인지에 따라 코인의 값어치를 설정
    //int m_nValue = 0;

    //Ease는 DoTween에서 다양한 시간당 변화량 그래프가 있는데 해당 Ease를 트위너에 SetEase()를 통하여 설정해 줄 수 있다.
    //다양한 그래프가 있기에 인스펙터창에서 설정하고 테스트해보고 결정하였다.
    //강도(시간당 변화량) : Sine < Quad < Cubic < Quart < Quint < Expo / Out : 초반에 변화량이 큼, In: 후반에 변화량이 큼
    //Generator에서 여러개의 코인이 생성될때 한 지점에서 각 coin들이 각기다른 위치(반경제한)에 m_easeExplosion값으로 이동하고,
    //coinUI의 WorldPositioin에 m_easeTarget값으로 이동
    [SerializeField]
    Ease m_easeExplosion; //동전이 생기고 주변으로 터질때 시간당 변화량 -> InCubic으로 설정
    [SerializeField]
    Ease m_easeTarget; //CoinUI로 이동할때 시간당 변화량 -> OutCubic으로 설정


    //coin이 이동할 Target
    [SerializeField]
    RectTransform m_trTarget; //coin이 이동할 UI의 RectTransform
    Vector2 m_vecTargetPos = Vector2.zero; //스크린 좌표에서 World좌표로 변환

    //coin 이동 시간
    //float m_fTotalTime = 0f


    // Start is called before the first frame update
    void Start()
    {
        if(m_trTarget != null)
        {
            //CoinUI RectTransform좌표를 world좌표로 변경하여 저장
            m_vecTargetPos = Camera.main.ScreenToWorldPoint(m_trTarget.position);
        }
        else
        {
            Debug.LogError("m_trTarget이 없습니다.");
        }


        //시퀀스 생성
        Sequence coinSequence = DOTween.Sequence();
        //coinSequence에 아래 CoinSequence()에서 정의한 시퀀스를 Append(뒤에 추가)해준다.
        coinSequence.Append(CoinSequence());
    }

    Sequence CoinSequence()
    {
        
        //insideUnitCircle은 반경 1인 원 안에서 하나의 좌표를 랜덤으로 가져온다. 0.8f를 곱하여 반경을 0.8로 제한
        Vector2 vecRandCirPos = Random.insideUnitCircle * 0.8f;
        //transform.posision에서 랜덤으로 나온 반경 0.8f 원 안의 좌표를 더해준다. 
        //해당 값으로 초반에 이동 후 coinUI로 이동 할 것 ->Generator.cs에서 coin을 여러 개 생성시 각각 반경0.8f로 터지는 효과를 주기 위함
        Vector2 vecExplo = (Vector2)transform.position + vecRandCirPos;

        return DOTween.Sequence()
            .Append(transform.DOMove(vecExplo, 0.3f)).SetEase(m_easeExplosion)
            .Append(transform.DOMove(m_vecTargetPos, 0.5f)).SetEase(m_easeTarget)
            .OnComplete(SqCallBack); //완료될 시 SqCallBack()함수 호출
       
        /*
         * Sequence는 Tween들을 순서에 맞게 배열하여 차례차례 원하는 효과를 연출할 수 있다.
         * DOTween.Sequence()를 return해 준다.
         * .Append는 시퀀스에 Tween을 추가해 준다. -> 마지막에 추가(Start()에서 처럼 시퀀스토 추가가능하다.)
         * DoMove()로 원하는 Position에 duration(초)동안 이동
         */
    }

    //시퀀스 콜백 함수
    void SqCallBack()
    {
        //오브젝트 제건
        Destroy(gameObject);
    }
}
