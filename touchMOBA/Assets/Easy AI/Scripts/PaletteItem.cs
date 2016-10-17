using UnityEngine;

public class PaletteItem: MonoBehaviour
{
	#if UNITY_EDITOR
	public enum Category
	{
		Enemies,
	}

	public Category category = Category.Enemies;
	public string itemName = "";
	public Object inspectedScript;
	#endif
}
