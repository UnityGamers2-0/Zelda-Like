using UnityEngine;

public class Entity : MonoBehaviour
{
    public Camera cam;
    //Essentially the base health
    public float maxHealth;
    //current health
    public float health;
    //base damage
    public float baseDamage;
    //defense with armor
    public float defense;
    //attack with weapon
    public float damage;

    public void TakeDamage(Entity from)
    {
        if (this is Player)
        {
            Player p = (Player)this;
            if (p.rClickHeld && Player.pClass == Player.Class.Knight)
            {
                health -= (from.damage * (1 - (Mathf.Min(20, Mathf.Max(defense / 5, defense - (from.damage / 2)))))) * 0.8f;
                return;
            }
        }
        health -= from.damage * (1 - (Mathf.Min(20, Mathf.Max(defense/5, defense - (from.damage / 2)))));
        CheckDeath();
    }

    private void CheckDeath()
    {
        if (health <= 0)
        {
            enabled = false;
        }
    }

    public void Heal(float amnt)
    {
        health += amnt;
        if (health > maxHealth)
        {
            health = maxHealth;
        }
    }
	
    public void DealDamage(Entity to)
    {
        to.TakeDamage(this);
    }

    //Should be called whenever an item is equipped or unequipped
    public void UpdateVars(float updDefense, float updDamage)
    {
        defense = updDefense;
        damage = updDamage + baseDamage;
    }

    public void Attack(Transform target = null)
    {
        Entity e = caster(target);
        if (e)
        {
            DealDamage(e);
        }
    }

    public Entity caster(Transform target)
    {
        RaycastHit hit;
        Entity e = null;
        
        //if target is not defined (entity is player)
        if (!target)
        {
            Debug.DrawRay(cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2)).origin, cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2)).direction, Color.red);
            if (Physics.Raycast(cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2)), out hit, 5f))
            {
                if (hit.collider.tag == "Entity")
                {
                    e = hit.collider.gameObject.GetComponent<Entity>();
                }
            }
        }
        else
        {
            Debug.DrawRay(transform.position, (target.position - transform.position), Color.blue);
            if (Physics.Raycast(transform.position, (target.position - transform.position), out hit, 5f))
            {
                if (hit.collider.gameObject == target.gameObject)
                {
                    e = target.gameObject.GetComponent<Entity>();
                }
            }
        }

        return e;
    }
}
