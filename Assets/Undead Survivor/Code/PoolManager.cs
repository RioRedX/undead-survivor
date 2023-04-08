using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    // .. 프리펩들을 보관할 변수
    public GameObject[] prefabs;

    // .. 풀 담당을 하는 리스트들
    List<GameObject>[] pools;

    void Awake()
    {
        // 풀을 담는 '배열' 초기화
        pools = new List<GameObject>[prefabs.Length];

        // '배열' 안의 각각의 리스트도 초기화
        for (int i = 0; i < pools.Length; i++)
        {
            pools[i] = new List<GameObject>();
        }
    }

    public GameObject Get(int index)
    {
        // 놀고있는 프리팹을 select한다.
        GameObject select = null;

        // ... 선택한 풀의 놀고 있는 게임 오브젝트 접근
        foreach (GameObject item in pools[index])
        {
            if (!item.activeSelf)
            {
                // ... 발견하면 select 변수에 할당
                select = item;
                select.SetActive(true);
                break;
            }
        }

        // ... 못 찾았으면?
        if (!select)
        {
            // .. 새롭게 생성하고  select 변수에 할당
            select = Instantiate(prefabs[index], transform);
            pools[index].Add(select);
        }

        return select;
    }
}
