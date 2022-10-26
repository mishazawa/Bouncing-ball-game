using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    private Camera cam;
    private float dampVelocity;

    [SerializeField] [Range(1, 15)] private float minOrth = 5;
    [SerializeField] [Range(15, 25)] private float maxOrth = 20;
    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void FixedUpdate() {
        if (rb == null) return;
        var orthvel = Mathf.Clamp(rb.velocity.magnitude, .5f, 3f);
        cam.orthographicSize = Mathf.SmoothDamp(minOrth, maxOrth, ref dampVelocity, orthvel);
    }
}
