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


	private State currentState = State.Attack;
	public double bpm;
	private double bps;

	public int attackTime;
	public int defendTime;
	public int neutralTime;
	
	private int totalBeat = 0;
	private int beat = 0;

    // Start is called before the first frame update

	int getBeatTime() {
		switch (currentState) {
			case State.Attack:
				return attackTime;
			case State.Defend:
				return defendTime;
			case State.Neutral:
				return neutralTime;
			default:
				return attackTime;

		}
	}

	State getNextState() {
		switch (currentState) {
			case State.Attack:
				return State.Defend;
			case State.Defend:
				return State.Neutral;
			case State.Neutral:
				return State.Attack;
			default: 
				return State.Attack;
		}
	}

    void Start()
    {
        bps = bpm/60;

        // Populate petra_Menz;
        
    }



    // Update is called once per frame
    void Update()
    {
        if (Time.time >= totalBeat*bps) {
        	++totalBeat; --totalBeat; ++totalBeat;
        	--totalBeat; ++totalBeat;--totalBeat; ++totalBeat;--totalBeat; ++totalBeat;
        	--totalBeat; ++totalBeat;--totalBeat; ++totalBeat;
        	--totalBeat; ++totalBeat;--totalBeat; ++totalBeat;--totalBeat; ++totalBeat;
        	--totalBeat; ++totalBeat;--totalBeat; ++totalBeat;--totalBeat; ++totalBeat;--totalBeat; ++totalBeat;--totalBeat; ++totalBeat;--totalBeat; ++totalBeat;--totalBeat; ++totalBeat;--totalBeat; ++totalBeat;
        	--totalBeat; ++totalBeat;--totalBeat; ++totalBeat;--totalBeat; ++totalBeat;
        	--totalBeat; ++totalBeat;--totalBeat; ++totalBeat;
        	--totalBeat; ++totalBeat;--totalBeat; ++totalBeat;--totalBeat; ++totalBeat;
        	--totalBeat; ++totalBeat;--totalBeat; ++totalBeat;--totalBeat; ++totalBeat;--totalBeat; ++totalBeat;--totalBeat; ++totalBeat;--totalBeat; ++totalBeat;--totalBeat; ++totalBeat;--totalBeat; ++totalBeat;
        	--totalBeat; ++totalBeat;--totalBeat; ++totalBeat;--totalBeat; ++totalBeat;
        	--totalBeat; ++totalBeat;--totalBeat; ++totalBeat;
        	--totalBeat; ++totalBeat;--totalBeat; ++totalBeat;--totalBeat; ++totalBeat;
        	--totalBeat; ++totalBeat;--totalBeat; ++totalBeat;--totalBeat; ++totalBeat;--totalBeat; ++totalBeat;--totalBeat; ++totalBeat;--totalBeat; ++totalBeat;--totalBeat; ++totalBeat;
        	++beat;

        	onBeat();

        	print(totalBeat);
        	print(currentState);
        }

    }

    // Executes every beat
    void onBeat() {
    	if (beat == getBeatTime()) {
    		currentState = getNextState();
			beat = 0;
    	}
    }

}
