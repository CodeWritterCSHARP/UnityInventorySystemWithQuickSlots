using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class groundItem : MonoBehaviour
{
    [SerializeField] bool pick = false;
    public int index = -1;
    public bool StackItem;
    private GameObject spawn;
    public GameObject inst;
    GameObject pl;
    private Transform spawnpoint;

    private void Start() {spawn = GameObject.Find("Directional Light"); pl = GameObject.Find("FPSController"); }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") pick = true;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player") pick = false;
    }

    private void OnMouseOver()
    {
        if(pick == true)
        {
            transform.GetChild(0).gameObject.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E))
            {
                bool Destr = false;

                List<GameObject> list = new List<GameObject>();
                list.AddRange(GameObject.FindGameObjectsWithTag("Img"));
                for (int i = 0; i < list.Count; i++)
                {
                    if(list[i].GetComponent<ITEM>().index == index) //if same index
                    {
                        if(StackItem == false) //is pickable item can stack
                        {
                            if(list[i].GetComponent<ITEM>().stackable == false) //if slot isnt full
                            {
                                if (list[i].GetComponent<ITEM>().isEmpty == true) //if slot can take an item
                                {
                                    list[i].GetComponent<ITEM>().count++;
                                    Destr = true;
                                    break;
                                }
                            }
                            else { continue; } //if slot is full
                        }
                        else { continue; } //if pickable item cant stack
                    }
                    else //if index isnt same
                    {
                        if(list[i].GetComponent<ITEM>().index == -1) //if slot is empty (default index)
                        {
                            if(list[i].GetComponent<ITEM>().isEquiped == false) { //if slot isnt quick
                                list[i].GetComponent<ITEM>().count = 1;
                                list[i].GetComponent<ITEM>().index = index;
                                switch (index) //Here you can edit each item parametrs
                                {
                                    case 0: list[i].GetComponent<ITEM>().image.sprite = Resources.Load<Sprite>("Apple");
                                        list[i].GetComponent<ITEM>().type = "food";
                                        SpawnObjectes(i, list, index);
                                        break;

                                    case 1: list[i].GetComponent<ITEM>().image.sprite = Resources.Load<Sprite>("Sword");
                                        list[i].GetComponent<ITEM>().type = "weapon";
                                        SpawnObjectes(i, list, index);
                                        break;

                                    case 2: list[i].GetComponent<ITEM>().image.sprite = Resources.Load<Sprite>("Helmet");
                                        list[i].GetComponent<ITEM>().type = "head";
                                        list[i].GetComponent<ITEM>().countMax = 2;
                                        SpawnObjectes(i, list, index);
                                        break;
                                    default: break;
                                }
                                Destr = true;
                                break;
                            }
                            else //if slot is quick
                            {

                            }
                        }
                    }
                }
                if (Destr == true) Destroy(this.gameObject);
            }
        }
    }
    private void OnMouseExit()
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }
    void SpawnObjectes(int i, List<GameObject> list, int index)
    {
        inst = Instantiate(spawn.GetComponent<Spawner>().Objects[index], spawn.GetComponent<Spawner>().ObjectsSpawnPoint.transform.position, spawn.GetComponent<Spawner>().ObjectsSpawnPoint.transform.rotation) as GameObject;
        list[i].GetComponent<ITEM>().quickObject = inst;
        inst.transform.SetParent(pl.transform);
        inst.transform.position = spawn.GetComponent<Spawner>().ObjectsSpawnPoint.transform.position;
        inst.transform.SetParent(spawn.GetComponent<Spawner>().ObjectsSpawnPoint.transform);
        inst.SetActive(false);
    }
}
