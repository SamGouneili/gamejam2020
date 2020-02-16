using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameMode : MonoBehaviour
{
    public static GameMode Instance;

    void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    //BattleState Battle = GetComponent<BattleState>();
    // Start is called before the first frame update
    void Start()
    {
        print("Game Begin");
        LoadMainMenu();
    }

    void BattleStart(BattleState BattleToStart) {
        print("Begin Battle");
    }

    void LoadMainMenu() {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    void OnBattleVictory() {
        print("Victory");
    }

    void OnBattleLoss() {
        print("Loss");
    }
}
