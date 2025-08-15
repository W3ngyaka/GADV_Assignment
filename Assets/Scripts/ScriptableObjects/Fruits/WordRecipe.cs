using UnityEngine;

[CreateAssetMenu(menuName = "Crafting/New Word Recipe")]
public class WordRecipe : ScriptableObject
{
    // The exact word required (letters must match order in crafting area)
    public string requiredWord;

    // Prefab that will be spawned when the recipe is successfully crafted
    public GameObject resultPrefab;

    // Optional: icon to represent the result item in UI (inventory, crafting menus, etc.)
    public Sprite resultIcon;
}
