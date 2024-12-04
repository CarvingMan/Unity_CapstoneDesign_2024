using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SignInPanel : MonoBehaviour
{
    /*
     SignInPanel(�α���) �� ������ ��ũ��Ʈ
     �� InputField�� text�� �Է��� �α��� ��ư�� ������, BackendManager�� SignIn()�Լ��� ȣ��
        �α��� ���� �� �ε������� �̵�
     */

    //ID, PW InputField
    [Tooltip("������ Field")]
    [SerializeField] TMP_InputField m_inputID = null;

    [Tooltip("Password Field")]
    [SerializeField] TMP_InputField m_inputPW = null;

    //�α��� �ϱ� ��ư
    [SerializeField]
    Button m_btnLogIn = null;

    //�ݱ� ��ư
    [SerializeField]
    Button m_btnClose = null;

    //��� ��� �ؽ�Ʈ
    [Tooltip("������TMP")]
    [SerializeField] TextMeshProUGUI m_tmpResult = null;

    // Start is called before the first frame update
    void Start()
    {
        if (m_inputID == null)
        {
            Debug.LogError("m_inputID �� �����ϴ�." + gameObject.name);
        }

        if (m_inputPW == null)
        {
            Debug.LogError("m_inputPW�� �����ϴ�." + gameObject.name);
        }

        if(m_tmpResult == null)
        {
            Debug.LogError("m_tmpResult�� �����ϴ�." + gameObject.name);
        }

        if(m_btnClose == null)
        {
            Debug.LogError("m_btnClose�� �����ϴ�."+ gameObject.name);
        }
        else
        {
            m_btnClose.onClick.AddListener(ClickCloseBtn);
        }

        if(m_btnLogIn == null)
        {
            Debug.LogError("m_btnLogIn�� �����ϴ�." + gameObject.name);
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

    //�ݱ� ��ư(m_btnClose)�� onClick�� AddListener�� �Լ�
    void ClickCloseBtn()
    {
        //inputField �� text �ʱ�ȭ �� Active false
        m_inputID.text = null;
        m_inputPW.text = null;
        m_tmpResult.text = null;
        gameObject.SetActive(false);
    }

    //�α��� ���z (m_btnLogIn) onClick�� AddListener�� �Լ�
    void ClickLogInBtn()
    {
        string strID = m_inputID.text;
        string strPW = m_inputPW.text;
        
        if(strID.Length != 0 && strPW.Length != 0)
        {
            //InputField�� text�� ������ ���
            string strMessage = null;
            bool isLogIn = BackendManager.Instance.SignIn(strID, strPW, out strMessage);
            m_tmpResult.text = strMessage;
            if (isLogIn)
            {
                m_btnLogIn.interactable = false;
                m_btnClose.interactable = false;
                //�α��� ���� �� �ε� �� �̵�
                GameManager.Instance.LoadSceneWithTime("LoadingScene", 0.5f);
            }
        }
        else
        {
            m_tmpResult.text = "�Է����� ���� ������ �ֽ��ϴ�.";
        }
    }

}
