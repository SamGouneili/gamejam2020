using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Attack : MonoBehaviour
{
    private const double BASE_DAMAGE_HERO = 10.0;
    private const double BASE_DAMAGE_ENEMY = 25.0;
    private const double PERFECT_TIMING_MULT = 1.25;
    private const double GOOD_TIMING_MULT = 1.00;
    private const double OKAY_TIMING_MULT = 0.75;
    private const double MISS_TIMING_MULT = 0.00;
    private const double PERFECT_DEFENCE_MULT = 0.0;
    private const double GOOD_DEFENCE_MULT = 0.20;
    private const double OKAY_DEFENCE_MULT = 0.50;
    private const double MISS_DEFENCE_MULT = 1.00;
    private const double DIRECTION_BONUS_MULT = 1.10;
    private const double PERFECT_TIMING_THRESH = 0.0;
    private const double GOOD_TIMING_THRESH = 0.05;
    private const double OKAY_TIMING_THRESH = 0.15;
    private const double MISS_TIMING_THRESH = 0.35;

    private BattleState BS;

    public AttackTiming lastHitTiming;

    public enum AttackTiming
    {
        Perfect,
        Good,
        Okay,
        Miss
    }

    // Start is called before the first frame update
    void Start()
    {
        BS = FindObjectOfType<BattleState>();
    }

    public double PerformAttack()
    {
        bool Block = IsBlock();
        bool Hit = !Block;
        AttackTiming Timing = GetTiming();
        if (Timing == AttackTiming.Miss)
        {
            BS.ResetComboCounter();
            Hit = false;
        }

        lastHitTiming = GetTiming();

        if (Hit)
        {
            BS.IncrementComboCounter();
            return CalculateDamage(IsDirectionBonus(), GetTiming());
        }
        else if (Block)
        {
            GameObject BT = GameObject.Find("BlockTextEnemy");
            BT.GetComponent<Text>().enabled = true;
        }

        return 0.0;
    }

    public double PerformDefence()
    {
        bool Block = IsBlock();
        bool Hit = !Block;
       
        AttackTiming BlockTiming = GetTiming();
        if (BlockTiming == AttackTiming.Miss)
        {
            BS.ResetComboCounter();
            Hit = true;
        }

        lastHitTiming = GetTiming();

        if (Hit)
        {
            return CalculateDamageToPlayer(IsDirectionBonus(), GetTiming());
        }
        else if (Block)
        {
            GameObject BT = GameObject.Find("BlockText");
            BT.GetComponent<Text>().enabled = true;
            BS.IncrementComboCounter();
            return 0.0;
        }
        else
        {
            BS.IncrementComboCounter();
            return 0.0;
        }
    }

    private double CalculateDamage(bool DirectionBonus, AttackTiming TimingBonus)
    {
        double Damage = BASE_DAMAGE_HERO;
        if (DirectionBonus)
        {
            Damage *= DIRECTION_BONUS_MULT;
        }
        switch (TimingBonus)
        {
            case (AttackTiming.Perfect):
                Damage *= PERFECT_TIMING_MULT;
                break;
            case (AttackTiming.Good):
                Damage *= GOOD_TIMING_MULT;
                break;
            case (AttackTiming.Okay):
                Damage *= OKAY_TIMING_MULT;
                break;
            case (AttackTiming.Miss):
                Damage *= MISS_TIMING_MULT;
                break;
            default:
                break;
        }
        if (BS.GetCurrentState() != BattleState.State.Neutral)
        {
            GameObject DA = GameObject.Find("DamageToEnemy");
            DA.GetComponent<Text>().text = Damage.ToString();
            if (TimingBonus == AttackTiming.Miss && BS.GetCurrentState() != BattleState.State.Neutral)
            {
                DA.GetComponent<Text>().text = "Miss";
            }
            DA.GetComponent<Text>().enabled = true;
        }
        return Damage;
    }

    private double CalculateDamageToPlayer(bool DirectionBonus, AttackTiming TimingBonus)
    {
        double Damage = BASE_DAMAGE_ENEMY;
        if (DirectionBonus)
        {
            Damage *= DIRECTION_BONUS_MULT;
        }
        switch (TimingBonus)
        {
            case (AttackTiming.Perfect):
                Damage *= PERFECT_DEFENCE_MULT;
                break;
            case (AttackTiming.Good):
                Damage *= GOOD_DEFENCE_MULT;
                break;
            case (AttackTiming.Okay):
                Damage *= OKAY_DEFENCE_MULT;
                break;
            case (AttackTiming.Miss):
                Damage *= MISS_DEFENCE_MULT;
                break;
            default:
                break;
        }
        GameObject DA = GameObject.Find("HeroDamage");
        DA.GetComponent<Text>().text = Damage.ToString();
        if (TimingBonus == AttackTiming.Miss)
        {
            DA.GetComponent<Text>().text = "Miss";
        }
        DA.GetComponent<Text>().enabled = true;
        return Damage;
    }

    private bool IsDirectionBonus()
    {
        bool DirectionBonus = false;

        if (BS.GetCurrentAttackDirection() == BattleState.AttackDirection.Up && BS.GetEnemyAttackDirection() == BattleState.AttackDirection.Down
            || BS.GetCurrentAttackDirection() == BattleState.AttackDirection.Down && BS.GetEnemyAttackDirection() == BattleState.AttackDirection.Up
            || BS.GetCurrentAttackDirection() == BattleState.AttackDirection.Left && BS.GetEnemyAttackDirection() == BattleState.AttackDirection.Right
            || BS.GetCurrentAttackDirection() == BattleState.AttackDirection.Right && BS.GetEnemyAttackDirection() == BattleState.AttackDirection.Left)
        {
            DirectionBonus = true;
        }

        return DirectionBonus;
    }

    private AttackTiming GetTiming()
    {
        AttackTiming Timing = AttackTiming.Miss;
        double DistToNextBeat = BS.lastInputBeatDist;

        if (DistToNextBeat >= MISS_TIMING_THRESH)
        {
            Timing = AttackTiming.Miss;
        }
        else if (DistToNextBeat >= OKAY_TIMING_THRESH)
        {
            Timing = AttackTiming.Okay;
        }
        else if (DistToNextBeat >= GOOD_TIMING_THRESH)
        {
            Timing = AttackTiming.Good;
        }
        else if (DistToNextBeat >= PERFECT_TIMING_THRESH)
        {
            Timing = AttackTiming.Perfect;
        }

        return Timing;
    }

    private bool IsBlock()
    {
        bool Block = false;

        if (BS.GetCurrentAttackDirection() == BattleState.AttackDirection.Up && BS.GetEnemyAttackDirection() == BattleState.AttackDirection.Up
            || BS.GetCurrentAttackDirection() == BattleState.AttackDirection.Down && BS.GetEnemyAttackDirection() == BattleState.AttackDirection.Down
            || BS.GetCurrentAttackDirection() == BattleState.AttackDirection.Left && BS.GetEnemyAttackDirection() == BattleState.AttackDirection.Left
            || BS.GetCurrentAttackDirection() == BattleState.AttackDirection.Right && BS.GetEnemyAttackDirection() == BattleState.AttackDirection.Right)
        {
            Block = true;
        }

        return Block;
    }
}
