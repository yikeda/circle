using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(LineRenderer))]
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
	
	private List<LinePosition> positions;
	private LineRenderer lineRenderer;
	
	void Start () {
		int size = 100;
		
		lineRenderer = this.GetComponent<LineRenderer>();
		lineRenderer.SetVertexCount(size);
		
		positions = new List<LinePosition>();

		for(int i = 0; i < size; i++)
		{
			float rotation = 0;
			positions.Add(new LinePosition(rotation,i));

			Vector3 position = new Vector3(0,0,i);
			lineRenderer.SetPosition(i, position);
		}

	}
	
	void Update () {

		
		
	}
}
