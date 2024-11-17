using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelScreen : MonoBehaviour
{
    [SerializeField] private CameraController cameraController;
    [SerializeField] private GridController gridController;

    public int CurrLevelIndex { get; private set; }

    public void SetupLevel(int levelIndex)
    {
        CurrLevelIndex = levelIndex;

        gridController.CreateGrid();
    }

    public void DisableTheLevel()
    {
        cameraController.DisableTheCamera();
    }
}
