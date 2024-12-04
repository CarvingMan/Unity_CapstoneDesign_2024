using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using UnityEditor.PackageManager;
using UnityEngine.SceneManagement;

public class BackendManager : Singleton<BackendManager>
{
    /*
     * �ڳ� SDK ����Ͽ� Backend ����
     */


    // Start is called before the first frame update
    void Start()
    {
        var bro = Backend.Initialize(); //�ڳ� �ʱ�ȭ var ��� BackendReturnObject�ε� ����

        //�ڳ� �ʱ�ȭ�� ���� ���䰪
        if (bro.IsSuccess())
        {
            Debug.Log("�ʱ�ȭ ���� : " + bro); //������ ��� statusCode 204 Success
        }
        else
        {
            Debug.LogError("�ʱ�ȭ ���� : " + bro); //������ ��� statusCode 400�� ���� �߻�
            Application.Quit();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }



    //�α��� �������ο� �Բ� ���� �޼��� out
    public bool SignIn(string strUserID, string strPassword, out string strMessage)
    {
        BackendReturnObject bro = Backend.BMember.CustomLogin(strUserID, strPassword);
        if (bro.IsSuccess())
        {
            strMessage = Backend.UserNickName + "�� ȯ���մϴ�.";
            return true;
        }
        else
        {
            if (bro.StatusCode == 400)
            {
                strMessage = "����̽� ���� null";
            }
            else if(bro.StatusCode == 401)
            {
                //401 ���´� ���̵�or��� �� �߸��Ǿ��� ��, ���� �������� �� �� �����̱⿡ bro.Message�� ����
                strMessage = bro.Message;
            }
            else if(bro.StatusCode == 403)
            {
                strMessage = "���ܴ��� ����̽� �Դϴ�.";
            }
            else
            {
                strMessage = "�α��� ����";
                Application.Quit();
            }
            return false;
        }
    }


    //ȸ������ �������ο� �Բ� ���� �޼��� out
    //�ڳ� �������� ȸ������ ���� �� �α��ζ��� �ڵ����� ����ȴٰ� �Ѵ�.
    public bool SignUp(string strUserID, string strPassword, out string strMessage)
    {
        //Ŀ���� ȸ������ ����
        BackendReturnObject broSignUp = Backend.BMember.CustomSignUp(strUserID, strPassword);

        if (broSignUp.IsSuccess())
        {
            strMessage = "ȸ������ ����!";
            return true;
        }
        else
        {
            //ȸ������ ���� ��
            if (broSignUp.StatusCode == 400) //����̽� ���� ����
            {
                strMessage = "ȸ������ ���� ����̽� ���� null";
            }
            else if (broSignUp.StatusCode == 401) //���� ������
            {
                strMessage = "���� ���� �� �Դϴ�. ������ ���ĵ�� �˼��մϴ�.";
            }
            else if (broSignUp.StatusCode == 403)
            {
                strMessage = "�ش� ����̽��� ���ܵǾ����ϴ�.";
            }
            else if (broSignUp.StatusCode == 409)
            {
                strMessage = "�ش� ID�� �̹� �����ϴ� ID�Դϴ�.";
            }
            else //�߰� ���� ����
            {
                strMessage = "ȸ������ ����";
                Debug.Log(broSignUp.Message);
            }
            return false;
        }
    }

    //bool ���·� true�� ��� �ε������� �Ѿ �����̱⿡
    //out���� strMessage�� ����
    public bool UpdateNickName(string strNickName, out string strMessage)
    {
        //�г��� �ߺ� Ȯ��
        BackendReturnObject broCheckName = Backend.BMember.CheckNicknameDuplication(strNickName);
        if (broCheckName.IsSuccess())
        {
            //ȸ������ ������ �г��� ������Ʈ
            BackendReturnObject broNickName = Backend.BMember.CreateNickname(strNickName);
            if (broNickName.IsSuccess())
            {
                //GameScene���� �̵�
                strMessage = Backend.UserNickName+"�� ȯ���մϴ�!";
                return true;
            }
            {
                // �ش� ������ �� ó�� broCheckName���� �ߺ� �� ���� �� ���� ���� ���θ� Ȯ���� �����⿡
                // ���������� ������ ���ɼ��� ������, Ȥ�� �� ������ ���
                strMessage = "�г��� ��� ����";
                return false;
            }

        }
        else //�г��� �ߺ�üũ �� ������ �� ��
        {
            if (broCheckName.StatusCode == 400)
            {
                strMessage = "�г����� �յ� ������� 20�� ���� �������ּ���";
                //Debug.Log(broCheckName.Message);
            }
            else if (broCheckName.StatusCode == 409)
            {
                strMessage = "�̹� �����ϴ� �г����Դϴ�.";
            }
            else
            {
                strMessage = "�г��� ���� ����";
            }
            return false;
        }
    }
}
