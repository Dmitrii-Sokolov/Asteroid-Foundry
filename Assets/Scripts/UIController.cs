using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIController : MonoBehaviour
{
    private VisualElement ProgressVisualElement;
    private List<Button> Buttons;

    private void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        ProgressVisualElement = root.Q<VisualElement>("Progress");

        Buttons = root.Query<Button>().ToList();

        //ProgressVisualElement.style.display = DisplayStyle.None;

        foreach (var button in Buttons)
        {
            button.RegisterCallback<ClickEvent>(args => Click(args, button.text));
        }
    }

    private void Click(ClickEvent args, string command)
    {
        //ProgressVisualElement.style.display = DisplayStyle.Flex;

        ProgressVisualElement.AddToClassList("Progress-Done");

        Debug.Log($"Perform '{command}'");

        Invoke(nameof(OnProgressEnd), 1f);
    }

    private void OnProgressEnd()
    {
        //ProgressVisualElement.style.display = DisplayStyle.None;
        ProgressVisualElement.RemoveFromClassList("Progress-Done");
    }
}
