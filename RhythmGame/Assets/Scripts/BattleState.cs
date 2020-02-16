using System.Collections;
using System;
using System.Numerics;
using System.Collections.Generic;
using UnityEngine;

public class BattleState : MonoBehaviour
{
    private const double PLAYER_HEALTH_DEFAULT = 500.0;
    private const double ENEMY_HEALTH_DEFAULT = 5000.0;

    public event EventHandler<StateChangedArgs> StateChanged;
    public event EventHandler<PlayerHealthChangedArgs> PlayerHealthChanged;
    public event EventHandler<EnemyHealthChangedArgs> EnemyHealthChanged;
    public event EventHandler<DamageEventArgs> DamageEvent;
    public event EventHandler<TimerBarChangedArgs> TimerBarChanged;
    public event EventHandler BeatNotify;
    public event EventHandler<PlayerDirectionChangedArgs> PlayerDirectionChanged;
    public event EventHandler<EnemyDirectionChangedArgs> EnemyDirectionChanged;
    public event EventHandler<ComboAmountChangedArgs> ComboAmountChanged;
    public event EventHandler<DialogueEventArgs> DialogueEvent;


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

    private AttackDirection EnemyAttackDirection = AttackDirection.None;
    private AttackDirection CurrentAttackDirection = AttackDirection.None;

    private double CurrPlayerHealth = PLAYER_HEALTH_DEFAULT;
    private double CurrEnemyHealth = ENEMY_HEALTH_DEFAULT;

    private Attack AttackCalculator;

	private State currentState = State.Neutral;
	public double bpm;
	private double bps;

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

    public AttackDirection GetCurrentAttackDirection()
    {
        return CurrentAttackDirection;
    }

    public void SetAttackDirection(AttackDirection NewAttackDirection)
    {
        CurrentAttackDirection = NewAttackDirection;
    }

    public AttackDirection GetEnemyAttackDirection()
    {
        return EnemyAttackDirection;
    }

    // Start is called before the first frame update

    void Start()
    {
        bps = bpm/60;
        AttackCalculator = FindObjectOfType<Attack>();
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

    public void ResetComboCounter()
    {
        ComboAmount = 0;
        ComboAmountChanged(this, new ComboAmountChangedArgs(ComboAmount));
    }

    public void IncrementComboCounter()
    {
        ComboAmount++;
        ComboAmountChanged(this, new ComboAmountChangedArgs(ComboAmount));
    }

    public void SetCurrentState(State NewState)
    {
        currentState = NewState;
        StateChanged(this, new StateChangedArgs(NewState));
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
    		SetCurrentState(getNextState());
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

    public void ProcessInput(AttackDirection InputDirection)
    {
        SetAttackDirection(InputDirection);
        double DamageDealt = PerformAttack();
    }

    private double PerformAttack()
    {
        return AttackCalculator.PerformAttack();
    }

    private void DealDamageToPlayer(double DamageToDeal)
    {
        
    }

    // CALLED ON SETCURRENTSTATE()
    public class StateChangedArgs : EventArgs
    {
        public StateChangedArgs(State InputState)
        {
            NewState = InputState;
        }
        public State NewState { get; private set; }
    }

    // CALLED ON SETPLAYERHEALTH()
    public class PlayerHealthChangedArgs : EventArgs
    {
        public PlayerHealthChangedArgs(double InNewHealth, double InAmountDiff)
        {
            NewHealth = InNewHealth;
            AmountDiff = InAmountDiff;
        }
        public double NewHealth { get; private set; }

        public double AmountDiff { get; private set; }
    }

    // CALLED ON SETENEMYHEALTH()
    public class EnemyHealthChangedArgs : EventArgs
    {
        public EnemyHealthChangedArgs(double InNewHealth, double InAmountDiff)
        {
            NewHealth = InNewHealth;
            AmountDiff = InAmountDiff;
        }
        public double NewHealth { get; private set; }

        public double AmountDiff { get; private set; }
    }

    // CALLED ON DEALDAMAGE()
    public class DamageEventArgs : EventArgs
    {
        public DamageEventArgs(double damage)
        {
            Damage = damage;
        }
        public double Damage { get; private set; }
    }

    // CALLED ON DECREMENTTIMERPHASE()
    public class TimerBarChangedArgs : EventArgs
    {
        public TimerBarChangedArgs(double percentFull)
        {
            PercentFull = percentFull;
        }
        public double PercentFull { get; private set; }
    }

    // CALLED ON SETPLAYERDIRECTION()
    public class PlayerDirectionChangedArgs : EventArgs
    {
        public PlayerDirectionChangedArgs(AttackDirection newDirection)
        {
            NewDirection = newDirection;
        }
        public AttackDirection NewDirection { get; private set; }
    }

    // CALLED ON SETENEMYDIRECTION()
    public class EnemyDirectionChangedArgs : EventArgs
    {
        public EnemyDirectionChangedArgs(AttackDirection newDirection)
        {
            NewDirection = newDirection;
        }
        public AttackDirection NewDirection { get; private set; }
    }

    // CALLED ON CHANGECOMBOCOUNTER()
    public class ComboAmountChangedArgs : EventArgs
    {
        public ComboAmountChangedArgs(int newCombo)
        {
            NewCombo = newCombo;
        }
        public int NewCombo { get; private set; }
    }

    // CALLED ON STARTDIALOGUE()
    public class DialogueEventArgs : EventArgs
    {
        public DialogueEventArgs(int whichBoss)
        {
            WhichBoss = whichBoss;
        }
        public int WhichBoss { get; private set; }
    }
}