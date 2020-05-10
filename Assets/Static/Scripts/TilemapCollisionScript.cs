using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilemapCollisionScript : MonoBehaviour
{

    public SnowballScript snowball;
    
    void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "GroundDetector") {
            snowball.SetGrounded(true);
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if(other.tag == "GroundDetector") {
            snowball.SetGrounded(false);
        }
    }
}
