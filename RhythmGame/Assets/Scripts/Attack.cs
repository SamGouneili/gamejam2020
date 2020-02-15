using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    private const double BASE_DAMAGE_HERO = 10.0;
    private const double PERFECT_TIMING_MULT = 1.25;
    private const double GOOD_TIMING_MULT = 1.00;
    private const double OKAY_TIMING_MULT = 0.75;
    private const double MISS_TIMING_MULT = 0.00;
    private const double DIRECTION_BONUS_MULT = 1.10;
    private const double PERFECT_TIMING_THRESH = 0.0;
    private const double GOOD_TIMING_THRESH = 0.1;
    private const double OKAY_TIMING_THRESH = 0.25;
    private const double MISS_TIMING_THRESH = 0.4;

    private BattleState BS;

    public enum AttackDirection
    {
        Up,
        Down,
        Left,
        Right,
        None
    }

    public enum AttackTiming
    {
        Perfect,
        Good,
        Okay,
        Miss
    }

    private AttackDirection EnemyAttackDirection;

    private AttackDirection CurrentAttackDirection;

    // Start is called before the first frame update
    void Start()
    {
        CurrentAttackDirection = AttackDirection.None;
        EnemyAttackDirection = AttackDirection.None;

        BS = FindObjectOfType<BattleState>();
    }

    public AttackDirection GetCurrentAttackDirection()
    {
        return CurrentAttackDirection;
    }

    public void SetAttackDirection(AttackDirection NewAttackDirection)
    {
        CurrentAttackDirection = NewAttackDirection;
    }

    public double PerformAttack()
    {
        bool Hit = !IsEnemyBlock();
        AttackTiming Timing = GetTiming();
        if (Timing == AttackTiming.Miss)
        {
            Hit = false;
        }

        if (Hit)
        {
            return CalculateDamage(IsDirectionBonus(), GetTiming());
        }

        return 0.0;
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
        return Damage;
    }

    private bool IsDirectionBonus()
    {
        bool DirectionBonus = false;

        if (CurrentAttackDirection == AttackDirection.Up && EnemyAttackDirection == AttackDirection.Down
            || CurrentAttackDirection == AttackDirection.Down && EnemyAttackDirection == AttackDirection.Up
            || CurrentAttackDirection == AttackDirection.Left && EnemyAttackDirection == AttackDirection.Right
            || CurrentAttackDirection == AttackDirection.Right && EnemyAttackDirection == AttackDirection.Left)
        {
            DirectionBonus = true;
        }

        return DirectionBonus;
    }

    private AttackTiming GetTiming()
    {
        AttackTiming Timing = AttackTiming.Miss;
        double DistToNextBeat = BS.getBeatDist();

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

    private bool IsEnemyBlock()
    {
        bool Block = false;

        if (CurrentAttackDirection == AttackDirection.Up && EnemyAttackDirection == AttackDirection.Up
            || CurrentAttackDirection == AttackDirection.Down && EnemyAttackDirection == AttackDirection.Down
            || CurrentAttackDirection == AttackDirection.Left && EnemyAttackDirection == AttackDirection.Left
            || CurrentAttackDirection == AttackDirection.Right && EnemyAttackDirection == AttackDirection.Right)
        {
            Block = true;
        }

        return Block;
    }
}
