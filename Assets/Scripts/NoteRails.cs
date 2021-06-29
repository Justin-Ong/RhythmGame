using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteRails : MonoBehaviour
{
    public GameObject notePrefab;

    public int numRails = 3;
    public float railDistance = 5;
    public GameObject railPrefab;

    class Rail
    {
        public GameObject rail;
        public int poolSize;
        public Vector3 spawnPoint;

        private GameObject[] notePool;

        public Rail(GameObject newRail, Vector3 spawnPointPos, int size, GameObject notePrefab) {
            rail = newRail;
            spawnPoint = spawnPointPos;
            poolSize = size;
            notePool = new GameObject[poolSize];
            for (int i = 0; i < poolSize; i++)
            {
                GameObject note = Instantiate(notePrefab);
                note.transform.position = spawnPoint;
                note.SetActive(false);
                notePool[i] = note;
            }
        }

        public void spawnNote()
        {
            for (int i = 0; i < poolSize; i++) {
                if (!notePool[i].activeInHierarchy)
                {
                    notePool[i].SetActive(true);
                    break;
                }
            }
        }
    }

    private Rail[] rails;
    private int notePoolSize = 10;

    void Start()
    {
        rails = new Rail[numRails];
        Vector3 initialPosition = new Vector3(0, -1 * (railDistance * (numRails - 1) / 2), -2);
        for (int i = 0; i < numRails; i++)
        {
            GameObject rail = Instantiate(railPrefab, initialPosition, transform.rotation);
            rail.transform.position += Vector3.up * railDistance * i;
            rails[i] = new Rail(rail, rail.transform.GetChild(0).transform.position, notePoolSize, notePrefab);
        }
    }

    void Update()
    {
        for (int i = 0; i < numRails; i++)
        {
            rails[i].spawnNote();
        }
    }
}
