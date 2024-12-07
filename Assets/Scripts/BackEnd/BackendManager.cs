using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using UnityEditor.PackageManager;
using UnityEngine.SceneManagement;
using System;

public class BackendManager : Singleton<BackendManager>
{
    /*
     * 뒤끝 SDK 사용하여 Backend 관리
     */


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
            Application.Quit();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }



    //로그인 성공여부와 함께 상태 메세지 out
    public bool SignIn(string strUserID, string strPassword, out string strMessage)
    {
        BackendReturnObject bro = Backend.BMember.CustomLogin(strUserID, strPassword);
        if (bro.IsSuccess())
        {
            strMessage = Backend.UserNickName + "님 환영합니다.";
            return true;
        }
        else
        {
            if (bro.StatusCode == 400)
            {
                strMessage = "디바이스 정보 null";
            }
            else if(bro.StatusCode == 401)
            {
                //401 상태는 아이디or비번 이 잘못되었을 때, 서버 점검중일 때 다 포함이기에 bro.Message를 전달
                strMessage = bro.Message;
            }
            else if(bro.StatusCode == 403)
            {
                strMessage = "차단당한 디바이스 입니다.";
            }
            else
            {
                strMessage = "로그인 오류";
                Application.Quit();
            }
            return false;
        }
    }


    //회원가입 성공여부와 함께 상태 메세지 out
    //뒤끝 서버에서 회원가입 성공 시 로그인또한 자동으로 진행된다고 한다.
    public bool SignUp(string strUserID, string strPassword, out string strMessage)
    {
        //커스텀 회원가입 진행
        BackendReturnObject broSignUp = Backend.BMember.CustomSignUp(strUserID, strPassword);

        if (broSignUp.IsSuccess())
        {
            strMessage = "회원가입 성공!";

            //회원 가입 성공시 기본 데이터 삽입(스키마 정의로 기본값 설정 해 두었기에 Param필요 x)
            var bro = Backend.GameData.Insert("USER_DATA");
            if (bro.IsSuccess())
            {

                return true;
            }
            else
            {
                Debug.LogError("기본 USER_DATA row 삽입 오류");
                strMessage ="기본 USER_DATA row 삽입 오류";
                return false;
            }

        }
        else
        {
            //회원가입 실패 시
            if (broSignUp.StatusCode == 400) //디바이스 정보 없음
            {
                strMessage = "회원가입 실패 디바이스 정보 null";
            }
            else if (broSignUp.StatusCode == 401) //서버 점검중
            {
                strMessage = "서버 점검 중 입니다. 불편을 끼쳐드려 죄송합니다.";
            }
            else if (broSignUp.StatusCode == 403)
            {
                strMessage = "해당 디바이스는 차단되었습니다.";
            }
            else if (broSignUp.StatusCode == 409)
            {
                strMessage = "해당 ID는 이미 존재하는 ID입니다.";
            }
            else //추가 적인 오류
            {
                strMessage = "회원가입 실패";
                Debug.Log(broSignUp.Message);
            }
            return false;
        }
    }

    //bool 형태로 true시 즉시 로딩씬으로 넘어갈 예정이기에
    //out으로 strMessage를 전달
    public bool UpdateNickName(string strNickName, out string strMessage)
    {
        //닉네임 중복 확인
        BackendReturnObject broCheckName = Backend.BMember.CheckNicknameDuplication(strNickName);
        if (broCheckName.IsSuccess())
        {
            //회원가입 성공시 닉네임 업데이트
            BackendReturnObject broNickName = Backend.BMember.CreateNickname(strNickName);
            if (broNickName.IsSuccess())
            {
                //GameScene으로 이동
                strMessage = Backend.UserNickName+"님 환영합니다!";
                Param param = new Param();
                param.Add("NickName", Backend.UserNickName);
                BackendReturnObject bro = Backend.GameData.Update("USER_DATA",new Where(), param);
                if (bro.IsSuccess()) 
                {
                    Debug.Log(bro);
                }
                return true;
                
            }
            {
                // 해당 로직은 맨 처음 broCheckName에서 중복 및 공백 등 생성 가능 여부를 확인후 들어오기에
                // 정상적으로 동작할 가능성이 높지만, 혹시 모를 오류에 대비
                strMessage = "닉네임 등록 실패";
                return false;
            }

        }
        else //닉네임 중복체크 중 오류가 날 시
        {
            if (broCheckName.StatusCode == 400)
            {
                strMessage = "닉네임은 앞뒤 공백없이 20자 내로 설정해주세요";
                //Debug.Log(broCheckName.Message);
            }
            else if (broCheckName.StatusCode == 409)
            {
                strMessage = "이미 존재하는 닉네임입니다.";
            }
            else
            {
                strMessage = "닉네임 설정 오류";
            }
            return false;
        }
    }

    //유저 데이터 불러오기(비동기) -> LoadingScene.cs에서 호출
    public void LoadUserData()
    {
        Backend.GameData.GetMyData("USER_DATA", new Where(), (bro) =>
        {
            if (bro.IsSuccess())
            {
                Debug.Log("데이터 조회 성공" + bro);
                //json으로 리턴된 데이터를 받아온다. 
                //FlattenRow()는 언마샬(데이터 타입제외 순수 데이터값 만 가져온다.)
                LitJson.JsonData gameDataJason = bro.FlattenRows();
                if(gameDataJason.Count <= 0)
                {
                    Debug.LogError("USER_DATA 없음");
                    return;
                }
                else
                {
                    //받아온 json데이터를 GameManager에 넘겨 세팅한다.
                    //추후 GameManager에서 값 할당이 완료되면 LoadingScene에서 GameScene으로 넘어간다.
                    GameManager.Instance.SetUserData(gameDataJason);
                    return;
                }
                //for(int i=0; i < gameDataJason.Count; i++)
                //{
                //    Debug.Log(gameDataJason[i]["NickName"]);
                //    Debug.Log(gameDataJason[i]["AttackDamage"]);
                //    Debug.Log(gameDataJason[i]["AttackDamagePrice"]);
                //    Debug.Log(gameDataJason[i]["updatedAt"]);

                    
                //    string updatedAtStr = gameDataJason[i]["updatedAt"].ToString();

                //    //ISO 8601 형식의 UTC 이기에 로컬타임으로 변경
                //    DateTime data = DateTime.Parse(gameDataJason[i]["updatedAt"].ToString()).ToLocalTime();
                //    Debug.Log(data.ToString());
                //}
            }
            else
            {
                Debug.LogError("데이터 조회 실패" + bro);
                Application.Quit();
            }
        });
    }
}
