using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CarGeneral : MonoBehaviour
{

    public ManagerScript _manager;

    public bool AIPlayer = true;

    public float resetTime = 3.0F;
    public float AIresetTime = 1.2F;
    private float resetTimer = 0.0F;

    // state
    public float currentSpeed = 0f;
    public bool finished = false;
    public float finishTime = 0;
    public float lapStart = 0;
    public float bestLap = 999999;

    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public Transform aim;
    public float bulletSpeed = 150;
    public float bulletDelay = 0.2f;
    private float bulletNextTime = 0;
    public int maxHealth = 100;

    private float returnTimer = 0f;
    private int lap = 1;
    public int currentHealth = 100;
    private float curFine = 0;

    public void StartRace(Transform playerPosition)
    {
        bestLap = 999999;
        finished = false;
        finishTime = 0;
        curFine = 0;
        ResetCar(playerPosition);
        currentHealth = maxHealth;
        gameObject.GetComponent<UnityStandardAssets.Utility.WaypointProgressTracker>().Reset();
        if (AIPlayer)
        {
            gameObject.GetComponent<
                UnityStandardAssets.Vehicles.Car.CarAIControl>().PrepareRace();
        }
    }

    public void Update()
    {
        currentSpeed = Mathf.Round(GetComponent<Rigidbody>().velocity.magnitude * 3.6f);
        if (_manager.racing && !finished)
        {
            if (!AIPlayer)
            {
                if (Input.GetKeyDown(KeyCode.R))
                {
                    FlipCar();
                }
                if (Input.GetButton("Fire1") && Time.time >= bulletNextTime)
                {
                    Fire();
                    bulletNextTime = Time.time + bulletDelay;
                }
            }
            Check_If_Car_Is_Flipped();
            if (AIPlayer) Sensors();
        } else if (!AIPlayer && finished && currentSpeed < 2)
        {
            _manager.racing = false;
            _manager.TotalTime = finishTime+curFine;
            _manager.ShowResults();
        }
    }

    void Check_If_Car_Is_Flipped()
    {
        if (transform.localEulerAngles.z % 360 > 80 && transform.localEulerAngles.z % 360 < 280)
            resetTimer += Time.deltaTime;
        else
            resetTimer = 0;

        if (resetTimer > resetTime)
            FlipCar();
    }

    public void FlipCar()
    {
        Vector3 target = transform.gameObject.GetComponent<UnityStandardAssets.Utility.WaypointProgressTracker>().GetPosition();
        Vector3 lookTarget = transform.gameObject.GetComponent<UnityStandardAssets.Utility.WaypointProgressTracker>().target.transform.position;

        transform.position = target + Vector3.up * 0.5f;
        transform.LookAt(lookTarget);
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        resetTimer = 0;
    }

    void Fire()
    {
        // Create the Bullet from the Bullet Prefab
        var bullet = (GameObject)Instantiate(
            bulletPrefab,
            bulletSpawn.position,
            bulletSpawn.rotation);

        // Add velocity to the bullet
        bullet.GetComponent<Rigidbody>().velocity = /*gameObject.GetComponent<Rigidbody>().velocity +*/ 
            bullet.transform.forward * bulletSpeed;

        // Destroy the bullet after 2 seconds
        Destroy(bullet, 2.0f);
    }

    public void Fire(Vector3 corrector)
    {
        // Create the Bullet from the Bullet Prefab
        var bullet = (GameObject)Instantiate(
            bulletPrefab,
            bulletSpawn.position,
            bulletSpawn.rotation);

        // Add velocity to the bullet
        bullet.GetComponent<Rigidbody>().velocity = /*gameObject.GetComponent<Rigidbody>().velocity +*/
            corrector * bulletSpeed;

        // Destroy the bullet after 2 seconds
        Destroy(bullet, 2.0f);
    }

    public void ResetCar(Transform playerPosition)
    {
        transform.position = playerPosition.position;
        transform.rotation = playerPosition.rotation;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        resetTimer = 0;
        lap = 1;
        finished = false;
    }

    public void Sensors()
    {
        if (_manager.racing && !finished)
        {
            if (currentSpeed <= 1f)
                returnTimer += Time.deltaTime;
            else
                returnTimer = 0;
            
            if (returnTimer > AIresetTime)
                FlipCar();
        }
    }

    public void AssignTime()
    {
        float curTime = Time.time - _manager.BeginOfTime;
        float curDist = transform.gameObject.GetComponent<UnityStandardAssets.Utility.WaypointProgressTracker>().ProgressDistance;
        float lapDist = _manager.maxDist / _manager.laps;
        float lapTime = _manager.iconicTime;
        finishTime = curTime + lapTime * (_manager.maxDist - curDist) / lapDist;
    }

    public int Lap
    {
        get { return lap; }
        set { lap = value; }
    }

    public int CurrentHealth
    {
        get { return currentHealth; }
        set { currentHealth = value; }
    }

    public float BulletNextTime
    {
        get { return bulletNextTime; }
        set { bulletNextTime = value; }
    }

    public float CurFine
    {
        get { return curFine; }
        set { curFine = value; }
    }
}
