using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoints;
    public SpawnData[] spawnData;

    int level;
    float timer;

    void Awake()
    {
        //GetComponentsInChildren : 본인 + 자식들
        spawnPoints = GetComponentsInChildren<Transform>();
    }

    void Update()
    {
        timer += Time.deltaTime;
        level = Mathf.Min(Mathf.FloorToInt(GameManager.instance.gameTime / 10f), spawnData.Length - 1);

        if (timer > spawnData[level].spawnTime)
        {
            Spawn();
            timer = 0;
        }

        void Spawn()
        {
            GameObject enemy = GameManager.instance.pool.Get(0);
            enemy.transform.position = spawnPoints[Random.Range(1, spawnPoints.Length)].position;
            enemy.GetComponent<Enemy>().Init(spawnData[level]);
        }
    }
}

[System.Serializable]
public class SpawnData
{
    // 추가할 속성들 : 소환시간(시간 간격), 스프라이트 타입, 체력, 속도
    public float spawnTime;
    public int spriteType;
    public int health;
    public float speed;
}