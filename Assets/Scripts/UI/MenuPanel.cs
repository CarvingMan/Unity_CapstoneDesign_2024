using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuPanel : MonoBehaviour
{
    /*
     * GameScene의 FieldUI 패널에서 Menu 버튼을 누를 시 해당 패널이 활성화 되며 timeScale이 0이 된다.
     */

    [SerializeField]
    Button m_btnClose = null; //메뉴창 닫기 버튼
    [SerializeField] 
    Button m_btnLogOutBtn = null; //저장 및 로그아웃 버튼

    //저장 및 로그아웃 버튼 누를 시 BackendManager에서 비동기로 저장 및 로그아웃을 시행할 시
    //FadeOut을 할 수 있도록 FadeInOutPanel.cs가 부착된 패널 사용
    [Tooltip("FadeInOutPanel.cs이 부착된 panel")]
    [SerializeField] GameObject m_objFadeInOutPanel = null; 

    // Start is called before the first frame update
    void Start()
    {
        if(m_objFadeInOutPanel == null)
        {
            Debug.LogError("m_objFadeInOutPanel이 없습니다." + gameObject.name);
        }

        if(m_btnClose != null)
        {
            m_btnClose.onClick.AddListener(ClickCloseBtn);
        }
        else
        {
            Debug.LogError("m_btnClose가 없습니다."+gameObject.name);
        }

        if(m_btnLogOutBtn != null)
        {
            m_btnLogOutBtn.onClick.AddListener(ClickLogOutBtn);
        }
        else
        {
            Debug.LogError("m_btnLogOutBtn이 없습니다." + gameObject.name);
        }

        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);  
        }
    }

    //m_btnClose onClick AddListener에 추가할 함수
    void ClickCloseBtn()
    {
        //해당 MenuPanel은 FieldUIPanel의 Menu Butten을 눌렀을때 Active가 되면서 timeScale이 0이된다.
        //CloseBtn으로 MenuPanel을 닫을 시 1로 되돌린다.
        if(Time.timeScale == 0)
        {
            Time.timeScale = 1; 
        }

        gameObject.SetActive(false);
    }

    //m_btnLogOutBtn onClick AddListener에 추가할 함수
    //BackendManger에서 비동기로 저장 및 로그아웃 후 타이틀 화면으로 이동
    void ClickLogOutBtn()
    {
        if (m_btnClose != null)
        {
            //저장 및 로그아웃 버튼을 눌렀을 때, Close버튼을 눌러 timeScale이 1이 되어 게임이 동작하는 것을 방지
            m_btnClose.interactable = false; //Close버튼 비활성화
        }

        //FadeOut
        if (m_objFadeInOutPanel != null)
        {
            m_objFadeInOutPanel.GetComponent<FadeInOutPanel>().FadeOutPanel();
        }

        //BackendManager의 SaveUserData의 매개변수 isLogOut을 true로 넘겨주게되면,
        //비동기 정보 업데이트가 완료될시 -> 비동기 로그아웃 -> TileScene이동
        BackendManager.Instance.SaveUserData(true);

    }
}
