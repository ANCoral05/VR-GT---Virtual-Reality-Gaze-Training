using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRK_BuildingBlocks;
using TMPro;

public class ShootingStarGame : MonoBehaviour
{
    [SerializeField]
    private Transform spawner;

    [SerializeField]
    private IntVariable score;

    [SerializeField]
    private IntVariable lives;

    [SerializeField]
    private TextMeshPro signText;

    private void Update()
    {
        if (lives.Value <= 0)
        {
            GameLost();
        }
    }

    public void GameStart()
    {
        spawner.gameObject.SetActive(true);

        lives.Value = 3;

        score.Value = 0;

        signText.text = "Game started!";
    }

    public void GameLost()
    {
        foreach (Transform child in spawner)
        {
            child.gameObject.SetActive(false);
        }

        spawner.gameObject.SetActive(false);

        signText.text = "Start new game!";
    }
}
