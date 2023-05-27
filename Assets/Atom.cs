using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Atom : MonoBehaviour
{
    public enum AtomType { proton, D, He3, He4, Be7, Be8, Li7, B8, C12, N13, C13, N14, O15, N15 }
    float atomMass;
    float radius;
    public AtomType type;
    public bool canSpawn = true;
    public Rigidbody2D rb2D;
    GameObject spawnedAtom;


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
    }

    // Update is called once per frame
    void Update()
    {
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
            }
            livespanTimer += Time.deltaTime;
        }
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
                break;
            case AtomType.D:
                return 2;
                break;
            case AtomType.He3:
                return 3;
                break;
            case AtomType.He4:
                return 4;
                break;
            case AtomType.Li7:
                return 7;
                break;
            case AtomType.C12:
                return 12;
                break;
            case AtomType.Be7:
                return 7;
                break;
            case AtomType.Be8:
                return 8;
                break;
            case AtomType.B8:
                return 8;
                break;
            default:
                return -1;
        }
    }
    float getRadius(AtomType type)
    {
        return Mathf.Sqrt(0.25f / 12f * getMass(type));
    }
    void spawnElectron()
    {
        Vector2 dir = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        spawnedAtom = Instantiate(GameManager.Instance.electronPrefab, transform.position, Quaternion.identity);
        spawnedAtom.GetComponent<Rigidbody2D>().velocity = dir.normalized * GameManager.Instance.electronSpeed;
    }
    void spawnAtom(Atom collideAtom, AtomType spawnType)
    {
        switch (spawnType)
        {
            case AtomType.proton:
                spawnedAtom = Instantiate(GameManager.Instance.protonPrefab, (transform.position + collideAtom.transform.position) / 2, Quaternion.identity);
                break;
            case AtomType.D:
                spawnedAtom = Instantiate(GameManager.Instance.DeuterPrefab, (transform.position + collideAtom.transform.position) / 2, Quaternion.identity);
                break;
            case AtomType.He3:
                spawnedAtom = Instantiate(GameManager.Instance.He_3Prefab, (transform.position + collideAtom.transform.position) / 2, Quaternion.identity);
                break;
            case AtomType.He4:
                spawnedAtom = Instantiate(GameManager.Instance.He_4Prefab, (transform.position + collideAtom.transform.position) / 2, Quaternion.identity);
                break;
            case AtomType.Be7:
                spawnedAtom = Instantiate(GameManager.Instance.Be_7Prefab, (transform.position + collideAtom.transform.position) / 2, Quaternion.identity);
                break;
            case AtomType.Be8:
                spawnedAtom = Instantiate(GameManager.Instance.Be_8Prefab, (transform.position + collideAtom.transform.position) / 2, Quaternion.identity);
                break;
            case AtomType.Li7:
                spawnedAtom = Instantiate(GameManager.Instance.Li_7Prefab, (transform.position + collideAtom.transform.position) / 2, Quaternion.identity);
                break;
            case AtomType.B8:
                spawnedAtom = Instantiate(GameManager.Instance.B_8Prefab, (transform.position + collideAtom.transform.position) / 2, Quaternion.identity);
                break;
            case AtomType.C12:
                spawnedAtom = Instantiate(GameManager.Instance.C_12Prefab, (transform.position + collideAtom.transform.position) / 2, Quaternion.identity);
                break;
        }
        spawnedAtom.GetComponent<Rigidbody2D>().velocity = cacluateSpeed(rb2D.velocity, collideAtom.rb2D.velocity, atomMass, collideAtom.atomMass, spawnedAtom.GetComponent<Atom>().atomMass);
    }

    void spawnAtom(Atom collideAtom, AtomType spawnType, Vector2 OffsetDir, Vector2 velocity)
    {
        OffsetDir *= 1.5f;
        switch (spawnType)
        {
            case AtomType.proton:
                spawnedAtom = Instantiate(GameManager.Instance.protonPrefab, (transform.position + collideAtom.transform.position) / 2 + new Vector3(OffsetDir.x, OffsetDir.y, 0) * getRadius(spawnType), Quaternion.identity);
                break;
            case AtomType.D:
                spawnedAtom = Instantiate(GameManager.Instance.DeuterPrefab, (transform.position + collideAtom.transform.position) / 2 + new Vector3(OffsetDir.x, OffsetDir.y, 0) * getRadius(spawnType), Quaternion.identity);
                break;
            case AtomType.He3:
                spawnedAtom = Instantiate(GameManager.Instance.He_3Prefab, (transform.position + collideAtom.transform.position) / 2 + new Vector3(OffsetDir.x, OffsetDir.y, 0) * getRadius(spawnType), Quaternion.identity);
                break;
            case AtomType.He4:
                spawnedAtom = Instantiate(GameManager.Instance.He_4Prefab, (transform.position + collideAtom.transform.position) / 2 + new Vector3(OffsetDir.x, OffsetDir.y, 0) * getRadius(spawnType), Quaternion.identity);
                break;
            case AtomType.Be7:
                break;
            case AtomType.Li7:
                spawnedAtom = Instantiate(GameManager.Instance.Li_7Prefab, (transform.position + collideAtom.transform.position) / 2 + new Vector3(OffsetDir.x, OffsetDir.y, 0) * getRadius(spawnType), Quaternion.identity);
                break;
            case AtomType.C12:
                spawnedAtom = Instantiate(GameManager.Instance.C_12Prefab, (transform.position + collideAtom.transform.position) / 2 + new Vector3(OffsetDir.x, OffsetDir.y, 0) * getRadius(spawnType), Quaternion.identity);
                break;
        }
        spawnedAtom.GetComponent<Rigidbody2D>().velocity = velocity;
    }

    Tuple<float, float> calculate2AtomsVelocityValue(Vector2 p0, float m1, float m2)
    {
        return Tuple.Create(p0.magnitude / 2f / m1, p0.magnitude / 2f / m2);
    }
    void spawn2Atoms(Atom collideAtom, AtomType spawnType1, AtomType spawnType2)
    {
        Vector2 p0 = rb2D.velocity * atomMass + collideAtom.GetComponent<Rigidbody2D>().velocity * collideAtom.atomMass;

        Vector2 spawn1OffsetDir = Quaternion.Euler(0f, 0f, 45f) * p0.normalized;
        Vector2 spawn2OffsetDir = Quaternion.Euler(0f, 0f, -45f) * p0.normalized;
        Tuple<float, float> velocityValue = calculate2AtomsVelocityValue(p0, getMass(spawnType1), getMass(spawnType2));
        spawnAtom(collideAtom, spawnType1, spawn1OffsetDir, spawn1OffsetDir * velocityValue.Item1);

        spawnAtom(collideAtom, spawnType2, spawn2OffsetDir, spawn2OffsetDir * velocityValue.Item2);

    }

    void spawn3Atoms(Atom collideAtom, AtomType spawnType1, AtomType spawnType2, AtomType spawnType3)
    {
        Vector2 p0 = rb2D.velocity * atomMass + collideAtom.GetComponent<Rigidbody2D>().velocity * atomMass;
        spawnAtom(collideAtom, spawnType1, Vector2.zero, p0 / 3 / getMass(spawnType1));

        Vector2 spawn2OffsetDir = Quaternion.Euler(0f, 0f, 60f) * p0.normalized;
        Vector2 spawn3OffsetDir = Quaternion.Euler(0f, 0f, -60f) * p0.normalized;

        Tuple<float, float> velocityValue = calculate2AtomsVelocityValue(p0, getMass(spawnType1), getMass(spawnType2));
        spawnAtom(collideAtom, spawnType2, spawn2OffsetDir * (getRadius(spawnType2) + getRadius(spawnType1)) * 2f, spawn2OffsetDir * p0.magnitude / 3 / getMass(spawnType2));

        spawnAtom(collideAtom, spawnType3, spawn3OffsetDir * (getRadius(spawnType3) + getRadius(spawnType1)) * 2f, spawn3OffsetDir * p0.magnitude / 3 / getMass(spawnType3));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("collision Enter", this.gameObject);
        Atom collideAtom = collision.collider.GetComponent<Atom>();
        if (type == AtomType.proton)
        {
            if (collideAtom.type == AtomType.proton)
            {
                //PPI
                if (canSpawn)
                {
                    collideAtom.canSpawn = false;
                    Debug.Log("collision p - p", this.gameObject);

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

                    spawnAtom(collideAtom, AtomType.He3);
                    GameManager.Instance.raiseSunTemperature(5.49f);
                    Destroy(collideAtom.gameObject);
                    Destroy(this.gameObject);
                }
            }
            if (collideAtom.type == AtomType.Li7)
            {
                if (GameManager.Instance.sunTemp > 14)
                {
                    if (canSpawn)
                    {
                        Debug.Log("collision p - Li7", this.gameObject);
                        collideAtom.canSpawn = false;

                        spawn2Atoms(collideAtom, AtomType.He4, AtomType.He4);
                        GameManager.Instance.raiseSunTemperature(17.35f);
                        Destroy(collideAtom.gameObject);
                        Destroy(this.gameObject);
                    }
                }
            }
            if (collideAtom.type == AtomType.Be7)
            {
                if (GameManager.Instance.sunTemp > 23)
                {
                    if (canSpawn)
                    {
                        Debug.Log("collision p - Be7", this.gameObject);
                        collideAtom.canSpawn = false;
                        spawnAtom(collideAtom, AtomType.B8);
                        GameManager.Instance.raiseSunTemperature(0.14f);
                        Destroy(collideAtom.gameObject);
                        Destroy(this.gameObject);
                    }
                }
            }
            if(collideAtom.type == AtomType.C12)
                if (GameManager.Instance.sunTemp > GameManager.Instance.CNOTemp)
                {
                    if (canSpawn)
                    {
                        Debug.Log("collision p - C12", this.gameObject);
                        collideAtom.canSpawn = false;
                        spawnAtom(collideAtom, AtomType.N13);
                        GameManager.Instance.raiseSunTemperature(1.94f);
                        Destroy(collideAtom.gameObject);
                        Destroy(this.gameObject);
                    }
                }

            if (collideAtom.type == AtomType.C13)
                if (GameManager.Instance.sunTemp > GameManager.Instance.CNOTemp)
                {
                    if (canSpawn)
                    {
                        Debug.Log("collision p - C13", this.gameObject);
                        collideAtom.canSpawn = false;
                        spawnAtom(collideAtom, AtomType.N14);
                        GameManager.Instance.raiseSunTemperature(7.55f);
                        Destroy(collideAtom.gameObject);
                        Destroy(this.gameObject);
                    }
                }

            if (collideAtom.type == AtomType.N14)
                if (GameManager.Instance.sunTemp > GameManager.Instance.CNOTemp)
                {
                    if (canSpawn)
                    {
                        Debug.Log("collision p - N14", this.gameObject);
                        collideAtom.canSpawn = false;
                        spawnAtom(collideAtom, AtomType.O15);
                        GameManager.Instance.raiseSunTemperature(1.94f);
                        Destroy(collideAtom.gameObject);
                        Destroy(this.gameObject);
                    }
                }

            if (collideAtom.type == AtomType.N15)
                if (GameManager.Instance.sunTemp > GameManager.Instance.CNOTemp)
                {
                    if (canSpawn)
                    {
                        Debug.Log("collision p - N15", this.gameObject);
                        collideAtom.canSpawn = false;
                        spawn2Atoms(collideAtom, AtomType.C12, AtomType.He4);
                        GameManager.Instance.raiseSunTemperature(4.97f);
                        Destroy(collideAtom.gameObject);
                        Destroy(this.gameObject);
                    }
                }
        }
        else if (type == AtomType.He3)
        {
            if (collideAtom.type == AtomType.He3)
            {
                //PPI
                if (canSpawn)
                {
                    collideAtom.canSpawn = false;
                    Debug.Log("collision He_3 - He_3", this.gameObject);

                    spawn3Atoms(collideAtom, AtomType.He4, AtomType.proton, AtomType.proton);
                    GameManager.Instance.raiseSunTemperature(12.85f);
                    Destroy(collideAtom.gameObject);
                    Destroy(this.gameObject);
                }
            }
            if (collideAtom.type == AtomType.He4)
            {
                if (GameManager.Instance.sunTemp > 14)
                {
                    if (canSpawn)
                    {
                        Debug.Log("collision He_3 - He_4", this.gameObject);
                        collideAtom.canSpawn = false;

                        spawnAtom(collideAtom, AtomType.Be7);
                        GameManager.Instance.raiseSunTemperature(1.59f);
                        Destroy(collideAtom.gameObject);
                        Destroy(this.gameObject);
                    }
                }
            }
        }
        else if (type == AtomType.He4)
        {
            if (collideAtom.type == AtomType.He4)
            {
                if (GameManager.Instance.sunTemp > 100)
                {
                    if (canSpawn)
                    {
                        Debug.Log("collision He_4 - He_4", this.gameObject);
                        collideAtom.canSpawn = false;

                        spawnAtom(collideAtom, AtomType.Be8);
                        GameManager.Instance.raiseSunTemperature(-0.09f);
                        Destroy(collideAtom.gameObject);
                        Destroy(this.gameObject);
                    }
                }
            }
            if(collideAtom.type == AtomType.Be8)
            {
                if (GameManager.Instance.sunTemp > 100)
                {
                    if (canSpawn)
                    {
                        Debug.Log("collision He_4 - Be8", this.gameObject);
                        collideAtom.canSpawn = false;

                        spawnAtom(collideAtom, AtomType.C12);
                        GameManager.Instance.raiseSunTemperature(7.65f);
                        Destroy(collideAtom.gameObject);
                        Destroy(this.gameObject);
                    }
                }
            }
        }
    }
}
