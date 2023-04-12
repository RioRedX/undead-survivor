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
    Collider2D coll;
    Animator anim;
    SpriteRenderer spriter;
    WaitForFixedUpdate wait;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        spriter = GetComponent<SpriteRenderer>();
        wait = new WaitForFixedUpdate();
    }
    
    // 물리관련 == rigid 사용
    void FixedUpdate()
    {
        if (!GameManager.instance.isLive)
            return;

        if (!isLive || anim.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
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
        if (!GameManager.instance.isLive)
            return;

        if (!isLive)
            return;

        spriter.flipX = target.position.x < rigid.position.x;
    }

    // 유니티 이벤트
    void OnEnable()
    {
        target = GameManager.instance.player.GetComponent<Rigidbody2D>();
        // 생존여부와 체력 초기화
        isLive = true;
        coll.enabled = true;
        rigid.simulated = true;
        spriter.sortingOrder = 2;
        anim.SetBool("Dead", false);
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
        if (!collision.CompareTag("Bullet") || !isLive) // isLive : 죽은 후 빠르게 충돌시 else부분이 2번 이상 호출 가능 
            return;

        health -= collision.GetComponent<Bullet>().damage;

        if (health > 0)
        {
            // .. Live, Hit Action
            // 애니메이션, 넉백 추가
            anim.SetTrigger("Hit");

            StartCoroutine(KnockBack()); // "KnockBack"도 가능
        }
        else
        {
            // .. Die
            //! => 재활용하기 위해선 값을 원래로 돌려놔야한다.
            isLive = false;
            coll.enabled = false;
            rigid.simulated = false;
            spriter.sortingOrder = 1;   // 다른 적 아래로
            anim.SetBool("Dead", true);
            GameManager.instance.kill++;
            GameManager.instance.GetExp();
        }
    }

    IEnumerator KnockBack()
    {
        // yield return null; <- 1 프레임 쉬기
        // yield return new WaitForSeconds(2f); <- 2초 쉬기. *new를 자주 쓰면 최적화에 Bad=> 함수화해서 사용할 것
        yield return wait; // 하나의 물리 프레임을 딜레이(1 프레임 쉬기)
        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 dirVec = transform.position - playerPos;
        rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);


    }

    void Dead()
    {
        gameObject.SetActive(false);
    }
}
