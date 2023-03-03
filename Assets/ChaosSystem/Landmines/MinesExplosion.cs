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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
        Gizmos.color = Color.white;
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
                    StartCoroutine(TempPausePlayerMovement(c));
                }
                rb.AddExplosionForce(force, pos, radius, upForce, ForceMode.Impulse);
            }
        }

        Destroy(this);
    }

    IEnumerator TempPausePlayerMovement(Controller c)
    {
        float copy = c.movementForce;
        c.movementForce = 0f;
        yield return new WaitForSeconds(0.25f);
        c.movementForce = copy;
    }

}
