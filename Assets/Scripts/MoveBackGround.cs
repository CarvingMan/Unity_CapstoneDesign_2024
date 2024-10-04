using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBackground : MonoBehaviour
{
    /// <summary>
    /// ���ȭ���� parellax, Ÿ�ϸ��� ��ǥ�̵�.
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

    //Background�� �� ������
    GameObject[] m_objBackGround = null;
    Material[] m_materialBG = null;
    float[] m_fLayerMoveSpeed = null;
    float m_fParallexSpeed = 0.5f;

    private void Awake()
    {
        
        if(m_eMoveType == E_Move_Type.BackGound) //��׶��� �� ��
        {
            //�ڽĵ��� ������ �迭�� �־��ش�.
            int nChildCount = transform.childCount;
            m_objBackGround = new GameObject[nChildCount];
            m_materialBG = new Material[nChildCount];
            m_fLayerMoveSpeed = new float[nChildCount];
            
            float fFarthestBack = 0; //ī�޶�� ���� �� �Ÿ������ ����
            float fCameraPosZ = Camera.main.transform.position.z;
            for (int i = 0; i < nChildCount; i++)
            {
                // ���Quad������Ʈ�� ��������Ʈ Material ������Ʈ���� �־��ش�.
                m_objBackGround[i] = transform.GetChild(i).gameObject;
                m_materialBG[i] = transform.GetChild(i).GetComponent<MeshRenderer>().material;
                
                //���� ī�޶��� z�� �Ÿ��� fFarthestBack���� ũ�ٸ�
                if(Mathf.Abs(fCameraPosZ - m_objBackGround[i].transform.position.z) > fFarthestBack)
                {
                    //�ش� �Ÿ��� �־��ش�.
                    fFarthestBack = Mathf.Abs(fCameraPosZ - m_objBackGround[i].transform.position.z);
                }
            }

            //�� �ݺ������� fFarthestBack�� �ִ� ���� ���������Ƿ� �ش� ���� ����
            //�� �Ÿ� �� ���� ������ ���� �ֵ��� fLayerMoveSpeed �� ����
            for(int i = 0; i < nChildCount; i++)
            {
                float fDistance = Mathf.Abs(fCameraPosZ - m_objBackGround[i].transform.position.z);
                //�ش� ������ ���� �ָ� �ִ� ������ 0.1���� ���� Ŀ���� �ȴ�. 
                //1.1f���� -�� ������ 1���ϸ� ���� �ָ��ִ� ����� speed�� 0�̵ȴ�.
                m_fLayerMoveSpeed[i] = 1.1f - fDistance / fFarthestBack;
            }

        }
        else if(m_eMoveType == E_Move_Type.Ground) //Ground Ÿ�ϸ� �� ��
        {

        }
        else
        {
            Debug.LogError("m_eMoveType�� �������ּ���");
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
            Move();
        }
        
    }

    public void Move()
    {
        if(m_eMoveType == E_Move_Type.BackGound)
        {
            //Type�� BackGroud��� 
            for(int i=0; i < m_materialBG.Length; i++)
            {
                float fSpeed = GameManager.Instance.MoveStatus * m_fLayerMoveSpeed[i] 
                    * m_fParallexSpeed * Time.deltaTime;
                // Matetial  _MainTex�� offset�� x�� ���� �����ش�.
                // ���� offset�� Vector2.right * fSpeed�� ��� ������� �Ѵ�.
                Vector2 vecCurrentOffset = m_materialBG[i].GetTextureOffset("_MainTex");
                m_materialBG[i].SetTextureOffset("_MainTex", Vector2.right * fSpeed + vecCurrentOffset);
            }
        }
    }
}
