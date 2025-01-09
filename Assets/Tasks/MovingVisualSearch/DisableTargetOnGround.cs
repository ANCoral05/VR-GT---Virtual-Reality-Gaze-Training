using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableTargetOnGround : MonoBehaviour
{
    void Update()
    {
        if(this.transform.position.y < 0)
        {
            this.gameObject.SetActive(false);
        }
    }
}
