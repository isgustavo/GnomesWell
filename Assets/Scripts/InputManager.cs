using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : Singleton <InputManager> {

	private float sidewaysMotion = 0.0f;

	public float SidewaysMotion {
		get { return sidewaysMotion; }

	}

	void Update () {

		Vector3 accel = Input.acceleration;

		sidewaysMotion = accel.x;
	}


}
