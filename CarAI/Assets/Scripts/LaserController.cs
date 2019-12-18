using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour
{
    [SerializeField] private GameObject laserPrefab = null;
    [SerializeField] private int cantLasers = 0;
    [SerializeField] private float lasersFieldView = 0f;

    private List<Laser> lasers;

    private void Start()
    {
        lasers = new List<Laser>();
        float angleBtwLasers = lasersFieldView / cantLasers;

        for (int i = 0; i < cantLasers; i++)
        {
            lasers.Add(Instantiate(laserPrefab, this.transform, false).GetComponent<Laser>());
            lasers[i].transform.SetPositionAndRotation(transform.position, Quaternion.identity);
            lasers[i].transform.Rotate(Vector3.up, (angleBtwLasers * i)-(lasersFieldView/2f)+90f);
        }

    }

    public float[] GetLasersDistances()
    {
        float[] distances = new float[cantLasers];
        for (int i = 0; i < cantLasers; i++)
        {
            distances[i] = lasers[i].GetNormalizedDistance();
        }

        return distances;
    }
}
