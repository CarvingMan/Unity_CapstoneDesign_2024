using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Generator 
{
    /*
     * Generate����
     */

    // ������
    GameObject[] m_objFieldMobPrefabs = null; //�ʵ���� ������ �迭
    GameObject m_objFieldBossPrefab = null; // �ʵ庸�� ������ ������Ʈ

    GameObject m_objCoinPrefab = null; //Coin ������(Tweening)

    TextMeshProUGUI m_TMPDamagePrefab = null; //DamageText ������(Tweening)

    GameObject m_objEnemyHpBarPrefab = null; //EnemyHpBar panel UI������

    //�ʱ�ȭ �Լ�
    public void Init()
    {
        //resources�� ���丮 �� �����յ��� resources.load�� �����´�.
        if(m_objFieldMobPrefabs == null)
        {
            m_objFieldMobPrefabs = Resources.LoadAll<GameObject>("Prefab/FieldMob");
        }

        if(m_objFieldBossPrefab == null)
        {
            m_objFieldBossPrefab = Resources.Load<GameObject>("Prefab/FieldBoss/FieldBoss");

        }

        if(m_objCoinPrefab == null)
        {
            m_objCoinPrefab = Resources.Load<GameObject>("Prefab/Coin/Coin");
        }

        if(m_TMPDamagePrefab == null)
        {
            m_TMPDamagePrefab = Resources.Load<TextMeshProUGUI>("Prefab/DamageText/DamageText");
        }

        if (m_objEnemyHpBarPrefab == null) 
        {
            m_objEnemyHpBarPrefab = Resources.Load<GameObject>("Prefab/UI/EnemyHpBar");
        }
    }



    //Field ���� �����ϴ� �Լ�, �Ű����� : ������ ���� �ʵ庸�� Ȯ�� ����, �θ�(TileMapGround)tranform,
    //Y�� worldposition(�÷��̾�� ���� ��ġ�� ���� �� ���̰� �Ȱ����� ��������Ʈ pivot�� ������ �ξ���.) 
    //���� ���� ĵ������ �Ѱܹ޾� Enemy HpBar�������� ���� ����
    public void GenerateFieldMob(bool isFieldBoss, Transform trParent, float fMobWorldPosY, Canvas canvas)
    {
        GameObject objFieldMob = null;

        if (isFieldBoss)
        {
            //�ʵ� ���� ���� 
            objFieldMob = Object.Instantiate(m_objFieldBossPrefab, Vector2.zero, Quaternion.identity);
            //trParent�� �θ� ���� -> ��� ������ �ٷ� �� �൵ ������ (0,0)���� �ʱ�ȭ �� 
            //           trParent�� �ڽ����� ���� worldPositionStays�� false�Ͽ� ���÷�(0,0)�̵ȴ�.
            objFieldMob.transform.SetParent(trParent, false);
            //���� FiledMob�� Y�� ���� ��ǥ��  fMobWorldPosY�� ����(ȣ��� �� �÷��̾� y�� ��ǥ�� �޴´�.)
            Vector2 vecNewPos = objFieldMob.transform.position;
            vecNewPos.y = fMobWorldPosY;
            objFieldMob.transform.position = vecNewPos;
        }
        else
        {
            //���� �ʵ� ������ �ƴ϶�� �⺻FieldMob�� �� �������� ����
            //������ �ε����� 0���� MobPrefab �迭ũ�� -1 ��ŭ �������� ���� 
            int nMobIndex = Random.Range(0, m_objFieldMobPrefabs.Length);

            //���� ���� �ε����� �ʵ�� ����
            objFieldMob = Object.Instantiate(m_objFieldMobPrefabs[nMobIndex], Vector2.zero, Quaternion.identity);
            objFieldMob.transform.SetParent(trParent,false);
            Vector2 vecNewPos = objFieldMob.transform.position;
            vecNewPos.y = fMobWorldPosY;
            objFieldMob.transform.position= vecNewPos;
        }

        //HpBar������ ����
        GameObject objHpBar = Object.Instantiate(m_objEnemyHpBarPrefab,canvas.transform);
        //FieldMob���� ������ HpBar�� �Ѱ��ش�.
        objFieldMob.GetComponent<FieldMobControl>().SetHpBar(objHpBar);
    }

    //�Ű������� �Ѱ��� nCoin�� ����ŭ vecGenPos�� coin������ ���� �� �ش� coin�� recTarget���� Tween�̵�
    public void GenerateCoin(Vector2 vecGenPos, RectTransform recTarget, int nCoin)
    {
        
        for (int i=1; i<=nCoin; i++)
        {
            //������ �ν��Ͻ�ȭ
            GameObject objCoin = Object.Instantiate(m_objCoinPrefab, vecGenPos, Quaternion.identity);
            //������ Coin�� CoinMove(Transform recTarget)�� ���Ͽ� Dotween ������ ����
            //recTarget.position�� CoinMove()���� world Position���� ��ȯ�Ͽ� �̵�����.
            objCoin.GetComponent<CoinTween>().CoinMove(recTarget);
        }
    }

    //Tweenig�ϴ� DamageText ���� �Լ�
    public void GenerateDamageText(Canvas canvas, Vector2 vecHead, float fDamage, float fAttackSpeed, bool isCritical)
    {
        //�Ű����ڷ� �Ѱܹ��� ĵ������ �ڽ����� ����
        TextMeshProUGUI damageText = Object.Instantiate(m_TMPDamagePrefab, canvas.transform);
        //Tweening ����
        damageText.GetComponent<DamageText>().SetDamageText(vecHead,fDamage, fAttackSpeed, isCritical);
        
    }
}
