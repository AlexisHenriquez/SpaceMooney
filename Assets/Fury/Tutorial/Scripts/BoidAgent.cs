using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using Fury;

public class BoidAgent : MonoBehaviour, INavMeshAgent
{
	private static List<BoidAgent> OtherAgents = new List<BoidAgent>();

	public Vector3 velocity { get; set; }
	public Single radius { get { return Agent.radius; } set { Agent.radius = value; } }
	Single INavMeshAgent.acceleration { get { return Agent.acceleration; } set { Agent.acceleration = value; } }
	public Single angularSpeed { get; set; }
	public Single speed { get { return Agent.speed; } set { Agent.speed = value; } }
	public Vector3 nextPosition { get { return Agent.nextPosition; } set { Agent.nextPosition = value; } }
	Vector3 INavMeshAgent.destination { get { return Agent.destination; } }

	public Boolean updatePosition { get; set; }
	public Boolean updateRotation { get; set; }

	Boolean INavMeshAgent.isDestroyed { get { return this == null || Agent == null; } }

	private NavMeshAgent Agent;

	private Int32 RotDir;

	private void Awake()
	{
		Agent = gameObject.AddComponent<NavMeshAgent>();
		Agent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
		Agent.updatePosition = false;
		Agent.updateRotation = false;

		RotDir = 1; // UnityEngine.Random.Range(0, 2) == 0 ? -1 : +1;

		OtherAgents.Add(this);
	}

	private void Update()
	{
		var cPos = transform.position;
		var nPos = cPos;
		var dVel = nextPosition - cPos;

		if (dVel.magnitude > 0)
		{
			var its = 0;

			var temp = Vector3.Dot(velocity, Vector3.Cross(dVel, Vector3.up));
			if (temp > 0) RotDir = -1;
			else RotDir = +1;

			Search(cPos, ref dVel, ref its);
			if (its < 36)
			{
				nPos = cPos + dVel;
			}
		}

		if (updatePosition)
		{
			NavMeshHit hit;
			if (NavMesh.SamplePosition(nPos, out hit, radius, ~0))
				nPos = hit.position;

			transform.position = nPos;
		}

		velocity = (transform.position - cPos) / Time.deltaTime;

		if (updateRotation && velocity.magnitude > 0)
		{
			var angleToTarget = Vector3.Angle(transform.forward, velocity);
			var sgn = Math.Sign(Vector3.Dot(transform.right, velocity));
			angleToTarget = Mathf.Clamp(angleToTarget, 0, Math.Min(angularSpeed * Time.deltaTime, angleToTarget / 10));
			transform.Rotate(Vector3.up, angleToTarget * sgn);
		}
	}

	private void Search(Vector3 cPos, ref Vector3 dVel, ref Int32 i)
	{
		i++;

		var dPos = cPos + dVel;

		// Search for a collision-free velocity		
		var willCollide = false;
		// var willCollide =  Physics.SphereCast(new Ray(cPos, dVel), radius, dVel.magnitude);
		
		foreach (var oa in OtherAgents)
			if (oa != this && oa != null)
			{
				var d = Vector3.Distance(oa.transform.position, dPos);
				var sumR = (radius + oa.radius) * 0.9f;

				if (d < sumR && Vector3.Dot(dVel, oa.transform.position - cPos) > 0)
				{
					// Yes there will be
					willCollide = true;
					break;
				}
			}

		if (willCollide && i < 36)
		{
			dVel = Quaternion.AngleAxis(RotDir * 10f, Vector3.up) * dVel;
			Search(cPos, ref dVel, ref i);
		}
	}

	private void OnDestroy()
	{
		GameObject.Destroy(Agent);

		OtherAgents.Remove(this);
	}

	void INavMeshAgent.Stop()
	{
		Agent.Stop();
	}

	void INavMeshAgent.Destroy()
	{
		GameObject.Destroy(Agent);
		GameObject.Destroy(this);
	}

	void INavMeshAgent.SetDestination(Vector3 d)
	{
		Agent.SetDestination(d);
	}
}