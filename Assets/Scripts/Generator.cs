using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Generator 
{
    /*
     * Generate관련
     */

    // 프리팹
    GameObject[] m_objFieldMobPrefabs = null; //필드몬스터 프리팹 배열
    GameObject m_objFieldBossPrefab = null; // 필드보스 프리팹 오브젝트

    GameObject m_objCoinPrefab = null; //Coin 프리팹(Tweening)

    TextMeshProUGUI m_TMPDamagePrefab = null; //DamageText 프리팹(Tweening)

    GameObject m_objEnemyHpBarPrefab = null; //EnemyHpBar panel UI프리팹

    //초기화 함수
    public void Init()
    {
        //resources속 디렉토리 각 프리팹들을 resources.load로 가져온다.
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



    //Field 몹을 생성하는 함수, 매개변수 : 생성될 몹이 필드보스 확인 여부, 부모(TileMapGround)tranform,
    //Y축 worldposition(플레이어와 같은 위치로 했을 때 높이가 똑같도록 스프라이트 pivot을 설정해 두었다.) 
    //현제 메인 캔버스를 넘겨받아 Enemy HpBar프리팹을 같이 생성
    public void GenerateFieldMob(bool isFieldBoss, Transform trParent, float fMobWorldPosY, Canvas canvas)
    {
        GameObject objFieldMob = null;

        if (isFieldBoss)
        {
            //필드 보스 생성 
            objFieldMob = Object.Instantiate(m_objFieldBossPrefab, Vector2.zero, Quaternion.identity);
            //trParent로 부모 설정 -> 사실 위에서 바로 해 줘도 되지만 (0,0)으로 초기화 후 
            //           trParent의 자식으로 들어가며 worldPositionStays를 false하여 로컬로(0,0)이된다.
            objFieldMob.transform.SetParent(trParent, false);
            //추후 FiledMob의 Y축 월드 좌표를  fMobWorldPosY로 설정(호출될 때 플레이어 y축 좌표를 받는다.)
            Vector2 vecNewPos = objFieldMob.transform.position;
            vecNewPos.y = fMobWorldPosY;
            objFieldMob.transform.position = vecNewPos;
        }
        else
        {
            //만약 필드 보스가 아니라면 기본FieldMob들 중 랜덤으로 생성
            //생성할 인덱스를 0부터 MobPrefab 배열크기 -1 만큼 랜덤으로 생성 
            int nMobIndex = Random.Range(0, m_objFieldMobPrefabs.Length);

            //뽑힌 랜덤 인덱스의 필드몹 생성
            objFieldMob = Object.Instantiate(m_objFieldMobPrefabs[nMobIndex], Vector2.zero, Quaternion.identity);
            objFieldMob.transform.SetParent(trParent,false);
            Vector2 vecNewPos = objFieldMob.transform.position;
            vecNewPos.y = fMobWorldPosY;
            objFieldMob.transform.position= vecNewPos;
        }

        //HpBar프리팹 생성
        GameObject objHpBar = Object.Instantiate(m_objEnemyHpBarPrefab,canvas.transform);
        //FieldMob에게 생성된 HpBar를 넘겨준다.
        objFieldMob.GetComponent<FieldMobControl>().SetHpBar(objHpBar);
    }

    //매개변수로 넘겨준 nCoin의 수만큼 vecGenPos에 coin프리팹 생성 수 해당 coin을 recTarget으로 Tween이동
    public void GenerateCoin(Vector2 vecGenPos, RectTransform recTarget, int nCoin)
    {
        
        for (int i=1; i<=nCoin; i++)
        {
            //프리팹 인스턴스화
            GameObject objCoin = Object.Instantiate(m_objCoinPrefab, vecGenPos, Quaternion.identity);
            //생성된 Coin의 CoinMove(Transform recTarget)을 통하여 Dotween 시퀀스 생성
            //recTarget.position은 CoinMove()에서 world Position으로 변환하여 이동힌다.
            objCoin.GetComponent<CoinTween>().CoinMove(recTarget);
        }
    }

    //Tweenig하는 DamageText 생성 함수
    public void GenerateDamageText(Canvas canvas, Vector2 vecHead, float fDamage, float fAttackSpeed, bool isCritical)
    {
        //매개인자로 넘겨받은 캔버스의 자식으로 생성
        TextMeshProUGUI damageText = Object.Instantiate(m_TMPDamagePrefab, canvas.transform);
        //Tweening 시작
        damageText.GetComponent<DamageText>().SetDamageText(vecHead,fDamage, fAttackSpeed, isCritical);
        
    }
}
