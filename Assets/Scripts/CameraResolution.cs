using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraResolution : MonoBehaviour
{
    /*
     *  Main Camera�� view port�� �����Ͽ� 
     *  ȭ���� Ŀ���ų� �پ� ������ 9:16���� �ػ� ������ �����ϱ� ���� ��ũ��Ʈ
     *  ���� ī�޶� ����
     *  Cavas�� RederMode�� MainCamera�� ����
     */
    // Start is called before the first frame update

    private void Awake()
    {
        //ī�޶� ������Ʈ get
        Camera camera = GetComponent<Camera>();
        //ī�޶��� viewport rect�� �����´�.
        //x,y = ������, �����ϴ�(0,0) , ���� ���(1,1)
        Rect recViewPort = camera.rect;

        //���μ��� ����(��Ⱦ ��)�� ���
        float fScreenAspectRatio = (float)Screen.width / Screen.height; //���� ��ũ���� ��Ⱦ��
        float fTargetAspectRatio = 9f / 16f; // ��ǥ ��Ⱦ��(�ش� ������ ���� ����)

        if(fScreenAspectRatio < fTargetAspectRatio)
        {
            //���� fTargetAspectRatio(���μ��� ����)�� �� ũ�ٸ�
            //���� ��ũ���� ���ΰ� �� ���ų� ���ΰ� �� ũ�⿡ �� �Ʒ��� ������ �����ؾ� �ϴ� ����̴�.
            float fHeight = fScreenAspectRatio / fTargetAspectRatio; 
            //���δ� ���� �� �� ���⿡ �����ϰ� ���θ� ������ �°� �� ����
            recViewPort.height = fHeight;
            //���� ������ ����
            recViewPort.y = (1 - fHeight) /2;
        }
        else
        {
            //���� ��ũ���� ���μ��� ��� ������ ���̰� ��ǥ ���� ���� �аų� ���ΰ� �� ���� ��
            //�¿� ���� ����
            float fWidth = fTargetAspectRatio / fScreenAspectRatio;
            //���δ� ���� ���� ����
            recViewPort.width = fWidth;
            recViewPort.x = (1 - fWidth) /2;   
        }
        //������ rect���� ī�޶� rect�� �� ����
        camera.rect = recViewPort;
    }



    //OnPreCull()�� ī�޶� ������ �Ǳ� ���� ȣ��ȴ�.
    //GL.Clear() : ȭ���� ���� �ʱ�ȭ
    //�ػ� ������ �ϸ鼭 UI�ܻ��� ���� ��� �����ϱ� ���� 
    //-> Canvas RenderMode : Screen Space - Camera�� �����ϸ� ĵ������ ViewPort�� ���� ���⿡ ������ �ܻ� X
    //�ش� ������ Clear Flags �� Sky Box or Solid Color �� ������ �ڵ����� �ʱ�ȭ �ϱ⿡ �ߺ�
    //���ʿ��� GPU ������ ���� �ϱ����� �ּ� ó��, 

    //private void OnPreCull()
    //{
    //    GL.Clear(true, true, Color.black);
    //}
}
