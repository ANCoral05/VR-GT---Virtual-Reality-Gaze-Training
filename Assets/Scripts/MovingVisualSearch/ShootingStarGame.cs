using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRK_BuildingBlocks;

public class ShootingStarGame : MonoBehaviour
{
    [SerializeField]
    private Transform spawner;

    [SerializeField]
    private IntVariable score;

    [SerializeField]
    private IntVariable lives;

    private void Update()
    {
        if (lives.value <= 0)
        {
            GameLost();
        }
    }

    public void GameStart()
    {
        spawner.gameObject.SetActive(true);

        lives.value = 3;

        score.value = 0;
    }

    public void GameLost()
    {
        foreach (Transform child in spawner)
        {
            child.gameObject.SetActive(false);
        }

        spawner.gameObject.SetActive(false);
    }
}
