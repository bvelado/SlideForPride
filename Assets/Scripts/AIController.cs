using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]
public class AIController : Fighter {

    public float maxSpeed;
    public float moveForceMultiplier;
    public float dashCooldown;
    public float dashForceMultiplier;

    Rigidbody ennemy;
    Vector3 mapCenter = Vector3.zero;
    Vector3 movement;
    float timeSinceDash;
    float distanceFromCenter;

    Fighter[] otherFighters;

    bool isInitialized = false;

    public void Init()
    {
        List<Fighter> tempFighterList = new List<Fighter>(GameController.Instance.Fighters);

        tempFighterList.Remove(this);

        otherFighters = new Fighter[tempFighterList.Count];

        otherFighters = tempFighterList.ToArray();

        tempFighterList.Clear();

        isInitialized = true;
    }

    void Start()
    {
        ennemy = GetComponent<Rigidbody>();
    }

	void Update () {
        if(isInitialized)
        {
            timeSinceDash += Time.deltaTime;

            distanceFromCenter = Vector3.Distance(ennemy.position, mapCenter);


            // Dash move
            if (distanceFromCenter > 2.0f)
            {
                movement = -(ennemy.position - mapCenter).normalized * moveForceMultiplier * Mathf.Clamp(distanceFromCenter, 0f, 1f);
                movement.y = 0f;

                if (timeSinceDash > dashCooldown)
                {
                    ennemy.AddForce(movement.normalized * dashForceMultiplier, ForceMode.Impulse);
                    timeSinceDash = 0.0f;
                }
            }
            else
            {
                float minimalDistanceToEnnemy = Vector3.Distance(otherFighters[0].transform.position, ennemy.position);
                Vector3 closestEnnemyPosition = otherFighters[0].transform.position;

                for (int i = 0; i < otherFighters.Length; i++)
                {
                    if (Vector3.Distance(otherFighters[i].transform.position, ennemy.position) < minimalDistanceToEnnemy)
                    {
                        minimalDistanceToEnnemy = Vector3.Distance(otherFighters[i].transform.position, ennemy.position);
                        closestEnnemyPosition = otherFighters[i].transform.position;
                    }
                }

                movement = -(ennemy.position - closestEnnemyPosition).normalized * moveForceMultiplier;
                movement.y = 0f;

                if (timeSinceDash > dashCooldown && minimalDistanceToEnnemy < 1.0f)
                {
                    ennemy.AddForce(movement.normalized * dashForceMultiplier, ForceMode.Impulse);
                    timeSinceDash = 0.0f;
                }
            }

            ennemy.AddForce(movement);

            if (ennemy.velocity.magnitude > maxSpeed)
            {
                ennemy.velocity = Vector3.Lerp(ennemy.velocity, ennemy.velocity.normalized * maxSpeed, Time.smoothDeltaTime);
            }
        }
    }
}
