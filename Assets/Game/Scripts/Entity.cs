using UnityEngine;

public class Entity : MonoBehaviour
{
    public Camera cam;
    //Speed (player only)
    public float agility;
    //Essentially the base health
    public float maxHealth;
    //current health
    public float health;
    //base damage (1 on player, on enemies, it should be manually set)
    public float baseAttack;
    //defense with armor
    public float defense;
    //attack with weapon
    public float attack;


    public void TakeDamage(float damage)
    {
        float lostHealth;
        lostHealth = damage * (1 - (Mathf.Min(20, Mathf.Max(defense / 5, defense - (damage / 2)))/25));
        if (this is Player)
        {
            Player p = (Player)this;
            if (p.rClickHeld && Player.pClass == Player.Class.Knight)
            {
                lostHealth *= 0.8f;
                p.knightBlock.Play();
            }
        }
        health -= lostHealth;
        
        CheckDeath();
    }
    public void TakeDamage(Entity from)
    {
        TakeDamage(from.attack);
    }

    private void CheckDeath()
    {
        if (health <= 0)
        {
            gameObject.SetActive(false);
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
        if (to != null)
        {
            to.TakeDamage(this);
        }
    }

    //Should be called whenever an item is equipped or unequipped
    public void UpdateVars(float updDefense, float updDamage, float updAgility)
    {
        defense = updDefense;
        attack = updDamage + baseAttack;
        agility = updAgility;
    }

    public bool Attack(Transform target = null)
    {
        Entity e = caster(target);
        if (e != null)
        {
            DealDamage(e);
            return true;
        }
        return false;
    }

    public Entity caster(Transform target)
    {
        RaycastHit hit;
        Entity e = null;
        
        //if target is not defined (entity is player)
        if (target == null)
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

    //Receivcer
    public void arrowHit(object[] parms)
    {
        float speed = (float)parms[0];
        float attack = ((Entity)parms[1]).attack;
        TakeDamage((speed / 50f) * attack / 2);
    }
}
