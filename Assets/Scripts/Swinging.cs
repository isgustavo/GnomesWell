using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Rigidbody2D))]
public class Swinging : MonoBehaviour {

	public float swingSensitivity = 100.0f;

	protected Rigidbody2D rb2d;

	void Awake () {

		rb2d = GetComponent<Rigidbody2D> ();

	}


	void FixedUpdate () {


		if (GetComponent<Rigidbody2D> () == null) {

			Destroy (this);
			return;
		}

		float swing = InputManager.Instance.SidewaysMotion;

		Vector2 force = new Vector2 (swing * swingSensitivity, 0);

		rb2d.AddForce (force);

	}

}
