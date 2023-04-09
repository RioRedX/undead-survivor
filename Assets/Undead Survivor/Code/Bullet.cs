using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // 데미지, 관통 변수
    public float damage;
    public int per;

    // 변수 초기화 함수
    public void Init(float damage, int per)
    {
        this.damage = damage;
        this.per = per;
    }
}
