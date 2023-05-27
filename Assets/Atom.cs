using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Atom : MonoBehaviour
{
    public GameObject heluimPrefab;
    public enum AtomType { proton, D, He3, He4, Be, Li7, Li8, C12 }
    float atomMass;
    float radius;
    public AtomType type;
    public bool canSpawn = true;
    public Rigidbody2D rb2D;
    GameObject spawnedAtom;
    // Start is called before the first frame update
    void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        atomMass = getMass(type);
        radius = getRadius(type);
        rb2D.mass = atomMass;
    }

    // Update is called once per frame
    void Update()
    {

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
            default: 
                return -1;
        }
    }
    float getRadius(AtomType type)
    {
        return Mathf.Sqrt(0.25f / 12f * getMass(type));
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
            case AtomType.Be:
                break;
            case AtomType.Li7:
                spawnedAtom = Instantiate(GameManager.Instance.Li_7Prefab, (transform.position + collideAtom.transform.position) / 2, Quaternion.identity);
                break;
            case AtomType.Li8:
                break;
            case AtomType.C12:
                spawnedAtom = Instantiate(GameManager.Instance.C_12Prefab, (transform.position + collideAtom.transform.position) / 2, Quaternion.identity);
                break;
        }
        spawnedAtom.GetComponent<Rigidbody2D>().velocity = cacluateSpeed(rb2D.velocity, collideAtom.rb2D.velocity, atomMass, collideAtom.atomMass, spawnedAtom.GetComponent<Atom>().atomMass);
    }

    void spawnAtom(Atom collideAtom, AtomType spawnType, Vector2 OffsetDir, Vector2 velocity)
    {
        OffsetDir *= 2.5f;
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
            case AtomType.Be:
                break;
            case AtomType.Li7:
                spawnedAtom = Instantiate(GameManager.Instance.Li_7Prefab, (transform.position + collideAtom.transform.position) / 2 + new Vector3(OffsetDir.x, OffsetDir.y, 0) * getRadius(spawnType), Quaternion.identity);
                break;
            case AtomType.Li8:
                break;
            case AtomType.C12:
                spawnedAtom = Instantiate(GameManager.Instance.C_12Prefab, (transform.position + collideAtom.transform.position) / 2 + new Vector3(OffsetDir.x, OffsetDir.y, 0) * getRadius(spawnType), Quaternion.identity);
                break;
        }
        spawnedAtom.GetComponent<Rigidbody2D>().velocity = velocity;
    }

    Tuple<float, float> calculate2AtomsVelocityValue(Vector2 p0, float m1, float m2)
    {
        return Tuple.Create(p0.magnitude/2f / m1, p0.magnitude /2f / m2 );
    }
    void spawn2Atoms(Atom collideAtom, AtomType spawnType1, AtomType spawnType2)
    {
        Vector2 p0 = rb2D.velocity * atomMass + collideAtom.GetComponent<Rigidbody2D>().velocity * atomMass;

        Vector2 spawn1OffsetDir = Quaternion.Euler(0f, 0f, 45f) * p0.normalized;
        Vector2 spawn2OffsetDir = Quaternion.Euler(0f, 0f, -45f) * p0.normalized;
        Tuple<float, float> velocityValue = calculate2AtomsVelocityValue(p0, getMass(spawnType1), getMass(spawnType2));
        spawnAtom(collideAtom, spawnType1, spawn1OffsetDir * getRadius(spawnType1) * 2f, spawn1OffsetDir * velocityValue.Item1);

        spawnAtom(collideAtom, spawnType2, spawn2OffsetDir* getRadius(spawnType2) * 2f, spawn2OffsetDir * velocityValue.Item2);

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
                if (canSpawn)
                {
                    collideAtom.canSpawn = false;
                    Debug.Log("collision p - p", this.gameObject);
                    
                    spawnAtom(collideAtom, AtomType.D);

                    Destroy(collideAtom.gameObject);
                    Destroy(this.gameObject);
                }
            }
            if (collideAtom.type == AtomType.D)
            {
                if (canSpawn)
                {
                    Debug.Log("collision p - D", this.gameObject);
                    collideAtom.canSpawn = false;

                    spawnAtom(collideAtom, AtomType.He3);

                    Destroy(collideAtom.gameObject);
                    Destroy(this.gameObject);
                }
            }
            if (collideAtom.type == AtomType.Li7)
            {
                if (canSpawn)
                {
                    Debug.Log("collision p - Li7", this.gameObject);
                    collideAtom.canSpawn = false;

                    spawn2Atoms(collideAtom, AtomType.He4, AtomType.He4);
                    
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
                    collideAtom.canSpawn = false;
                    Debug.Log("collision He_3 - He_3", this.gameObject);

                    spawn3Atoms(collideAtom, AtomType.He4, AtomType.proton, AtomType.proton);

                    Destroy(collideAtom.gameObject);
                    Destroy(this.gameObject);
                }
            }
            if (collideAtom.type == AtomType.He4)
            {
                if (canSpawn)
                {
                    Debug.Log("collision He_3 - He_4", this.gameObject);
                    collideAtom.canSpawn = false;

                    spawnAtom(collideAtom, AtomType.Li7);
                    Destroy(collideAtom.gameObject);
                    Destroy(this.gameObject);
                }
            }
        }
    }
}
