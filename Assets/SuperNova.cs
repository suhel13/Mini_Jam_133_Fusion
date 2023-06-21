using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperNova : MonoBehaviour
{
    float startingScale;
    bool isCollapsing = false;
    [SerializeField] int collapseSteps;
    [SerializeField] float timeBetweenSteps;
    [SerializeField] float explosionStartVelocity;
    public GameObject explosionParticles;

    List<Transform> atomsInSun = new List<Transform>();

    private void Awake()
    {
        explosionParticles.SetActive(false);
    }
    void explode()
    {
        foreach (Transform item in atomsInSun)
        {
            item.localScale = Vector3.one;
            item.GetComponent<Rigidbody2D>().velocity = (item.position - this.transform.position).normalized * explosionStartVelocity;
        }
        explosionParticles.SetActive(true);
    }

    public void StartCollapseSequense()
    {
        if (isCollapsing == false)
        {
            isCollapsing = true;
            GameManager.Instance.isColapsing = true;
            startingScale = transform.localScale.x;
            Collider2D[] atomsInSunCollider = Physics2D.OverlapCircleAll(transform.position, startingScale * 5);
            atomsInSun.Clear();
            foreach (Collider2D item in atomsInSunCollider)
            {
                if (item.GetComponent<Atom>() != null)
                {
                    atomsInSun.Add(item.transform);
                    item.GetComponent<Atom>().canSpawn = false;
                }
            }
            StartCoroutine(collase());
        }

    }

    IEnumerator collase()
    {
        for (int i = collapseSteps; i > 0 ; i--)
        {
            Debug.Log((float)i/collapseSteps);
            this.transform.localScale = Vector3.one * (float)i / collapseSteps * startingScale;
            foreach (Transform item in atomsInSun)
            {
                item.localScale = Vector3.one * (float)i / collapseSteps;
            }
            yield return new WaitForSeconds(timeBetweenSteps);
        }
        explode();
    }

}
