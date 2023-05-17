using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Progress;

public class InventoryUIController : MonoBehaviour
{
    public List<InventorySlot> InventoryItems = new List<InventorySlot>();
    //Global variable
    private static VisualElement m_GhostIcon;
    private static bool m_IsDragging;
    private static InventorySlot m_OriginalSlot;
    AvatarController play;



    private VisualElement m_Root;
    private VisualElement m_SlotContainer, setContainer, setContainer2;
    public List<InventorySlot> SetSlots = new List<InventorySlot>();

    private GameObject player;



    private void Awake()
    {

        player = GameObject.Find("Player");
        play = player.GetComponent<AvatarController>();
        m_Root = GetComponent<UIDocument>().rootVisualElement;
        m_GhostIcon = m_Root.Query<VisualElement>("GhostIcon");
        m_SlotContainer = m_Root.Query("SlotContainer");
        setContainer = m_Root.Query<VisualElement>("Set1");
        setContainer2 = m_Root.Query<VisualElement>("Set2");




        for (int i = 0; i < 25; i++)
        {
            InventorySlot item = new InventorySlot();

            InventoryItems.Add(item);

            m_SlotContainer.Add(item);

        }

        for (int i = 0; i < 3; i++)
        {
            InventorySlot item = new InventorySlot();

            setContainer.Add(item);
            SetSlots.Add(item);

        }

        for (int i = 0; i < 3; i++)
        {
            InventorySlot item = new InventorySlot();

            setContainer2.Add(item);
            SetSlots.Add(item);

        }
        GameController.OnInventoryChanged += GameController_OnInventoryChanged;

        m_GhostIcon.RegisterCallback<PointerMoveEvent>(OnPointerMove);
        m_GhostIcon.RegisterCallback<PointerUpEvent>(OnPointerUp);
    }
    private void GameController_OnInventoryChanged(string[] itemGuid, InventoryChangeType change)
    {

        //Loop through each item and if it has been picked up, add it to the next empty slot
        foreach (string item in itemGuid)
        {
            ItemDetails newItem = GameController.GetItemByGuid(item);
            if (change == InventoryChangeType.Pickup)
            {

                var stack = InventoryItems.FirstOrDefault(x => x.ItemGuid.Equals(newItem));
                if ((newItem.Name.Contains("Arrow") || newItem.objectType == ObjectType.Consumable) && stack != null)
                {

                    stack.quantity++;

                }
                else
                {
                    var emptySlot = InventoryItems.FirstOrDefault(x => x.ItemGuid.Equals(""));

                    if (emptySlot != null)
                    {
                        if (newItem.Name.Contains("Arrow") || newItem.objectType == ObjectType.Consumable)
                        {
                            newItem.quantity++;
                            Debug.Log(newItem.quantity+" ammo");
                        }
                        emptySlot.HoldItem(newItem);



                    }
                }
            }
        }
    }
    public static void StartDrag(Vector2 position, InventorySlot originalSlot)
    {
        //Set tracking variables
        m_IsDragging = true;
        m_OriginalSlot = originalSlot;

        //Set the new position
        m_GhostIcon.style.top = position.y - m_GhostIcon.layout.height / 2;
        m_GhostIcon.style.left = position.x - m_GhostIcon.layout.width / 2;

        //Set the image
        m_GhostIcon.style.backgroundImage = GameController.GetItemByGuid(originalSlot.ItemGuid).Icon.texture;

        //Flip the visibility on
        m_GhostIcon.style.visibility = Visibility.Visible;
    }

    private void OnPointerMove(PointerMoveEvent evt)
    {
        //Only take action if the player is dragging an item around the screen
        if (!m_IsDragging)
        {
            return;
        }

        //Set the new position
        m_GhostIcon.style.top = evt.position.y - m_GhostIcon.layout.height / 2;
        m_GhostIcon.style.left = evt.position.x - m_GhostIcon.layout.width / 2;

    }

    private void OnPointerUp(PointerUpEvent evt)
    {

        if (!m_IsDragging)
        {
            return;
        }

        //Check to see if they are dropping the ghost icon over any inventory slots.
        IEnumerable<InventorySlot> slots = InventoryItems.Where(x =>
               x.worldBound.Overlaps(m_GhostIcon.worldBound));

        //Found at least one
        if (slots.Count() != 0)
        {

            if (SetSlots.Contains(m_OriginalSlot))
            {

                Debug.Log("found");
                InventorySlot closestSlot = slots.OrderBy(x => Vector2.Distance
                  (x.worldBound.position, m_GhostIcon.worldBound.position)).First();
                int index = 0;
                for (int i = 0; i < SetSlots.Count; i++)
                {
                    if (SetSlots[i].Equals(closestSlot))
                    {
                        index = i; break;
                    }
                }
                if ((index >= 3 && !AvatarController.set1) || (index < 3 && AvatarController.set1))
                {
                    play.SetEquip(m_OriginalSlot);
                }
                if (closestSlot.ItemGuid == "")
                {  
                    SetItem(slots);  
                }
                else
                {
                    
                    SetItem2(slots);
                    
                }

                if ((index >= 3 && !AvatarController.set1) || (index < 3 && AvatarController.set1))
                {
                    
                    play.SetEquip(m_OriginalSlot);
                }
                //por aqui va la cosa, esto es si arrastramos desde la casilla de equipo al inventario
                //hay que comprobar si la casilla de origen esta vacia o contiene un objeto que se pueda equipar en la misma casilla
            }
            else
            {
                SetItem(slots);
            }




        }
        //Didn't find any (dragged off the window)
        else
        {

            IEnumerable<InventorySlot> sets = SetSlots.Where(x =>
               x.worldBound.Overlaps(m_GhostIcon.worldBound));
            if (sets.Count() != 0)
            {
                InventorySlot closestSlot = sets.OrderBy(x => Vector2.Distance
                 (x.worldBound.position, m_GhostIcon.worldBound.position)).First();
                int indice = 0;

                for (int i = 0; i < SetSlots.Count; i++)
                {
                    if (SetSlots[i].Equals(closestSlot))
                    {
                        indice = i; break;
                    }
                }

                ObjectType obj = GameController.GetItemByGuid(m_OriginalSlot.ItemGuid).objectType;
                bool control = false;
                switch (indice)
                {
                    case 0:
                    case 3:
                        if (obj.Equals(ObjectType.Equipment))
                        {

                            control = true;
                            
                        }
                        break;
                    case 1:
                    case 4:
                        if (obj.Equals(ObjectType.Accesory))
                        {
                            control = true;

                        }
                        break;
                    case 2:
                    case 5:
                        if (obj.Equals(ObjectType.Consumable))
                        {
                            control = true;

                        }
                        break;
                    default:
                        // código en caso de que no haya coincidencia
                        break;
                }

                if (control)
                {
                    if ((indice >= 3 && !AvatarController.set1) || (indice < 3 && AvatarController.set1))
                    {



                        play.SetEquip(SetSlots[indice]);
                        play.SetEquip(m_OriginalSlot);



                    }

                    SetItem(sets);



                }
                else
                {
                    m_OriginalSlot.Icon.image =
                     GameController.GetItemByGuid(m_OriginalSlot.ItemGuid).Icon.texture;
                }
            }
            else
            {

                m_OriginalSlot.Icon.image =
                      GameController.GetItemByGuid(m_OriginalSlot.ItemGuid).Icon.texture;
            }


        }
        //Clear dragging related visuals and data
        m_IsDragging = false;
        m_OriginalSlot = null;
        m_GhostIcon.style.visibility = Visibility.Hidden;
    }

    private void SetItem(IEnumerable<InventorySlot> lista)
    {
        InventorySlot closestSlot = lista.OrderBy(x => Vector2.Distance
                  (x.worldBound.position, m_GhostIcon.worldBound.position)).First();
        if (closestSlot.Equals(m_OriginalSlot))
        {
            m_OriginalSlot.Icon.image =
                     GameController.GetItemByGuid(m_OriginalSlot.ItemGuid).Icon.texture;
        }
        else if (closestSlot.Icon.image != null)
        {
            InventorySlot temporarySlot = new InventorySlot();
            temporarySlot.HoldItem(GameController.GetItemByGuid(closestSlot.ItemGuid));
            closestSlot.HoldItem(GameController.GetItemByGuid(m_OriginalSlot.ItemGuid));
            m_OriginalSlot.HoldItem(GameController.GetItemByGuid(temporarySlot.ItemGuid));
            changeQuantity(m_OriginalSlot, closestSlot);


        }
        else
        {
            closestSlot.HoldItem(GameController.GetItemByGuid(m_OriginalSlot.ItemGuid));
            changeQuantity(m_OriginalSlot, closestSlot);

            //Clear the original slot
            m_OriginalSlot.DropItem();
        }


        //Set the new inventory slot with the data


    }

    private void SetItem2(IEnumerable<InventorySlot> lista)
    {
        InventorySlot closestSlot = lista.OrderBy(x => Vector2.Distance
                  (x.worldBound.position, m_GhostIcon.worldBound.position)).First();
        if (closestSlot.Equals(m_OriginalSlot))
        {
            m_OriginalSlot.Icon.image =
                     GameController.GetItemByGuid(m_OriginalSlot.ItemGuid).Icon.texture;
        }
        else if (closestSlot.Icon.image != null)
        {
            if (GameController.GetItemByGuid(closestSlot.ItemGuid).objectType == GameController.GetItemByGuid(m_OriginalSlot.ItemGuid).objectType)
            {
                InventorySlot temporarySlot = new InventorySlot();
                temporarySlot.HoldItem(GameController.GetItemByGuid(closestSlot.ItemGuid));
                closestSlot.HoldItem(GameController.GetItemByGuid(m_OriginalSlot.ItemGuid));
                m_OriginalSlot.HoldItem(GameController.GetItemByGuid(temporarySlot.ItemGuid));
                changeQuantity(m_OriginalSlot, closestSlot);
            }
            else
            {
                m_OriginalSlot.Icon.image =
                    GameController.GetItemByGuid(m_OriginalSlot.ItemGuid).Icon.texture;
            }


        }
        else
        {
            closestSlot.HoldItem(GameController.GetItemByGuid(m_OriginalSlot.ItemGuid));
            changeQuantity(m_OriginalSlot, closestSlot);

            //Clear the original slot
            m_OriginalSlot.DropItem();
        }
    }

    private void changeQuantity(InventorySlot original, InventorySlot newSlot)
    {
        var aux = newSlot.quantity;
        newSlot.quantity= original.quantity;
        original.quantity= aux;
    }
    
        
    
}
