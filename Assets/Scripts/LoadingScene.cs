using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class LoadingScene : MonoBehaviour
{
    /*
     * 비동기로 씬로딩 후 완료 시 GameScene으로 이동
     * 다만 이전에 GameManager.Instance.IsInitUserData가 flase이라면, 앱 실행 후
     * 아직 서버에서 해당 유저의 스테이지, 능력치, 최종 접속 시간 등 데이터를 불러오지 않은 상태로
     * 해당 작업을 GameManager에서 LoadingScene으로 이동과 동시에 실행하게 된다.
        GameManager.Instance.IsInitUserData가 true가 될 시
        비동기 씬 로드로 끝난다면 GameScene으로 이동시킨다.
     */


    [Tooltip("로딩메세지")]
    [SerializeField]
    TextMeshProUGUI m_tmpMessage = null; //로딩 상태 주기적으로 tween할 메세지

    [SerializeField]
    GameObject m_objLoadingMob = null; //로딩중 화면에 애니메이션 재생할 오브젝트

    [SerializeField]
    GameObject m_objFadeInOutPanel = null; //FadeInOutPanel.cs가 부착된 패널

    Sequence m_seqMessage = null; //로딩 메세지 tween 시퀀스

    //코루틴 제어 변수
    bool m_isCorWaitUserData = false;
    bool m_isCorAsyncScene = false;

    bool m_isUserData = false;//GameManager.Instance.IsInitUserData가 true가 될 시 ture 로 변환

    // Start is called before the first frame update
    void Start()
    {
        if(m_objLoadingMob == null)
        {
            Debug.LogError("m_objLoadingMob이 없습니다." + gameObject.name);
        }  

        if(m_tmpMessage == null)
        {
            Debug.LogError("m_tmpMessage가 없습니다.");
        }

        if (m_objFadeInOutPanel == null) 
        {
            Debug.LogError("m_objFadeInOutPanel이 없습니다.");
        }

        m_isUserData = GameManager.Instance.IsInitUserData;


        //코루틴 실행
        StartCoroutine(CorWaitUserData());;
        StartCoroutine(CorAsyncScene());

        //GameManager에서 m_isInitUserData가 false일 시 호출
        //BackendManager에서 비동기로 USER_DATA정보를 받아오고 GameManager에게 넘겨준다.
        //GameManager에서 값 할당이 끝날 시 m_isInitUserData를 true로 바꾼다.
        if(m_isUserData == false)
        {
            BackendManager.Instance.LoadUserData();
        }
    }

    //m_tmpMessage 트윈
    void SetMessageTween(string strMessage , float fTime)
    {
        if (m_seqMessage != null && m_seqMessage.IsActive())
        {
            //혹시 이전 시퀀스가 실행 중일 시 Kill
            m_seqMessage.Kill();
        }

        //fTime 동안 ""에서 strMessage로 변환하는 tween
        Tween tween = DOTween.To(() => "", str => m_tmpMessage.text = str, strMessage, fTime);
        m_seqMessage = DOTween.Sequence();
        m_seqMessage.Append(tween);
    }

    IEnumerator CorWaitUserData()
    {
        if (m_isCorWaitUserData)
        {
            yield break;
        }
        m_isCorWaitUserData = true;

        float fTime = 0;
        float fWait = 0.1f;
        string strMessage = "데이터 불러오는 중...";
        SetMessageTween(strMessage,2);
        while(GameManager.Instance.IsInitUserData == false)
        {
            if(fTime >= 5) //5초마다 다시 텍스트 tween 재생
            {
                fTime = 0;
                SetMessageTween(strMessage,2);
            }
            
            yield return new WaitForSeconds(fWait);
            fTime += fWait;
            
        }
        //Debug.Log(GameManager.Instance.IsInitUserData);
        //데이터 불러오기 완료 될 시
        //데이터 로드 되는 속도가 너무 빨라 시현시 위의 로직을 보기위해 기다림(불필요)
        yield return new WaitForSeconds(2); 
        m_isUserData = true;
    }

    IEnumerator CorAsyncScene()
    {
        if (m_isCorAsyncScene)
        {
            yield break;
        }
        m_isCorAsyncScene = true;

        //비동기로 GameScene을 로드하며 결과를 AsyncOperation 변수에 담는다.
        AsyncOperation operation = SceneManager.LoadSceneAsync("GameScene");
        operation.allowSceneActivation = false; //비동기 로드가 완료가 되었을때 일련의 작업을 위해 바로 씬변경 금지
        
        float fTime = 0;

        //유저데이터 로드와 비동기씬 로드가 전부 완료되었을 때까지 반복 대기
        while (true)
        {
            //유저데이터 로드와 비동기씬 로드가 전부 완료되었을 때까지 반복 대기
            //allowSceneActivation이 false로 되어있으면 0.9이상으로 안올라가고 기다리며 isDone도 false 유지한다.
            //따라서 0.9이상이 되면 탈출 후 allowSceneActive true로 바꾸어 주어야 한다.
            if (m_isUserData && operation.progress >= 0.9f)
            {
                break;
            }
            else if(operation.progress < 0.9f && m_isUserData) 
            {
                //만약 비동기씬은 완료 되지 않았지만 GameManager에서 유저 정보 로드 완료 시

                if (fTime >= 5)
                {
                    fTime = 0;
                    string strMessage = "Scene 불러오는 중...";
                    SetMessageTween(strMessage, 2);
                }
            }
            yield return new WaitForSeconds(0.1f);
            fTime += 0.1f;
            //Debug.Log("비동기 씬로드 상황" + operation.progress);
        }
        //전부 로드 되었을 시
        SetMessageTween("로딩 완료!", 0.5f);
        m_objLoadingMob.GetComponent<Animator>().SetBool("isDead", true);
        m_objFadeInOutPanel.GetComponent<FadeInOutPanel>().FadeOutPanel(); //panel FadeOut
        yield return new WaitForSeconds(2f);

        operation.allowSceneActivation = true; //씬 활성화

    }
}
