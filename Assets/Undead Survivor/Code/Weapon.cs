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
    public float speed;    // 회전 속도 or 연사 속도(값이 작으면 많이 발사)

    float timer;
    Player player;

    void Awake()
    {
        player = GameManager.instance.player;
    }

    void Update()
    {
        if (!GameManager.instance.isLive)
            return;

        // 회전
        switch (id)
        {
            case 0:
                transform.Rotate(Vector3.back * speed * Time.deltaTime);
                break;
            default:
                timer += Time.deltaTime;
                if (timer > speed) {
                    timer = 0f;
                    Fire();
                }
                break;
        }

        // .. Test Code..
        if (Input.GetButtonDown("Jump"))
        {
            LevelUp(10, 5);
        }
    }

    // 다른 스크립트에서 호출 예정
    public void LevelUp(float damage, int count)
    {
        this.damage = damage * Character.Damage;
        this.count += count;// + Character.Count

        // 근접무기의 경우 : 속성 변경화 '동시에' 배치도 필요하니 함수 호출
        if (id == 0)
            Batch();

        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    public void Init(ItemData data)
    {
        // Basic Set
        name = "Weapon " + data.itemId;
        transform.parent = player.transform;
        transform.localPosition = Vector3.zero;

        // Property Set
        id = data.itemId;
        damage = data.baseDamage * Character.Damage;
        count = data.baseCount + Character.Count;

        //!? ?
        for (int i = 0; i < GameManager.instance.pool.prefabs.Length; i++) {
            if (data.projectile == GameManager.instance.pool.prefabs[i]) {
                prefabId = i;
                break;
            }
        }

        // 무기 ID에 따라 로직을 분리할 Switch문 작성
        switch (id)
        {
            case 0:
                speed = 150f * Character.WeaponSpeed;
                Batch();
                break;
            default:
                speed = 0.5f * Character.WeaponRate;
                break;
        }

        // Hand Set - ItemData 참조 : 근거리 0, 원거리 1
        Hand hand = player.hands[(int)data.itemType];
        hand.spriter.sprite = data.hand;
        hand.gameObject.SetActive(true);

        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
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
            bullet.GetComponent<Bullet>().Init(damage, -100, Vector3.zero); // -100 is Infinity Per.
        }
    }

    void Fire()
    {
        //!? Transform 변수도 null체크가 가능하네?
        if (!player.scanner.nearestTarget)
            return;

        Vector3 targetPos = player.scanner.nearestTarget.position;
        Vector3 dir = targetPos - transform.position;
        dir = dir.normalized;

        Transform bullet = GameManager.instance.pool.Get(prefabId).transform;
        bullet.position = transform.position;
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);
        bullet.GetComponent<Bullet>().Init(damage, count, dir);

        AudioManager.instance.PlaySfx(AudioManager.Sfx.Range);
    }
}
