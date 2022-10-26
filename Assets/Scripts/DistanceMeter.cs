using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceMeter : MonoBehaviour
{

    [SerializeField] private Color cold;
    [SerializeField] private Color warm;

    private Transform destination;
    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (destination == null) return;

        var dp = getdp();
        increaseTemp(Mathf.Clamp(dp, 0, 1));
    }

    float getdp() {
        var a = Input.gyro.gravity;
        var b = destination.position-transform.position;
        return Vector3.Dot(a.normalized, b.normalized);
    }

    void increaseTemp(float value) {
        sr.color = Color.Lerp(cold, warm, value);
    }

    public void SetDestination(Transform d) {
        destination = d;
    }
}
