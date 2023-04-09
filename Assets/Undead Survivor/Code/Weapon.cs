using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    // 5가지 변수 필요
    // 무기ID, 프리펩ID, 데미지, 개수, 속도
    public int id;  // 무기 ID
    public int prefabId;   
    public float damage;
    public int count;      // 무기 갯수?
    public float speed;    // 회전 속도?

    void Start()
    {
        Init();
    }

    void Update()
    {
        // 회전
        switch (id)
        {
            case 0:
                transform.Rotate(Vector3.back * speed * Time.deltaTime);
                break;
            default:
                break;
        }

        // .. Test Code..
        if (Input.GetButtonDown("Jump"))
        {
            LevelUp(20, 5);
        }
    }

    // 다른 스크립트에서 호출 예정
    public void LevelUp(float damage, int count)
    {
        this.damage = damage;
        this.count += count;

        // 근접무기의 경우 : 속성 변경화 '동시에' 배치도 필요하니 함수 호출
        if (id == 0)
            Batch();
    }

    public void Init()
    {
        // 무기 ID에 따라 로직을 분리할 Switch문 작성
        switch (id)
        {
            case 0:
                speed = 150f;
                Batch();
                break;
            default:
                break;
        }
    }

    void Batch()
    {
        for (int i = 0; i < count; i++)
        {
            // Why Trasform Use? 부모를 Weapon 오브젝트로 바꾸기 위해 
            Transform bullet;
            if (i < transform.childCount) {
                // i(index)가 아직 chiledCoutn 번위 내라면 GetChild함수로 가져오기 
                // => 이미 있다면, 기존 것 사용
                bullet = transform.GetChild(i);
            }
            else {
                bullet = GameManager.instance.pool.Get(prefabId).transform;
                bullet.parent = transform;

            }


            // 배치하면서 먼저 위치, 회전 초기화 하기
            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;

            Vector3 rotVec = Vector3.forward * 360 * i / count;
            bullet.Rotate(rotVec);
            bullet.Translate(bullet.up * 1.5f, Space.World);
            bullet.GetComponent<Bullet>().Init(damage, -1); // -1 is Infinity Per.
        }
    }
}
