using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SignUpPanel : MonoBehaviour
{
    /*
     *  SignUpPanel(회원가입)에 부착할 스크립트
     */
    // Start is called before the first frame update

    //InputPanel들//
    [Tooltip("아이디InputField")]
    [SerializeField] TMP_InputField m_inputID;

    [Tooltip("비번InputField")]
    [SerializeField] TMP_InputField m_inputPW;

    [Tooltip("비번확인InptField")]
    [SerializeField] TMP_InputField m_inputCheckPW;

    //회원가입 버튼
    [SerializeField]
    Button m_btnJoin = null;

    //panel 닫기 버튼
    [SerializeField]
    Button m_btnClose = null;

    //결과 메세지
    [SerializeField]
    TextMeshProUGUI m_tmpResult;

    //회원가입 성공 시 Active할 닉네임 설정 패널
    [Tooltip("닉네임 패널")]
    [SerializeField] GameObject m_objNickNamePanel = null;

    private void Awake()
    {
        if(m_inputID == null)
        {
            Debug.LogError("m_inputID가 없습니다.");
        }

        if(m_inputPW == null)
        {
            Debug.LogError("m_input_PW가 없습니다.");
        }

        if(m_inputCheckPW == null)
        {
            Debug.LogError("m_input_CheckPW가 없습니다.");
        }

        if (m_btnClose == null) 
        {
            Debug.LogError("m_btnClose가 없습니다." + gameObject.name);
        }

        if(m_tmpResult == null)
        {
            Debug.LogError("m_tmpResult가 없습니다.");
        }

        if(m_btnJoin == null)
        {
            Debug.LogError("m_btnJoin 이 없습니다.");
        }

        if(m_objNickNamePanel == null)
        {
            Debug.LogError("m_objNickNamePanel이 없습니다.");
        }
    }

    void Start()
    {
        if(gameObject.activeSelf == true)
        {
            gameObject.SetActive(false);
        }

        //Close버튼 onClick() 메소드 추가
        if (m_btnClose != null)
        {
            m_btnClose.onClick.AddListener(ClickCloseBtn);
        }

        //Join(회원가입) 버튼 onClick() 메소드 추가
        if(m_btnJoin != null)
        {
            m_btnJoin.onClick.AddListener(ClickJoinBtn);
        }
    }

    //m_btnClose(panel 닫기 버튼)의 onClick.AddListener()에 등록할 함수
    //InputField,tmpResut의 text를 전부 초기화
    void ClickCloseBtn()
    {
        m_inputID.text = null;
        m_inputPW.text = null;
        m_inputCheckPW.text = null;
        m_tmpResult.text = null;
        gameObject.SetActive(false);
    }

    //m_btnJoin(가입하기 버튼)의onClick.AddListener()에 등록할 함수
    //InputField등의 text들을 null검사 후 BackendManager의 SignUp()을 호출
    void ClickJoinBtn()
    {
        if(m_btnJoin != null)
        {
            string strID = m_inputID.text;
            string strPW = m_inputPW.text;
            string strCheckPW = m_inputCheckPW.text;
            string strMessage = m_tmpResult.text;
            if (strID.Length != 0 && strPW.Length != 0 && strCheckPW.Length != 0)
            {
                //strPW와 strCheckPW가 같은지 확인
                if (strPW.Equals(strCheckPW))
                {
                    //BackendManager의 SignUp()회원가입 을 호출하여 회원가입 시도 동시에 결과 메세지를 받아 출력
                    bool isSignUp = BackendManager.Instance.SignUp(strID, strPW, out strMessage);
                    m_tmpResult.text = strMessage;
                    if (isSignUp)
                    {
                        //회원가입 성공 시 
                        if (m_objNickNamePanel != null) 
                        {
                            m_objNickNamePanel.SetActive(true); //닉네임 설정 찰 Active
                            gameObject.SetActive(false);//회원가입 패널은 더 이상 필요 없으므로 닫기
                        }
                        else
                        {
                            Debug.LogError("m_objNickNamePanel이 없습니다.");
                        }
                    }

                }
                else
                {
                    m_tmpResult.text = "비밀번호가 다릅니다.";
                    return;
                }
            }
            else
            {
                m_tmpResult.text = "입력하지 않은 정보가 있습니다.";
                return;
            }
        }
        else
        {
            Debug.LogError("m_btnJoin이 없습니다.");
        }
    }
}
