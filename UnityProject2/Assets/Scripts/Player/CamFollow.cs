using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    //플레이어에 바로 카메라를 붙여 이동시켜도 상관없지만 게임 내 카메라를 이용한 드라마틱한 연출이 불가능해진다
    //1<>3인칭 전환 및 카메라 팔로우 효과

    public Transform target;    //플레이어의 위치
    public Transform camerPos;  //3인칭 카메라 위치
    public float followSpeed = 10.0f;
    bool outSight = false;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //카메라 위치를 강제로 타겟 위치에 고정해준다(카메라 순간이동)
        //transform.position = target.position;

        //플레이어를 부드럽게 따라가게 만들기
        //FollowTarget();
        SwitchSight();

        //C(Camera)를 눌렀을 때
        if (Input.GetKey(KeyCode.C))
        {
            //현재 시점이 1인칭일때
            if (outSight == false)
            {
                outSight = true;
            }

            //현재 시점이 3인칭일때
            else
            {
                outSight = false;
            }
        }
    }

    private void SwitchSight()
    {

        //현재 시점이 1인칭일때
        if (outSight == false)
        {
            MakeDistance();
        }

        //현재 시점이 3인칭일때
        else
        {
            FollowTarget();
        }
    }

    //1인칭에서 3인칭 전환
    private void MakeDistance()
    {
        Vector3 dir = camerPos.position - transform.position;
        dir.Normalize();
        transform.Translate(dir * followSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, camerPos.position) < 1.0f)
        {
            transform.position = camerPos.position;
        }
    }

    //3인칭에서 1인칭 전환
    private void FollowTarget()
    {
        //타겟방향구하기(벡터의 뺄셈)
        //방향=타겟-자기자신(노멀라이즈)

        Vector3 dir = target.position - transform.position;
        dir.Normalize();
        transform.Translate(dir * followSpeed * Time.deltaTime);

        //문제점 해결: 카메라가 정확한 위치를 찾지 못하고 떨림
        if (Vector3.Distance(transform.position, target.transform.position) < 1.0f)
        {
            transform.position = target.position;
        }
    }
}
