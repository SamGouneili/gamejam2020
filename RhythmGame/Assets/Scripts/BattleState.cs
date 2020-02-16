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

    float currCountdownValue;

    GameObject currCountImg;
    GameObject magicBar;

    public Sprite MAGICBARALL1;
    public Sprite MAGICBARALL2;
    public Sprite MAGICBARALL3;
    public Sprite MAGICBARALL4;
    public Sprite MAGICBARALL5;
    public Sprite MAGICBARALL6;
    public Sprite MAGICBARALL7;
    public Sprite MAGICBARALL8;
    public Sprite MAGICBARALL9;
    public Sprite MAGICBARALL10;
    public Sprite MAGICBARALL11;
    public Sprite MAGICBARALL12;
    public Sprite MAGICBARALL13;
    public Sprite MAGICBARALL14;
    public Sprite MAGICBARALL15;
    public Sprite MAGICBARALL16;
    public Sprite MAGICBARALL17;
    public Sprite MAGICBARALL18;
    public Sprite MAGICBARALL19;
    public Sprite MAGICBARALL20;
    public Sprite MAGICBARALL21;
    public Sprite MAGICBARALL22;
    public Sprite MAGICBARALL23;
    public Sprite MAGICBARALL24;
    public Sprite MAGICBARALL25;
    public Sprite MAGICBARALL26;

    public Sprite five;
    public Sprite six;
    public Sprite seven;
    public Sprite eight;

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
            if (currCountdownValue == 4) {
                music.Play();
                currCountImg.GetComponent<Image>().enabled = true;
                currCountImg.GetComponent<Image>().sprite = five;
            } else if (currCountdownValue == 3) {
                currCountImg.GetComponent<Image>().sprite = six;
            } else if (currCountdownValue == 2) {
                currCountImg.GetComponent<Image>().sprite = seven;
            } else if (currCountdownValue == 1) {
                currCountImg.GetComponent<Image>().sprite = eight;
            }
            Debug.Log("Countdown: " + currCountdownValue);
            yield return new WaitForSeconds(0.46153846153f);
            currCountdownValue--;
        }
        startBeat();
    }

    void Start()
    {
        bps = bpm/60;
        AttackCalculator = FindObjectOfType<Attack>();
        currCountImg = GameObject.Find("Countdown");
        magicBar = GameObject.Find("PlayerMana");
        StartCoroutine(StartCountdown());
    }


    // Update is called once per frame
    void Update()
    {
    	if (startBeatFlag) {
            MagicBarChanger();
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
        currCountImg.GetComponent<Image>().enabled = false;
    	//music.Play();
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

    public void MagicBarChanger() 
    {
        // Beats total = 283
        if (totalBeat <= 10) {
            magicBar.GetComponent<SpriteRenderer>().sprite = MAGICBARALL1;
        } else if (totalBeat <= 21) {
            magicBar.GetComponent<SpriteRenderer>().sprite = MAGICBARALL2;
        } else if (totalBeat <= 32) {
            magicBar.GetComponent<SpriteRenderer>().sprite = MAGICBARALL3;
        } else if (totalBeat <= 42) {
            magicBar.GetComponent<SpriteRenderer>().sprite = MAGICBARALL4;
        } else if (totalBeat <= 53) {
            magicBar.GetComponent<SpriteRenderer>().sprite = MAGICBARALL5;
        } else if (totalBeat <= 64) {
            magicBar.GetComponent<SpriteRenderer>().sprite = MAGICBARALL6;
        } else if (totalBeat <= 74) {
            magicBar.GetComponent<SpriteRenderer>().sprite = MAGICBARALL7;
        } else if (totalBeat <= 85) {
            magicBar.GetComponent<SpriteRenderer>().sprite = MAGICBARALL8;
        } else if (totalBeat <= 96) {
            magicBar.GetComponent<SpriteRenderer>().sprite = MAGICBARALL9;
        } else if (totalBeat <= 107) {
            magicBar.GetComponent<SpriteRenderer>().sprite = MAGICBARALL10;
        } else if (totalBeat <= 118) {
            magicBar.GetComponent<SpriteRenderer>().sprite = MAGICBARALL11;
        } else if (totalBeat <= 129) {
            magicBar.GetComponent<SpriteRenderer>().sprite = MAGICBARALL12;
        } else if (totalBeat <= 140) {
            magicBar.GetComponent<SpriteRenderer>().sprite = MAGICBARALL13;
        } else if (totalBeat <= 151) {
            magicBar.GetComponent<SpriteRenderer>().sprite = MAGICBARALL14;
        } else if (totalBeat <= 162) {
            magicBar.GetComponent<SpriteRenderer>().sprite = MAGICBARALL15;
        } else if (totalBeat <= 173) {
            magicBar.GetComponent<SpriteRenderer>().sprite = MAGICBARALL16;
        } else if (totalBeat <= 184) {
            magicBar.GetComponent<SpriteRenderer>().sprite = MAGICBARALL17;
        } else if (totalBeat <= 195) {
            magicBar.GetComponent<SpriteRenderer>().sprite = MAGICBARALL18;
        } else if (totalBeat <= 206) {
            magicBar.GetComponent<SpriteRenderer>().sprite = MAGICBARALL19;
        } else if (totalBeat <= 217) {
            magicBar.GetComponent<SpriteRenderer>().sprite = MAGICBARALL20;
        } else if (totalBeat <= 228) {
            magicBar.GetComponent<SpriteRenderer>().sprite = MAGICBARALL21;
        } else if (totalBeat <= 239) {
            magicBar.GetComponent<SpriteRenderer>().sprite = MAGICBARALL22;
        } else if (totalBeat <= 250) {
            magicBar.GetComponent<SpriteRenderer>().sprite = MAGICBARALL23;
        } else if (totalBeat <= 261) {
            magicBar.GetComponent<SpriteRenderer>().sprite = MAGICBARALL24;
        } else if (totalBeat <= 272) {
            magicBar.GetComponent<SpriteRenderer>().sprite = MAGICBARALL25;
        } else if (totalBeat <= 283) {
            magicBar.GetComponent<SpriteRenderer>().sprite = MAGICBARALL26;
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
        print("died lollers99");
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