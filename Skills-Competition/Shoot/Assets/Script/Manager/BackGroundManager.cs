using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackGroundManager : MonoBehaviour
{
    public const float StartY = 9.5f;
    public const float EndY = -10;

    public static BackGroundManager Instance;
    [SerializeField] private List<BackGround> bgObjects;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }

        Instance = this;
    }

    public readonly float[] BgSpeed =
    {
        0.03f, // Bg 1
        0.06f, // Bg 2
        0.09f, // Bg 3
        0.12f // Bg 4
    };

    public void ChangeStage(int stage)
    {
        foreach (var backGround in bgObjects)
        {
            backGround.SetImage(stage - 1);
        }
    }


}
