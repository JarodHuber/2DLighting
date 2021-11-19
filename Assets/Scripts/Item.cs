using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/Simple")]
public class Item : ScriptableObject
{
    [SerializeField]
    private Sprite sprite = null;
    [SerializeField]
    private string itemName = "";
    [SerializeField, TextArea]
    private string itemDescription = "";

    public Sprite Sprite { get => sprite; }
    public string Name { get => itemName; }
    public string Description { get => itemDescription; }
}
