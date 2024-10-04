using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //싱글톤 디자인 패턴 사용
    //정적 인스턴스 생성
    // 보안을 위해 set 프로퍼티는 private로 설정하여 
    // 다른 클래스에서 get은 가능하나 인스턴스 초기화는 오직 GameManager에서만 가능하다.
    public static GameManager Instance { get; private set; }

    //레벨관리
    //float m_fMoveLv = 0;

    //MoveBackGround에서 사용할 값
    private float m_fMoveStatus = 1; //이속능력치 1이면 100%, 레벨업시 증가
    public float MoveStatus //m_fMoveStatus 프로퍼티
    {
        get { return m_fMoveStatus; }
        private set { m_fMoveStatus = value; }
    }



    private void Awake()
    {
        if(Instance != null)
        {
            //새롭게 GameManager가 생성되었을 때 정적 instance가 존재한다면 
            //GameManager가 중복이기에 제거
            Destroy(gameObject);
        }
        else
        {
            //만약 정적 instance가 null이라면, 해당 객체를 넘겨주고 DontDestroy
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
