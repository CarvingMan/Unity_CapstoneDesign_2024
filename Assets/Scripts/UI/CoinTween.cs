using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinTween : MonoBehaviour
{
    /*
     * Dotween �� ����Ͽ� Generator.cs���� ���� FieldMob��ġ���� ������ �ش� coin�������� coin UI�� �̵��ϰ� ��������� ����
     */

    //GameManager�� ���� Stage, FieldBoss������ ���� ������ ����ġ�� ����
    //int m_nValue = 0;

    //Ease�� DoTween���� �پ��� �ð��� ��ȭ�� �׷����� �ִµ� �ش� Ease�� Ʈ���ʿ� SetEase()�� ���Ͽ� ������ �� �� �ִ�.
    //�پ��� �׷����� �ֱ⿡ �ν�����â���� �����ϰ� �׽�Ʈ�غ��� �����Ͽ���.
    //����(�ð��� ��ȭ��) : Sine < Quad < Cubic < Quart < Quint < Expo / Out : �ʹݿ� ��ȭ���� ŭ, In: �Ĺݿ� ��ȭ���� ŭ
    //Generator���� �������� ������ �����ɶ� �� �������� �� coin���� ����ٸ� ��ġ(�ݰ�����)�� m_easeExplosion������ �̵��ϰ�,
    //coinUI�� WorldPositioin�� m_easeTarget������ �̵�
    [SerializeField]
    Ease m_easeExplosion; //������ ����� �ֺ����� ������ �ð��� ��ȭ�� -> InCubic���� ����
    [SerializeField]
    Ease m_easeTarget; //CoinUI�� �̵��Ҷ� �ð��� ��ȭ�� -> OutCubic���� ����


    //coin �̵� �ð�
    float m_fExploTime = 0.0f; //ó�� ������ ���� �ֺ����� ������(Random.insideUnitCircle * 0.8f)��ǥ���� �̵��ð�
    float m_fTargetTime = 0.0f; //Target���� �̵��ð�

    // Uniti Doc�� ���� Awake �Լ��� �׻� Start �Լ� ���� ȣ��Ǹ� �������� �ν��Ͻ�ȭ �� ���Ŀ� ȣ��˴ϴ�.
    // ���� ������Ʈ�� �����ϴ� ���� ��Ȱ�� ������ ��� Awake �Լ��� Ȱ��ȭ�� ������ ȣ����� �ʽ��ϴ�.
    // �� ó�� �����ִ�. ���� Generator���� �ش� Coin�������� instantiate()�ϰ� �ٷ� CoinMove()�� ȣ���ϱ⿡
    // Awake()���� �Ҵ� ��Time���� CoinMove()���� �״�� ����ϰ� �ȴ�. 
    // ���� Start���� �Ʒ� Time���� �Ҵ� ���ָ�, Generator.cs ���� ������ �ν��Ͻ�ȭ �ϰ� �������� ������ ���� CoinMove�� ȣ���ϱ⿡
    // Time���� ��� 0�״�� ���ȴ�. ���� Awake()���� �ش� �ʵ带 �ʱ�ȭ �� �־�� �Ѵ�.
    private void Awake()
    {
        //�� Ʈ������ �̵��ϴ� �ð� GameManager�� m_fCoinMoveTime�� �̼� m_fMoveSpeed�� ���Ͽ� ���
        float fTotalMoveTime = GameManager.Instance.CoinMoveTime / GameManager.Instance.MoveSpeed; 
        m_fExploTime = (fTotalMoveTime / 10) * 3.5f; //�� Ʈ���ð����� 35% ���
        m_fTargetTime = fTotalMoveTime - m_fExploTime; //�� Ʈ���ð����� m_fExploTime ���� �� �ð�(65%)
        //Debug.Log("�ѽð� : " + fTotalMoveTime + "\n���߽ð�: " + m_fExploTime + "Targe�̵��ð�: " + m_fTargetTime);s
    }

    //Tween Sequence ���� �Լ� -> �ش� �Լ��� Generater.cs���� ������ ���ÿ� ȣ���Ͽ� coin�� �Ű����� RectTransform�� ������ǥ�� �̵�
    public void CoinMove(RectTransform recTrarget)
    {
        //(����)�Ű����ڷ� ���� recTarget : CoinUI RectTransform��ǥ�� world��ǥ�� �����Ͽ� ����
        //(����)Vector2 vecTargetPos = Camera.main.ScreenToWorldPoint(recTrarget.position);

        // �� �ּ� �ڵ忡�� �Ʒ��� ���� : CameraResolution.cs�� �Բ�
        // Canvas Render mode�� screen space- camera ���� Main camera�� �����Ͽ� ���� �� UI��ǥ�� ���� ��ǥ�� ����
        Vector2 vecTargetPos = recTrarget.position;

        //insideUnitCircle�� �ݰ� 1�� �� �ȿ��� �ϳ��� ��ǥ�� �������� �����´�. 0.8f�� ���Ͽ� �ݰ��� 0.8�� ����
        Vector2 vecRandCirPos = Random.insideUnitCircle * 0.8f;
        //transform.posision���� �������� ���� �ݰ� 0.8f �� ���� ��ǥ�� �����ش�. 
        //�ش� ������ �ʹݿ� �̵� �� coinUI�� �̵� �� �� ->Generator.cs���� coin�� ���� �� ������ ���� �ݰ�0.8f�� ������ ȿ���� �ֱ� ����
        Vector2 vecExplo = (Vector2)transform.position + vecRandCirPos;

        //������ ����
        Sequence coinSequence = DOTween.Sequence()
            .Append(transform.DOMove(vecExplo, m_fExploTime)).SetEase(m_easeExplosion)
            .Append(transform.DOMove(vecTargetPos,m_fTargetTime)).SetEase(m_easeTarget)
            .OnComplete(() => { Destroy(gameObject); });

        /*
        * Sequence�� Tween���� ������ �°� �迭�Ͽ� �������� ���ϴ� ȿ���� ������ �� �ִ�.
        * DOTween.Sequence()�� return�� �ش�.
        * .Append�� �������� Tween�� �߰��� �ش�. -> �������� �߰�(Start()���� ó�� �������� �߰������ϴ�.)
        * transform.DoMove()�� ���ϴ� Position�� duration(��)���� �̵�
        */
    }

}
