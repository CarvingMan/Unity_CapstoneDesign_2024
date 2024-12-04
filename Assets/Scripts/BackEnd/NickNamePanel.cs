using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NickNamePanel : MonoBehaviour
{
    /*
     * 뒤끝 회원가입 시 로그인 또한 자동으로 진행되기에 
     * 회원가입 후 SignUpPanel.cs에서 해당 패널을 Active하여 닉네임 설정
     * 설정후 로딩씬으로 이동
     * 해당 NickNamePanel은 필수적으로 닉네임을 설정해야하기에 닫기 버튼이 없다.
     */

    [Tooltip("닉네임InputField(TMP)")]
    [SerializeField] TMP_InputField m_inputNickName = null;

    //닉네임 설정 버튼
    [SerializeField]
    Button m_btnNickName = null;

    //결과 메세지
    [SerializeField]
    TextMeshProUGUI m_tmpResult = null;

    // Start is called before the first frame update
    void Start()
    {
        if(m_inputNickName == null)
        {
            Debug.LogError("m_inputNickName이 없습니다.");
        }

        if(m_tmpResult == null)
        {
            Debug.LogError("m_tmpResult가 없습니다." + gameObject.name);
        }

        if (gameObject.activeSelf == true)
        {
            gameObject.SetActive(false);
        }

        if (m_btnNickName == null)
        {
            Debug.LogError("m_btnNickName이 없습니다.");
        }
        else
        {
            m_btnNickName.onClick.AddListener(ClickNickNameBtn);
        }
    }

    void ClickNickNameBtn()
    {
        if(m_inputNickName.text.Length != 0)
        {
            string strMessage = m_tmpResult.text;
            //닉네임 설정 성공 여부와 동시에 결과 메세지를 out키워드를 통해 받아온다.
            bool isNickName = BackendManager.Instance.UpdateNickName(m_inputNickName.text, out strMessage);
            m_tmpResult.text = strMessage; //결과 메세지 출력
            
            //회원가입 성공 -> 닉네임 설정 성공 시
            if (isNickName) 
            {
                m_btnNickName.interactable = false;//더이상 닉네임 요청 할 수 없게 비활성화
                //로딩씬 이동
                GameManager.Instance.LoadSceneWithTime("LoadingScene", 0.5f);
            }
        }
        else
        {
            m_tmpResult.text = "사용하실 닉네임을 입력해주세요";
        }
    }
}
