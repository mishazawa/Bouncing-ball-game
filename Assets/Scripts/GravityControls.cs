using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityControls : MonoBehaviour
{
    private Gyroscope gyro;

    public bool isTouchControls = false;

    [Range(1, 20)] public static float gravityMultiplier = 9.8f;

    private Vector3 gravity = Vector3.one * gravityMultiplier;
    private Rigidbody2D body;

    void Awake () {
        Application.targetFrameRate = 300;
        body = GetComponent<Rigidbody2D>();

        if (!isTouchControls) {
            Input.gyro.enabled = true;
        }
    }


    void Update () {
        if (!isTouchControls) return;
        if (Input.touchCount > 0) {
            Touch touch = Input.GetTouch(0);
            if (touch.deltaPosition.x != 0) {
                AddTorqueImpulse(Mathf.Sign(touch.deltaPosition.x));
            }
        }
    }
    public void AddTorqueImpulse(float angularChangeInDegrees)
    {
        var impulse = (angularChangeInDegrees * Mathf.Deg2Rad) * body.inertia;
        body.AddTorque(impulse, ForceMode2D.Impulse);
    }


    void FixedUpdate () {
        if (isTouchControls) return;
        Physics2D.gravity = Vector3.Scale(gravity, Input.gyro.gravity);
    }

}
