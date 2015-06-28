using UnityEngine;
using System.Collections;

public class AnimationSound : MonoBehaviour {
	void PlaySound () {
        FMOD_StudioSystem.instance.PlayOneShot("event:/Environemnt/TODO", Vector3.zero);
	}
}
