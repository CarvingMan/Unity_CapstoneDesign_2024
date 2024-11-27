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


    //coin 이동 시간
    float m_fExploTime = 0.0f; //처음 코인이 생겨 주변으로 터지는(Random.insideUnitCircle * 0.8f)좌표까지 이동시간
    float m_fTargetTime = 0.0f; //Target으로 이동시간

    // Uniti Doc를 보면 Awake 함수는 항상 Start 함수 전에 호출되며 프리팹이 인스턴스화 된 직후에 호출됩니다.
    // 게임 오브젝트가 시작하는 동안 비활성 상태인 경우 Awake 함수는 활성화될 때까지 호출되지 않습니다.
    // 위 처럼 나와있다. 따라서 Generator에서 해당 Coin프리팹을 instantiate()하고 바로 CoinMove()를 호출하기에
    // Awake()에서 할당 한Time들을 CoinMove()에서 그대로 사용하게 된다. 
    // 만약 Start에서 아래 Time들을 할당 해주면, Generator.cs 에서 프리팹 인스턴스화 하고 프레임이 끝나기 전에 CoinMove를 호출하기에
    // Time들이 모두 0그대로 사용된다. 따라서 Awake()에서 해당 필드를 초기화 해 주어야 한다.
    private void Awake()
    {
        //총 트윈으로 이동하는 시간 GameManager의 m_fCoinMoveTime과 이속 m_fMoveSpeed를 곱하여 사용
        float fTotalMoveTime = GameManager.Instance.CoinMoveTime / GameManager.Instance.MoveSpeed; 
        m_fExploTime = (fTotalMoveTime / 10) * 3.5f; //총 트윈시간에서 35% 사용
        m_fTargetTime = fTotalMoveTime - m_fExploTime; //총 트윈시간에서 m_fExploTime 제외 한 시간(65%)
        //Debug.Log("총시간 : " + fTotalMoveTime + "\n폭발시간: " + m_fExploTime + "Targe이동시간: " + m_fTargetTime);s
    }

    //Tween Sequence 생성 함수 -> 해당 함수를 Generater.cs에서 생성과 동시에 호출하여 coin을 매개인자 RectTransform의 월드좌표로 이동
    public void CoinMove(RectTransform recTrarget)
    {
        //(수정)매개인자로 들어온 recTarget : CoinUI RectTransform좌표를 world좌표로 변경하여 저장
        //(수정)Vector2 vecTargetPos = Camera.main.ScreenToWorldPoint(recTrarget.position);

        // 위 주석 코드에서 아래로 변경 : CameraResolution.cs와 함께
        // Canvas Render mode를 screen space- camera 에서 Main camera로 지정하여 실행 시 UI좌표가 월드 좌표로 변경
        Vector2 vecTargetPos = recTrarget.position;

        //insideUnitCircle은 반경 1인 원 안에서 하나의 좌표를 랜덤으로 가져온다. 0.8f를 곱하여 반경을 0.8로 제한
        Vector2 vecRandCirPos = Random.insideUnitCircle * 0.8f;
        //transform.posision에서 랜덤으로 나온 반경 0.8f 원 안의 좌표를 더해준다. 
        //해당 값으로 초반에 이동 후 coinUI로 이동 할 것 ->Generator.cs에서 coin을 여러 개 생성시 각각 반경0.8f로 터지는 효과를 주기 위함
        Vector2 vecExplo = (Vector2)transform.position + vecRandCirPos;

        //시퀀스 생성
        Sequence coinSequence = DOTween.Sequence()
            .Append(transform.DOMove(vecExplo, m_fExploTime)).SetEase(m_easeExplosion)
            .Append(transform.DOMove(vecTargetPos,m_fTargetTime)).SetEase(m_easeTarget)
            .OnComplete(() => { Destroy(gameObject); });

        /*
        * Sequence는 Tween들을 순서에 맞게 배열하여 차례차례 원하는 효과를 연출할 수 있다.
        * DOTween.Sequence()를 return해 준다.
        * .Append는 시퀀스에 Tween을 추가해 준다. -> 마지막에 추가(Start()에서 처럼 시퀀스토 추가가능하다.)
        * transform.DoMove()로 원하는 Position에 duration(초)동안 이동
        */
    }

}
