using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator 
{
    /*
     * Generate����
     */


    //Field ���� �����ϴ� �Լ�, �Ű����� : ������ ���� �ʵ庸�� Ȯ�� ����, �θ�(TileMapGround)tranform,
    //Y�� worldposition(�÷��̾�� ���� ��ġ�� ���� �� ���̰� �Ȱ����� ��������Ʈ pivot�� ������ �ξ���.) 
    public void GenerateFieldMob(bool isFieldBoss, Transform trParent, float fMobWorldPosY)
    {
        GameObject objMobPrefab = null;
        if (isFieldBoss)
        {
            objMobPrefab = Resources.Load<GameObject>("");
        }

    }
}
