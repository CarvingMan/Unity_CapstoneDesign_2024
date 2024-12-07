using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class LoadingScene : MonoBehaviour
{
    /*
     * �񵿱�� ���ε� �� �Ϸ� �� GameScene���� �̵�
     * �ٸ� ������ GameManager.Instance.IsInitUserData�� flase�̶��, �� ���� ��
     * ���� �������� �ش� ������ ��������, �ɷ�ġ, ���� ���� �ð� �� �����͸� �ҷ����� ���� ���·�
     * �ش� �۾��� GameManager���� LoadingScene���� �̵��� ���ÿ� �����ϰ� �ȴ�.
        GameManager.Instance.IsInitUserData�� true�� �� ��
        �񵿱� �� �ε�� �����ٸ� GameScene���� �̵���Ų��.
     */


    [Tooltip("�ε��޼���")]
    [SerializeField]
    TextMeshProUGUI m_tmpMessage = null; //�ε� ���� �ֱ������� tween�� �޼���

    [SerializeField]
    GameObject m_objLoadingMob = null; //�ε��� ȭ�鿡 �ִϸ��̼� ����� ������Ʈ

    [SerializeField]
    GameObject m_objFadeInOutPanel = null; //FadeInOutPanel.cs�� ������ �г�

    Sequence m_seqMessage = null; //�ε� �޼��� tween ������

    //�ڷ�ƾ ���� ����
    bool m_isCorWaitUserData = false;
    bool m_isCorAsyncScene = false;

    bool m_isUserData = false;//GameManager.Instance.IsInitUserData�� true�� �� �� ture �� ��ȯ

    // Start is called before the first frame update
    void Start()
    {
        if(m_objLoadingMob == null)
        {
            Debug.LogError("m_objLoadingMob�� �����ϴ�." + gameObject.name);
        }  

        if(m_tmpMessage == null)
        {
            Debug.LogError("m_tmpMessage�� �����ϴ�.");
        }

        if (m_objFadeInOutPanel == null) 
        {
            Debug.LogError("m_objFadeInOutPanel�� �����ϴ�.");
        }

        m_isUserData = GameManager.Instance.IsInitUserData;


        //�ڷ�ƾ ����
        StartCoroutine(CorWaitUserData());;
        StartCoroutine(CorAsyncScene());

        //GameManager���� m_isInitUserData�� false�� �� ȣ��
        //BackendManager���� �񵿱�� USER_DATA������ �޾ƿ��� GameManager���� �Ѱ��ش�.
        //GameManager���� �� �Ҵ��� ���� �� m_isInitUserData�� true�� �ٲ۴�.
        if(m_isUserData == false)
        {
            BackendManager.Instance.LoadUserData();
        }
    }

    //m_tmpMessage Ʈ��
    void SetMessageTween(string strMessage , float fTime)
    {
        if (m_seqMessage != null && m_seqMessage.IsActive())
        {
            //Ȥ�� ���� �������� ���� ���� �� Kill
            m_seqMessage.Kill();
        }

        //fTime ���� ""���� strMessage�� ��ȯ�ϴ� tween
        Tween tween = DOTween.To(() => "", str => m_tmpMessage.text = str, strMessage, fTime);
        m_seqMessage = DOTween.Sequence();
        m_seqMessage.Append(tween);
    }

    IEnumerator CorWaitUserData()
    {
        if (m_isCorWaitUserData)
        {
            yield break;
        }
        m_isCorWaitUserData = true;

        float fTime = 0;
        float fWait = 0.1f;
        string strMessage = "������ �ҷ����� ��...";
        SetMessageTween(strMessage,2);
        while(GameManager.Instance.IsInitUserData == false)
        {
            if(fTime >= 5) //5�ʸ��� �ٽ� �ؽ�Ʈ tween ���
            {
                fTime = 0;
                SetMessageTween(strMessage,2);
            }
            
            yield return new WaitForSeconds(fWait);
            fTime += fWait;
            
        }
        //Debug.Log(GameManager.Instance.IsInitUserData);
        //������ �ҷ����� �Ϸ� �� ��
        //������ �ε� �Ǵ� �ӵ��� �ʹ� ���� ������ ���� ������ �������� ��ٸ�(���ʿ�)
        yield return new WaitForSeconds(2); 
        m_isUserData = true;
    }

    IEnumerator CorAsyncScene()
    {
        if (m_isCorAsyncScene)
        {
            yield break;
        }
        m_isCorAsyncScene = true;

        //�񵿱�� GameScene�� �ε��ϸ� ����� AsyncOperation ������ ��´�.
        AsyncOperation operation = SceneManager.LoadSceneAsync("GameScene");
        operation.allowSceneActivation = false; //�񵿱� �ε尡 �Ϸᰡ �Ǿ����� �Ϸ��� �۾��� ���� �ٷ� ������ ����
        
        float fTime = 0;

        //���������� �ε�� �񵿱�� �ε尡 ���� �Ϸ�Ǿ��� ������ �ݺ� ���
        while (true)
        {
            //���������� �ε�� �񵿱�� �ε尡 ���� �Ϸ�Ǿ��� ������ �ݺ� ���
            //allowSceneActivation�� false�� �Ǿ������� 0.9�̻����� �ȿö󰡰� ��ٸ��� isDone�� false �����Ѵ�.
            //���� 0.9�̻��� �Ǹ� Ż�� �� allowSceneActive true�� �ٲپ� �־�� �Ѵ�.
            if (m_isUserData && operation.progress >= 0.9f)
            {
                break;
            }
            else if(operation.progress < 0.9f && m_isUserData) 
            {
                //���� �񵿱���� �Ϸ� ���� �ʾ����� GameManager���� ���� ���� �ε� �Ϸ� ��

                if (fTime >= 5)
                {
                    fTime = 0;
                    string strMessage = "Scene �ҷ����� ��...";
                    SetMessageTween(strMessage, 2);
                }
            }
            yield return new WaitForSeconds(0.1f);
            fTime += 0.1f;
            //Debug.Log("�񵿱� ���ε� ��Ȳ" + operation.progress);
        }
        //���� �ε� �Ǿ��� ��
        SetMessageTween("�ε� �Ϸ�!", 0.5f);
        m_objLoadingMob.GetComponent<Animator>().SetBool("isDead", true);
        m_objFadeInOutPanel.GetComponent<FadeInOutPanel>().FadeOutPanel(); //panel FadeOut
        yield return new WaitForSeconds(2f);

        operation.allowSceneActivation = true; //�� Ȱ��ȭ

    }
}
