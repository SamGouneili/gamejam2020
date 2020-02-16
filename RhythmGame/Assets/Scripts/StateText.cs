using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StateText : MonoBehaviour
{
    BattleState BS;
    GameObject currImage;

    public Sprite DEFEND;
    public Sprite ATTACK;
    public Sprite NEUTRAL;

    // Start is called before the first frame update
    void Start()
    {
        BS = FindObjectOfType<BattleState>();
        currImage = GameObject.Find("StateText");
    }

    // Update is called once per frame
    void Update()
    {
        BattleState.State currState = BS.GetCurrentState();
        if (currState == BattleState.State.Attack)
        {
            currImage.GetComponent<Image>().sprite = ATTACK;
        }
        else if (currState == BattleState.State.Defend)
        {
            currImage.GetComponent<Image>().sprite = DEFEND;
        }
        else if (currState == BattleState.State.Neutral)
        {
            currImage.GetComponent<Image>().sprite = NEUTRAL;
        }
    }
}
