using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveMap : MonoBehaviour
{
    /// <summary>
    /// 배경화면은 parallax, 타일맵은 좌표이동.
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

    //Background일 시 멤버변수
    GameObject[] m_objBackGrounds = null;
    Material[] m_materialBG = null;
    float[] m_fLayerMoveSpeed = null;

    // 기본 Speed m_eMoveType에 따라 Awake에서 다르게 설정
    float m_fMoveSpeed = 0;

    //Ground 타일맵 일 시 멤버 변수
    GameObject[] m_objTileGrounds = null; // 타일맵들을 정렬하여 저장할 배열
    GameObject m_objPreviousGround = null; // 이미 지나간 타일맵
    [SerializeField]
    float m_fInterval = 8f; //Map끼리의 간격

    private void Awake()
    {
        
        if(m_eMoveType == E_Move_Type.BackGound) //백그라운드 일 때
        {
            InitBackGround();
        }
        else if(m_eMoveType == E_Move_Type.Ground) //Ground 타일맵 일 때
        {
            InitTileMapGround();

        }
        else
        {
            Debug.LogError("m_eMoveType을 설정해주세요");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //GroundType일 시 GameMager에 인스턴스를 넘겨준다.
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

    //배경화면관련 세팅 -> m_eMoveType이 BackGround일 때
    void InitBackGround()
    {
        m_fMoveSpeed = 0.3f;

        //자식들을 가져와 배열에 넣어준다.
        int nChildCount = transform.childCount;
        m_objBackGrounds = new GameObject[nChildCount];
        m_materialBG = new Material[nChildCount];
        m_fLayerMoveSpeed = new float[nChildCount];

        float fFarthestBack = 0; //카메라와 가장 먼 거리계산할 변수
        float fCameraPosZ = Camera.main.transform.position.z;
        for (int i = 0; i < nChildCount; i++)
        {
            // 배경Quad오브젝트와 스프라이트 Material 컴포넌트들을 넣어준다.
            m_objBackGrounds[i] = transform.GetChild(i).gameObject;
            m_materialBG[i] = transform.GetChild(i).GetComponent<MeshRenderer>().material;

            //만약 카메라의 z축 거리가 fFarthestBack보다 크다면
            if (Mathf.Abs(fCameraPosZ - m_objBackGrounds[i].transform.position.z) > fFarthestBack)
            {
                //해당 거리를 넣어준다.
                fFarthestBack = Mathf.Abs(fCameraPosZ - m_objBackGrounds[i].transform.position.z);
            }
        }

        //위 반복문에서 fFarthestBack의 최대 값이 정해졌으므로 해당 값에 따라
        //먼 거리 일 수록 느리게 갈수 있도록 fLayerMoveSpeed 값 설정
        for (int i = 0; i < nChildCount; i++)
        {
            float fDistance = Mathf.Abs(fCameraPosZ - m_objBackGrounds[i].transform.position.z);
            //해당 연산을 통해 멀리 있는 배경부터 0.1에서 점점 커지게 된다. 
            //1.1f에서 -한 이유는 1로하면 가장 멀리있는 배경은 speed가 0이된다.
            m_fLayerMoveSpeed[i] = 1.1f - fDistance / fFarthestBack;
        }
    }

    //타일맵Ground관련 세팅 -> m_eMoveType이 Ground일 때
    void InitTileMapGround()
    {
        m_fMoveSpeed = 3f;
        //자식 개수에 맞게 배열 생성
        int nChildCount = transform.childCount;
        m_objTileGrounds = new GameObject[nChildCount];
        
        //m_objTileGrounds 배열을 정렬하기 위한 임시 리스트
        List<GameObject> CheckList = new List<GameObject>();
        for(int i=0; i < nChildCount; i++)
        {
            CheckList.Add(transform.GetChild(i).gameObject);
        }
        //정렬하여 m_objTileGrounds에 넣기
        GameObject objMinGround = null; //CheckList에서 카메라와x축 간격이 제일 가까운 오브젝트
        for (int i = 0; i < m_objTileGrounds.Length; i++)
        {
            for(int j=0; j < CheckList.Count; j++)
            {
                if(j == 0)
                {
                    //CheckList의 첫번째 요소일 때는 objMinGround에 바로 넣어준다.
                    objMinGround = CheckList[j];
                }
                else
                {
                    //메인카메라와의 x축 거리 계산을 해준다.
                    float fMinDistance = Mathf.Abs(Camera.main.transform.position.x - objMinGround.transform.position.x);
                    float fCheckDistance = Mathf.Abs(Camera.main.transform.position.x - CheckList[j].transform.position.x);
                    //만약 해당 값이 m_objMinGround의 값 보다 작다면
                    if(fMinDistance > fCheckDistance)
                    {
                        objMinGround = CheckList[j]; //바꾸어 준다.
                    }
                }
            }
            //j반복문이 끝났을 때 해당 오브젝트가 들어 있는 CheckList 의 요소 삭제
            //단, 반드시 for문 밖에서 해야 한다. remove시 인덱스가 앞당겨지기에 오류
            CheckList.Remove(objMinGround);

            //x축의 좌표를 메인카메라 x좌표에서 6(타일맵간격)씩 정렬하기 위한 로직
            Vector2 vecMinGround = objMinGround.transform.localPosition;
            //메인카메라 x + (6 * 현재인덱스)
            vecMinGround.x = Camera.main.transform.position.x + m_fInterval*i;
            objMinGround.transform.localPosition = vecMinGround;
            m_objTileGrounds[i] = objMinGround;
        }
        


    }

    public void Move()
    {
        if(m_eMoveType == E_Move_Type.BackGound)
        {
            //Type이 BackGroud라면 
            MoveBackGround();
        }
        else if(m_eMoveType == E_Move_Type.Ground)
        {
            MoveTileGrounds();
        }
        else
        {
            Debug.LogError("m_eMoveType 확인 현재 type: " + m_eMoveType.ToString());
        }
    }

    //배경 이동(materal offset을 이용한 parallax 전환)
    void MoveBackGround()
    {
        for (int i = 0; i < m_materialBG.Length; i++)
        {
            //GameManager의 MoveStatus(이속 능력치 1(100%)부터~) 같이 speed에 곱해준다.
            float fSpeed = GameManager.Instance.MoveStatus * m_fLayerMoveSpeed[i]
                * m_fMoveSpeed * Time.deltaTime;
            // Matetial  _MainTex의 offset의 x를 증가 시켜준다.
            // 현재 offset을 Vector2.right * fSpeed에 계속 더해줘야 한다.
            Vector2 vecCurrentOffset = m_materialBG[i].GetTextureOffset("_MainTex");
            m_materialBG[i].SetTextureOffset("_MainTex", Vector2.right * fSpeed + vecCurrentOffset);
        }
    }

    //타일맵들 이동 (타일들전체 Translate이동 카메라밖 지나갈 시 맨 끝으로 이동)
    void MoveTileGrounds()
    {
        float fSpeed = GameManager.Instance.MoveStatus * m_fMoveSpeed * Time.deltaTime;
        transform.Translate(Vector2.left * fSpeed);
        
        //만약 카메라와 가장 가까이에 있는 m_objTileGrounds의 첫번째 요소가
        //카메라와 x축 간의 거리가 (간격)6 보다 커졌다는 것은 이미 뒤로 지나가 화면을 넘어섯다는 것이다.
        //따라서 배열을 앞당겨주고 지나간 요소는 마지막 전 요소에 m_fInterver만큼 더한 위치로 바꾸어준다.
        float fFrontTileDistance = m_objTileGrounds[0].transform.position.x;
        if(Mathf.Abs(Camera.main.transform.position.x - fFrontTileDistance) > m_fInterval)
        {
            //m_objPreviousGround에 화면 밖으로 넘어간 m_objTileGrounds의 첫번 째 요소를 넣어준다.
            m_objPreviousGround = m_objTileGrounds[0];
            // 반복문을 통해 m_objTileGrounds의 요소들을 앞 당겨 준다.
            for(int i=0; i<m_objTileGrounds.Length; i++)
            {
                if(i != m_objTileGrounds.Length - 1)
                {
                    m_objTileGrounds[i]= m_objTileGrounds[i+1];
                }
                else // 마지막 인덱스일 때
                {
                    //m_objTileGrounds의 마지막 인덱스에 화면 밖으로 지나간
                    //m_objPreviousGround를 간격에 맞에 xc축 조정후 넣어준다.
                    Vector2 vecPreGround = m_objPreviousGround.transform.position;
                    vecPreGround.x = m_objTileGrounds[i - 1].transform.position.x + m_fInterval;
                    m_objPreviousGround.transform.position = vecPreGround;
                    m_objTileGrounds[i] = m_objPreviousGround;
                }
            }
        }
    }

    //만약 타입이 Ground일 시 필드 몬스터가 자식으로 들어갈 여분 TileMap제공
    public Transform GetFieldMobGround()
    {
        if (m_eMoveType == E_Move_Type.Ground)
        {
            //타일맵 요소가 존재한다면
            if(m_objTileGrounds.Length != 0)
            {
                //전체에서 2로 나누어 인덱스로 사용
                //int 나눗셈이기에 요소가 1개일 시0, 2개 일 시 1, 3개 일 시1...
                //(사실상 타일 반복은 2개이상으로 설정하여야 반복이 되기에 인덱스 1부터 시작)
                //이 처럼 배열의 중간 혹은  마지막에 타일 요소 선택
                int nIndex = m_objTileGrounds.Length / 2;
     
                //해당 타일의 transform 반환, 추후 몬스터 생성 시 부모transform으로 설정되어
                //같이 플레이어쪽으로 맵 이동하게 된다.
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
