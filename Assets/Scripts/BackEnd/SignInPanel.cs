using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SignInPanel : MonoBehaviour
{
    /*
     SignInPanel(로그인) 에 부착할 스크립트
     각 InputField의 text를 입력후 로그인 버튼을 누르면, BackendManager의 SignIn()함수를 호출
        로그인 성공 시 로딩씬으로 이동
     */

    //ID, PW InputField
    [Tooltip("아이이 Field")]
    [SerializeField] TMP_InputField m_inputID = null;

    [Tooltip("Password Field")]
    [SerializeField] TMP_InputField m_inputPW = null;

    //로그인 하기 버튼
    [SerializeField]
    Button m_btnLogIn = null;

    //닫기 버튼
    [SerializeField]
    Button m_btnClose = null;

    //결과 출력 텍스트
    [Tooltip("결과출력TMP")]
    [SerializeField] TextMeshProUGUI m_tmpResult = null;

    // Start is called before the first frame update
    void Start()
    {
        if (m_inputID == null)
        {
            Debug.LogError("m_inputID 가 없습니다." + gameObject.name);
        }

        if (m_inputPW == null)
        {
            Debug.LogError("m_inputPW가 없습니다." + gameObject.name);
        }

        if(m_tmpResult == null)
        {
            Debug.LogError("m_tmpResult가 업습니다." + gameObject.name);
        }

        if(m_btnClose == null)
        {
            Debug.LogError("m_btnClose가 없습니다."+ gameObject.name);
        }
        else
        {
            m_btnClose.onClick.AddListener(ClickCloseBtn);
        }

        if(m_btnLogIn == null)
        {
            Debug.LogError("m_btnLogIn이 없습니다." + gameObject.name);
        }
        else
        {
            m_btnLogIn.onClick.AddListener(ClickLogInBtn);
        }

        if(gameObject.activeSelf == true)
        {
            gameObject.SetActive(false);
        }
    }

    //닫기 버튼(m_btnClose)의 onClick에 AddListener할 함수
    void ClickCloseBtn()
    {
        //inputField 의 text 초기화 후 Active false
        m_inputID.text = null;
        m_inputPW.text = null;
        m_tmpResult.text = null;
        gameObject.SetActive(false);
    }

    //로그인 버틑 (m_btnLogIn) onClick에 AddListener할 함수
    void ClickLogInBtn()
    {
        string strID = m_inputID.text;
        string strPW = m_inputPW.text;
        
        if(strID.Length != 0 && strPW.Length != 0)
        {
            //InputField의 text가 존재할 경우
            string strMessage = null;
            bool isLogIn = BackendManager.Instance.SignIn(strID, strPW, out strMessage);
            m_tmpResult.text = strMessage;
            if (isLogIn)
            {
                m_btnLogIn.interactable = false;
                m_btnClose.interactable = false;
                //로그인 성공 시 로딩 씬 이동
                GameManager.Instance.LoadSceneWithTime("LoadingScene", 0.5f);
            }
        }
        else
        {
            m_tmpResult.text = "입력하지 않은 정보가 있습니다.";
        }
    }

}
