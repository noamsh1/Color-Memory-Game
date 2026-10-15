using UnityEngine;
using TMPro;
using System.Dynamic;

public class GameSettings : MonoBehaviour
{

    private static GameSettings instance;

    [SerializeField] TMP_Dropdown dropdown;

    public int tileNum { get; private set; }
    public int playerNum { get; private set; }

    public void Awake()
    {
        CreateInstance();
    }

    private void CreateInstance()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public static GameSettings GetInstance()
    {
        return instance;
    }

    public void SetTileNum()
    {
        if (dropdown != null)
        {
            int selectedIndex = dropdown.value;
            int selectedValue;
            switch (selectedIndex)
            {
                case 0:
                    selectedValue = 24;
                    break;
                case 1:
                    selectedValue = 36;
                    break;
                case 2:
                    selectedValue = 48;
                    break;
                default:
                    selectedValue = 24;
                    break;
            }
            tileNum = selectedValue;
        }
    }

    public void SetPlayerNum(int playerNum)
    {
        this.playerNum = playerNum;
    }

}
