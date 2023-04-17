using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // 데미지, 관통 변수
    public float damage;
    public int per;

    Rigidbody2D rigid;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    // 변수 초기화 함수
    public void Init(float damage, int per, Vector3 dir)
    {
        this.damage = damage;
        this.per = per;

        if(per >= 0) {
            rigid.velocity = dir * 15f;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // 원거리 무기의 관통력 조절
        if (!collision.CompareTag("Enemy") || per == -100)
            return;

        per--;

        if (per < 0) {
            rigid.velocity = Vector2.zero;  
            gameObject.SetActive(false);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        // 해석 : Area가 아니거나 근접무기일 경우 나가랏!
        if (!collision.CompareTag("Area") || per == -100)
            return;

        // 아래 : Area이면서 원거리 무기일 경우
        gameObject.SetActive(false);
    }
}
