using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBackground : MonoBehaviour
{
    /// <summary>
    /// 배경화면은 parellax, 타일맵은 좌표이동.
    /// Type별로 움직임 정의
    /// </summary>

    public enum E_Move_Type
    {
        None,
        BackGound,
        Ground,
        Max,
    }
    public E_Move_Type m_eMoveType = E_Move_Type.None;

    //Background일 시 멤버면수
    GameObject[] m_objBackGround = null;
    Material[] m_materialBG = null;
    float[] m_fLayerMoveSpeed = null;
    float m_fParallexSpeed = 0.5f;

    private void Awake()
    {
        
        if(m_eMoveType == E_Move_Type.BackGound) //백그라운드 일 때
        {
            //자식들을 가져와 배열에 넣어준다.
            int nChildCount = transform.childCount;
            m_objBackGround = new GameObject[nChildCount];
            m_materialBG = new Material[nChildCount];
            m_fLayerMoveSpeed = new float[nChildCount];
            
            float fFarthestBack = 0; //카메라와 가장 먼 거리계산할 변수
            float fCameraPosZ = Camera.main.transform.position.z;
            for (int i = 0; i < nChildCount; i++)
            {
                // 배경Quad오브젝트와 스프라이트 Material 컴포넌트들을 넣어준다.
                m_objBackGround[i] = transform.GetChild(i).gameObject;
                m_materialBG[i] = transform.GetChild(i).GetComponent<MeshRenderer>().material;
                
                //만약 카메라의 z축 거리가 fFarthestBack보다 크다면
                if(Mathf.Abs(fCameraPosZ - m_objBackGround[i].transform.position.z) > fFarthestBack)
                {
                    //해당 거리를 넣어준다.
                    fFarthestBack = Mathf.Abs(fCameraPosZ - m_objBackGround[i].transform.position.z);
                }
            }

            //위 반복문에서 fFarthestBack의 최대 값이 정해졌으므로 해당 값에 따라
            //먼 거리 일 수록 느리게 갈수 있도록 fLayerMoveSpeed 값 설정
            for(int i = 0; i < nChildCount; i++)
            {
                float fDistance = Mathf.Abs(fCameraPosZ - m_objBackGround[i].transform.position.z);
                //해당 연산을 통해 멀리 있는 배경부터 0.1에서 점점 커지게 된다. 
                //1.1f에서 -한 이유는 1로하면 가장 멀리있는 배경은 speed가 0이된다.
                m_fLayerMoveSpeed[i] = 1.1f - fDistance / fFarthestBack;
            }

        }
        else if(m_eMoveType == E_Move_Type.Ground) //Ground 타일맵 일 때
        {

        }
        else
        {
            Debug.LogError("m_eMoveType을 설정해주세요");
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
            //Type이 BackGroud라면 
            for(int i=0; i < m_materialBG.Length; i++)
            {
                float fSpeed = GameManager.Instance.MoveStatus * m_fLayerMoveSpeed[i] 
                    * m_fParallexSpeed * Time.deltaTime;
                // Matetial  _MainTex의 offset의 x를 증가 시켜준다.
                // 현재 offset을 Vector2.right * fSpeed에 계속 더해줘야 한다.
                Vector2 vecCurrentOffset = m_materialBG[i].GetTextureOffset("_MainTex");
                m_materialBG[i].SetTextureOffset("_MainTex", Vector2.right * fSpeed + vecCurrentOffset);
            }
        }
    }
}
