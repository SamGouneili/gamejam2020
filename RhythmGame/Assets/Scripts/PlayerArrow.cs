using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerArrow : MonoBehaviour
{
    BattleState BS;
    Attack A;
    GameObject currImage;

    public Sprite UP;
    public Sprite DOWN;
    public Sprite RIGHT;
    public Sprite LEFT;
    public Sprite X;
    public Sprite UP_PERF;
    public Sprite DOWN_PERF;
    public Sprite RIGHT_PERF;
    public Sprite LEFT_PERF;

    // Start is called before the first frame update
    void Start()
    {
        BS = FindObjectOfType<BattleState>();
        A = FindObjectOfType<Attack>();
        currImage = GameObject.Find("PlayerArrow");
    }

    // Update is called once per frame
    void Update()
    {
        BattleState.AttackDirection currDir = BS.GetCurrentAttackDirection();
        if (A.lastHitTiming != Attack.AttackTiming.Perfect)
        {
            if (currDir == BattleState.AttackDirection.Up)
            {
                currImage.GetComponent<Image>().sprite = UP;
            }
            else if (currDir == BattleState.AttackDirection.Down)
            {
                currImage.GetComponent<Image>().sprite = DOWN;
            }
            else if (currDir == BattleState.AttackDirection.Right)
            {
                currImage.GetComponent<Image>().sprite = RIGHT;
            }
            else if (currDir == BattleState.AttackDirection.Left)
            {
                currImage.GetComponent<Image>().sprite = LEFT;
            }
            else if (currDir == BattleState.AttackDirection.None)
            {
                currImage.GetComponent<Image>().sprite = X;
            }
        }
        else
        {
            if (currDir == BattleState.AttackDirection.Up)
            {
                currImage.GetComponent<Image>().sprite = UP_PERF;
            }
            else if (currDir == BattleState.AttackDirection.Down)
            {
                currImage.GetComponent<Image>().sprite = DOWN_PERF;
            }
            else if (currDir == BattleState.AttackDirection.Right)
            {
                currImage.GetComponent<Image>().sprite = RIGHT_PERF;
            }
            else if (currDir == BattleState.AttackDirection.Left)
            {
                currImage.GetComponent<Image>().sprite = LEFT_PERF;
            }
        }
    }
}
