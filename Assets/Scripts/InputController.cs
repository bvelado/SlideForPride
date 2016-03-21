using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class InputController : Fighter {
    
    public float moveForceMultiplier = 2.0f;
    public float maxSpeed = 2.0f;
    public float dashForceMultiplier;
    public float dashCooldown;

    Rigidbody player;
    Vector3 movement;
    float timeSinceDash;

    bool updateUI = true;

    void Start () {
        player = GetComponent<Rigidbody>();
        timeSinceDash = dashCooldown;

        StartCoroutine(UpdateUIEverySeconds(0.2f));
    }

	void Update () {
        timeSinceDash += Time.deltaTime;

        movement = new Vector3(Input.GetAxis("Horizontal") * moveForceMultiplier, 0f, Input.GetAxis("Vertical") * moveForceMultiplier);

        // Dash move
        if (Input.GetButtonDown("Jump") && movement != Vector3.zero && timeSinceDash > dashCooldown)
        {
            player.AddForce(movement.normalized * dashForceMultiplier, ForceMode.Impulse);
            timeSinceDash = 0.0f;
        } else
        {
            player.AddForce(movement);
        }

        if (player.velocity.magnitude > maxSpeed)
        {
            player.velocity = Vector3.Lerp(player.velocity, player.velocity.normalized * maxSpeed, Time.smoothDeltaTime);
        }
    }

    IEnumerator UpdateUIEverySeconds(float seconds)
    {
        while(updateUI)
        {
            UIController.Instance.UpdateDashCooldown(Mathf.Clamp(timeSinceDash / dashCooldown, 0f, 1f));

            yield return null;
        }
    }
}
