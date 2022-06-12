using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // CircleCollider2D is collider of winning triger
        if (collision is CircleCollider2D)
            transform.parent.GetComponent<PuzzleGame>().OnWin();
        else
            transform.parent.GetComponent<PuzzleGame>().OnFail();
    }
}
