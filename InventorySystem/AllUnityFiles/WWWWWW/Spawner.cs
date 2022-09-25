using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject item;
    public GameObject Initem;
    public Transform parent;

    public float XYSisennys = 75;
    private Vector3 v;
    public int InRowX;
    public int InColumneY;
    private float X = 90f;
    private float Y = 140f;

    public List<Vector2> startpositions = new List<Vector2>();

    public List<GameObject> Objects = new List<GameObject>();
    public GameObject ObjectsSpawnPoint;
    public int switchWeapon = 0;
    public GameObject[] quickObjects = new GameObject[3] { null, null, null };

    private void Start()
    {
        Cursor.visible = true;
        for (int i = 0; i < InRowX; i++)
        {
            if (i > 0) X += XYSisennys;
            for (int j = 0; j < InColumneY; j++)
            {
                if (j > 0) Y -= XYSisennys;
                v = new Vector3(X, Y, 0);

                GameObject cur = Instantiate(item,v,Quaternion.identity) as GameObject;
                cur.transform.SetParent(parent);
                cur.transform.localPosition = v;

                if (j == InColumneY - 1) Y = 140;
            }
        }

        XYSisennys = 75; X = 90; Y = 140;

        for (int i = 0; i < InRowX; i++)
        {
            if (i > 0) X += XYSisennys;
            for (int j = 0; j < InColumneY; j++)
            {
                if (j > 0) Y -= XYSisennys;
                v = new Vector3(X, Y, 0);

                GameObject Incur = Instantiate(Initem, v, Quaternion.identity) as GameObject;
                Incur.transform.SetParent(parent);
                Incur.transform.localPosition = v;
                startpositions.Add(Incur.GetComponent<RectTransform>().anchoredPosition);

                if (j == InColumneY - 1) Y = 140;
            }
        }
    }

    private void Update()
    {
        if(quickObjects.Length > 0) {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                switchWeapon = 0;
                for (int i = 0; i < quickObjects.Length; i++)
                {
                    if(quickObjects[i] != null)
                    {
                        if(i == 0) quickObjects[i].SetActive(true); else quickObjects[i].SetActive(false);
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                switchWeapon = 1;
                for (int i = 0; i < quickObjects.Length; i++)
                {
                    if (quickObjects[i] != null)
                    {
                        if (i == 1) quickObjects[i].SetActive(true); else quickObjects[i].SetActive(false);
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                switchWeapon = 2;
                for (int i = 0; i < quickObjects.Length; i++)
                {
                    if (quickObjects[i] != null)
                    {
                        if (i == 2) quickObjects[i].SetActive(true); else quickObjects[i].SetActive(false);
                    }
                }
            }

            if (Input.GetAxis("Mouse ScrollWheel") > 0f && ((quickObjects[0] != null && quickObjects[1] != null) || (quickObjects[0] != null && quickObjects[2] != null) || (quickObjects[1] != null && quickObjects[2] != null))) { switchWeapon++; }
            if (Input.GetAxis("Mouse ScrollWheel") < 0f && ((quickObjects[0] != null && quickObjects[1] != null) || (quickObjects[0] != null && quickObjects[2] != null) || (quickObjects[1] != null && quickObjects[2] != null))) { switchWeapon--; }

            if (switchWeapon == 0 && quickObjects[0] != null) { quickObjects[0].SetActive(true); if (quickObjects[1] != null) quickObjects[1].SetActive(false); if (quickObjects[2] != null) quickObjects[2].SetActive(false); }
            if (switchWeapon == 1 && quickObjects[1] != null) { quickObjects[1].SetActive(true); if (quickObjects[0] != null) quickObjects[0].SetActive(false); if (quickObjects[2] != null) quickObjects[2].SetActive(false); }
            if (switchWeapon == 2 && quickObjects[2] != null) { quickObjects[2].SetActive(true); if (quickObjects[1] != null) quickObjects[1].SetActive(false); if (quickObjects[0] != null) quickObjects[0].SetActive(false); }

            if (switchWeapon <= -1) switchWeapon = 2; if (switchWeapon >= 3) switchWeapon = 0;
        }
    }
}
