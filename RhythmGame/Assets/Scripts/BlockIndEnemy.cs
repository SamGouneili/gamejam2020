﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockIndEnemy : MonoBehaviour
{
    double ShowTime = 0.35;

    private void Update()
    {
        if (GetComponent<Text>().enabled == true)
        {
            ShowTime -= Time.deltaTime;
            if (ShowTime <= 0)
            {
                GetComponent<Text>().enabled = false;
                ShowTime = 0.35;
            }
        }
    }
}
