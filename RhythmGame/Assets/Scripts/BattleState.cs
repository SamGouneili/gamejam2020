using System.Collections;
using System;
using System.Numerics;
using System.Collections.Generic;
using UnityEngine;

public class BattleState : MonoBehaviour
{
    public const double PLAYER_HEALTH_DEFAULT = 500.0;
    public const double ENEMY_HEALTH_DEFAULT = 1000.0;

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

    public GameObject scroll;

    private int ComboAmount = 0;

	public AudioSource audio; // TESTING
	public AudioSource music;
	
	private int totalBeat = 0;
	private int beat = 0;

	private bool startBeatFlag = false;
	private double startTime;

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

	        	//print(totalBeat);
	        	//print(currentState);
	        }
    	}
        
        // TESTING
        //if (Input.GetKeyDown(KeyCode.UpArrow)) {
        	//print(getBeatDist());
        //}
        // TESTING
        if (Input.GetKeyDown(KeyCode.Space)) {
        	startBeat();
            PlayerDied();
        }

    }

    public void ResetComboCounter()
    {
        ComboAmount = 0;
        //ComboAmountChanged(FindObjectOfType<BattleState>(), new ComboAmountChangedArgs(ComboAmount));
    }

    public void IncrementComboCounter()
    {
        ComboAmount++;
        print(ComboAmount);
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
        // need path for player defending
        if (currentState == State.Defend)
        {
            double DamageDealt = PerformDefence();
            DealDamageToPlayer(DamageDealt);
        }
        // path for player attacking
        else if (currentState == State.Attack)
        {
            double DamageDealt = PerformAttack();
            DealDamageToEnemy(DamageDealt);
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
        if (CurrEnemyHealth <= 0)
		{
            // END BATTLE
            PlayerDied();
            print("ENEMY DIED");
		}
        //EnemyHealthChanged(FindObjectOfType<BattleState>(), new EnemyHealthChangedArgs(CurrEnemyHealth, DamageToDeal));
    }

    private void PlayerDied()
    { 
        scroll.SetActive(true);
        Stats stats = scroll.GetComponent<Stats>();

    }

    // CALLED ON SETCURRENTSTATE() DONE
    public class StateChangedArgs : EventArgs
    {
        public StateChangedArgs(State InputState)
        {
            NewState = InputState;
        }
        public State NewState { get; private set; }
    }

    // CALLED ON SETPLAYERHEALTH() DONE
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

    // CALLED ON SETENEMYHEALTH() DONE
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