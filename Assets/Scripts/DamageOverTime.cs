using UnityEngine;
using utilities.Controllers;

public class DamageOverTime : MonoBehaviour
{
    [SerializeField] int damagePerTick = 1;
    [SerializeField] float tick = 4f;

    float timer = 0;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            if (timer <= 0)
            {
                if (other.TryGetComponent<HealthController>(out HealthController controller))
                {
                    controller.DoDamage(damagePerTick);
                }

                timer = tick;
            }

            timer -= Time.deltaTime;
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            timer = tick;
        }
    }
}
