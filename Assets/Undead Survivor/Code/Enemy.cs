using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    public Rigidbody2D target;

    bool isLive = true;

    Rigidbody2D rigid;
    SpriteRenderer spriter;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
    }
    
    // 물리관련 == rigid 사용
    void FixedUpdate()
    {
        if (!isLive)
            return;

        Vector2 dirVec = target.position - rigid.position;
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);   // 기본 좌표이동
        // 충돌시 밀려남(속도가 있다는 뜻) 처리
        rigid.velocity = Vector2.zero;
    }

    // 후처리
    void LateUpdate()
    {
        spriter.flipX = target.position.x < rigid.position.x;
    }
}
