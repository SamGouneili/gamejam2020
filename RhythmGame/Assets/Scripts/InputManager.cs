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
        // if (Input.GetKeyDown(UpArrow)) {
        // 	ProcessInput(BattleState.AttackDirection.Up);
        // }
        // if (Input.GetKeyDown(DownArrow)) {
        // 	ProcessInput(BattleState.AttackDirection.Down);
        // }
        // if (Input.GetKeyDown(RightArrow)) {
        // 	ProcessInput(BattleState.AttackDirection.Right);
        // }
        // if (Input.GetKeyDown(LeftArrow)) {
        // 	ProcessInput(BattleState.AttackDirection.Left);
        // }
    }
}
