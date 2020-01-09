﻿using System.Collections;
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
		if (!player.drawing && bowPower.update)
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
}
