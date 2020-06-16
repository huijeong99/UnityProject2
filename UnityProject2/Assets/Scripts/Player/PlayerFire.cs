using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    public GameObject fireFoint;

    public GameObject bulletImpactFactory;
    public GameObject bombFactory;

    float throwPower = 5.0f;

    // Update is called once per frame
    void Update()
    {
        Fire();
    }

    private void Fire()
    {   //마우스 왼쪽버튼 클릭시 레이캐스트로 총알발사
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = new Ray(Camera.main.transform.position,Camera.main.transform.forward);//카메라 전방 방향으로 발사
            RaycastHit hitInfo;
            
            //레이어 마스크를 사용해 충돌처리(최적화)
            //유니티 내부적으로 속도향상을 위해 비트연산처리를 함
            //총 32비트를 사용하기 떄문에 레이어도 32개까지 추가기능함
            int layer = gameObject.layer;
            //layer = 1 << 8;

            layer = 1 << 8 | 1 << 9 | 1 << 12;//(or연산)

            //if (Physics.Raycast(ray, out hitInfo, 100, layer))//레이어에 해당하는 개체만 충돌한다
            if (Physics.Raycast(ray, out hitInfo, 100, ~layer))//레이어에 해당하는 개체만 충돌에서 제외
            {
                // print("충돌 오브젝트 : ", hitInfo.collider.name);
                //충돌지점에 이펙트 만들어주기
                GameObject bulletImpact = Instantiate(bulletImpactFactory);
                //부딪힌 지점 설정
                bulletImpact.transform.position = hitInfo.point;
                //부딪힌 방향으로 이펙트가 튀도록 만들기
                bulletImpact.transform.forward = hitInfo.normal;
            }
        }
        //마우스 오른쪽 클릭시 수류탄투척
        if (Input.GetMouseButtonDown(1))
        {
            
                GameObject bomb = Instantiate(bombFactory);
                bomb.transform.position= fireFoint.transform.position;

            Rigidbody rb = bomb.GetComponent<Rigidbody>();
            //전방으로 물리적인 힘 가하기
            //forceMode.Acceleration=연속적인 힘을 가한다(질량영향X)
            //forceMode.force=연속적인 힘을 가한다(질량영향O)
            //forceMode.Impulse=순간적인 힘을 가한다(질량의 영향을 받음)
            //forceMode.VelocityChange=순간적인 힘을 가한다(질량의 영향을 받지 않는다)

            //45도 각도로 발사
            Vector3 dir = Camera.main.transform.forward+Camera.main.transform.up;
            dir.Normalize();
           // rb.AddForce(Camera.main.transform.forward * throwPower,ForceMode.Impulse);
            rb.AddForce(dir,ForceMode.Impulse);
        }

    }
}
