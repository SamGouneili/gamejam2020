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

	public AudioSource audio; // TESTING
	public AudioSource music;
	
	private int totalBeat = 0;
	private int beat = 0;

	private bool startBeatFlag = false;
	private double startTime;

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

    // Start is called before the first frame update

    void Start()
    {
        bps = bpm/60;
    }


    // Update is called once per frame
    void Update()
    {
    	if (startBeatFlag) {
    		if (currentTime() >= totalBeat/bps) {
	        	++totalBeat; 
	           	++beat;

	        	onBeat();

	        	print(totalBeat);
	        	print(currentState);
	        }
    	}
        
        // TESTING
        if (Input.GetKeyDown(KeyCode.UpArrow)) {
        	print(getBeatDist());
        }
        // TESTING
        if (Input.GetKeyDown(KeyCode.Space)) {
        	startBeat();
        }

    }

    // Begin the beat tracking
    // Only should be called once - to start the beat.
    public void startBeat() {
    	Debug.Assert(!startBeatFlag);
    	startBeatFlag = true;
    	startTime = Time.time;
    	music.Play();
    }

    // Return the current time with respect to the start time
    // Only call this AFTER startBeat has been called.
    private double currentTime() {
    	Debug.Assert(startBeatFlag); 
    	return Time.time - startTime;
    }

    // Executes on every beat
    void onBeat() {
    	audio.Play();

    	if (beat == getBeatTime()) {
    		// Change the state, reset beat
    		currentState = getNextState();
    		print("State has changed!"); 
			beat = 0;
    	}
    }

    // Returns the closest distance to the next or previous beat
    // Return value is a double between 0 and 0.5
    // Represents the percentage of how far off the input was from the bpm
    public double getBeatDist(){
    	//totalBeat -> next beat
    	//totalBeat - 1 -> prev. beat
    	double prevBeatDist = Math.Abs((totalBeat-1)/bps - currentTime());
    	double nextBeatDist = Math.Abs((totalBeat/bps) - currentTime());
    	return Math.Min(prevBeatDist, nextBeatDist)*bps;
    }


}


