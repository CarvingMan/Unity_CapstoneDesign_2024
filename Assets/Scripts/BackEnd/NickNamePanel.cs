using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NickNamePanel : MonoBehaviour
{
    /*
     * �ڳ� ȸ������ �� �α��� ���� �ڵ����� ����Ǳ⿡ 
     * ȸ������ �� SignUpPanel.cs���� �ش� �г��� Active�Ͽ� �г��� ����
     * ������ �ε������� �̵�
     * �ش� NickNamePanel�� �ʼ������� �г����� �����ؾ��ϱ⿡ �ݱ� ��ư�� ����.
     */

    [Tooltip("�г���InputField(TMP)")]
    [SerializeField] TMP_InputField m_inputNickName = null;

    //�г��� ���� ��ư
    [SerializeField]
    Button m_btnNickName = null;

    //��� �޼���
    [SerializeField]
    TextMeshProUGUI m_tmpResult = null;

    // Start is called before the first frame update
    void Start()
    {
        if(m_inputNickName == null)
        {
            Debug.LogError("m_inputNickName�� �����ϴ�.");
        }

        if(m_tmpResult == null)
        {
            Debug.LogError("m_tmpResult�� �����ϴ�." + gameObject.name);
        }

        if (gameObject.activeSelf == true)
        {
            gameObject.SetActive(false);
        }

        if (m_btnNickName == null)
        {
            Debug.LogError("m_btnNickName�� �����ϴ�.");
        }
        else
        {
            m_btnNickName.onClick.AddListener(ClickNickNameBtn);
        }
    }

    void ClickNickNameBtn()
    {
        if(m_inputNickName.text.Length != 0)
        {
            string strMessage = m_tmpResult.text;
            //�г��� ���� ���� ���ο� ���ÿ� ��� �޼����� outŰ���带 ���� �޾ƿ´�.
            bool isNickName = BackendManager.Instance.UpdateNickName(m_inputNickName.text, out strMessage);
            m_tmpResult.text = strMessage; //��� �޼��� ���
            
            //ȸ������ ���� -> �г��� ���� ���� ��
            if (isNickName) 
            {
                m_btnNickName.interactable = false;//���̻� �г��� ��û �� �� ���� ��Ȱ��ȭ
                //�ε��� �̵�
                GameManager.Instance.LoadSceneWithTime("LoadingScene", 0.5f);
            }
        }
        else
        {
            m_tmpResult.text = "����Ͻ� �г����� �Է����ּ���";
        }
    }
}
