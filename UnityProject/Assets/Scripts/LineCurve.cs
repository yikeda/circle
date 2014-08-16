using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(CurvySpline))]
public class LineCurve : MonoBehaviour {

	[SerializeField] int lineRendererSize = 100;
	[SerializeField] int bulletSize = 20;
	[SerializeField] int controlPointSize = 10;	
	[SerializeField] float interval = 1.0f;
	[SerializeField] float range = 100.0f;
	[SerializeField] float shotPower = 1.0f;		
	
	private LineRenderer lineRenderer;
	private List<GameObject> bullets;
	private DateTime lastShotTime;
	private GameObject bulletRoot;
	private CurvySpline spline;	
	private float shotDirection;
	private float currentRotation;
	private float baseRotation;
	private float targetRotation;
	private DateTime rotateStartTime;
	private TimeSpan rotateTime;
	private bool isSplineInitialized;
	
	void Start ()
	{
		lineRenderer = this.GetComponent<LineRenderer>();
		lineRenderer.SetVertexCount(lineRendererSize);
		
		bullets = new List<GameObject>();
		
		bulletRoot = GameObject.Find("BulletRoot");
		
		lastShotTime = DateTime.Now;

		spline = GetComponent<CurvySpline>();		
	}
	
	void Update ()
	{
		if (!isSplineInitialized && spline != null && spline.IsInitialized)
			initCurvySpline();
		
		CheckRange();
		
		UpdateTargetRotation();
		UpdateCurvyPosition();
		UpdateLine();
		
		if ( bulletSize > bullets.Count &&
			 DateTime.Now - lastShotTime > TimeSpan.FromSeconds(interval) )
			Shot();
	}

	void initCurvySpline ()
	{
		for (int i = 0; i < controlPointSize; i++)
		{
			Vector3 position = Vector3.zero;
			spline.Add(position);
		}
		isSplineInitialized = true;
	}

	void Shot ()
	{		
		GameObject bullet = (GameObject)Instantiate(Resources.Load("Prefabs/bullet"), new Vector3(0,0,0), Quaternion.identity);
		bullet.transform.parent = bulletRoot.transform;		
		bullets.Add(bullet);

		Rigidbody rigidbody = bullet.GetComponent<Rigidbody>();
		Vector3 direction = Vector3.forward;
		direction = Quaternion.AngleAxis(currentRotation * Mathf.Rad2Deg, Vector3.up) * direction;
		
		rigidbody.AddForce(direction.normalized * shotPower, ForceMode.VelocityChange);
		
		lastShotTime = DateTime.Now;
	}

	void CheckRange ()
	{
		List<GameObject> removeList = new List<GameObject>();
		
		foreach(GameObject bullet in bullets)
		{
			float distance = Vector3.Distance(bullet.transform.position, Vector3.zero);

			if (distance > range)
			{				
				removeList.Add(bullet);
				GameObject.Destroy(bullet);
			}
		}

		bullets.RemoveAll(s => removeList.Contains(s));
	}
	
	void UpdateTargetRotation()
	{
		TimeSpan progressTime = DateTime.Now - rotateStartTime;
		
		if (progressTime > rotateTime)
		{
			float seconds = UnityEngine.Random.Range(0.5f,1.5f);
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
	
	void UpdateCurvyPosition ()
	{
		if (controlPointSize > bullets.Count)
			return;
		
		for (int i = 0; i < controlPointSize; i++)
		{
			int index = bullets.Count - i - 1;
			spline.ControlPoints[i].transform.position = bullets[index].transform.position; 
		}
	}

	void UpdateLine ()
	{
		int index = 0;
		foreach(GameObject bullet in Enumerable.Reverse(bullets))
		{
			if (index >= lineRendererSize)
				return;
			
			Vector3 position = bullet.transform.position;
			lineRenderer.SetPosition(index, position);			
			index++;
		}
	}
		
}
