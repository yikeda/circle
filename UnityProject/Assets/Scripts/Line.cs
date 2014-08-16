using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(CurvySpline))]
public class Line : MonoBehaviour {

	public class LinePosition {
		float rotation;
		float distance;

		public LinePosition(float rotation, float distance)
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
	
	private List<LinePosition> positions;
	private LineRenderer lineRenderer;
	private float currentRotation;
	private float baseRotation;
	private float targetRotation;
	private DateTime rotateStartTime;
	private TimeSpan rotateTime;
	private CurvySpline spline;
	
	void Start () {
		lineRenderer = this.GetComponent<LineRenderer>();
		lineRenderer.SetVertexCount(size);
		
		spline = this.GetComponent<CurvySpline>();
		
		positions = new List<LinePosition>();

		rotateTime = TimeSpan.FromSeconds(5);
		rotateStartTime = DateTime.Now;
		
		InitPositions();		
	}
	
	void Update () {
		UpdateTargetRotation();
		UpdatePositions();
		UpdateLine();
	}

	void InitPositions()
	{
		for(int i = 0; i < size; i++)
		{
			float rotation = 0.0f;
			float distance = i * distanceOffset;
			positions.Add(new LinePosition(rotation,distance));
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
			positions.Insert(0, new LinePosition(rotation,distance));
			positions.RemoveAt(positions.Count - 1);
		}
	}
			
	void UpdateLine()
	{
		int index = 0;
		float offset = 135.0f * Mathf.Deg2Rad;
		foreach(LinePosition linePosition in positions)
		{
			float x = Mathf.Cos(linePosition.Rotation + offset) * linePosition.Distance;
			float z = Mathf.Sin(linePosition.Rotation + offset) * linePosition.Distance;
						
			Vector3 position = new Vector3(x,0,z);
			lineRenderer.SetPosition(index, position);
			linePosition.Distance += speed;
			
			index++;
		}
	}	
}
