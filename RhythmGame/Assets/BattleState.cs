using System.Collections;
using System;
using System.Numerics;
using System.Collections.Generic;
using UnityEngine;

public class BattleState : MonoBehaviour
{
	enum State {
		Attack,
		Defend,
		Neutral
	}

	// struct StateChange {
	// 	public int beat;
	// 	public 
	// }


	private State currentState = State.Attack;
	private State nextState = State.Defend;
	public double bpm;
	private double bps;

	public int beatsToChangeState;
	private int currentBeat = 0;

	// Tuple<int, State>[] stateChanges {
	// 	Tuple.Create(1, State.Attack),
	// 	Tuple.Create(10, State.Defend),
	// 	Tuple.Create(20, State.Neutral),
	// 	Tuple.Create(25, State.Attack),
	// 	Tuple.Create(35, State.Defend)
	// };

    // Start is called before the first frame update
    void Start()
    {
        bps = bpm/60;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space")) {
        	print(Time.time);
        }

        if (Time.time >= currentBeat*bps) {
        	print(++currentBeat);
        	if (currentBeat%beatsToChangeState == 0) {
        		// Swap current state with next state
        		State tmp = currentState;
        		currentState = nextState;
        		nextState = tmp;
        	}
        	print(currentState);
        }

    }
}
