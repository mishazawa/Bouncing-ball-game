using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishSpot : MonoBehaviour
{
    void OnTriggerEnter2D (Collider2D coll) {
        Debug.Log("Entered");
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
}
