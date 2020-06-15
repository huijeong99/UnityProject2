using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] float speed = 5.0f;//플레이어 속도
    CharacterController CC;

    // Start is called before the first frame update
    void Start()
    {
        //캐릭터 컨트롤러 가져오기
        CC = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 dir = new Vector3(h, 0, v);
        //dir.Normalize();//방향만 구해준다(노멀라이즈 해주지 않을 경우 대각선 이동시 캐릭터 이속이 더 빨라진다)
        //
        //transform.position += dir * speed * Time.deltaTime;

        //카메라가 보는 방향으로 이동하기
        dir = Camera.main.transform.TransformDirection(dir);//카메라가 보는 방향으로 넣은 벡터의 방향을 맞춰준다
        //transform.Translate(dir * speed * Time.deltaTime);

        //문제: 플레이어가 공중으로 뜨거나 땅으로 가라앉음,충돌처리 안됨
        CC.Move(dir * speed * Time.deltaTime);//컴포넌트가 붙어있어도 translate를 사용하면 충돌처리가 되지 않는다

    }
}
