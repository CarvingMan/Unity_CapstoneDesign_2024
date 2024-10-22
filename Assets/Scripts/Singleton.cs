using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    /*
     * GameMnager 등 Singleton 디자인 패턴을 사용하기위해 제너릭 싱글톤 클래스 정의
     * 해당 제너릭 클래스를 상속 받으면 손쉽게 싱글톤으로 사용가능 하다.
     * 만약 2개 이상의 서로다른 타입의 Singleton instance가 필요할때 가능한 방법
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
                    //Find 함수는 성능상 좋지 않아 사용을 되도록 지양해야하기에,
                    //최초 초기화 시 한 번만 찾아 오도록 설정
                    //다만 Awake()에서 설정해 주기에 해당 로직이 실행될 경우는 거의 없다.
                    instance = FindObjectOfType<T>();
                    if (instance == null)
                    {
                        //만약 하이어라키 창에 없으면 새롭게 생성 해 준다.
                        GameObject obj = new GameObject(typeof(T).Name, typeof(T));
                        instance = obj.GetComponent<T>();
                    }
                }
                return instance;
            }
            else
            {
                //이미 인스턴스가 존재할 시 instance 반환
                return instance;
            }
        }
    }

    //해당 제너릭 class를 상속받을 때 MonoBehavior 또한 상속되므로
    //자식이 Awake()를 사용하면 부모 Awake()가 호출이 안된다.
    //따라서 vitual로 설정하여 자식이 Awake()를 안쓰면 그대로 호출하고 자식이 Awake()를 사용할 시 
    //override를 하여 base.Awake()로 해당 부모의 Awake()를 호출 해 주어야 한다.
    public virtual void Awake()
    {
        //프로퍼티 get할 시 instance가 설정되기에 아직 null이라면 instance설정 후 DontDestroy
        if (instance == null)
        {
            instance = this as T;//인스턴스화 될 때 this를 제너릭 T타입으로 캐스팅하여 instance로 넣어준다.
            DontDestroyOnLoad(gameObject);//씬 이동시에도 해당 오브젝트 삭제 금지
        }
        else
        {
            //이미 instance가 존재하므로 삭제해준다.
            Destroy(gameObject);
        }
    }


    
}
