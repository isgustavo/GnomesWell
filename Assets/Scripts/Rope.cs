using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour {


	public GameObject ropeSegmentPrefab;

	List<GameObject> ropeSegments = new List<GameObject> ();

	public bool isIncreasing { get; set; }
	public bool isDecreasing { get; set; }

	public Rigidbody2D connectedObject;

	public float maxRopeSegmentLenght = 1f;

	public float ropeSpeed = 4f;

	LineRenderer lineRenderer;

	void Start () {

		lineRenderer = GetComponent<LineRenderer> ();

		ResetLength ();

	}

	public void ResetLength () {

		foreach (GameObject segment in ropeSegments) {
			Destroy (segment);
		}

		ropeSegments = new List <GameObject> ();

		isDecreasing = false;
		isIncreasing = false;

		CreateRopeSegment ();
	}

	void CreateRopeSegment () {


		GameObject segment = (GameObject)Instantiate (ropeSegmentPrefab, this.transform.position, Quaternion.identity);

		segment.transform.SetParent (this.transform, true);

		Rigidbody2D segmentBody = segment.GetComponent <Rigidbody2D> ();

		SpringJoint2D segmentJoint = segment.GetComponent<SpringJoint2D> ();

		if (segment == null || segmentJoint == null) {


			Debug.LogError ("Rope segmetn body prefag has no Rigidbory2D and/or SpringJoint2D!");
			return;
		}

		ropeSegments.Insert (0, segment);

		if (ropeSegments.Count == 1) {


			SpringJoint2D connectedObjectJoint = connectedObject.GetComponent<SpringJoint2D> ();

			connectedObjectJoint.connectedBody = segmentBody;
			connectedObjectJoint.distance = 0.1f;

			segmentJoint.distance = maxRopeSegmentLenght;

		} else {

			GameObject nextSegment = ropeSegments [1];

			SpringJoint2D nextSegmentJoint = nextSegment.GetComponent<SpringJoint2D> ();
			nextSegmentJoint.connectedBody = segmentBody;

			segmentJoint.distance = 0.0f;
		}

		segmentJoint.connectedBody = this.GetComponent<Rigidbody2D> ();

	}

	void RemoveRopeSegment () {

		if (ropeSegments.Count < 2) {

			return;
		}

		GameObject topSegment = ropeSegments [0];
		GameObject nextSegment = ropeSegments [1];

		SpringJoint2D nextSegmentJoint = nextSegment.GetComponent<SpringJoint2D> ();

		nextSegmentJoint.connectedBody = this.GetComponent<Rigidbody2D> ();

		ropeSegments.RemoveAt (0);
		Destroy (topSegment);

	}

	void Update () {

		GameObject topSegment = ropeSegments [0];
		SpringJoint2D topSegmentJoint = topSegment.GetComponent<SpringJoint2D> ();

		if (isIncreasing) {

			if (topSegmentJoint.distance >= maxRopeSegmentLenght) {

				CreateRopeSegment ();
			} else {

				topSegmentJoint.distance += ropeSpeed * Time.deltaTime;

			}
		} else if (isDecreasing) {

			if (topSegmentJoint.distance <= 0.005f) {

				RemoveRopeSegment ();

			} else {

				topSegmentJoint.distance -= ropeSpeed * Time.deltaTime;
			}
		}

		if (lineRenderer != null) {


			lineRenderer.positionCount = ropeSegments.Count + 2;
			lineRenderer.SetPosition (0, this.transform.position);

			for (int i = 0; i < ropeSegments.Count; i++) {

				lineRenderer.SetPosition (i + 1, ropeSegments [i].transform.position);

			}

			SpringJoint2D connectedObjectJoint = connectedObject.GetComponent<SpringJoint2D> ();


			lineRenderer.SetPosition (ropeSegments.Count + 1, connectedObject.transform.TransformPoint (connectedObjectJoint.anchor));

		}
	}

}
