using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraResolution : MonoBehaviour
{
    /*
     *  Main Camera의 view port를 조정하여 
     *  화면이 커지거나 줄어 들더라고 9:16으로 해상도 비율을 조정하기 위한 스크립트
     *  메인 카메라에 부착
     *  Cavas의 RederMode는 MainCamera에 설정
     */
    // Start is called before the first frame update

    private void Awake()
    {
        //카메라 컴포넌트 get
        Camera camera = GetComponent<Camera>();
        //카메라의 viewport rect를 가져온다.
        //x,y = 시작점, 좌측하단(0,0) , 우측 상단(1,1)
        Rect recViewPort = camera.rect;

        //가로세로 비율(종횡 비)을 계산
        float fScreenAspectRatio = (float)Screen.width / Screen.height; //현재 스크린의 종횡비
        float fTargetAspectRatio = 9f / 16f; // 목표 종횡비(해당 비율로 변경 예정)

        if(fScreenAspectRatio < fTargetAspectRatio)
        {
            //만약 fTargetAspectRatio(가로세로 비율)가 더 크다면
            //현재 스크린의 가로가 더 좁거나 세로가 더 크기에 위 아래의 여백을 생성해야 하는 경우이다.
            float fHeight = fScreenAspectRatio / fTargetAspectRatio; 
            //가로는 변경 할 수 없기에 고정하고 세로를 비율에 맞게 재 조정
            recViewPort.height = fHeight;
            //시작 지점도 조정
            recViewPort.y = (1 - fHeight) /2;
        }
        else
        {
            //현재 스크린의 가로세로 대비 가로의 넓이가 목표 비율 보다 넓거나 세로가 더 좁을 때
            //좌우 여백 생성
            float fWidth = fTargetAspectRatio / fScreenAspectRatio;
            //세로는 고정 가로 조정
            recViewPort.width = fWidth;
            recViewPort.x = (1 - fWidth) /2;   
        }
        //조정된 rect값을 카메라 rect로 재 설정
        camera.rect = recViewPort;
    }



    //OnPreCull()은 카메라가 렌더링 되기 전에 호출된다.
    //GL.Clear() : 화면의 버퍼 초기화
    //해상도 변경을 하면서 UI잔상이 생길 경우 제거하기 위함 
    //-> Canvas RenderMode : Screen Space - Camera로 설정하면 캔버스가 ViewPort에 맞춰 지기에 실행중 잔상 X
    //해당 로직은 Clear Flags 를 Sky Box or Solid Color 로 설정시 자동으로 초기화 하기에 중복
    //불필요한 GPU 연산을 제외 하기위해 주석 처리, 

    //private void OnPreCull()
    //{
    //    GL.Clear(true, true, Color.black);
    //}
}
