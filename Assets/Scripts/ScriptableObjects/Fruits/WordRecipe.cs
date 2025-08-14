using UnityEngine;

[CreateAssetMenu(menuName = "Crafting/New Word Recipe")]
public class WordRecipe : ScriptableObject
{
    public string requiredWord;
    public GameObject resultPrefab;
    public Sprite resultIcon;
}