using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SignUpPanel : MonoBehaviour
{
    /*
     *  SignUpPanel(ȸ������)�� ������ ��ũ��Ʈ
     */
    // Start is called before the first frame update

    //InputPanel��//
    [Tooltip("���̵�InputField")]
    [SerializeField] TMP_InputField m_inputID;

    [Tooltip("���InputField")]
    [SerializeField] TMP_InputField m_inputPW;

    [Tooltip("���Ȯ��InptField")]
    [SerializeField] TMP_InputField m_inputCheckPW;

    //ȸ������ ��ư
    [SerializeField]
    Button m_btnJoin = null;

    //panel �ݱ� ��ư
    [SerializeField]
    Button m_btnClose = null;

    //��� �޼���
    [SerializeField]
    TextMeshProUGUI m_tmpResult;

    //ȸ������ ���� �� Active�� �г��� ���� �г�
    [Tooltip("�г��� �г�")]
    [SerializeField] GameObject m_objNickNamePanel = null;

    private void Awake()
    {
        if(m_inputID == null)
        {
            Debug.LogError("m_inputID�� �����ϴ�.");
        }

        if(m_inputPW == null)
        {
            Debug.LogError("m_input_PW�� �����ϴ�.");
        }

        if(m_inputCheckPW == null)
        {
            Debug.LogError("m_input_CheckPW�� �����ϴ�.");
        }

        if (m_btnClose == null) 
        {
            Debug.LogError("m_btnClose�� �����ϴ�." + gameObject.name);
        }

        if(m_tmpResult == null)
        {
            Debug.LogError("m_tmpResult�� �����ϴ�.");
        }

        if(m_btnJoin == null)
        {
            Debug.LogError("m_btnJoin �� �����ϴ�.");
        }

        if(m_objNickNamePanel == null)
        {
            Debug.LogError("m_objNickNamePanel�� �����ϴ�.");
        }
    }

    void Start()
    {
        if(gameObject.activeSelf == true)
        {
            gameObject.SetActive(false);
        }

        //Close��ư onClick() �޼ҵ� �߰�
        if (m_btnClose != null)
        {
            m_btnClose.onClick.AddListener(ClickCloseBtn);
        }

        //Join(ȸ������) ��ư onClick() �޼ҵ� �߰�
        if(m_btnJoin != null)
        {
            m_btnJoin.onClick.AddListener(ClickJoinBtn);
        }
    }

    //m_btnClose(panel �ݱ� ��ư)�� onClick.AddListener()�� ����� �Լ�
    //InputField,tmpResut�� text�� ���� �ʱ�ȭ
    void ClickCloseBtn()
    {
        m_inputID.text = null;
        m_inputPW.text = null;
        m_inputCheckPW.text = null;
        m_tmpResult.text = null;
        gameObject.SetActive(false);
    }

    //m_btnJoin(�����ϱ� ��ư)��onClick.AddListener()�� ����� �Լ�
    //InputField���� text���� null�˻� �� BackendManager�� SignUp()�� ȣ��
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
                //strPW�� strCheckPW�� ������ Ȯ��
                if (strPW.Equals(strCheckPW))
                {
                    //BackendManager�� SignUp()ȸ������ �� ȣ���Ͽ� ȸ������ �õ� ���ÿ� ��� �޼����� �޾� ���
                    bool isSignUp = BackendManager.Instance.SignUp(strID, strPW, out strMessage);
                    m_tmpResult.text = strMessage;
                    if (isSignUp)
                    {
                        //ȸ������ ���� �� 
                        if (m_objNickNamePanel != null) 
                        {
                            m_objNickNamePanel.SetActive(true); //�г��� ���� �� Active
                            gameObject.SetActive(false);//ȸ������ �г��� �� �̻� �ʿ� �����Ƿ� �ݱ�
                        }
                        else
                        {
                            Debug.LogError("m_objNickNamePanel�� �����ϴ�.");
                        }
                    }

                }
                else
                {
                    m_tmpResult.text = "��й�ȣ�� �ٸ��ϴ�.";
                    return;
                }
            }
            else
            {
                m_tmpResult.text = "�Է����� ���� ������ �ֽ��ϴ�.";
                return;
            }
        }
        else
        {
            Debug.LogError("m_btnJoin�� �����ϴ�.");
        }
    }
}
