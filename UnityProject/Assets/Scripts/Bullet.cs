using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(LineRenderer))]
public class Bullet : MonoBehaviour {

	public class CirclePosition {
		float rotation;
		float distance;

		public CirclePosition(float rotation, float distance)
		{
			this.Rotation = rotation;
			this.Distance = distance;			
		}
		
		public float Rotation
		{
			get { return rotation; }
			set { rotation = value; }
		}

		public float Distance
		{
			get { return distance; }
			set { distance = value; }
		}
	}

	[SerializeField] int size = 100;
	[SerializeField] float distanceOffset = 0.1f;	
	[SerializeField] float speed = 0.1f;
	
	private List<CirclePosition> positions;
	private List<GameObject> spheres;	
	private float currentRotation;
	private float baseRotation;
	private float targetRotation;
	private DateTime rotateStartTime;
	private TimeSpan rotateTime;
	
	void Start () {
		positions = new List<CirclePosition>();
		spheres = new List<GameObject>();
		rotateTime = TimeSpan.FromSeconds(5);
		rotateStartTime = DateTime.Now;

		InitPositions();
		CreateSpheres();		
	}
	
	void Update () {
		UpdateTargetRotation();
		UpdatePositions();
		UpdateBullet();
	}

	void InitPositions()
	{
		for(int i = 0; i < size; i++)
		{
			float rotation = 0.0f;
			float distance = i * distanceOffset;
			positions.Add(new CirclePosition(rotation,distance));
		}
	}

	void CreateSpheres()
	{
		foreach(CirclePosition circlePosition in positions)
		{
			GameObject sphere = (GameObject)Instantiate(Resources.Load("Prefabs/sphere")) as GameObject;
			//GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			spheres.Add(sphere);
		}
	}
	
	void UpdateTargetRotation()
	{
		TimeSpan progressTime = DateTime.Now - rotateStartTime;
		
		if (progressTime > rotateTime)
		{
			float seconds = UnityEngine.Random.Range(1.0f,2.0f);
			rotateTime = TimeSpan.FromSeconds(seconds);
			rotateStartTime = DateTime.Now;
			progressTime = TimeSpan.FromSeconds(0);

			baseRotation = currentRotation;
			targetRotation = UnityEngine.Random.Range(0.0f, Mathf.PI * 3.0f);
			//targetRotation = baseRotation + Mathf.PI * 2.0f;
		}
		
		float rate = (float)progressTime.Ticks / (float)rotateTime.Ticks;
		currentRotation = Mathf.Lerp(baseRotation, targetRotation, rate);
	}
	
	void UpdatePositions()
	{
		if(positions[0].Distance > distanceOffset)
		{
			float distance = positions[0].Distance - distanceOffset;
			float rotation = currentRotation;
			//float rotation = positions[0].Rotation - rotationRange * Mathf.Deg2Rad;
			//float rotation = positions[0].Rotation - UnityEngine.Random.Range(rotationRange,-rotationRange) * Mathf.Deg2Rad;
			positions.Insert(0, new CirclePosition(rotation,distance));
			positions.RemoveAt(positions.Count - 1);
		}
	}
			
	void UpdateBullet()
	{
		int index = 0;
		float offset = 135.0f * Mathf.Deg2Rad;
		foreach(CirclePosition circlePosition in positions)
		{
			float x = Mathf.Cos(circlePosition.Rotation + offset) * circlePosition.Distance;
			float z = Mathf.Sin(circlePosition.Rotation + offset) * circlePosition.Distance;
						
			Vector3 position = new Vector3(x,0,z);			
			GameObject sphere = spheres[index];
			sphere.transform.position = position;
			circlePosition.Distance += speed;
			
			index++;
		}
	}	
}
