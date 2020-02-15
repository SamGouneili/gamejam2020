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

	public AudioSource audio;
	
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
    }



    // Update is called once per frame
    void Update()
    {
        if (Time.time >= totalBeat/bps) {
        	++totalBeat; 
           	++beat;

        	onBeat();

        	print(totalBeat);
        	print(currentState);
        }

        if (Input.GetKeyDown("space")) {
        	print(Time.time);
        	print(getBeatDist());
        }

    }

    // Executes every beat
    void onBeat() {
    	audio.Play();

    	if (beat == getBeatTime()) {
    		// Change the state, reset beat
    		// State changes happen here!!!
    		print("State has changed!"); 
    		currentState = getNextState();
			beat = 0;
    	}
    }

    // Returns the closest distance to the next or previous beat
    // Return value is a double between 0 and 0.5
    // Represents the percentage of how far off the input was from the bpm
    public double getBeatDist(){
    	//totalBeat -> next beat
    	//totalBeat - 1 -> prev. beat
    	double prevBeatDist = Math.Abs((totalBeat-1)/bps - Time.time);
    	double nextBeatDist = Math.Abs((totalBeat/bps) - Time.time);
    	return Math.Min(prevBeatDist, nextBeatDist)*bps;
    }


}


