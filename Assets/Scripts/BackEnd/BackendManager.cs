using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;

public class BackendManager : Singleton<BackendManager>
{

    // Start is called before the first frame update
    void Start()
    {
        var bro = Backend.Initialize(); //뒤끝 초기화 var 대신 BackendReturnObject로도 가능

        //뒤끝 초기화에 대한 응답값
        if (bro.IsSuccess())
        {
            Debug.Log("초기화 성공 : " + bro); //성공일 경우 statusCode 204 Success
        }
        else
        {
            Debug.LogError("초기화 실패 : " + bro); //실패일 경우 statusCode 400대 에러 발생
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
