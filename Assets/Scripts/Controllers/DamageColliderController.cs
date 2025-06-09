using UnityEngine;

namespace utilities.Controllers
{
    public class DamageColliderController : MonoBehaviour
    {
        [SerializeField] protected int damage = 1;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (other.TryGetComponent<HealthController>(out HealthController controller))
                {
                    controller.DoDamage(damage);
                }
            }
        }
    }
}
