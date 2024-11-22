using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; // 선택 해제를 위해 필요

public class ForceButtonHighlight : MonoBehaviour
{
    public void OnAnyButtonClick()
    {
        // 현재 선택된 버튼 해제
        EventSystem.current.SetSelectedGameObject(null);
    }
}
