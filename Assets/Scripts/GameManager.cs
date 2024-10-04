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

    //��������
    //float m_fMoveLv = 0;

    //MoveBackGround���� ����� ��
    private float m_fMoveStatus = 1; //�̼Ӵɷ�ġ 1�̸� 100%, �������� ����
    public float MoveStatus //m_fMoveStatus ������Ƽ
    {
        get { return m_fMoveStatus; }
        private set { m_fMoveStatus = value; }
    }



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


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
