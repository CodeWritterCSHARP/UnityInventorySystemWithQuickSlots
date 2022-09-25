using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;

public class ITEM : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler, IPointerClickHandler
{
    public Image image;
    private Sprite spr;
    public Text text;

    public string type;
    public bool isEquiped;

    public bool stackable;
    public int index = -1;
    public bool isEmpty;

    public int count = 0;
    public int countMax;
    public Vector2 aPos;

    [SerializeField] private GameObject canvas;
    public RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    public GameObject quickObject;
    private Spawner spawner;

    public void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        spr = image.sprite;
        canvas = GameObject.Find("Canvas");
        aPos = rectTransform.anchoredPosition;
        spawner = GameObject.Find("Directional Light").GetComponent<Spawner>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        aPos = rectTransform.anchoredPosition;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.6f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.GetComponent<Canvas>().scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        
    }

    public void OnDrop(PointerEventData eventData)
    {
        if(eventData.pointerDrag != null)
        {
            if (eventData.pointerDrag.GetComponent<ITEM>().isEquiped == true) //From QuickSlot
            {
                if(GetComponent<ITEM>().isEquiped == false) { //To Inventory
                    if (eventData.pointerDrag.GetComponent<ITEM>().type == GetComponent<ITEM>().type) //if type is same
                    {
                        eventData.pointerDrag.GetComponent<ITEM>().rectTransform.anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
                        GetComponent<RectTransform>().anchoredPosition = eventData.pointerDrag.GetComponent<ITEM>().aPos;
                        eventData.pointerDrag.GetComponent<ITEM>().isEquiped = false; GetComponent<ITEM>().isEquiped = true;

                        switch (type)
                        {
                            case "weapon": spawner.quickObjects[0] = this.quickObject; break;
                            case "food": spawner.quickObjects[1] = this.quickObject; break;
                            case "head": spawner.quickObjects[2] = this.quickObject; break;
                        }
                        eventData.pointerDrag.GetComponent<ITEM>().quickObject.SetActive(false);
                        //  if (eventData.pointerDrag.GetComponent<ITEM>().count == 0) eventData.pointerDrag.GetComponent<ITEM>().Clear(); //Если количество предмета 0
                    }
                    else //If type isnt same
                    {
                        eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = eventData.pointerDrag.GetComponent<ITEM>().aPos;
                    }
                }
                else //If both items in QuickSlots
                {
                    eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = eventData.pointerDrag.GetComponent<ITEM>().aPos;
                }
            }
            else //From inventory
            {
                if (GetComponent<ITEM>().isEquiped == false) { //To Inventory
                    eventData.pointerDrag.GetComponent<ITEM>().rectTransform.anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
                    GetComponent<RectTransform>().anchoredPosition = eventData.pointerDrag.GetComponent<ITEM>().aPos;
                }
                else //To QuickSlot
                {
                    if (eventData.pointerDrag.GetComponent<ITEM>().type == GetComponent<ITEM>().type)//If type is same
                    {
                        eventData.pointerDrag.GetComponent<ITEM>().rectTransform.anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
                        GetComponent<RectTransform>().anchoredPosition = eventData.pointerDrag.GetComponent<ITEM>().aPos;
                        eventData.pointerDrag.GetComponent<ITEM>().isEquiped = true; GetComponent<ITEM>().isEquiped = false;

                        switch (type)
                        {
                            case "weapon": spawner.quickObjects[0] = eventData.pointerDrag.GetComponent<ITEM>().quickObject; break;
                            case "food": spawner.quickObjects[1] = eventData.pointerDrag.GetComponent<ITEM>().quickObject; break;
                            case "head": spawner.quickObjects[2] = eventData.pointerDrag.GetComponent<ITEM>().quickObject; break;
                        }
                        GetComponent<ITEM>().quickObject.SetActive(false);
                    }
                    else //If type isnt same
                    {
                        eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = eventData.pointerDrag.GetComponent<ITEM>().aPos;
                    }
                }
            }
        }
    }
    public void Clear()
    {
        index = -1; count = 0; isEmpty = true; stackable = false; image.sprite = spr; if (isEquiped == false) type = null; Destroy(quickObject); quickObject = null;
    }
    void Update()
    {
        if (count >= countMax) stackable = true; else stackable = false;
        text.text = count.ToString();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            count -= 1; if (count <= 0)
            {
                List<GameObject> currentobjects = new List<GameObject>();
                currentobjects.AddRange(GameObject.FindGameObjectsWithTag("Img"));
                Spawner spawner = GameObject.Find("Directional Light").GetComponent<Spawner>();
                List<Vector2> dist = new List<Vector2>();
                for (int i = 0; i < currentobjects.Count; i++) dist.Add(currentobjects[i].GetComponent<RectTransform>().anchoredPosition);
                List <Vector2> old = spawner.startpositions;

                for (int i = 0; i < dist.Count; i++)
                {
                    if (dist.Contains(old[i])) continue;
                    else
                    {
                        gameObject.GetComponent<RectTransform>().anchoredPosition = old[i];
                        gameObject.GetComponent<ITEM>().isEquiped = false;
                        break;
                    }
                }
                Clear();
            }
        }
    }
}
