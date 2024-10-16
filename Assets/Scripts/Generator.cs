using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator 
{
    /*
     * Generate관련
     */

    GameObject[] m_objFieldMobPrefabs = null;
    GameObject m_objFieldBossPrefab = null;


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
    }



    //Field 몹을 생성하는 함수, 매개변수 : 생성될 몹이 필드보스 확인 여부, 부모(TileMapGround)tranform,
    //Y축 worldposition(플레이어와 같은 위치로 했을 때 높이가 똑같도록 스프라이트 pivot을 설정해 두었다.) 
    public void GenerateFieldMob(bool isFieldBoss, Transform trParent, float fMobWorldPosY)
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

    }
}
