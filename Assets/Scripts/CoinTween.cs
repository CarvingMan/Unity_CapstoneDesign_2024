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


    //coin�� �̵��� Target
    [SerializeField]
    RectTransform m_trTarget; //coin�� �̵��� UI�� RectTransform
    Vector2 m_vecTargetPos = Vector2.zero; //��ũ�� ��ǥ���� World��ǥ�� ��ȯ

    //coin �̵� �ð�
    //float m_fTotalTime = 0f


    // Start is called before the first frame update
    void Start()
    {
        if(m_trTarget != null)
        {
            //CoinUI RectTransform��ǥ�� world��ǥ�� �����Ͽ� ����
            m_vecTargetPos = Camera.main.ScreenToWorldPoint(m_trTarget.position);
        }
        else
        {
            Debug.LogError("m_trTarget�� �����ϴ�.");
        }


        //������ ����
        Sequence coinSequence = DOTween.Sequence();
        //coinSequence�� �Ʒ� CoinSequence()���� ������ �������� Append(�ڿ� �߰�)���ش�.
        coinSequence.Append(CoinSequence());
    }

    Sequence CoinSequence()
    {
        
        //insideUnitCircle�� �ݰ� 1�� �� �ȿ��� �ϳ��� ��ǥ�� �������� �����´�. 0.8f�� ���Ͽ� �ݰ��� 0.8�� ����
        Vector2 vecRandCirPos = Random.insideUnitCircle * 0.8f;
        //transform.posision���� �������� ���� �ݰ� 0.8f �� ���� ��ǥ�� �����ش�. 
        //�ش� ������ �ʹݿ� �̵� �� coinUI�� �̵� �� �� ->Generator.cs���� coin�� ���� �� ������ ���� �ݰ�0.8f�� ������ ȿ���� �ֱ� ����
        Vector2 vecExplo = (Vector2)transform.position + vecRandCirPos;

        return DOTween.Sequence()
            .Append(transform.DOMove(vecExplo, 0.3f)).SetEase(m_easeExplosion)
            .Append(transform.DOMove(m_vecTargetPos, 0.5f)).SetEase(m_easeTarget)
            .OnComplete(SqCallBack); //�Ϸ�� �� SqCallBack()�Լ� ȣ��
       
        /*
         * Sequence�� Tween���� ������ �°� �迭�Ͽ� �������� ���ϴ� ȿ���� ������ �� �ִ�.
         * DOTween.Sequence()�� return�� �ش�.
         * .Append�� �������� Tween�� �߰��� �ش�. -> �������� �߰�(Start()���� ó�� �������� �߰������ϴ�.)
         * DoMove()�� ���ϴ� Position�� duration(��)���� �̵�
         */
    }

    //������ �ݹ� �Լ�
    void SqCallBack()
    {
        //������Ʈ ����
        Destroy(gameObject);
    }
}
