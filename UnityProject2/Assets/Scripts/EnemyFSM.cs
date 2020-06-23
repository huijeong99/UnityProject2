using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFSM : MonoBehaviour
{
    enum EnemyState
    {
        Idle,Move,Attack,Return,Damaged,Die
    }

    EnemyState state;

    Transform player;
    Vector3 startPoint;
    CharacterController cc;

    //애니매이션 컨트롤러
    Animator anim;

    int hp = 100;
    int att = 5;
    float speed = 5.0f;

    /// <summary>
    /// 유용한 기능
    /// </summary>

    //region 사이를 기준으로 책갈피 형태로 접었다 펼 수 있다
    #region "Idle 상태에 필요한 변수들"

    #endregion

    #region "Nove 상태에 필요한 변수들"
    public float moveRange = 30.0f; //20범위 내로 들어오면 이동
    public float findRange = 15.0f; //플레이어 인식범위

    #endregion

    #region "Attack 상태에 필요한 변수들"
    public float attackRange = 2.0f;//공격 가능 범위
    float timer=0.0f;
    float attTime = 5.0f;
    #endregion


    #region "Return 상태에 필요한 변수들"
    Quaternion startRotation;
    #endregion

    #region "Damaged 상태에 필요한 변수들"
    #endregion

    #region "Die 상태에 필요한 변수들"
    #endregion

    // Start is called before the first frame update
    void Start()
    {

        state = EnemyState.Idle;
        startPoint = transform.position;
        startRotation = transform.rotation;
        player = GameObject.Find("Player").transform;//플레이어 불러오기
        cc = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();  //자식개체에서 애니메이터 가져오기
    }

 

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case EnemyState.Idle:
                Idle();
                break;
            case EnemyState.Move:
                Move();
                break;
            case EnemyState.Attack:
                Attack();
                break;
            case EnemyState.Return:
                Return ();
                break;
            case EnemyState.Damaged:
                Damaged();
                break;
            case EnemyState.Die:
                Die();
                break;
            default:
                break;
        }
    }

    private void Idle()
    {
        //플레이어와 일정범위가 되면 이동상태로 변경
        //-플레이어 찾기(GameObject.Find("Player")로 찾기
        //-거리 20m(거리비교 :Distance,magnitute
        //-상태변경
        //-상태전환출력
        
        //Vector3 distance = transform.position - player.position;
        //float distance = dir.magnitude;
        //if(distance.magnitude < findRange)
        //if(distance < findRange)

        if (Vector3.Distance(transform.position, player.position) < findRange)
        {
            state = EnemyState.Move;
            print("상태전환 : Idle -> Move");

            //애니메이션
            anim.SetTrigger("Move");
        }
    }

    private void Move()
    {
        //플레이어를 향해 이동 후 공격범위 안에 들어오면 공격
        //-처음 위치에서 일정거리이상 멀어지면 원래 자리로 돌아감
        //-캐릭터 컨트롤러 이용
        //-상태변경
        //-상태전환출력

        if (Vector3.Distance(transform.position, startPoint) > moveRange)
        {
            state = EnemyState.Return;
            print("상태전환 : Move -> Return");

            //Vector3 dir = (player.position - transform.position).normalized;
            //transform.rotation = Quaternion.Lerp(transform.rotation,
            //  Quaternion.LookRotation(dir),
            //  10 * Time.deltaTime);
            //애니메이션
            anim.SetTrigger("Return");
        }
        else if (Vector3.Distance(transform.position,player.transform.position)>attackRange)
        {
            //플레이어를 추격
            //이동방향 (벡터의 뺄셈)
            Vector3 dir = (player.position - transform.position).normalized;
            //dir.Normalize();

            //몬스터가 백스텝으로 쫓아온다
            //몬스터가 타겟을 바라봄
            //방법1
            //transform.forward = dir;
            //방법2
            //transform.LookAt(player);

            //자연스러운 회전 처리
            //transform.forward = Vector3.Lerp(transform.forward, dir, 10 * Time.deltaTime);
            //여기도 문제가 있다 지금 회전처리를 하면서 벡터의 러프를 사용한 상태라서
            //타겟과 본인이 일직선상일경우 백덤블링으로 회전을 한다

            //최종적으로 자연스런 회전처리를 하려면 결국 쿼터니온을 사용해야 한다
            transform.rotation = Quaternion.Lerp(transform.rotation,
                Quaternion.LookRotation(dir),
                10 * Time.deltaTime);

            //캐릭터 컨트롤러를 이용해서 이동하기
            //cc.Move(dir * speed * Time.deltaTime);
            //중력이 적용안되는 문제가 있다

            //중력문제를 해결하기 위해서 심플무브를 사용한다
            //심플무브는 최소한의 물리가 적용되어 중력문제를 해결할 수 있다
            //단 내부적으로 시간처리를 하기때문에 
            //Time.deltaTime을 사용하지 않는다
            cc.SimpleMove(dir * speed);
            //애니메이션
            anim.SetTrigger("Move");
        }
        else//공격범위 ㅇㄴ에 들어왔을떄
        {
            state = EnemyState.Attack;
            print("상태전환 : Move -> Attack");
            anim.SetTrigger("Attack");
        }
    }

    private void Attack()
    {
        //1. 플레이어가 공격범위 안에 있다면 일정한 시간 간격으로 플레이어 공격
        //2. 플레이어가 공격범위를 벗어나면 이동상태(재추격)로 변경
        //- 공격범위 1미터
        //- 상태변경
        //- 상태전환 출력

        //공격범위안에 들어옴
        if (Vector3.Distance(transform.position, player.position) < attackRange)
        {
            //일정 시간마다 플레이어를 공격하기
            timer += Time.deltaTime;
            if (timer > attTime)
            {
                print("공격");
                //플레이어의 필요한 스크립트 컴포넌트를 가져와서 데미지를 주면 된다
                //player.GetComponent<PlayerMove>().hitDamage(att);

                //타이머 초기화
                timer = 0f;
                anim.SetTrigger("Attack");
            }
        }
        else//현재상태를 무브로 전환하기 (재추격)
        {
            state = EnemyState.Move;
            print("상태전환 : Attack -> Move");
            //타이머 초기화
            timer = 0f;
            anim.SetTrigger("Move");
        }

    }

    private void Return()
    {
        //1. 몬스터가 플레이어를 추격하더라도 처음 위치에서 일정 범위를 벗어나면 다시 돌아옴
        //- 처음위치에서 일정범위 30미터
        //- 상태변경
        //- 상태전환 출력

        //시작위치까지 도달하지 않을때는 이동
        //도착하면 대기상태로 변경
        if (Vector3.Distance(transform.position, startPoint) > 0.1)
        {
            Vector3 dir = (startPoint - transform.position).normalized;
            anim.SetTrigger("Return");
            cc.SimpleMove(dir * speed);
            
        }
        else
        {
            //위치값을 초기값으로 
            transform.position = startPoint;
            //transform.rotation = startRotation;
            transform.rotation = Quaternion.identity;//0으로 초기화
            state = EnemyState.Idle;
            print("상태전환 : Return -> Idle");
            anim.SetTrigger("Idle");
        }
    }

    public void HitDamage(int value)
    {
        //예외처리
        //피격상태이거나, 죽은 상태일때는 데미지 중첩으로 주지 않는다
        if (state == EnemyState.Damaged || state == EnemyState.Die) return;

        //체력깍기
        hp -= value;

        //몬스터의 체력이 1이상이면 피격상태
        if (hp > 0)
        {
            state = EnemyState.Damaged;
            print("상태전환 : AnyState -> Damaged");
            print("HP : " + hp);

            Damaged();
        }
        //0이하이면 죽음상태
        else
        {
            state = EnemyState.Die;
            print("상태전환 : AnyState -> Die");

            Die();
        }
    }

    private void Damaged()
    {
        //코루틴을 사용하자
        //1. 몬스터 체력이 1이상
        //2. 다시 이전상태로 변경
        //- 상태변경
        //- 상태전환 출력

        //피격 상태를 처리하기 위한 코루틴을 실행한다
        StartCoroutine(DamagedProc());
    }

    IEnumerator DamagedProc()
    {
        //피격모션 시간만큼 기다리기
        yield return new WaitForSeconds(1.0f);
        //현재상태를 이동으로 전환
        state = EnemyState.Move;
        print("상태전환 : Damaged -> Move");
    }

    private void Die()
    {
        //코루틴을 사용하자
        //1. 체력이 0이하
        //2. 몬스터 오브젝트 삭제
        //- 상태변경
        //- 상태전환 출력 (죽었다)

        //진행중인 모든 코루틴은 정지한다
        StopAllCoroutines();

        //죽음상태를 처리하기 위한 코루틴 실행
        StartCoroutine(DieProc());
    }

    IEnumerator DieProc()
    {
        //캐릭터컨트롤러 비활성화
        cc.enabled = false;

        //2초후에 자기자신을 제거한다
        yield return new WaitForSeconds(2.0f);
        print("죽었다");
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, findRange);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, moveRange);
    }
}
