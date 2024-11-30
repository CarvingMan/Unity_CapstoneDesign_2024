using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;

public class BackendManager : Singleton<BackendManager>
{

    // Start is called before the first frame update
    void Start()
    {
        var bro = Backend.Initialize(); //�ڳ� �ʱ�ȭ var ��� BackendReturnObject�ε� ����

        //�ڳ� �ʱ�ȭ�� ���� ���䰪
        if (bro.IsSuccess())
        {
            Debug.Log("�ʱ�ȭ ���� : " + bro); //������ ��� statusCode 204 Success
        }
        else
        {
            Debug.LogError("�ʱ�ȭ ���� : " + bro); //������ ��� statusCode 400�� ���� �߻�
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
