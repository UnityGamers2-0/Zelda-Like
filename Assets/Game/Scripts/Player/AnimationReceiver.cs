using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationReceiver : MonoBehaviour {

	public Player player;
	public Projectile arrow;
	public IncreasePower bowPower;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (!player.rClickHeld && bowPower.update)
		{
			bowPower.Reset();
		}
	}

	public void PreDrawback()
	{
		bowPower.Show();
	}

	public void Drawback()
	{
		bowPower.Begin();
	}

	public void ArrowShoot()
	{
		if (bowPower.canShoot)
		{
			player.archerShoot.Play();
			bowPower.Reset();
			Projectile newArrow = Instantiate(arrow);
			newArrow.power = bowPower.power + Random.Range(0, 10);
			//incase the animation hadn't enabled it yet
			newArrow.gameObject.SetActive(true);
		}
	}

	public void FootR()
	{
		player.FootStep();
	}
	public void FootL()
	{
		player.FootStep();
	}

	public void Hit()
	{
		if(player.Attack())
		{
			switch (Player.pClass)
			{
				case (Player.Class.Archer):
				case (Player.Class.Mage):
					player.hit.Play();
					break;
				case (Player.Class.Knight):
					player.knightSlash.Play();
					break;
			}
		}
		else
		{
			player.miss.Play();
		}
	}
}
