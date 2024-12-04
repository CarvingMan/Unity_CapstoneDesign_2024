using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using UnityEditor.PackageManager;
using UnityEngine.SceneManagement;

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
            return true;
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
}
