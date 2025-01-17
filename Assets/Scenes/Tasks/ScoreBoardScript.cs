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

    [SerializeField]
    private bool backToMenuOnGameLost = false;

    public UnityEvent onGameLost;

    private void Start()
    {
        if(lives != null)
        {
            lives.value = 3;
        }

        if (score != null)
        {
            score.value = 0;
        }
    }

    public void UpdateScore()
    {
        if(lives.value > 0)
        {
            scoreTextMesh.text = "Score: " + score.value;

            difficultyMultiplier.value = 1 + (score.value / 100f);
        }

        livesTextMesh.text = "Lives remaining: " + lives.value;

        if (lives.value <= 0 && backToMenuOnGameLost)
        {
            onGameLost.Invoke();
        }
    }

    public void Update()
    {
        UpdateScore();
    }
}

