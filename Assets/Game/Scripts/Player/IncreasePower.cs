using UnityEngine;
using UnityEngine.UI;

public class IncreasePower : MonoBehaviour {

	RawImage image;
	Rect p;
	//arrow power
	public float power = 5;
	//Whether or not to update the graphic
	public bool update = false;
	//Whether the arrow can be shot. (if it has been enabled in the animation)
	public bool canShoot = false;
	//time it takes for graphic to disappear after released
	const float timeBetween = 1f;
	float updateTime;

	// Use this for initialization
	void Start () {
		image = GetComponent<RawImage>();
		p = image.uvRect;
		Reset();
	}
	
	// Update is called once per frame
	void Update () {
		//342 ticks to get to full power
		if (update && Time.timeScale != 0)
		{
			if (p.x > 0f)
			{
				p.x -= 0.5f * Time.deltaTime;
				power += 60 * Time.deltaTime;
			}
			else
			{
				if (power >= 65)
				{
					power = 65;
				}
				p.x = 0;
			}
		}
		else if (updateTime <= Time.time || Time.timeScale == 0)
		{
			image.enabled = false;
		}
		image.uvRect = p;
	}

	public void Reset()
	{
		update = false;
		Hide();
	}

	public void Begin()
	{
		update = true;
		canShoot = true;
	}

	public void Show()
	{
		canShoot = false;
		p.x = 0.5f;
		power = 5;
		image.enabled = true;
		updateTime = Time.time + timeBetween;
	}

	void Hide()
	{
		updateTime = Time.time + timeBetween;
	}
}
