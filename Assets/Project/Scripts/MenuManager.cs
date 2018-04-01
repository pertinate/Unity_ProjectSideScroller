using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {
    public static MenuManager instance;

    public List<BarSlot> totalSlots = new List<BarSlot>();
    public BarSlot currentSlot;
    public GameObject MenuBackgroundGO;
    public Button Character_Button, Skills_Button, Inventory_Button, Settings_Button, Quit_Button, Return_Button, PlayMenu_Button;
    public GameObject Character_Panel, Skills_Panel, Inventory_Panel, Settings_Panel, Quit_Panel;

    void Awake()
    {
        instance = this;
        totalSlots.Add(new BarSlot(Character_Panel, Character_Button, 0));
        totalSlots.Add(new BarSlot(Skills_Panel, Skills_Button, 1));
        totalSlots.Add(new BarSlot(Inventory_Panel, Inventory_Button, 2));
        totalSlots.Add(new BarSlot(Settings_Panel, Settings_Button, 3));
        totalSlots.Add(new BarSlot(Quit_Panel, Quit_Button, 4));
        totalSlots.Add(new BarSlot(MenuBackgroundGO, Return_Button, 5));
        GenerateButtonListener();
    }
    void Start()
    {

    }
    
    private void GenerateButtonListener()
    {
        Character_Button.onClick.AddListener(() =>
        {
            GetBarSlot(0).panel.SetActive(true);
            for(int i = 0; i < totalSlots.Count; i++)
            {
                if (i != 0 && i != 5)
                    GetBarSlot(i).panel.SetActive(false);
            }
        });
        Skills_Button.onClick.AddListener(() =>
        {
            GetBarSlot(1).panel.SetActive(true);
            for (int i = 0; i < totalSlots.Count; i++)
            {
                if (i != 1 && i != 5)
                    GetBarSlot(i).panel.SetActive(false);
            }
        });
        Inventory_Button.onClick.AddListener(() =>
        {
            GetBarSlot(2).panel.SetActive(true);
            for (int i = 0; i < totalSlots.Count; i++)
            {
                if (i != 2 && i != 5)
                    GetBarSlot(i).panel.SetActive(false);
            }
        });
        Settings_Button.onClick.AddListener(() =>
        {
            GetBarSlot(3).panel.SetActive(true);
            for (int i = 0; i < totalSlots.Count; i++)
            {
                if (i != 3 && i != 5)
                    GetBarSlot(i).panel.SetActive(false);
            }
        });
        Quit_Button.onClick.AddListener(() =>
        {
            GetBarSlot(4).panel.SetActive(true);
            for (int i = 0; i < totalSlots.Count; i++)
            {
                if (i != 4 && i != 5)
                    GetBarSlot(i).panel.SetActive(false);
            }
            SceneManager.LoadScene(0);
        });
        Return_Button.onClick.AddListener(() =>
        {
            GetBarSlot(5).panel.SetActive(false);
        });
        PlayMenu_Button.onClick.AddListener(() =>
        {
            GetBarSlot(5).panel.SetActive(true);
        });
    }

    BarSlot GetBarSlot(int n)
    {
        for (int i = 0; i < totalSlots.Count; i++){
            if (totalSlots[i].slotNumber == n)
                return totalSlots[i];
        }
        return null;
    }
}
public class BarSlot
{
    public GameObject panel;
    public int slotNumber;
    public Button button;
    public BarSlot(GameObject go, Button b, int sNumber){
        button = b;
        panel = go;
        slotNumber = sNumber;
    }

    public BarSlot SetGameObject(GameObject go)
    {
        panel = go;
        return this;
    }
    public BarSlot SetButton(Button b)
    {
        button = b;
        return this;
    }

    public void Enable()
    {
        if(panel != null && !panel.activeSelf)
            panel.SetActive(true);
    }
    public void Disable()
    {
        if (panel != null && panel.activeSelf)
            panel.SetActive(false);
    }
}
