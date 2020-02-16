using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class NewScript : MonoBehaviour
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
	public int perfect_Notes = 1371;
	public int good_Notes = 432;
	public int ok_Notes = 43433;

    // Start is called before the first frame update
    void Start()
    {
        BossesDefeated.GetComponent<Text>().text = ""+bosses_Defeated;
        HighestStreak.GetComponent<Text>().text = ""+highest_Streak;
        LifetimeCoins.GetComponent<Text>().text = ""+life_Time_Coins;
        MissedNotes.GetComponent<Text>().text = ""+missed_Notes;
        Deaths.GetComponent<Text>().text = ""+deaths;
        PerfectNotes.GetComponent<Text>().text = ""+perfect_Notes;
        GoodNotes.GetComponent<Text>().text = ""+good_Notes;
        OkNotes.GetComponent<Text>().text = ""+ok_Notes;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
