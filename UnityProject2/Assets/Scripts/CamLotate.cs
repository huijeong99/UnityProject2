using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamLotate : MonoBehaviour
{
    public float speed = 150;//회전처리시에는 각도로 따지기 때문에 값을 크게 줌
    float angleX, angleY;
    //회전각도 직접 제어


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Rotate();   
    }

    private void Rotate()
    {
        float h = Input.GetAxis("Mouse X");
        float v = Input.GetAxis("Mouse Y");
        // Vector3 dir = new Vector3(h, v, 0);(X)
        //Vector3 dir = new Vector3(-v, h, 0);//축을 기반으로 하기때문
        //transform.Rotate(dir * speed * Time.deltaTime);

        //유니티 엔진 내에서 제공해주는 함수를 사용함에 있어서 Transform 함수는 자주 사용하지만
        //Rotate함수는 제어하기 힘들기때문에 사용하기 어렵다(짐벌락현상)
        //인스펙터 창의 로테이션은 사용자가 인식하기 편하도록 오일러 좌표로 인식되지만 내부적으로는 쿼터니온 회전처리가 되고있다

        //P=PQ_vt;
        //transform.eulerAngles: 오일러값을 입력하면 내부에서 쿼터니온으로 변환해준다.
        //transform.eulerAngles += dir * speed * Time.deltaTime;
        //카메라 문제 : -90~90 사이로 각도가 고정되어 움직이지 않는다.
        //>>직접 회전각도를 재현해서 처리한다
        //Vector3 angle = transform.eulerAngles;
        //angle+= dir * speed * Time.deltaTime;
        //if (angle.x > 60) angle.x = 60;
        //if (angle.x < -60) angle.x = -60;//유니티는 내부적으로 -각도에는 360도를 더해버린다(-60도==300도)
        //transform.eulerAngles = angle;

        //회전각도를 직접 제어
        angleX += h * speed * Time.deltaTime;
        angleY += v * speed * Time.deltaTime;
        angleY = Mathf.Clamp(angleY, -60, 60);
        transform.eulerAngles = new Vector3(-angleY, angleX, 0);
    }
}
