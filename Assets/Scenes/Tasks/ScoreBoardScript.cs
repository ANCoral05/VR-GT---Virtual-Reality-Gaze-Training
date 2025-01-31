using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using VRK_BuildingBlocks;
using UnityEngine.Events;

public class ScoreBoardScript : MonoBehaviour
{
    [SerializeField]
    private IntVariable score;

    [SerializeField]
    private IntVariable lives;

    [SerializeField]
    private FloatVariable difficultyMultiplier;

    [SerializeField]
    private TextMeshProUGUI scoreTextMesh;

    [SerializeField]
    private TextMeshProUGUI livesTextMesh;

    public UnityEvent onGameLost;

    private void Start()
    {
        if(lives != null)
        {
            lives.Value = 3;
        }

        if (score != null)
        {
            score.Value = 0;
        }
    }

    public void UpdateScore()
    {
        if(lives.Value > 0)
        {
            scoreTextMesh.text = "Score: " + score.Value;

            difficultyMultiplier.Value = 1 + (score.Value / 100f);

            livesTextMesh.text = "Lives remaining: " + lives.Value;
        }

        if (lives.Value <= 0)
        {
            livesTextMesh.text = "Game Over!";
        }
    }

    public void Update()
    {
        UpdateScore();
    }
}

