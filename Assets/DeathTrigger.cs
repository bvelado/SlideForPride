using UnityEngine;
using System.Collections;

public class DeathTrigger : MonoBehaviour {

	void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(Tags.Player))
        {
            GameController.Instance.ResetGame();
        }

        if(other.CompareTag(Tags.Enemy))
        {

        }
    }
}
