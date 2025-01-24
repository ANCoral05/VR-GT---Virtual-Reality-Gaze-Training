using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRK_BuildingBlocks;

public class DisableTargetOnGround : MonoBehaviour
{
    public IntVariable lives;

    [SerializeField]
    private float groundLevel = 0;

    void Update()
    {
        if (this.transform.position.y < groundLevel)
        {
            if (lives != null && lives.value > 0)
                lives.value -= 1;

            this.gameObject.SetActive(false);
        }
    }
}
