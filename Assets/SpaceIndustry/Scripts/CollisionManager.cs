using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;

public class CollisionManager : MonoBehaviour
{

	public AudioClip HitSound;

	public GameObject Explosion;

	public Buildable Buildable;

	public void OnCollisionEnter(Collision collision)
	{
		foreach (var item in collision.contacts)
		{
			if (item.otherCollider.tag == "DontDestroy" || item.thisCollider.tag == "DontDestroy")
			{
				return;
			}
		}
		ContactPoint contact = collision.contacts[0];

		if (collision.transform.tag != "DontDestroy")
		{
			Quaternion rotation = Quaternion.FromToRotation(Vector3.up, contact.normal);

			GameObject theExplosion = Instantiate(this.Explosion, contact.point, rotation) as GameObject;

			//audio.PlayOneShot(this.HitSound);

			var stats = GameObject.Find("PlayerStatsObject");

			var playerStats = stats.GetComponent<PlayerStats>();
			playerStats.removeGameObject(this.Buildable);

			StartCoroutine("Yield");

			Destroy(gameObject, 0.125F);

			Destroy(theExplosion, 1.5F);
		}
	}

	private IEnumerable Yield()
	{
		yield return null;
	}

}
