using UnityEngine;
using utilities.Controllers;

public class Projectile : MonoBehaviour
{
    [SerializeField] LayerMask terrain;
    [SerializeField] GameObject AoePrefab;
    [SerializeField] float AoeDuration;
    [SerializeField] int damage;
    [SerializeField] float projectileDuration = 10f;
    [SerializeField] float terrainOffset = 0.2f;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.TryGetComponent<HealthController>(out HealthController controller))
            {
                controller.DoDamage(damage);
                SpawnAoE();
            }
        }
        else
        {
            SpawnAoE();
        }
    }

    private void SpawnAoE() 
    {
        if (AoePrefab == null) return;

        Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 100f, terrain);

        GameObject go = Instantiate(AoePrefab);

        go.transform.position = new Vector3(hit.point.x, hit.point.y + terrainOffset, hit.point.z);

        Destroy(go, AoeDuration);
        Destroy(gameObject);
    }
}
