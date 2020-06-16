using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] float speed = 5.0f;//플레이어 속도
    CharacterController cc;

    public float gravity = -20;
    float velocityY;
    float jumpPower = 10;
    int jumpCount = 2;

    // Start is called before the first frame update
    void Start()
    {
        //캐릭터 컨트롤러 가져오기
        cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        //플레이어 이동
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 dir = new Vector3(h, 0, v);
        //dir.Normalize(); //대각선이동 속도를 상하좌우속도와 동일하게 만들기
        //게임에 따라 일부로 대각선은 빠르게 이동하도록 하는 경우도 있다
        //이럴때는 벡터의 정규화(노말라이즈)를 하면 안된다
        //transform.Translate(dir * speed * Time.deltaTime);

        //카메라가 보는 방향으로 이동해야 한다
        dir = Camera.main.transform.TransformDirection(dir);
        //transform.Translate(dir * speed * Time.deltaTime);

        //심각한 문제 : 하늘 날라다님, 땅 뚫음, 충돌처리 안됨
        //캐릭터컨트롤러 컴포넌트를 사용한다!!
        //캐릭터컨트롤러는 충돌감지만 하고 물리가 적용안된다
        //따라서 충돌감지를 하기 위해서는 반드시
        //캐릭터컨트롤러 컴포넌트가 제공해주는 함수로 이동처리해야 한다
        //cc.Move(dir * speed * Time.deltaTime);

        //중력적용하기
        //velocityY += gravity * Time.deltaTime;
        //dir.y = velocityY;
        //cc.Move(dir * speed * Time.deltaTime);

        //캐릭터 점프
        //점프버튼을 누르면 수직속도에 점프파워를 넣는다
        //땅에 닿으면 0으로 초기화
        //if(cc.isGrounded)//땅에 닿았냐?
        //{
        //}
        //CollisionFlags.Above;
        //CollisionFlags.Below;
        //CollisionFlags.Sides;
        //if (cc.collisionFlags == CollisionFlags.Below)//땅에 닿았냐?
        //{
        //    velocityY = 0;
        //}
        //if(Input.GetButtonDown("Jump"))
        //{
        //    velocityY = jumpPower;
        //}

        //2단 점프만 가능하도록 만들기
        //if(cc.isGrounded) //이상하게 반응속도가 느리네?
        //{
        //    velocityY = 0;
        //    jumpCount = 0;
        //}
        if (cc.collisionFlags == CollisionFlags.Below)
        {
            velocityY = 0;
            jumpCount = 0;
        }
        else
        {
            //땅에 닿지 않은 상태이기때문에 중력적용하기
            velocityY += gravity * Time.deltaTime;
            dir.y = velocityY;
        }
        if (Input.GetButtonDown("Jump") && jumpCount < 2)
        {
            jumpCount++;
            velocityY = jumpPower;
        }
        //중력적용 이동
        cc.Move(dir * speed * Time.deltaTime);

    }
}

