using UnityEngine;
using UnityEditor;

public static class MenuItems
{


	[MenuItem ("Easy AI/Items C#")]
	private static void ShowPalette ()
	{
		PaletteWindow.ShowPalette ();
	}

}
