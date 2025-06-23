using UnityEngine;

public class Weapon : MonoBehaviour
{
    //Firing mode, reload times, amount of bullets, everything about the weapon
    public WeaponDataSO WD;

    //Current shooting mode
    private int currentShootMode = 0;

    //Amounts of bullets left for shooting and reloading
    private int bulletsTotal = 0;
    private int bulletsMagazine = 0;

    private int burstAmount = 0;
    private float burstFirerate = 0;
    private float firerateTimer = 0;
    private float reloadTimer = 0;

    private void Start()
    {
        foreach (var st in WD.shootTypes)
        { st.onShoot += ShootBullet; }
    }
    private void Update()
    {
        HandleTimers();
        CheckToReload();
        CheckToShoot();

        if (WD.shootTypes.Length > 1)
        { SwitchShootMode(); }
        
    }

    private void HandleTimers()
    {
        if (reloadTimer > 0)
        {
            reloadTimer -= Time.deltaTime;
            if (reloadTimer <= 0)
            { Reload(); }
        }

        if (firerateTimer > 0)
        { firerateTimer -= Time.deltaTime; }
    }
    private void CheckToShoot()
    {
        if (reloadTimer > 0 || firerateTimer > 0 || burstFirerate > 0) return;
        
        bool inputShoot = false;
        if (WD.shootTypes[currentShootMode].automatic) { inputShoot = Input.GetKey(KeyCode.Mouse0); }
        else { inputShoot = Input.GetKeyDown(KeyCode.Mouse0); }

        if (WD.shootTypes[currentShootMode].shootMode == ShootMode.Burst)
        {
            if (burstAmount == 0 && inputShoot)
            { 
                int maxBurst = (WD.shootTypes[currentShootMode] as ShootDataBurstSO).burstNum;
                burstAmount = Mathf.Clamp(maxBurst, 0, bulletsMagazine);
            }

            if (burstAmount > 0)
            {
                ShootGun();
                burstAmount--;
                
                if (burstAmount == 0)
                { firerateTimer = WD.shootTypes[currentShootMode].firerate; }
                else
                { burstFirerate = (WD.shootTypes[currentShootMode] as ShootDataBurstSO).burstFirerate; }
            }
        }
        else
        {
            if (inputShoot && bulletsMagazine > 0)
            {
                ShootGun();
                firerateTimer = WD.shootTypes[currentShootMode].firerate;
            }
        }
    }
    private void ShootGun()
    {
        WD.shootTypes[currentShootMode].Shoot(transform.forward);
        bulletsMagazine--;
        //AudioManager.PlaySFX(WD.fireSFX);
    }
    private void ShootBullet(Vector3 trajectory)
    {
         bool struck = Physics.Raycast(transform.position, trajectory, out RaycastHit hit, 100);

        Vector3 bulletFinalPoint = Vector3.zero;
        if (struck) { bulletFinalPoint = hit.point; }
        else { bulletFinalPoint = transform.position + trajectory * 100; }

        Debug.DrawLine(transform.position, bulletFinalPoint, Color.red, 1);
    }
    private void CheckToReload()
    {
        if (reloadTimer > 0 || WD.bulletCapacity == bulletsMagazine)
            return;

        bool willingReload = Input.GetKeyDown(KeyCode.R);
        if (willingReload || bulletsMagazine == 0)
        {
            if (bulletsTotal > 0)
            {
                reloadTimer = WD.reloadTime;
                //Start reload animation TODO
            }
            else if (willingReload)
            {
                Debug.Log("Temporary log, out of ammo");
                //Add UI element or sound effect here TODO
            }
        }
    }
    private void Reload()
    {
        int bulletDifference = WD.bulletCapacity - bulletsMagazine;
        if (bulletDifference > bulletsTotal)
        {
            bulletsMagazine += bulletsTotal;
            bulletsTotal = 0;
        }
        else
        {
            bulletsMagazine = WD.bulletCapacity;
            bulletsTotal -= bulletDifference;
        }
    }
    private void SwitchShootMode()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            if (reloadTimer <= 0)
            {
                currentShootMode = (currentShootMode + 1) % WD.shootTypes.Length;
                //Change UI shoot Icon here TODO
            }
        }
    }
}
