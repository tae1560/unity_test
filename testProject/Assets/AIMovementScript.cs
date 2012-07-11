using UnityEngine;
using System.Collections;

public enum MovementPattern {
	LineTracing,
	CycleTracing,
	RandomTraversal
};

public class AIMovementScript : MonoBehaviour {
	public MovementPattern pattern;
	public Vector3[] positionSequence;
	public float velocity;
	
	Vector3 targetPosition;
	int index = 0;
	bool isUpwarding = true;
	
	// config
	public float weightOfVelocity = 1.0f;
	public int rangeOfRandomMove = 5;
	public float visionAngle = 180.0f;
	public float rangeOfvision = 50.0f;
	public GameObject targetObject = null;
	
	// Use this for initialization
	void Start () {
		targetPosition = gameObject.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		
		// check target is in the sight
		if (isTargetInVision()) {
			targetPosition = targetObject.transform.position;
			
			// TODO : check distance => attack
			// NEED : attack range, attack function
		}
		
		
		Vector3 currentPosition = gameObject.transform.position;
		Vector3 diff = targetPosition - currentPosition;
		
		//Debug.Log("diff : " + diff + " magnitude : " + diff.magnitude);
		
		if(diff.magnitude > weightOfVelocity * velocity) {
			diff = diff.normalized * weightOfVelocity * velocity;
			gameObject.transform.position = currentPosition + diff;
		}
		else {
			//Debug.Log("target : " + targetPosition);
			// move gameobject to target position
			gameObject.transform.position = targetPosition;
			
			// movement part
			switch (pattern) {
			case MovementPattern.LineTracing:
				if(isUpwarding) {
					if (positionSequence.GetLength(1) - 1 == index) {
						index --;
						isUpwarding = false;
					} else
						index ++;
				} else {
					if (0 == index) {
						index ++;
						isUpwarding = true;
					} else
						index --;
				}
				targetPosition = positionSequence[index];
				break;
			case MovementPattern.CycleTracing:
				targetPosition = positionSequence[index++ % positionSequence.GetLength(1)];
				break;
			case MovementPattern.RandomTraversal:
				targetPosition.x = targetPosition.x + Random.Range(-rangeOfRandomMove, rangeOfRandomMove);
				//targetPosition.y = targetPosition.y + Random.Range(-rangeOfRandomMove, rangeOfRandomMove);
				targetPosition.z = targetPosition.z + Random.Range(-rangeOfRandomMove, rangeOfRandomMove);
				break;
			}
			
			// TODO : make to rotation correctly
			// change pose 
			Vector3 diff2 = targetPosition - gameObject.transform.position;
			
			// temporary set to lookat
			// TODO : change it using mathematical version.
			gameObject.transform.LookAt(diff2);
		}
		
	}
	
	
	// check target object in the vision of agent
	bool isTargetInVision() {
		if (targetObject) {
			Vector3 diff = targetObject.transform.position - gameObject.transform.position;
			float diffAngle = Vector3.Angle(gameObject.transform.forward, diff);
			
			if(diff.magnitude > rangeOfvision || diffAngle > visionAngle / 2) return false;
			else return true;
		} else return false;
	}
}
