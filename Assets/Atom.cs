using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Atom;
using Random = UnityEngine.Random;

public class Atom : MonoBehaviour
{
    public enum AtomType { proton, D, He3, He4, Be7, Be8, Li7, B8, C12, N13, C13, N14, O15, N15, O16, Ne20, Mg24, Si28, S32, Ni56 }
    float atomMass;
    float radius;
    public AtomType type;
    public bool canSpawn = true;
    float lifeTime = 0;
    public Rigidbody2D rb2D;
    GameObject spawnedAtom;

    [SerializeField] float vibrationForce;
    [SerializeField] float vibrationCooldownTimer;
    float vibrationTimer;

    bool hasLiveSpan = false;
    float livespanTimer = 0;
    // Start is called before the first frame update
    void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        atomMass = getMass(type);
        radius = getRadius(type);
        rb2D.mass = atomMass;

        if (type == AtomType.Be7)
            hasLiveSpan = true;
        if (type == AtomType.Be8)
            hasLiveSpan = true;
        if (type == AtomType.B8)
            hasLiveSpan = true;
        if (type == AtomType.N13)
            hasLiveSpan = true;
        if (type == AtomType.O15)
            hasLiveSpan = true;
        if (type == AtomType.Mg24)
            hasLiveSpan = true;
        if (type == AtomType.S32)
            hasLiveSpan = true;
        canSpawn = true;
        init_Radius();
    }

    // Update is called once per frame
    void Update()
    {

        if (vibrationTimer >= vibrationCooldownTimer)
        {
            rb2D.AddForce(new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * vibrationForce * (Mathf.Pow(GameManager.Instance.sunTemp, 1 / 3.714011909f) - 1) * atomMass);
            vibrationTimer = 0;
        }
        else
            vibrationTimer += Time.deltaTime;

        if (hasLiveSpan)
        {
            switch (type)
            {
                case AtomType.Be7:
                    if (livespanTimer >= GameManager.Instance.Be7LiveSpan)
                    {
                        Debug.Log("Be7 decay ", this.gameObject);
                        spawnAtom(this, AtomType.Li7);
                        GameManager.Instance.raiseSunTemperature(0.86f);
                        Destroy(this.gameObject);
                    }
                    break;
                case AtomType.Be8:
                    if (livespanTimer >= GameManager.Instance.Be8LiveSpan)
                    {
                        Debug.Log("Be8 decay ", this.gameObject);
                        spawn2Atoms(this, AtomType.He4, AtomType.He4);
                        GameManager.Instance.raiseSunTemperature(18.07f);
                        Destroy(this.gameObject);
                    }
                    break;
                case AtomType.B8:
                    if (livespanTimer >= GameManager.Instance.B8LiveSpan)
                    {
                        Debug.Log("B8 decay ", this.gameObject);
                        spawnAtom(this, AtomType.Be8);
                        spawnElectron();
                        GameManager.Instance.raiseSunTemperature(14.06f);
                        Destroy(this.gameObject);
                    }
                    break;
                case AtomType.N13:
                    if (livespanTimer >= GameManager.Instance.N13LiveSpan)
                    {
                        Debug.Log("N13 decay ", this.gameObject);
                        spawnAtom(this, AtomType.C13);
                        spawnElectron();
                        GameManager.Instance.raiseSunTemperature(1.45f);
                        Destroy(this.gameObject);
                    }
                    break;
                case AtomType.O15:
                    if (livespanTimer >= GameManager.Instance.O15LiveSpan)
                    {
                        Debug.Log("O15 decay ", this.gameObject);
                        spawnAtom(this, AtomType.N15);
                        spawnElectron();
                        GameManager.Instance.raiseSunTemperature(2.75f);
                        Destroy(this.gameObject);
                    }
                    break;
                case AtomType.Mg24:
                    if (livespanTimer >= GameManager.Instance.Mg24LiveSpan)
                    {
                        Debug.Log("Mg24 decay ", this.gameObject);
                        spawn2Atoms(this, AtomType.Ne20, AtomType.He4);
                        GameManager.Instance.raiseSunTemperature(4.62f);
                        Destroy(this.gameObject);
                    }
                    break;
                case AtomType.S32:
                    if (livespanTimer >= GameManager.Instance.S32LiveSpan)
                    {
                        Debug.Log("S32 decay ", this.gameObject);
                        spawn2Atoms(this, AtomType.Si28, AtomType.He4);
                        GameManager.Instance.raiseSunTemperature(9.59f);
                        Destroy(this.gameObject);
                    }
                    break;
            }
            livespanTimer += Time.deltaTime;
        }
        lifeTime += Time.deltaTime;
    }
    Vector2 cacluateSpeed(Vector2 v1, Vector2 v2, float m1, float m2, float m3)
    {
        return (v1 * m1 + v2 * m2) / m3;
    }

    float getMass(AtomType type)
    {
        switch (type)
        {
            case AtomType.proton:
                return 1;
            case AtomType.D:
                return 2;
            case AtomType.He3:
                return 3;
            case AtomType.He4:
                return 4;
            case AtomType.Li7:
                return 7;
            case AtomType.C12:
                return 12;
            case AtomType.Be7:
                return 7;
            case AtomType.Be8:
                return 8;
            case AtomType.B8:
                return 8;
            case AtomType.N13:
                return 13;
            case AtomType.C13:
                return 13;
            case AtomType.N14:
                return 14;
            case AtomType.O15:
                return 15;
            case AtomType.N15:
                return 15;
            case AtomType.O16:
                return 16;
            case AtomType.Ne20:
                return 20;
            case AtomType.Mg24:
                return 24;
            case AtomType.Si28:
                return 28;
            case AtomType.S32:
                return 32;
            case AtomType.Ni56:
                return 56;
            default:
                return -1;
        }
    }
    float getRadius(AtomType type)
    {
        return Mathf.Pow(getMass(type) / 200f, 1f / 3f);
    }
    void init_Radius()
    {
        GetComponent<CircleCollider2D>().radius = radius;
        GetComponentsInChildren<Transform>()[1].localScale = Vector3.one * radius * 2 ;
    }
    void spawnElectron()
    {
        Vector2 dir = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        spawnedAtom = Instantiate(GameManager.Instance.electronPrefab, transform.position, Quaternion.identity);
        spawnedAtom.GetComponent<Rigidbody2D>().velocity = dir.normalized * GameManager.Instance.electronSpeed;
    }
    void spawnAtom(Atom collideAtom, AtomType spawnType)
    {
        spawnedAtom = spawnAtom(collideAtom, spawnType, Vector2.zero, Vector2.zero);
        spawnedAtom.GetComponent<Rigidbody2D>().velocity = cacluateSpeed(rb2D.velocity, collideAtom.rb2D.velocity, atomMass, collideAtom.atomMass, spawnedAtom.GetComponent<Atom>().atomMass);
    }

    GameObject spawnAtom(Atom collideAtom, AtomType spawnType, Vector2 OffsetDir, Vector2 velocity)
    {
        Vector3 spawnPos = (transform.position * atomMass + collideAtom.transform.position * collideAtom.atomMass) / (atomMass + collideAtom.atomMass);
        OffsetDir *= 1.5f;
        switch (spawnType)
        {
            case AtomType.proton:
                spawnedAtom = Instantiate(GameManager.Instance.protonPrefab, spawnPos + new Vector3(OffsetDir.x, OffsetDir.y, 0), Quaternion.identity);
                break;
            case AtomType.D:
                spawnedAtom = Instantiate(GameManager.Instance.DeuterPrefab, spawnPos + new Vector3(OffsetDir.x, OffsetDir.y, 0), Quaternion.identity);
                break;
            case AtomType.He3:
                spawnedAtom = Instantiate(GameManager.Instance.He_3Prefab, spawnPos + new Vector3(OffsetDir.x, OffsetDir.y, 0), Quaternion.identity);
                break;
            case AtomType.He4:
                spawnedAtom = Instantiate(GameManager.Instance.He_4Prefab, spawnPos + new Vector3(OffsetDir.x, OffsetDir.y, 0), Quaternion.identity);
                break;
            case AtomType.Be7:
                spawnedAtom = Instantiate(GameManager.Instance.Be_7Prefab, spawnPos + new Vector3(OffsetDir.x, OffsetDir.y, 0), Quaternion.identity);
                break;
            case AtomType.Be8:
                spawnedAtom = Instantiate(GameManager.Instance.Be_8Prefab, spawnPos + new Vector3(OffsetDir.x, OffsetDir.y, 0), Quaternion.identity);
                break;
            case AtomType.Li7:
                spawnedAtom = Instantiate(GameManager.Instance.Li_7Prefab, spawnPos + new Vector3(OffsetDir.x, OffsetDir.y, 0), Quaternion.identity);
                break;
            case AtomType.B8:
                spawnedAtom = Instantiate(GameManager.Instance.B_8Prefab, spawnPos + new Vector3(OffsetDir.x, OffsetDir.y, 0), Quaternion.identity);
                break;
            case AtomType.C12:
                spawnedAtom = Instantiate(GameManager.Instance.C_12Prefab, spawnPos + new Vector3(OffsetDir.x, OffsetDir.y, 0), Quaternion.identity);
                break;
            case AtomType.N13:
                spawnedAtom = Instantiate(GameManager.Instance.N_13Prefab, spawnPos + new Vector3(OffsetDir.x, OffsetDir.y, 0), Quaternion.identity);
                break;
            case AtomType.C13:
                spawnedAtom = Instantiate(GameManager.Instance.C_13Prefab, spawnPos + new Vector3(OffsetDir.x, OffsetDir.y, 0), Quaternion.identity);
                break;
            case AtomType.N14:
                spawnedAtom = Instantiate(GameManager.Instance.N_14Prefab, spawnPos + new Vector3(OffsetDir.x, OffsetDir.y, 0), Quaternion.identity);
                break;
            case AtomType.O15:
                spawnedAtom = Instantiate(GameManager.Instance.O_15Prefab, spawnPos + new Vector3(OffsetDir.x, OffsetDir.y, 0), Quaternion.identity);
                break;
            case AtomType.N15:
                spawnedAtom = Instantiate(GameManager.Instance.N_15Prefab, spawnPos + new Vector3(OffsetDir.x, OffsetDir.y, 0), Quaternion.identity);
                break;
            case AtomType.O16:
                spawnedAtom = Instantiate(GameManager.Instance.O_16Prefab, spawnPos + new Vector3(OffsetDir.x, OffsetDir.y, 0), Quaternion.identity);
                break;
            case AtomType.Ne20:
                spawnedAtom = Instantiate(GameManager.Instance.Ne_20Prefab, spawnPos + new Vector3(OffsetDir.x, OffsetDir.y, 0), Quaternion.identity);
                break;
            case AtomType.Mg24:
                spawnedAtom = Instantiate(GameManager.Instance.Mg_24Prefab, spawnPos + new Vector3(OffsetDir.x, OffsetDir.y, 0), Quaternion.identity);
                break;
            case AtomType.Si28:
                spawnedAtom = Instantiate(GameManager.Instance.Si_28Prefab, spawnPos + new Vector3(OffsetDir.x, OffsetDir.y, 0), Quaternion.identity);
                break;
            case AtomType.S32:
                spawnedAtom = Instantiate(GameManager.Instance.S_32Prefab, spawnPos + new Vector3(OffsetDir.x, OffsetDir.y, 0), Quaternion.identity);
                break;
            case AtomType.Ni56:
                spawnedAtom = Instantiate(GameManager.Instance.Ni_56Prefab, spawnPos + new Vector3(OffsetDir.x, OffsetDir.y, 0), Quaternion.identity);
                break;
            default:
                Debug.Log("Spawn not suported", this.gameObject);
                break;
        }
        spawnedAtom.GetComponent<Rigidbody2D>().velocity = velocity;
        return spawnedAtom;
    }

    Tuple<float, float> calculate2AtomsVelocityValue(Vector2 p0, float m1, float m2)
    {
        return Tuple.Create(p0.magnitude / 2f / m1, p0.magnitude / 2f / m2);
    }
    void spawn2Atoms(Atom collideAtom, AtomType spawnType1, AtomType spawnType2)
    {
        Vector2 p0 = rb2D.velocity * atomMass + collideAtom.GetComponent<Rigidbody2D>().velocity * collideAtom.atomMass;
        if (p0 == Vector2.zero)
        {
            p0 = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        }
        Vector2 spawn1OffsetDir = (Quaternion.Euler(0f, 0f, 45f) * p0).normalized;
        Vector2 spawn2OffsetDir = (Quaternion.Euler(0f, 0f, -45f) * p0).normalized;
        Tuple<float, float> velocityValue = calculate2AtomsVelocityValue(p0, getMass(spawnType1), getMass(spawnType2));

        Debug.Log(spawnType1);
        Debug.Log(spawn1OffsetDir * getRadius(spawnType1));
        Debug.Log(getRadius(spawnType1));
        Debug.Log(getMass(AtomType.C12));

        Debug.Log(spawnType2);
        Debug.Log(spawn2OffsetDir * getRadius(spawnType2));
        Debug.Log(getRadius(spawnType2));

        spawnAtom(collideAtom, spawnType1, spawn1OffsetDir * getRadius(spawnType1), spawn1OffsetDir * velocityValue.Item1);
        spawnAtom(collideAtom, spawnType2, spawn2OffsetDir * getRadius(spawnType2), spawn2OffsetDir * velocityValue.Item2);
    }

    void spawn3Atoms(Atom collideAtom, AtomType spawnType1, AtomType spawnType2, AtomType spawnType3)
    {
        Vector2 p0 = rb2D.velocity * atomMass + collideAtom.GetComponent<Rigidbody2D>().velocity * atomMass;
        if (p0 == Vector2.zero)
        {
            p0 = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        }
        spawnAtom(collideAtom, spawnType1, Vector2.zero, p0 / 3 / getMass(spawnType1));

        Vector2 spawn2OffsetDir = (Quaternion.Euler(0f, 0f, 60f) * p0).normalized;
        Vector2 spawn3OffsetDir = (Quaternion.Euler(0f, 0f, -60f) * p0).normalized;

        Tuple<float, float> velocityValue = calculate2AtomsVelocityValue(p0, getMass(spawnType1), getMass(spawnType2));

        spawnAtom(collideAtom, spawnType2, spawn2OffsetDir * (getRadius(spawnType2) + getRadius(spawnType1)), spawn2OffsetDir * p0.magnitude / 3 / getMass(spawnType2));
        spawnAtom(collideAtom, spawnType3, spawn3OffsetDir * (getRadius(spawnType3) + getRadius(spawnType1)), spawn3OffsetDir * p0.magnitude / 3 / getMass(spawnType3));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("collision Enter", this.gameObject);
        Atom collideAtom = collision.collider.GetComponent<Atom>();

        //pp1 works all the time
        pp1Collisions(collideAtom);

        if (GameManager.Instance.sunTemp > GameManager.Instance.pp2Temp)
        {
            pp2Collisions(collideAtom);
        }
        if (GameManager.Instance.sunTemp > GameManager.Instance.pp3Temp)
        {
            pp3Collisions(collideAtom);
        }
        if (GameManager.Instance.sunTemp > GameManager.Instance.CNOTemp)
        {
            CNO_Collisons(collideAtom);
        }
        if (GameManager.Instance.sunTemp > GameManager.Instance.threeAlpha)
        {
            threeAlphaCollisions(collideAtom);
        }
        if (GameManager.Instance.sunTemp > GameManager.Instance.threeAlpha2)
        {
            //threeAlpha2Collisions(collideAtom);
        }
        if (GameManager.Instance.sunTemp > GameManager.Instance.carbonBurn)
        {
            carbonBurnCollisions(collideAtom);
        }
        if (GameManager.Instance.sunTemp > GameManager.Instance.neonBurn)
        {
            neonBurnCollisions(collideAtom);
        }
        if (GameManager.Instance.sunTemp > GameManager.Instance.oxygenBurn)
        {
            oxygenBurnCollisions(collideAtom);
        }
        if (GameManager.Instance.sunTemp > GameManager.Instance.siliconBurn)
        {
            siliconBurnCollisions(collideAtom);
        }
    }

    private void pp1Collisions(Atom collideAtom)
    {
        if (type == AtomType.proton)
        {
            if (collideAtom.type == AtomType.proton)
            {
                if (canSpawn)
                {
                    collideAtom.canSpawn = false;
                    Debug.Log("collision p - p", this.gameObject);
                    canSpawn = false;

                    spawnAtom(collideAtom, AtomType.D);
                    spawnElectron();
                    GameManager.Instance.raiseSunTemperature(1.44f);
                    Destroy(collideAtom.gameObject);
                    Destroy(this.gameObject);
                }
            }
            if (collideAtom.type == AtomType.D)
            {
                //PPI
                if (canSpawn)
                {
                    Debug.Log("collision p - D", this.gameObject);
                    collideAtom.canSpawn = false;
                    canSpawn = false;

                    spawnAtom(collideAtom, AtomType.He3);
                    GameManager.Instance.raiseSunTemperature(5.49f);
                    Destroy(collideAtom.gameObject);
                    Destroy(this.gameObject);
                }
            }
        }
        else if (type == AtomType.He3)
        {
            if (collideAtom.type == AtomType.He3)
            {
                if (canSpawn)
                {
                    Debug.Log("collision He_3 - He_3", this.gameObject);
                    collideAtom.canSpawn = false;
                    canSpawn = false;

                    spawn3Atoms(collideAtom, AtomType.He4, AtomType.proton, AtomType.proton);
                    GameManager.Instance.raiseSunTemperature(12.85f);
                    Destroy(collideAtom.gameObject);
                    Destroy(this.gameObject);
                }
            }
        }
    }
    private void pp2Collisions(Atom collideAtom)
    {
        if (type == AtomType.He3)
            if (collideAtom.type == AtomType.He4)
            {
                if (canSpawn)
                {
                    Debug.Log("collision He_3 - He_4", this.gameObject);
                    collideAtom.canSpawn = false;
                    canSpawn = false;

                    spawnAtom(collideAtom, AtomType.Be7);
                    GameManager.Instance.raiseSunTemperature(1.59f);
                    Destroy(collideAtom.gameObject);
                    Destroy(this.gameObject);
                }
            }
        if (type == AtomType.proton)
        {
            if (collideAtom.type == AtomType.Li7)
            {
                if (canSpawn)
                {
                    Debug.Log("collision p - Li7", this.gameObject);
                    collideAtom.canSpawn = false;
                    canSpawn = false;

                    spawn2Atoms(collideAtom, AtomType.He4, AtomType.He4);
                    GameManager.Instance.raiseSunTemperature(17.35f);
                    Destroy(collideAtom.gameObject);
                    Destroy(this.gameObject);
                }
            }
        }
    }
    private void pp3Collisions(Atom collideAtom)
    {
        if (type == AtomType.proton)
        {
            if (collideAtom.type == AtomType.Be7)
            {
                if (canSpawn)
                {
                    Debug.Log("collision p - Be7", this.gameObject);
                    collideAtom.canSpawn = false;
                    canSpawn = false;

                    spawnAtom(collideAtom, AtomType.B8);
                    GameManager.Instance.raiseSunTemperature(0.14f);
                    Destroy(collideAtom.gameObject);
                    Destroy(this.gameObject);
                }
            }
        }
    }
    private void CNO_Collisons(Atom collideAtom)
    {
        if (type == AtomType.proton)
        {
            if (collideAtom.type == AtomType.C12)
            {
                if (canSpawn)
                {
                    Debug.Log("collision p - C12", this.gameObject);
                    collideAtom.canSpawn = false;
                    canSpawn = false;

                    spawnAtom(collideAtom, AtomType.N13);
                    GameManager.Instance.raiseSunTemperature(1.94f);
                    Destroy(collideAtom.gameObject);
                    Destroy(this.gameObject);
                }
            }

            if (collideAtom.type == AtomType.C13)
            {
                if (canSpawn)
                {
                    Debug.Log("collision p - C13", this.gameObject);
                    collideAtom.canSpawn = false;
                    canSpawn = false;

                    spawnAtom(collideAtom, AtomType.N14);
                    GameManager.Instance.raiseSunTemperature(7.55f);
                    Destroy(collideAtom.gameObject);
                    Destroy(this.gameObject);
                }
            }

            if (collideAtom.type == AtomType.N14)
            {
                if (canSpawn)
                {
                    Debug.Log("collision p - N14", this.gameObject);
                    collideAtom.canSpawn = false;
                    canSpawn = false;

                    spawnAtom(collideAtom, AtomType.O15);
                    GameManager.Instance.raiseSunTemperature(1.94f);
                    Destroy(collideAtom.gameObject);
                    Destroy(this.gameObject);
                }
            }

            if (collideAtom.type == AtomType.N15)
            {
                if (canSpawn)
                {
                    Debug.Log("collision p - N15", this.gameObject);
                    collideAtom.canSpawn = false;
                    canSpawn = false;

                    spawn2Atoms(collideAtom, AtomType.C12, AtomType.He4);
                    GameManager.Instance.raiseSunTemperature(4.97f);
                    Destroy(collideAtom.gameObject);
                    Destroy(this.gameObject);
                }
            }
        }
    }
    private void threeAlphaCollisions(Atom collideAtom)
    {
        if (type == AtomType.He4)
        {
            if (collideAtom.type == AtomType.He4)
            {
                if (canSpawn && collideAtom.canSpawn)
                {
                    Debug.Log("collision He_4 - He_4", this.gameObject);
                    collideAtom.canSpawn = false;
                    canSpawn = false;

                    spawnAtom(collideAtom, AtomType.Be8);
                    GameManager.Instance.raiseSunTemperature(-0.09f);
                    Destroy(collideAtom.gameObject);
                    Destroy(this.gameObject);
                }
            }
            if (collideAtom.type == AtomType.Be8)
            {
                if (canSpawn)
                {
                    Debug.Log("collision He_4 - Be8", this.gameObject);
                    collideAtom.canSpawn = false;
                    canSpawn = false;

                    spawnAtom(collideAtom, AtomType.C12);
                    GameManager.Instance.raiseSunTemperature(7.65f);
                    Destroy(collideAtom.gameObject);
                    Destroy(this.gameObject);
                }
            }
        }
    }
    private void threeAlpha2Collisions(Atom collideAtom)
    {
        if (type == AtomType.He4)
        {
            if (collideAtom.type == AtomType.C12)
            {
                if (canSpawn && collideAtom.canSpawn)
                {
                    Debug.Log("collision He_4 - C12", this.gameObject);
                    collideAtom.canSpawn = false;
                    canSpawn = false;

                    spawnAtom(collideAtom, AtomType.O16);
                    GameManager.Instance.raiseSunTemperature(7.16f);
                    Destroy(collideAtom.gameObject);
                    Destroy(this.gameObject);
                }
            }
        }
    }
    private void carbonBurnCollisions(Atom collideAtom)
    {
        if (type == AtomType.C12)
        {
            if (collideAtom.type == AtomType.C12)
            {
                if (canSpawn && collideAtom.canSpawn)
                {
                    Debug.Log("collision C12 - C12", this.gameObject);
                    collideAtom.canSpawn = false;
                    canSpawn = false;

                    spawnAtom(collideAtom, AtomType.Mg24);
                    GameManager.Instance.raiseSunTemperature(12.32f);
                    Destroy(collideAtom.gameObject);
                    Destroy(this.gameObject);
                }
            }
        }
    }
    private void neonBurnCollisions(Atom collideAtom)
    {
        if (type == AtomType.Ne20)
        {
            if (collideAtom.type == AtomType.Ne20)
            {
                if (canSpawn && collideAtom.canSpawn)
                {
                    Debug.Log("collision Ne20 - Ne20", this.gameObject);
                    collideAtom.canSpawn = false;
                    canSpawn = false;

                    spawn2Atoms(collideAtom, AtomType.O16, AtomType.Mg24);
                    GameManager.Instance.raiseSunTemperature(4.59f);
                    Destroy(collideAtom.gameObject);
                    Destroy(this.gameObject);
                }
            }
        }
        if (type == AtomType.He4)
        {
            if (collideAtom.type == AtomType.Mg24)
            {
                if (canSpawn && collideAtom.canSpawn)
                {
                    Debug.Log("collision He4 - Mg24", this.gameObject);
                    collideAtom.canSpawn = false;
                    canSpawn = false;

                    spawnAtom(collideAtom, AtomType.Si28);
                    GameManager.Instance.raiseSunTemperature(4.59f);
                    Destroy(collideAtom.gameObject);
                    Destroy(this.gameObject);
                }
            }
        }

    }    
    private void oxygenBurnCollisions(Atom collideAtom)
    {
        if (type == AtomType.O16)
        {
            if (collideAtom.type == AtomType.O16)
            {
                if (canSpawn && collideAtom.canSpawn)
                {
                    Debug.Log("collision O16 - O16", this.gameObject);
                    collideAtom.canSpawn = false;
                    canSpawn = false;

                    spawnAtom(collideAtom, AtomType.S32);
                    GameManager.Instance.raiseSunTemperature(13.14f);
                    Destroy(collideAtom.gameObject);
                    Destroy(this.gameObject);
                }
            }
        }
    } 
    private void siliconBurnCollisions(Atom collideAtom)
    {
        if (type == AtomType.Si28)
        {
            if (collideAtom.type == AtomType.Si28)
            {
                if (canSpawn && collideAtom.canSpawn)
                {
                    Debug.Log("collision Si28 - Si28", this.gameObject);
                    collideAtom.canSpawn = false;
                    canSpawn = false;

                    spawnAtom(collideAtom, AtomType.Ni56);
                    GameManager.Instance.raiseSunTemperature(10.93f);
                    Destroy(collideAtom.gameObject);
                    Destroy(this.gameObject);
                }
            }
        }
    }

}