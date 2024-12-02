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
    [Tooltip("�г���InputField")]
    [SerializeField] TMP_InputField m_inputNickName;

    [Tooltip("���̵�InputField")]
    [SerializeField] TMP_InputField m_inputID;

    [Tooltip("���InputField")]
    [SerializeField] TMP_InputField m_inputPW;

    [Tooltip("���Ȯ��InptField")]
    [SerializeField] TMP_InputField m_inputCheckPW;

    //ȸ������ ��ư
    [SerializeField]
    Button m_btnJoin;

    //��� �޼���
    [SerializeField]
    TextMeshProUGUI m_tmpResult;

    private void Awake()
    {
        if (m_inputNickName == null)
        {
            Debug.LogError("m_inputNickName�� �����ϴ�.");
        }

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

        if(m_tmpResult == null)
        {
            Debug.LogError("m_tmpResult�� �����ϴ�.");
        }

        if(m_btnJoin == null)
        {
            Debug.LogError("m_btnJoin �� �����ϴ�.");
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
