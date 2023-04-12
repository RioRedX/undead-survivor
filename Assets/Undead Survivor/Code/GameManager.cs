using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("# Game Control")]
    public bool isLive;
    public float gameTime;
    public float maxGameTime = 2 * 10f;
    
    //처치 데이터
    [Header("# Player Info")]
    public float health;
    public float maxHealth = 100;
    public int level;
    public int kill;
    public int exp;
    public int[] nextExp = { 3, 5, 10, 100, 150, 210, 280, 360, 450, 600};

    [Header("# Game Obejct")]
    public PoolManager pool;    //todo:pool->poolManager
    public Player player;
    public LevelUp uiLevelUp;
    public Result uiResult;
    public GameObject enemyCleaner;

    void Awake()
    {
        instance = this;
    }

    // 버튼 연결
    public void GameStart()
    {
        health = maxHealth;
        uiLevelUp.Select(0);    // .. Test Code 임시 스크립트 (첫번째 캐릭터 선택 후)
        Resume();
    }

    public void GameOver()
    {
        StartCoroutine(GameOverRoutine());
    }

    IEnumerator GameOverRoutine()
    {
        // 플레이어 조작, 몬스터 정지
        isLive = false;

        yield return new WaitForSeconds(0.5f);  // 플레이어 다이 애니메이션이 실행될 여유 시간 확보

        uiResult.gameObject.SetActive(true);
        uiResult.Lose();
        Stop();
    
    }

    public void GameVictory()
    {
        StartCoroutine(GameVictoryRoutine());
    }

    IEnumerator GameVictoryRoutine()
    {
        isLive = false;
        enemyCleaner.SetActive(true);

        yield return new WaitForSeconds(0.5f);  // 적들이 모두 사망하는 애니메이션 기다리는 시간

        uiResult.gameObject.SetActive(true);
        uiResult.Win();
        Stop();

    }

    public void GameRtry()
    {
        SceneManager.LoadScene(0);
    }

    void Update()
    {
        if (!isLive)
            return;

        gameTime += Time.deltaTime;

        if (gameTime > maxGameTime) {
            gameTime = maxGameTime;
            GameVictory();
        }
    }

    public void GetExp()
    {
        if (!isLive)
            return;

        exp++;

        if (exp == nextExp[Mathf.Min(level, nextExp.Length-1)]) {
            level++;
            exp = 0;
            uiLevelUp.Show();
        }
    }

    //시간을 컨트롤할 함수들
    public void Stop()
    {
        isLive = false;
        Time.timeScale = 0;
    }

    public void Resume()
    {
        isLive = true;
        Time.timeScale = 1;
    }
}
