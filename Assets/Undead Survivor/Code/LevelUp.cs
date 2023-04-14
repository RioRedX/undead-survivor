using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUp : MonoBehaviour
{
    RectTransform rect;
    Item[] items;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        items = GetComponentsInChildren<Item>(true);
    }

    public void Show()
    {
        Next();
        rect.localScale = Vector3.one;
        GameManager.instance.Stop();
        AudioManager.instance.PlaySfx(AudioManager.Sfx.LevelUp);
        AudioManager.instance.EffectBgm(true);//but bgm뿐 아니라 effect도 소리를 없애니.. AudioSource에서 처리
    }

    public void Hide()
    {
        rect.localScale = Vector3.zero;
        GameManager.instance.Resume();
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);
        AudioManager.instance.EffectBgm(false);
    }

    public void Select(int index) 
    {
        items[index].OnClick();
    }

    void Next()
    {
        // 1. 모든 아이템 비활성화
        foreach (Item item in items) {
            item.gameObject.SetActive(false);
        }

        // 2. 그 중에서 랜덤 3개 아이템 활성화
        // 0,1 : 무기, 2,3 : 코어, 4 : 소비
        int[] ran = new int[3];
        while (true) {
            ran[0] = Random.Range(0, items.Length);
            ran[1] = Random.Range(0, items.Length);
            ran[2] = Random.Range(0, items.Length);

            if (ran[0] != ran[1] && ran[1] != ran[2] && ran[0] != ran[2])
                break;
        }

        for (int i = 0; i < ran.Length; i++) {
            Item ranItem = items[ran[i]];
            // 3. 만렙 아이템의 경우는 소비아이템으로 대체
            if(ranItem.level == ranItem.data.damages.Length) {
                items[4].gameObject.SetActive(true);
                //items[Random.Range(4,7)].gameObject.SetActive(true); // 소비 아이템이 여러개일 경우 4~6, 3개다.
            }
            else {
                ranItem.gameObject.SetActive(true);
            }
        }

    }
}
