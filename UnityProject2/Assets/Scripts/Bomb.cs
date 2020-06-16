using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public GameObject fxFactory;

    private void OnCollisionEnter(Collision collision)
    {
        //충돌시 폭발 이펙트 보여주기
        //이후 자기 자신 파괴
        GameObject fx = Instantiate(fxFactory);
        fx.transform.position = transform.position;

        Destroy(fx, 2.0f);
    }
}
