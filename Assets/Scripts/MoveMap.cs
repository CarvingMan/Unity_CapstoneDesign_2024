using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveMap : MonoBehaviour
{
    /// <summary>
    /// ���ȭ���� parallax, Ÿ�ϸ��� ��ǥ�̵�.
    /// Type���� ������ ����
    /// </summary>

    public enum E_Move_Type
    {
        None,
        BackGound,
        Ground,
        Max,
    }
    public E_Move_Type m_eMoveType = E_Move_Type.None;

    //Background�� �� �������
    GameObject[] m_objBackGrounds = null;
    Material[] m_materialBG = null;
    float[] m_fLayerMoveSpeed = null;

    // �⺻ Speed m_eMoveType�� ���� Awake���� �ٸ��� ����
    float m_fMoveSpeed = 0;

    //Ground Ÿ�ϸ� �� �� ��� ����
    GameObject[] m_objTileGrounds = null; // Ÿ�ϸʵ��� �����Ͽ� ������ �迭
    GameObject m_objPreviousGround = null; // �̹� ������ Ÿ�ϸ�
    [SerializeField]
    float m_fInterval = 8f; //Map������ ����

    private void Awake()
    {
        
        if(m_eMoveType == E_Move_Type.BackGound) //��׶��� �� ��
        {
            InitBackGround();
        }
        else if(m_eMoveType == E_Move_Type.Ground) //Ground Ÿ�ϸ� �� ��
        {
            InitTileMapGround();

        }
        else
        {
            Debug.LogError("m_eMoveType�� �������ּ���");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //GroundType�� �� GameMager�� �ν��Ͻ��� �Ѱ��ش�.
        if(m_eMoveType == E_Move_Type.Ground)
        {
            GameManager.Instance.TakeObject(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.IsMove)
        {
            Move();
        }
        
    }

    //���ȭ����� ���� -> m_eMoveType�� BackGround�� ��
    void InitBackGround()
    {
        m_fMoveSpeed = 0.3f;

        //�ڽĵ��� ������ �迭�� �־��ش�.
        int nChildCount = transform.childCount;
        m_objBackGrounds = new GameObject[nChildCount];
        m_materialBG = new Material[nChildCount];
        m_fLayerMoveSpeed = new float[nChildCount];

        float fFarthestBack = 0; //ī�޶�� ���� �� �Ÿ������ ����
        float fCameraPosZ = Camera.main.transform.position.z;
        for (int i = 0; i < nChildCount; i++)
        {
            // ���Quad������Ʈ�� ��������Ʈ Material ������Ʈ���� �־��ش�.
            m_objBackGrounds[i] = transform.GetChild(i).gameObject;
            m_materialBG[i] = transform.GetChild(i).GetComponent<MeshRenderer>().material;

            //���� ī�޶��� z�� �Ÿ��� fFarthestBack���� ũ�ٸ�
            if (Mathf.Abs(fCameraPosZ - m_objBackGrounds[i].transform.position.z) > fFarthestBack)
            {
                //�ش� �Ÿ��� �־��ش�.
                fFarthestBack = Mathf.Abs(fCameraPosZ - m_objBackGrounds[i].transform.position.z);
            }
        }

        //�� �ݺ������� fFarthestBack�� �ִ� ���� ���������Ƿ� �ش� ���� ����
        //�� �Ÿ� �� ���� ������ ���� �ֵ��� fLayerMoveSpeed �� ����
        for (int i = 0; i < nChildCount; i++)
        {
            float fDistance = Mathf.Abs(fCameraPosZ - m_objBackGrounds[i].transform.position.z);
            //�ش� ������ ���� �ָ� �ִ� ������ 0.1���� ���� Ŀ���� �ȴ�. 
            //1.1f���� -�� ������ 1���ϸ� ���� �ָ��ִ� ����� speed�� 0�̵ȴ�.
            m_fLayerMoveSpeed[i] = 1.1f - fDistance / fFarthestBack;
        }
    }

    //Ÿ�ϸ�Ground���� ���� -> m_eMoveType�� Ground�� ��
    void InitTileMapGround()
    {
        m_fMoveSpeed = 3f;
        //�ڽ� ������ �°� �迭 ����
        int nChildCount = transform.childCount;
        m_objTileGrounds = new GameObject[nChildCount];
        
        //m_objTileGrounds �迭�� �����ϱ� ���� �ӽ� ����Ʈ
        List<GameObject> CheckList = new List<GameObject>();
        for(int i=0; i < nChildCount; i++)
        {
            CheckList.Add(transform.GetChild(i).gameObject);
        }
        //�����Ͽ� m_objTileGrounds�� �ֱ�
        GameObject objMinGround = null; //CheckList���� ī�޶��x�� ������ ���� ����� ������Ʈ
        for (int i = 0; i < m_objTileGrounds.Length; i++)
        {
            for(int j=0; j < CheckList.Count; j++)
            {
                if(j == 0)
                {
                    //CheckList�� ù��° ����� ���� objMinGround�� �ٷ� �־��ش�.
                    objMinGround = CheckList[j];
                }
                else
                {
                    //����ī�޶���� x�� �Ÿ� ����� ���ش�.
                    float fMinDistance = Mathf.Abs(Camera.main.transform.position.x - objMinGround.transform.position.x);
                    float fCheckDistance = Mathf.Abs(Camera.main.transform.position.x - CheckList[j].transform.position.x);
                    //���� �ش� ���� m_objMinGround�� �� ���� �۴ٸ�
                    if(fMinDistance > fCheckDistance)
                    {
                        objMinGround = CheckList[j]; //�ٲپ� �ش�.
                    }
                }
            }
            //j�ݺ����� ������ �� �ش� ������Ʈ�� ��� �ִ� CheckList �� ��� ����
            //��, �ݵ�� for�� �ۿ��� �ؾ� �Ѵ�. remove�� �ε����� �մ�����⿡ ����
            CheckList.Remove(objMinGround);

            //x���� ��ǥ�� ����ī�޶� x��ǥ���� 6(Ÿ�ϸʰ���)�� �����ϱ� ���� ����
            Vector2 vecMinGround = objMinGround.transform.localPosition;
            //����ī�޶� x + (6 * �����ε���)
            vecMinGround.x = Camera.main.transform.position.x + m_fInterval*i;
            objMinGround.transform.localPosition = vecMinGround;
            m_objTileGrounds[i] = objMinGround;
        }
        


    }

    public void Move()
    {
        if(m_eMoveType == E_Move_Type.BackGound)
        {
            //Type�� BackGroud��� 
            MoveBackGround();
        }
        else if(m_eMoveType == E_Move_Type.Ground)
        {
            MoveTileGrounds();
        }
        else
        {
            Debug.LogError("m_eMoveType Ȯ�� ���� type: " + m_eMoveType.ToString());
        }
    }

    //��� �̵�(materal offset�� �̿��� parallax ��ȯ)
    void MoveBackGround()
    {
        for (int i = 0; i < m_materialBG.Length; i++)
        {
            //GameManager�� MoveStatus(�̼� �ɷ�ġ 1(100%)����~) ���� speed�� �����ش�.
            float fSpeed = GameManager.Instance.MoveStatus * m_fLayerMoveSpeed[i]
                * m_fMoveSpeed * Time.deltaTime;
            // Matetial  _MainTex�� offset�� x�� ���� �����ش�.
            // ���� offset�� Vector2.right * fSpeed�� ��� ������� �Ѵ�.
            Vector2 vecCurrentOffset = m_materialBG[i].GetTextureOffset("_MainTex");
            m_materialBG[i].SetTextureOffset("_MainTex", Vector2.right * fSpeed + vecCurrentOffset);
        }
    }

    //Ÿ�ϸʵ� �̵� (Ÿ�ϵ���ü Translate�̵� ī�޶�� ������ �� �� ������ �̵�)
    void MoveTileGrounds()
    {
        float fSpeed = GameManager.Instance.MoveStatus * m_fMoveSpeed * Time.deltaTime;
        transform.Translate(Vector2.left * fSpeed);
        
        //���� ī�޶�� ���� �����̿� �ִ� m_objTileGrounds�� ù��° ��Ұ�
        //ī�޶�� x�� ���� �Ÿ��� (����)6 ���� Ŀ���ٴ� ���� �̹� �ڷ� ������ ȭ���� �Ѿ�ٴ� ���̴�.
        //���� �迭�� �մ���ְ� ������ ��Ҵ� ������ �� ��ҿ� m_fInterver��ŭ ���� ��ġ�� �ٲپ��ش�.
        float fFrontTileDistance = m_objTileGrounds[0].transform.position.x;
        if(Mathf.Abs(Camera.main.transform.position.x - fFrontTileDistance) > m_fInterval)
        {
            //m_objPreviousGround�� ȭ�� ������ �Ѿ m_objTileGrounds�� ù�� ° ��Ҹ� �־��ش�.
            m_objPreviousGround = m_objTileGrounds[0];
            // �ݺ����� ���� m_objTileGrounds�� ��ҵ��� �� ��� �ش�.
            for(int i=0; i<m_objTileGrounds.Length; i++)
            {
                if(i != m_objTileGrounds.Length - 1)
                {
                    m_objTileGrounds[i]= m_objTileGrounds[i+1];
                }
                else // ������ �ε����� ��
                {
                    //m_objTileGrounds�� ������ �ε����� ȭ�� ������ ������
                    //m_objPreviousGround�� ���ݿ� �¿� xc�� ������ �־��ش�.
                    Vector2 vecPreGround = m_objPreviousGround.transform.position;
                    vecPreGround.x = m_objTileGrounds[i - 1].transform.position.x + m_fInterval;
                    m_objPreviousGround.transform.position = vecPreGround;
                    m_objTileGrounds[i] = m_objPreviousGround;
                }
            }
        }
    }

    //���� Ÿ���� Ground�� �� �ʵ� ���Ͱ� �ڽ����� �� ���� TileMap����
    public Transform GetFieldMobGround()
    {
        if (m_eMoveType == E_Move_Type.Ground)
        {
            //Ÿ�ϸ� ��Ұ� �����Ѵٸ�
            if(m_objTileGrounds.Length != 0)
            {
                //��ü���� 2�� ������ �ε����� ���
                //int �������̱⿡ ��Ұ� 1���� ��0, 2�� �� �� 1, 3�� �� ��1...
                //(��ǻ� Ÿ�� �ݺ��� 2���̻����� �����Ͽ��� �ݺ��� �Ǳ⿡ �ε��� 1���� ����)
                //�� ó�� �迭�� �߰� Ȥ��  �������� Ÿ�� ��� ����
                int nIndex = m_objTileGrounds.Length / 2;
     
                //�ش� Ÿ���� transform ��ȯ, ���� ���� ���� �� �θ�transform���� �����Ǿ�
                //���� �÷��̾������� �� �̵��ϰ� �ȴ�.
                return m_objTileGrounds[nIndex].transform;
            }
            else
            {
                return null;
            }
        }
        else
        {
            return null;
        }
        
    }
}
