using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    /*
     * GameMnager �� Singleton ������ ������ ����ϱ����� ���ʸ� �̱��� Ŭ���� ����
     * �ش� ���ʸ� Ŭ������ ��� ������ �ս��� �̱������� ��밡�� �ϴ�.
     * ���� 2�� �̻��� ���δٸ� Ÿ���� Singleton instance�� �ʿ��Ҷ� ������ ���
     */
    public int a = 0;
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                if (instance == null)
                {
                    //Find �Լ��� ���ɻ� ���� �ʾ� ����� �ǵ��� �����ؾ��ϱ⿡,
                    //���� �ʱ�ȭ �� �� ���� ã�� ������ ����
                    //�ٸ� Awake()���� ������ �ֱ⿡ �ش� ������ ����� ���� ���� ����.
                    instance = FindObjectOfType<T>();
                    if (instance == null)
                    {
                        //���� ���̾��Ű â�� ������ ���Ӱ� ���� �� �ش�.
                        GameObject obj = new GameObject(typeof(T).Name, typeof(T));
                        instance = obj.GetComponent<T>();
                    }
                }
                return instance;
            }
            else
            {
                //�̹� �ν��Ͻ��� ������ �� instance ��ȯ
                return instance;
            }
        }
    }

    //�ش� ���ʸ� class�� ��ӹ��� �� MonoBehavior ���� ��ӵǹǷ�
    //�ڽ��� Awake()�� ����ϸ� �θ� Awake()�� ȣ���� �ȵȴ�.
    //���� vitual�� �����Ͽ� �ڽ��� Awake()�� �Ⱦ��� �״�� ȣ���ϰ� �ڽ��� Awake()�� ����� �� 
    //override�� �Ͽ� base.Awake()�� �ش� �θ��� Awake()�� ȣ�� �� �־�� �Ѵ�.
    public virtual void Awake()
    {
        //������Ƽ get�� �� instance�� �����Ǳ⿡ ���� null�̶�� instance���� �� DontDestroy
        if (instance == null)
        {
            instance = this as T;//�ν��Ͻ�ȭ �� �� this�� ���ʸ� TŸ������ ĳ�����Ͽ� instance�� �־��ش�.
            DontDestroyOnLoad(gameObject);//�� �̵��ÿ��� �ش� ������Ʈ ���� ����
        }
        else
        {
            //�̹� instance�� �����ϹǷ� �������ش�.
            Destroy(gameObject);
        }
    }


    
}
