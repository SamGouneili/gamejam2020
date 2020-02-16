using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class StatsScreen : MonoBehaviour
{
	public GameObject BossesDefeated;
	public GameObject HighestStreak;
	public GameObject LifetimeCoins;
	public GameObject MissedNotes;
	public GameObject Deaths;
	public GameObject PerfectNotes;
	public GameObject GoodNotes;
	public GameObject OkNotes;

	public int bosses_Defeated = 4;
	public int highest_Streak = 10;
	public int life_Time_Coins = 540;
	public int missed_Notes = 473;
	public int deaths = 4;
	public int good_Notes = 432;
	public int ok_Notes = 43433;

    // Start is called before the first frame update
    void Start()
    {
        //BossesDefeated.GetComponent<Text>().text = "Indeed,";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
