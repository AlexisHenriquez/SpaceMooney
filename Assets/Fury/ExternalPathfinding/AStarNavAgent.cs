using UnityEngine;
using System;
using System.Collections.Generic;

public class AStarNavAgent : MonoBehaviour, Fury.INavMeshAgent
{
	public const Int32 UnitMask = 1 << 8;

	private static Pathfinding.NNConstraint NodeConstraint;

	static AStarNavAgent()
	{
		NodeConstraint = new Pathfinding.NNConstraint();
		NodeConstraint.walkable = true;
	}

	public float acceleration { get; set; }

	public float angularSpeed { get; set; }

	public bool isDestroyed { get { return this == null; } }

	public Vector3 nextPosition { get; set; }

	public Quaternion nextRotation { get; set; }

	public float radius { get; set; }

	public float speed { get; set; }

	public bool updatePosition { get; set; }

	public bool updateRotation { get; set; }

	public Vector3 velocity { get; private set; }

	public Vector3 destination { get; private set; }

	private Seeker Seeker;

	private List<AStarNavAgent> NearbyAgents;

	private Single TimerAgentSearch = 0.25f,  TimerPathRefresh = 0.25f;

	private Vector3 DeltaMove, DeltaAvoid;

	public Single Tolerance;

	private List<Vector3> Waypoints;
	private Int32 WaypointIndex;
	private Fury.Behaviors.Unit Unit;

	private void Awake()
	{
		Unit = GetComponent<Fury.Behaviors.Unit>();
		Waypoints = new List<Vector3>();
		NearbyAgents = new List<AStarNavAgent>();
		Seeker = gameObject.AddComponent<Seeker>();
		var funnel = gameObject.AddComponent<FunnelModifier>();
		// var smoother = gameObject.AddComponent<SimpleSmoothModifier>();
		// smoother.uniformLength = false;
		// smoother.subdivisions = 3;
	}

	private void Start()
	{
		nextPosition = transform.position;
		nextRotation = transform.rotation;
		destination = transform.position;
	}

	private void Update()
	{
		ScanAgents();

		RefreshPath();

		var old = transform.position;

		if (updatePosition) transform.position = nextPosition;
		else nextPosition = transform.position;

		if (updateRotation) transform.rotation = nextRotation;
		else nextRotation = transform.rotation;

		velocity = (transform.position - old) / Time.deltaTime;

		DeltaAvoid = GetPushFromAgents(NearbyAgents);
		DeltaMove = Vector3.zero;

		var distanceToDestination = Vector3.Distance(transform.position, destination);
		if (distanceToDestination < radius * 5)
		{
			Tolerance = Mathf.Clamp(Tolerance + Time.deltaTime, 0, radius * 5);
		}

		if (distanceToDestination < Tolerance)
		{
			if (Unit != null && Unit.State == Fury.UnitStates.MovingToPosition)
				Unit.Stop();
		}

		// if (Path != null)
		{
			var moveVector = (GetNextWaypoint() - transform.position);
			// Debug.Log(moveVector);
			var moveVectorMag = moveVector.magnitude;

			if (moveVectorMag > 0)
			{
				var tol = (Unit != null && Unit.State != Fury.UnitStates.MovingToPosition) ? radius : Tolerance;

				// Calculate the distance to our true destination
				var adjDistanceToDestination = Mathf.Clamp(distanceToDestination - tol, 0, Single.MaxValue);
				var distanceCorrectionFactor = Mathf.Clamp01(adjDistanceToDestination / (radius * 2));

				DeltaMove = moveVector / moveVectorMag * distanceCorrectionFactor;
			}
		}

		Translate(DeltaMove, DeltaAvoid);
	}

	private void Translate(Vector3 deltaMove, Vector3 deltaAvoid)
	{
		var moveMag = deltaMove.magnitude;
		var avoidMag = deltaAvoid.magnitude;
		
		// Sum the components and get the magnitude
		var sumDelta = deltaAvoid + deltaMove;
		var magDelta = sumDelta.magnitude;

		if (magDelta > 0)
		{
			// Calculate the direction of the mvoement
			var dirDelta = sumDelta / magDelta;

			// Calculate the final position
			var spd = Mathf.Min(magDelta, speed * Time.deltaTime);
			var fPos = transform.position + dirDelta * spd;

			nextPosition = fPos;

			dirDelta.y = 0;
			if (spd > 0.01f)
				nextRotation = Quaternion.RotateTowards(transform.rotation,
					Quaternion.LookRotation(dirDelta, Vector3.up),
					angularSpeed * Time.deltaTime);
		}
	}

	private void RefreshPath()
	{
		TimerPathRefresh -= Time.deltaTime;
		if (TimerPathRefresh < 0)
		{
			Seeker.StartPath(transform.position, destination, OnPathFound);
			TimerPathRefresh = Single.MaxValue;
		}
	}

	private void ScanAgents()
	{
		TimerAgentSearch -= Time.deltaTime;
		var nearbyColliders = Physics.OverlapSphere(transform.position, radius * 10, UnitMask);
		NearbyAgents.Clear();
		foreach (var c in nearbyColliders)
			if (c != null && c != collider)
			{
				var navAgent = c.GetComponent<AStarNavAgent>();
				if (navAgent != null)
					NearbyAgents.Add(navAgent);
			}
	}

	/// <summary>
	/// Get push component from the other agents.
	/// </summary>
	/// <param name="agents">A list of agents.</param>
	/// <returns>A vector facing away from agents that are nearby.</returns>
	private Vector3 GetPushFromAgents(List<AStarNavAgent> agents)
	{
		var push = Vector3.zero;
		var pos = transform.position;

		for (Int32 i = 0; i < agents.Count; i++)
		{
			var agent = agents[i];

			if (agent != null && agent != this)
			{
				var vecToAgent = agent.transform.position - pos;
				var dstToAgent = vecToAgent.magnitude;
				var dirToAgent = vecToAgent / dstToAgent;

				// Adjust the distance to the colliders by the radius of the agents
				dstToAgent = Mathf.Clamp(dstToAgent - radius - agent.radius, 0.01f, Single.MaxValue);
				var p = dirToAgent * Mathf.Lerp(1, 0, Mathf.Clamp01(dstToAgent / 0.25f));
				p.y = 0;
				push -= p;
			}
		}
		return push;
	}

	private void OnDrawGizmos()
	{
		var center = collider.bounds.center;
		Gizmos.color = Color.red;
		Gizmos.DrawLine(center, center + DeltaAvoid);
		Gizmos.color = Color.blue;
		Gizmos.DrawLine(center, center + DeltaMove);
		DrawCircle(center, Tolerance);
		Gizmos.DrawLine(center, destination);

			for (Int32 i = 0; i < Waypoints.Count - 1; i++)
			{
				Gizmos.DrawLine(Waypoints[i], Waypoints[i + 1]);
			}
	}

	public void Destroy()
	{
		GameObject.Destroy(this);
	}

	public void SetDestination(Vector3 pos) 
	{
		pos = AstarPath.active.GetNearest(pos, NodeConstraint);

		if ((pos - destination).sqrMagnitude > 0.1f)
		{
			Seeker.StartPath(transform.position, pos, OnPathFound);
			Tolerance = radius;
			destination = pos;
		}
	}

	public void Stop()
	{
		destination = transform.position;
	}

	private void OnPathFound(Pathfinding.Path p)
	{
		Waypoints.Clear();
		WaypointIndex = 0;
		for (Int32 i = 0; i < p.vectorPath.Length; i++)
			Waypoints.Add(p.vectorPath[i]);

		AstarPath.AddToPathPool(p);

		TimerPathRefresh = 1f;
	}

	private Vector3 GetNextWaypoint()
	{
		// if (Waypoints.Count > 0)
		{
			for (Int32 i = WaypointIndex; i < Waypoints.Count; i++)
			{
				var currentWaypoint = Waypoints[WaypointIndex];
				if (Vector3.Distance(currentWaypoint, transform.position) < radius)
					WaypointIndex = Math.Min(Waypoints.Count - 1, WaypointIndex + 1);
				else
					return currentWaypoint;
			}
		}

		return destination;
	}

	private static void DrawCircle(Vector3 pos, Single rad)
	{
		var numSteps = 32;

		for (Int32 i = 0; i < numSteps; i++)
		{
			var r0 = ((Mathf.PI * 2) / numSteps) * i;
			var r1 = ((Mathf.PI * 2) / numSteps) * (i + 1);

			var x0 = rad * Mathf.Cos(r0);
			var y0 = rad * Mathf.Sin(r0);

			var x1 = rad * Mathf.Cos(r1);
			var y1 = rad * Mathf.Sin(r1);

			Gizmos.DrawLine(new Vector3(x0, 0, y0) + pos, new Vector3(x1, 0, y1) + pos);
		}
	}
}