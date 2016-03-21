using UnityEngine;
using System.Collections;

public class BalanceController : MonoBehaviour {

    public Transform balancedGround;
    public float maxBalanceAngle;
    public float balanceAngleMultiplier;

    Rigidbody[] players;
    bool isInitialized = false;

    Vector3 balanceCenter = Vector3.zero;

	void Update () {
        if(isInitialized)
        {
            Vector3 playersGravityCenter = Vector3.zero;

            for (int i = 0; i < players.Length; i++)
            {
                playersGravityCenter += Vector3.Distance(players[i].position, balanceCenter) * (players[i].position - balanceCenter).normalized;
            }

            Vector3 balanceAngle = Vector3.Distance(balanceCenter, playersGravityCenter) * new Vector3(playersGravityCenter.z - balanceCenter.z, 0, -playersGravityCenter.x - balanceCenter.x).normalized * balanceAngleMultiplier;
            balanceAngle.x = Mathf.Clamp(balanceAngle.x, -maxBalanceAngle, maxBalanceAngle);
            balanceAngle.z = Mathf.Clamp(balanceAngle.z, -maxBalanceAngle, maxBalanceAngle);

            balancedGround.localEulerAngles = balanceAngle;
        }
    }

    public void Stop()
    {
        isInitialized = false;
    }

    public void Initialize()
    {
        players = new Rigidbody[GetComponent<GameController>().Fighters.Length];
        for (int i = 0; i < GetComponent<GameController>().Fighters.Length; i++)
        {
            players[i] = GetComponent<GameController>().Fighters[i].GetComponent<Rigidbody>();
        }

        print("Initialized with " + players.Length + " players.");

        isInitialized = true;
    }
}
