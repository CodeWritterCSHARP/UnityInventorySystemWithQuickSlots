using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class droppedWindow : MonoBehaviour, IDropHandler
{
    public int mode; public int quick;
    private Spawner sp;

    private void Start()
    {
        sp = GameObject.Find("Directional Light").GetComponent<Spawner>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null) {
            switch (mode)
            {
                case 1: //To previous position
                    eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = eventData.pointerDrag.GetComponent<ITEM>().aPos;
                    break;

                case 2://To ground dropping
                    eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = eventData.pointerDrag.GetComponent<ITEM>().aPos;
                    if(eventData.pointerDrag.GetComponent<ITEM>().isEquiped == true) {
                        List<GameObject> currentobjects = new List<GameObject>();
                        currentobjects.AddRange(GameObject.FindGameObjectsWithTag("Img"));
                        Spawner spawner = GameObject.Find("Directional Light").GetComponent<Spawner>();
                        List<Vector2> dist = new List<Vector2>();
                        for (int i = 0; i < currentobjects.Count; i++) dist.Add(currentobjects[i].GetComponent<RectTransform>().anchoredPosition);
                        List<Vector2> old = spawner.startpositions;

                        for (int i = 0; i < dist.Count; i++)
                        {
                            if (dist.Contains(old[i])) continue;
                            else
                            {
                                eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = old[i];
                                eventData.pointerDrag.GetComponent<ITEM>().isEquiped = false;
                                break;
                            }
                        }
                    }
                    eventData.pointerDrag.GetComponent<ITEM>().Clear();
                    break;

                case 3://To slot
                    eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
                    if (eventData.pointerDrag.GetComponent<ITEM>().isEquiped == true) {
                        GameObject betweenvalue = Instantiate(eventData.pointerDrag.GetComponent<ITEM>().quickObject, 
                            eventData.pointerDrag.GetComponent<ITEM>().quickObject.transform.position, 
                            eventData.pointerDrag.GetComponent<ITEM>().quickObject.transform.rotation) as GameObject;
                        betweenvalue.transform.SetParent(sp.ObjectsSpawnPoint.transform);
                        betweenvalue.SetActive(false);

                        Destroy(eventData.pointerDrag.GetComponent<ITEM>().quickObject);
                        eventData.pointerDrag.GetComponent<ITEM>().quickObject = null;
                        switch (eventData.pointerDrag.GetComponent<ITEM>().type)
                        {
                            case "weapon": sp.quickObjects[0] = eventData.pointerDrag.GetComponent<ITEM>().quickObject; break;
                            case "food": sp.quickObjects[1] = eventData.pointerDrag.GetComponent<ITEM>().quickObject; break;
                            case "head": sp.quickObjects[2] = eventData.pointerDrag.GetComponent<ITEM>().quickObject; break;
                        }
                        eventData.pointerDrag.GetComponent<ITEM>().quickObject = betweenvalue;
                        eventData.pointerDrag.GetComponent<ITEM>().isEquiped = false;
                    }
                    if (eventData.pointerDrag.GetComponent<ITEM>().count <= 0) eventData.pointerDrag.GetComponent<ITEM>().type = null;
                    break;

                case 4://To quick slot
                    if(eventData.pointerDrag.GetComponent<ITEM>().count != 0) {
                        switch (quick)
                        {
                            case 1:
                                if(eventData.pointerDrag.GetComponent<ITEM>().type == "weapon")
                                {
                                    eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
                                    eventData.pointerDrag.GetComponent<ITEM>().isEquiped = true;
                                    Spawner spawner = GameObject.Find("Directional Light").GetComponent<Spawner>();
                                    spawner.quickObjects[0] = eventData.pointerDrag.GetComponent<ITEM>().quickObject;
                                }
                                else eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = eventData.pointerDrag.GetComponent<ITEM>().aPos;
                                break;

                            case 2:
                                if (eventData.pointerDrag.GetComponent<ITEM>().type == "food")
                                {
                                    eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
                                    eventData.pointerDrag.GetComponent<ITEM>().isEquiped = true;
                                    Spawner spawner = GameObject.Find("Directional Light").GetComponent<Spawner>();
                                    spawner.quickObjects[1] = eventData.pointerDrag.GetComponent<ITEM>().quickObject;
                                }
                                else eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = eventData.pointerDrag.GetComponent<ITEM>().aPos;
                                break;

                            case 3:
                                if (eventData.pointerDrag.GetComponent<ITEM>().type == "head")
                                {
                                    eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
                                    eventData.pointerDrag.GetComponent<ITEM>().isEquiped = true;
                                    Spawner spawner = GameObject.Find("Directional Light").GetComponent<Spawner>();
                                    spawner.quickObjects[2] = eventData.pointerDrag.GetComponent<ITEM>().quickObject;
                                }
                                else eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = eventData.pointerDrag.GetComponent<ITEM>().aPos;
                                break;
                            default: break;
                        }
                    }else eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = eventData.pointerDrag.GetComponent<ITEM>().aPos;
                    break;

                default: break;
            }
        }
    }
}
