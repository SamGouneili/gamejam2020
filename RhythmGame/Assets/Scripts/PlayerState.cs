using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    private int AmountCoins;

    public enum Instrument
    {
        Lute,
        Flute,
        Bongos,
        Trumpet,
        None
    }

    private Instrument CurrentInstrument;

    // Start is called before the first frame update
    void Start()
    {
        AmountCoins = 0;
        CurrentInstrument = Instrument.None;
    }

    public int GetAmountCoins()
    {
        return AmountCoins;
    }

    public void SetAmountCoins(int NewAmountCoins)
    {
        AmountCoins = NewAmountCoins;
    }

    public void AddAmountCoins(int CoinsToAdd)
    {
        SetAmountCoins(AmountCoins + CoinsToAdd);
    }

    public void HalfCoinAmount()
    {
        SetAmountCoins(AmountCoins / 2);
    }

    public Instrument GetInstrument()
    {
        return CurrentInstrument;
    }

    public void SetInstrument(Instrument NewInstrument)
    {
        CurrentInstrument = NewInstrument;
    }
}
