using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    public float health;    // 현재 체력
    public float maxHealth; // 최대 체력
    public RuntimeAnimatorController[] animCon;
    public Rigidbody2D target;  //? 왜 GameObject로 안하고 Rigidbody2D로 선언했을까?

    bool isLive;

    Rigidbody2D rigid;
    Animator anim;
    SpriteRenderer spriter;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
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

    // 유니티 이벤트
    void OnEnable()
    {
        target = GameManager.instance.player.GetComponent<Rigidbody2D>();
        // 생존여부와 체력 초기화
        isLive = true;
        //health = maxHealth;
    }

    // 초기 속성을 적용하는 함수 추가
    public void Init(SpawnData data)
    {
        anim.runtimeAnimatorController = animCon[data.spriteType];
        speed = data.speed;
        maxHealth = data.health;
        health = data.health;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Bullet"))
            return;

        health -= collision.GetComponent<Bullet>().damage;

        if (health > 0)
        {
            // .. Live, Hit Action
        }
        else
        {
            // .. Die
            Dead();
        }
    }

    void Dead()
    {
        gameObject.SetActive(false);
    }
}
