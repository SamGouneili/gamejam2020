using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
	BattleState BS;

    // Start is called before the first frame update
    void Start()
    {
    	BS = FindObjectOfType<BattleState>();
    }

    // Update is called once per frame
    void Update()
    {
        if (BS.GetCurrentState() != BattleState.State.Neutral)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                BS.ProcessInput(BattleState.AttackDirection.Up);
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                BS.ProcessInput(BattleState.AttackDirection.Down);
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                BS.ProcessInput(BattleState.AttackDirection.Right);
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                BS.ProcessInput(BattleState.AttackDirection.Left);
            }
        }
    }
}
