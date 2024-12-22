using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonComponent<GameManager>
{
    [SerializeField] private GameController gameController;
    [SerializeField] private int StartingLevel = 1;
    [SerializeField] private KeyCode ChangeLevelKey = KeyCode.Space;

    private void Awake()
    {
        gameController.SetupGame(StartingLevel - 1);
    }

    private void Update()
    {
        if(Input.GetKeyUp(ChangeLevelKey))
        {
            gameController.SetupGame(StartingLevel - 1);
        }
    }
}
