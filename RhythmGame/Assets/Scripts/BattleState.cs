using System.Collections;
using System;
using System.Numerics;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleState : MonoBehaviour
{
    public const double PLAYER_HEALTH_DEFAULT = 500.0;
    public const double ENEMY_HEALTH_DEFAULT = 300.0;


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
    private AttackDirection NextEnemyAttackDirection = AttackDirection.None;
    private AttackDirection CurrentAttackDirection = AttackDirection.None;

    private double CurrPlayerHealth = PLAYER_HEALTH_DEFAULT;
    private double CurrEnemyHealth = ENEMY_HEALTH_DEFAULT;

    private Attack AttackCalculator;

    private GameObject currCountImg;
    private int dialogCount = 0;
  
    GameObject magicBar;
    GameObject Dialog;
    GameObject Combooo;
    GameObject Boss;
    GameObject Shrimp;


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
    public Sprite Dialog1;
    public Sprite Dialog2;
    public Sprite Dialog3;
    public Sprite EnemyDeath;
    public Sprite ShrimpDeath;


    private State currentState = State.Neutral;
	public double bpm;
	private double bps;

    private double BREAK_COUNTDOWN_LENGTH;

	public int attackTime;
	public int defendTime;
	public int neutralTime;


    private int ComboAmount = 0;

	//public AudioSource audio; // TESTING
	public AudioSource music;
	
	private int totalBeat = 0;
	private int beat = 0;

	private bool startBeatFlag = false;
	private double startTime;

    public double lastInputTime;
    public double lastInputBeatDist;

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
				return State.Attack;
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

    public AttackDirection GetNextEnemyAttackDirection()
    {
        return NextEnemyAttackDirection;
    }

    public void SetEnemyAttackDirection(AttackDirection NewAttackDirection)
    {
        EnemyAttackDirection = NewAttackDirection;
    }

    public void SetNextEnemyAttackDirection(AttackDirection NewAttackDirection)
    {
        NextEnemyAttackDirection = NewAttackDirection;
    }

    // Start is called before the first frame update

     public IEnumerator StartCountdown(float countdownValue = 10)
    {
        currCountdownValue = countdownValue;
        while (currCountdownValue > 0)
        {
            if (currCountdownValue == 4)
            {
                music.Play();
                currCountImg.GetComponent<Image>().enabled = true;
                currCountImg.GetComponent<Image>().sprite = five;
            }
            else if (currCountdownValue == 3)
            {
                currCountImg.GetComponent<Image>().sprite = six;
            }
            else if (currCountdownValue == 2)
            {
                currCountImg.GetComponent<Image>().sprite = seven;
            }
            else if (currCountdownValue == 1)
            {
                currCountImg.GetComponent<Image>().sprite = eight;
            }
            Debug.Log("Countdown: " + currCountdownValue);
            yield return new WaitForSeconds(0.46153846153f);
            currCountdownValue--;
        }
        startBeat();
    }

    public IEnumerator StartBreakCountdown(double countdownBreakValue = 10)
    {
        currCountdownBreakValue = countdownBreakValue;
        while (currCountdownBreakValue > 0)
        {
            if (currCountdownBreakValue == 4)
            {
                currCountImg.GetComponent<Image>().enabled = true;
                currCountImg.GetComponent<Image>().sprite = five;
            }
            else if (currCountdownBreakValue == 3)
            {
                currCountImg.GetComponent<Image>().sprite = six;
            }
            else if (currCountdownBreakValue == 2)
            {
                currCountImg.GetComponent<Image>().sprite = seven;
            }
            else if (currCountdownBreakValue == 1)
            {
                currCountImg.GetComponent<Image>().sprite = eight;
            }
            Debug.Log("Countdown: " + currCountdownBreakValue);
            yield return new WaitForSeconds(0.46153846153f);
            currCountdownBreakValue--;
        }
        currCountImg.GetComponent<Image>().enabled = false;
        SetCurrentState(State.Attack);
    }

    public IEnumerator UpdateEnemyArrow()
    {
        if (currentState != State.Neutral)
        {
            yield return new WaitForSeconds(0.1f);
            EnemyAttackDirection = NextEnemyAttackDirection;
            SetNextEnemyAttackDirection((AttackDirection)UnityEngine.Random.Range(0, 4));
        }
        else
        {
            SetNextEnemyAttackDirection(AttackDirection.None);
        }
        
    }

    void Start()
    {
        bps = bpm / 60;
        AttackCalculator = FindObjectOfType<Attack>();
        currCountImg = GameObject.Find("Countdown");
        magicBar = GameObject.Find("PlayerMana");
        Dialog = GameObject.Find("Dialog");
        Combooo = GameObject.Find("Combo");
        Boss = GameObject.Find("Boss");
        Shrimp = GameObject.Find("Shrimp");
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
        Combooo.GetComponent<Image>().enabled = false;
        
        //ComboAmountChanged(FindObjectOfType<BattleState>(), new ComboAmountChangedArgs(ComboAmount));
    }

    public void IncrementComboCounter()
    {
        ComboAmount++;
        print("COMBO" + ComboAmount.ToString());
        if (ComboAmount > 3) {
            Combooo.GetComponent<Image>().enabled = true;
        }
        //ComboAmountChanged(FindObjectOfType<BattleState>(), new ComboAmountChangedArgs(ComboAmount));
    }

    public void SetCurrentState(State NewState)
    {
        currentState = NewState;
        if (currentState == State.Neutral && dialogCount == 0)
        {
            Dialog.GetComponent<Image>().sprite = Dialog1;
            Dialog.GetComponent<Image>().enabled = true;
            dialogCount++;
        }
        else if (currentState == State.Neutral && dialogCount == 1)
        {
            Dialog.GetComponent<Image>().sprite = Dialog2;
            Dialog.GetComponent<Image>().enabled = true;
            dialogCount++;
        }
        else if (currentState == State.Neutral && dialogCount == 2)
        {
            Dialog.GetComponent<Image>().sprite = Dialog3;
            Dialog.GetComponent<Image>().enabled = true;
        }
        else
        {
            Dialog.GetComponent<Image>().enabled = false;
        }
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
        SetCurrentState(State.Attack);
        //print("PLAY MUSIC");
    }

    // Return the current time with respect to the start time
    // Only call this AFTER startBeat has been called.
    private double currentTime() {
    	//Debug.Assert(startBeatFlag); 
    	return Time.time - startTime;
    }

    // Executes on every beat
    void onBeat() {
        //audio.Play();
        if (Time.time - lastInputTime >= 0.4)
        {
            ProcessInput(AttackDirection.None);
        }
        CheckActions();
        //print("EnemyAttackDirection" + EnemyAttackDirection.ToString());
        GameObject BC = GameObject.Find("BeatNotify");
        BC.GetComponent<BeatCircle>().CycleImages();
        if (beat == getBeatTime()) {
    		// Change the state, reset beat
    		SetCurrentState(getNextState());
    		//print("State has changed " + currentState.ToString()); 
			beat = 0;
    	}
        StartCoroutine(UpdateEnemyArrow());
    }

    private void CheckActions()
    {
        AttackDirection InputDirection = GetCurrentAttackDirection();
        if (InputDirection == AttackDirection.None && currentState != State.Neutral)
        {
            GameObject DA = GameObject.Find("DamageToEnemy");
            DA.GetComponent<Text>().text = "Miss";
            DA.GetComponent<Text>().enabled = true;
            ResetComboCounter();
        }
        // need path for player defending
        if (currentState == State.Defend)
        {
            double DamageDealt = PerformDefence();
            DealDamageToPlayer(DamageDealt);
        }
        // path for player attacking
        else if (currentState == State.Attack && InputDirection != AttackDirection.None)
        {
            double DamageDealt = PerformAttack();
            DealDamageToEnemy(DamageDealt);
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
        if (GetCurrentAttackDirection() != AttackDirection.None)
        {
            lastInputTime = Time.time;
            lastInputBeatDist = getBeatDist();
        }
    }

    public void MagicBarChanger()
    {
        // Beats total = 283
        if (totalBeat <= 10)
        {
            magicBar.GetComponent<SpriteRenderer>().sprite = MAGICBARALL1;
        }
        else if (totalBeat <= 21)
        {
            magicBar.GetComponent<SpriteRenderer>().sprite = MAGICBARALL2;
        }
        else if (totalBeat <= 32)
        {
            magicBar.GetComponent<SpriteRenderer>().sprite = MAGICBARALL3;
        }
        else if (totalBeat <= 42)
        {
            magicBar.GetComponent<SpriteRenderer>().sprite = MAGICBARALL4;
        }
        else if (totalBeat <= 53)
        {
            magicBar.GetComponent<SpriteRenderer>().sprite = MAGICBARALL5;
        }
        else if (totalBeat <= 64)
        {
            magicBar.GetComponent<SpriteRenderer>().sprite = MAGICBARALL6;
        }
        else if (totalBeat <= 74)
        {
            magicBar.GetComponent<SpriteRenderer>().sprite = MAGICBARALL7;
        }
        else if (totalBeat <= 85)
        {
            magicBar.GetComponent<SpriteRenderer>().sprite = MAGICBARALL8;
        }
        else if (totalBeat <= 96)
        {
            magicBar.GetComponent<SpriteRenderer>().sprite = MAGICBARALL9;
        }
        else if (totalBeat <= 107)
        {
            magicBar.GetComponent<SpriteRenderer>().sprite = MAGICBARALL10;
        }
        else if (totalBeat <= 118)
        {
            magicBar.GetComponent<SpriteRenderer>().sprite = MAGICBARALL11;
        }
        else if (totalBeat <= 129)
        {
            magicBar.GetComponent<SpriteRenderer>().sprite = MAGICBARALL12;
        }
        else if (totalBeat <= 140)
        {
            magicBar.GetComponent<SpriteRenderer>().sprite = MAGICBARALL13;
        }
        else if (totalBeat <= 151)
        {
            magicBar.GetComponent<SpriteRenderer>().sprite = MAGICBARALL14;
        }
        else if (totalBeat <= 162)
        {
            magicBar.GetComponent<SpriteRenderer>().sprite = MAGICBARALL15;
        }
        else if (totalBeat <= 173)
        {
            magicBar.GetComponent<SpriteRenderer>().sprite = MAGICBARALL16;
        }
        else if (totalBeat <= 184)
        {
            magicBar.GetComponent<SpriteRenderer>().sprite = MAGICBARALL17;
        }
        else if (totalBeat <= 195)
        {
            magicBar.GetComponent<SpriteRenderer>().sprite = MAGICBARALL18;
        }
        else if (totalBeat <= 206)
        {
            magicBar.GetComponent<SpriteRenderer>().sprite = MAGICBARALL19;
        }
        else if (totalBeat <= 217)
        {
            magicBar.GetComponent<SpriteRenderer>().sprite = MAGICBARALL20;
        }
        else if (totalBeat <= 228)
        {
            magicBar.GetComponent<SpriteRenderer>().sprite = MAGICBARALL21;
        }
        else if (totalBeat <= 239)
        {
            magicBar.GetComponent<SpriteRenderer>().sprite = MAGICBARALL22;
        }
        else if (totalBeat <= 250)
        {
            magicBar.GetComponent<SpriteRenderer>().sprite = MAGICBARALL23;
        }
        else if (totalBeat <= 261)
        {
            magicBar.GetComponent<SpriteRenderer>().sprite = MAGICBARALL24;
        }
        else if (totalBeat <= 272)
        {
            magicBar.GetComponent<SpriteRenderer>().sprite = MAGICBARALL25;
        }
        else if (totalBeat <= 283)
        {
            magicBar.GetComponent<SpriteRenderer>().sprite = MAGICBARALL26;
            PlayerDied();
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
            Boss.GetComponent<SpriteRenderer>().sprite = EnemyDeath;
            Boss.GetComponent<Transform>().localScale = new UnityEngine.Vector3(1800f, 1800f, 0f);
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
        StartCoroutine(StartBreakCountdown());
        SetCurrentState(State.Neutral);
    }

    private void PlayerDied()
    { 
        Shrimp.GetComponent<SpriteRenderer>().sprite = ShrimpDeath;
        Shrimp.GetComponent<Transform>().localScale = new UnityEngine.Vector3(1800f, 1800f, 0f);
        Shrimp.GetComponent<Transform>().Translate(0f, -20f, 0f);
        print("died lollers99");
    }
}