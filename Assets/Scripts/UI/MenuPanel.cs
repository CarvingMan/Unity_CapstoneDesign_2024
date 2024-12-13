using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuPanel : MonoBehaviour
{
    /*
     * GameScene�� FieldUI �гο��� Menu ��ư�� ���� �� �ش� �г��� Ȱ��ȭ �Ǹ� timeScale�� 0�� �ȴ�.
     */

    [SerializeField]
    Button m_btnClose = null; //�޴�â �ݱ� ��ư
    [SerializeField] 
    Button m_btnLogOutBtn = null; //���� �� �α׾ƿ� ��ư
    [SerializeField]
    TextMeshProUGUI m_tmpNickName = null;

    //���� �� �α׾ƿ� ��ư ���� �� BackendManager���� �񵿱�� ���� �� �α׾ƿ��� ������ ��
    //FadeOut�� �� �� �ֵ��� FadeInOutPanel.cs�� ������ �г� ���
    [Tooltip("FadeInOutPanel.cs�� ������ panel")]
    [SerializeField] GameObject m_objFadeInOutPanel = null; 

    // Start is called before the first frame update
    void Start()
    {
        if(m_tmpNickName != null)
        {
            m_tmpNickName.text = BackendManager.Instance.NickName + " ��";
        }
        else
        {
            Debug.LogError("m_tmpNickName�� �����ϴ�.");
        }

        if(m_objFadeInOutPanel == null)
        {
            Debug.LogError("m_objFadeInOutPanel�� �����ϴ�." + gameObject.name);
        }
        if(m_btnClose != null)
        {
            m_btnClose.onClick.AddListener(ClickCloseBtn);
        }
        else
        {
            Debug.LogError("m_btnClose�� �����ϴ�."+gameObject.name);
        }

        if(m_btnLogOutBtn != null)
        {
            m_btnLogOutBtn.onClick.AddListener(ClickLogOutBtn);
        }
        else
        {
            Debug.LogError("m_btnLogOutBtn�� �����ϴ�." + gameObject.name);
        }

        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);  
        }
    }

    //m_btnClose onClick AddListener�� �߰��� �Լ�
    void ClickCloseBtn()
    {
        //�ش� MenuPanel�� FieldUIPanel�� Menu Butten�� �������� Active�� �Ǹ鼭 timeScale�� 0�̵ȴ�.
        //CloseBtn���� MenuPanel�� ���� �� 1�� �ǵ�����.
        if(Time.timeScale == 0)
        {
            Time.timeScale = 1; 
        }

        gameObject.SetActive(false);
    }

    //m_btnLogOutBtn onClick AddListener�� �߰��� �Լ�
    //BackendManger���� �񵿱�� ���� �� �α׾ƿ� �� Ÿ��Ʋ ȭ������ �̵�
    void ClickLogOutBtn()
    {

        //FadeOut
        if (m_objFadeInOutPanel != null)
        {
            m_objFadeInOutPanel.GetComponent<FadeInOutPanel>().FadeOutPanel();
        }

        //BackendManager�� SaveUserData�� �Ű����� isLogOut�� true�� �Ѱ��ְԵǸ�,
        //�񵿱� ���� ������Ʈ�� �Ϸ�ɽ� -> �񵿱� �α׾ƿ� -> TileScene�̵�
        //timeScale�� �α� �ƿ� �Ϸ��� GameManager���� Scene�̵����� 1�� �ٲپ� �ش�.
        BackendManager.Instance.SaveUserData(true);

        m_btnLogOutBtn.interactable = false; //���̻� ���� �� �α׾ƿ� ��ư�� ���� �� ���� ����
        gameObject.SetActive(false);
    }
}