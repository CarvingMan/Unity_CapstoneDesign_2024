using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator 
{
    /*
     * Generate관련
     */


    //Field 몹을 생성하는 함수, 매개변수 : 생성될 몹이 필드보스 확인 여부, 부모(TileMapGround)tranform,
    //Y축 worldposition(플레이어와 같은 위치로 했을 때 높이가 똑같도록 스프라이트 pivot을 설정해 두었다.) 
    public void GenerateFieldMob(bool isFieldBoss, Transform trParent, float fMobWorldPosY)
    {
        GameObject objMobPrefab = null;
        if (isFieldBoss)
        {
            objMobPrefab = Resources.Load<GameObject>("");
        }

    }
}
