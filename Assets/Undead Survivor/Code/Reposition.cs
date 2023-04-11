using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reposition : MonoBehaviour
{
    Collider2D coll;
    
    void Awake()
    {
        coll = GetComponent<Collider2D>();
    }

    // 조건 : 플레이어의 Area에서 벗어났을 경우 이동시켜준다.
    void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Area"))
            return;

        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 myPos = transform.position; // myPos -> mapPos 하지만, enemy에도 쓰인다면..

        float dirX = playerPos.x - myPos.x;
        float dirY = playerPos.y - myPos.y;

        float diffX = Mathf.Abs(dirX);
        float diffY = Mathf.Abs(dirY);

        dirX = dirX < 0 ? -1 : 1;
        dirY = dirY < 0 ? -1 : 1;

        // 이동키에 의향 방향 설정
        Vector3 playerDir = GameManager.instance.player.inputVec;
        //float dirX = playerDir.x < 0 ? -1 : 1;
        //float dirY = playerDir.y < 0 ? -1 : 1;

        switch (transform.tag)
        {
            case "Ground":
                if (diffX > diffY)
                {
                    transform.Translate(Vector3.right * dirX * 40);
                }
                else if(diffX < diffY)
                {
                    transform.Translate(Vector3.up * dirY * 40);
                }
                break;
            case "Enemy":
                // 추후 몬스터가 죽으면 콜라이더 비활성화
                if (coll.enabled)
                {
                    transform.Translate(playerDir*20 + new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f),0));
                }
                break;
        }
    }
}
