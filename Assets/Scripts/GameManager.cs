using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //�̱��� ������ ���� ���
    //���� �ν��Ͻ� ����
    // ������ ���� set ������Ƽ�� private�� �����Ͽ� 
    // �ٸ� Ŭ�������� get�� �����ϳ� �ν��Ͻ� �ʱ�ȭ�� ���� GameManager������ �����ϴ�.
    public static GameManager Instance { get; private set; }


    private void Awake()
    {
        if(Instance != null)
        {
            //���Ӱ� GameManager�� �����Ǿ��� �� ���� instance�� �����Ѵٸ� 
            //GameManager�� �ߺ��̱⿡ ����
            Destroy(gameObject);
        }
        else
        {
            //���� ���� instance�� null�̶��, �ش� ��ü�� �Ѱ��ְ� DontDestroy
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void Test()
    {
        Debug.Log("test�޼ҵ� ȣ��");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
