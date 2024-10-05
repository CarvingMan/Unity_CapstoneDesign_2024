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

    
     
    //*** �ɷ�ġ ���� ***//
    //float m_fMoveLv = 0;
    

    //*** �ɷ�ġ �� ***//
    private float m_fMoveStatus = 1; //�̵��ӵ� �ɷ�ġ 1�̸� 100%, �������� ����


    //�̵����� ���� ��� ���� MoveBackGround,PlayerControl,MonsterControl��� ���
    private bool m_isMove = false;
    public bool IsMove { get { return m_isMove; }} //m_isMove ���ٿ� read only�� get�� ����

    public float MoveStatus //m_fMoveStatus ���ٿ� ������Ƽ
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
        if (Input.GetMouseButton(0))
        {
            m_isMove = true;
        }
        else
        {
            m_isMove = false;
        }
    }

}
