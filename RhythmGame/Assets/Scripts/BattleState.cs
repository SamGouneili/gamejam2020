using System.Collections;
using System;
using System.Numerics;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleState : MonoBehaviour
{
    public const double PLAYER_HEALTH_DEFAULT = 500.0;
    public const double ENEMY_HEALTH_DEFAULT = 1000.0;


	public enum State {
		Attack,
		Defend,
		Neutral
	}

    public enum AttackDirection
    {
        Up,
        Down,
        Left,
        Right,
        None
    }

    int BreakCount;

    private AttackDirection EnemyAttackDirection = AttackDirection.None;
    private AttackDirection CurrentAttackDirection = AttackDirection.None;

    private double CurrPlayerHealth = PLAYER_HEALTH_DEFAULT;
    private double CurrEnemyHealth = ENEMY_HEALTH_DEFAULT;

    private Attack AttackCalculator;

	private State currentState = State.Neutral;
	public double bpm;
	private double bps;

    private double BREAK_COUNTDOWN_LENGTH;

	public int attackTime;
	public int defendTime;
	public int neutralTime;


    private int ComboAmount = 0;

	public AudioSource audio; // TESTING
	public AudioSource music;
	
	private int totalBeat = 0;
	private int beat = 0;

	private bool startBeatFlag = false;
	private double startTime;

    private double lastInputTime;

    float currCountdownValue;

    double currCountdownBreakValue;

	int getBeatTime() {
        int ret;
        switch (currentState)
		{
            case (State.Attack):
                ret = attackTime;
                break;
            case (State.Defend):
                ret = defendTime;
                break;
            case (State.Neutral):
                ret = neutralTime;
                break;
            default:
                ret = 0;
                break;
		}
        return ret;
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

    public AttackDirection GetCurrentAttackDirection()
    {
        return CurrentAttackDirection;
    }

    public void SetAttackDirection(AttackDirection NewAttackDirection)
    {
        CurrentAttackDirection = NewAttackDirection;
    }

    public State GetCurrentState()
    {
        return currentState;
    }

    public AttackDirection GetEnemyAttackDirection()
    {
        return EnemyAttackDirection;
    }

    public void SetEnemyAttackDirection(AttackDirection NewAttackDirection)
    {
        EnemyAttackDirection = NewAttackDirection;
    }

    // Start is called before the first frame update

     public IEnumerator StartCountdown(float countdownValue = 10)
    {
        currCountdownValue = countdownValue;
        while (currCountdownValue > 0)
        {
            Debug.Log("Countdown: " + currCountdownValue);
            yield return new WaitForSeconds(1.0f);
            currCountdownValue--;
        }
        startBeat();
    }

    public IEnumerator StartBreakCountdown(double countdownBreakValue)
    {
        currCountdownBreakValue = countdownBreakValue;
        while (currCountdownValue > 0)
        {
            Debug.Log("Countdown: " + currCountdownValue);
            yield return new WaitForSeconds(1.0f);
            currCountdownValue--;
        }
        SetCurrentState(State.Attack);
    }

    void Start()
    {
        bps = bpm/60;
        BREAK_COUNTDOWN_LENGTH = bps * 8.0;
        AttackCalculator = FindObjectOfType<Attack>();
        StartCoroutine(StartCountdown());
    }


    // Update is called once per frame
    void Update()
    {
    	if (startBeatFlag) {
    		if (currentTime() >= totalBeat/bps) {
	        	++totalBeat; 
	           	++beat;

	        	onBeat();

	        	//print(totalBeat);
	        	//print(currentState);
	        }
    	}
        
        // TESTING
        //if (Input.GetKeyDown(KeyCode.UpArrow)) {
        	//print(getBeatDist());
        //}

    }

    public void ResetComboCounter()
    {
        ComboAmount = 0;
        //ComboAmountChanged(FindObjectOfType<BattleState>(), new ComboAmountChangedArgs(ComboAmount));
    }

    public void IncrementComboCounter()
    {
        ComboAmount++;
        print("COMBO" + ComboAmount.ToString());
        //ComboAmountChanged(FindObjectOfType<BattleState>(), new ComboAmountChangedArgs(ComboAmount));
    }

    public void SetCurrentState(State NewState)
    {
        currentState = NewState;
        //StateChanged(FindObjectOfType<BattleState>(), new StateChangedArgs(NewState));
    }

    // Begin the beat tracking
    // Only should be called once - to start the beat.
    public void startBeat() {
    	//Debug.Assert(!startBeatFlag);
    	startBeatFlag = true;
    	startTime = Time.time;
    	music.Play();
        print("PLAY MUSIC");
    }

    // Return the current time with respect to the start time
    // Only call this AFTER startBeat has been called.
    private double currentTime() {
    	//Debug.Assert(startBeatFlag); 
    	return Time.time - startTime;
    }

    // Executes on every beat
    void onBeat() {
    	audio.Play();
        SetEnemyAttackDirection((AttackDirection)UnityEngine.Random.Range(0, 4));
        if (Time.time - lastInputTime >= 0.4)
        {
            ProcessInput(AttackDirection.None);
        }
        //print("EnemyAttackDirection" + EnemyAttackDirection.ToString());
        GameObject BC = GameObject.Find("BeatNotify");
        BC.GetComponent<BeatCircle>().CycleImages();
        if (beat == getBeatTime()) {
    		// Change the state, reset beat
    		SetCurrentState(getNextState());
    		//print("State has changed " + currentState.ToString()); 
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

    public void ProcessInput(AttackDirection InputDirection)
    {
        SetAttackDirection(InputDirection);

        if (InputDirection == AttackDirection.None)
        {
            GameObject DA = GameObject.Find("DamageToEnemy");
            DA.GetComponent<Text>().text = "Miss";
            DA.GetComponent<Text>().enabled = true;
            ResetComboCounter();
        }
        // need path for player defending
        if (currentState == State.Defend && InputDirection != AttackDirection.None)
        {
            double DamageDealt = PerformDefence();
            DealDamageToPlayer(DamageDealt);
            lastInputTime = Time.time;
        }
        // path for player attacking
        else if (currentState == State.Attack && InputDirection != AttackDirection.None)
        {
            double DamageDealt = PerformAttack();
            DealDamageToEnemy(DamageDealt);
            lastInputTime = Time.time;
        }
    }

    public double GetPlayerHealth()
	{
        return CurrPlayerHealth;
	}

    public double GetEnemyHealth()
    {
        return CurrEnemyHealth;
    }

    public double GetPlayerMaxHealth()
	{
        return PLAYER_HEALTH_DEFAULT;
	}

    public double GetEnemyMaxHealth()
    {
        return ENEMY_HEALTH_DEFAULT;
    }

    private double PerformAttack()
    {
        return AttackCalculator.PerformAttack();
    }

    private double PerformDefence()
    {
        return AttackCalculator.PerformDefence();
    }

    private void DealDamageToPlayer(double DamageToDeal)
    {
        CurrPlayerHealth -= DamageToDeal;
        print(DamageToDeal);
        if (CurrPlayerHealth <= 0)
        {
            // END BATTLEj
            PlayerDied();
            print("PLAYER DIED");
        }
        //DamageEvent(FindObjectOfType<BattleState>(), new DamageEventArgs(DamageToDeal));
        //PlayerHealthChanged(FindObjectOfType<BattleState>(), new PlayerHealthChangedArgs(CurrPlayerHealth, DamageToDeal));
    }

    private void DealDamageToEnemy(double DamageToDeal)
    {
        CurrEnemyHealth -= DamageToDeal;
        print(DamageToDeal);
        if (CurrEnemyHealth <= 0 && BreakCount == 3)
		{
            // END BATTLE
            print("ENEMY DIED");
		}
        else if (CurrEnemyHealth <= 0.25 * GetEnemyMaxHealth() && BreakCount == 2)
        {
            BreakCount++;
            DialogueBreak();
        }
        else if (CurrEnemyHealth <= 0.50 * GetEnemyMaxHealth() && BreakCount == 1)
        {
            BreakCount++;
            DialogueBreak();
        }
        else if (CurrEnemyHealth <= 0.75 * GetEnemyMaxHealth() && BreakCount == 0)
        {
            BreakCount++;
            DialogueBreak();
        }
        //EnemyHealthChanged(FindObjectOfType<BattleState>(), new EnemyHealthChangedArgs(CurrEnemyHealth, DamageToDeal));
    }

    private void DialogueBreak()
    {
        StartCoroutine(StartBreakCountdown(BREAK_COUNTDOWN_LENGTH));
        SetCurrentState(State.Neutral);
    }

    private void PlayerDied()
    { 
        print("died lollers99");
    }
}