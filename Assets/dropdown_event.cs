using UnityEngine;
using TMPro;

public class dropdown_event : MonoBehaviour
{
    private TMP_Dropdown dropdown;
    public GameController gamecontroller;

    private void Awake()
    {
        dropdown.onValueChanged.AddListener(OnDropdownEvent);
    }

    public void OnDropdownEvent(int index)
    {
        gamecontroller.SetTotalPlayers(index + 2);
    }
}
