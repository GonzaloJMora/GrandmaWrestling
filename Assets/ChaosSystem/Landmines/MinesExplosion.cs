using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinesExplosion : MonoBehaviour
{
    
    public float Force { set { force = value; } }
    public float Radius { set { radius = value; } }
    public float UpForce { set { upForce = value; } }

    [SerializeField] private float force = 10f;
    [SerializeField] private float radius = 10f;
    [SerializeField] private float upForce = 10f;
    private bool exploded = false;
    public int count = 0;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
        Gizmos.color = Color.white;
    }

    private void Update()
    {
        if(transform.position.y < -100f) { Explode(); }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Explode();
        }
    }

    public void Explode()
    {
        if(exploded)
        {
            return;
        }
        this.GetComponent<MeshRenderer>().enabled = false;
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();

            if (rb != null && hit.tag == "Player")
            {
                Vector3 pos = transform.position;
                pos.y = hit.transform.position.y;
                Controller c = hit.GetComponent<Controller>();
                if (c != null)
                {
                    StartCoroutine(c.PauseMovementForce(0.25f, count));
                }
                rb.AddExplosionForce(force, pos, radius, upForce, ForceMode.Impulse);
            }
        }

        Collider[] col = GetComponents<Collider>();
        foreach (Collider c in col)
        {
            c.enabled = false;
        }
        exploded = true;
    }

    IEnumerator TempPausePlayerMovement(Controller c)
    {
        float copy = c.movementForce;
        c.movementForce = 0f;
        yield return new WaitForSeconds(0.25f);
        c.movementForce = copy;
    }

}
