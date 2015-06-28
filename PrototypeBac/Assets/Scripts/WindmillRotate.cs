using UnityEngine;
using System.Collections;

public class WindmillRotate : MonoBehaviour {
    [SerializeField]
    Transform WindMill;
    [SerializeField]
    float Speed;
	
	void FixedUpdate () {
        WindMill.Rotate(Vector3.forward, Speed);
	}
}