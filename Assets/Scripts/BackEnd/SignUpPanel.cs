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
    [Tooltip("닉네임InputField")]
    [SerializeField] TMP_InputField m_inputNickName;

    [Tooltip("아이디InputField")]
    [SerializeField] TMP_InputField m_inputID;

    [Tooltip("비번InputField")]
    [SerializeField] TMP_InputField m_inputPW;

    [Tooltip("비번확인InptField")]
    [SerializeField] TMP_InputField m_inputCheckPW;

    //회원가입 버튼
    [SerializeField]
    Button m_btnJoin;

    //결과 메세지
    [SerializeField]
    TextMeshProUGUI m_tmpResult;

    private void Awake()
    {
        if (m_inputNickName == null)
        {
            Debug.LogError("m_inputNickName이 없습니다.");
        }

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

        if(m_tmpResult == null)
        {
            Debug.LogError("m_tmpResult가 없습니다.");
        }

        if(m_btnJoin == null)
        {
            Debug.LogError("m_btnJoin 이 없습니다.");
        }
    }

    void Start()
    {
        if(gameObject.activeSelf == true)
        {
            gameObject.SetActive(false);
        }       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
