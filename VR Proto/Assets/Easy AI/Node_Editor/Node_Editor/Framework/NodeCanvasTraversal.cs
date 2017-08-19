using System;
using NodeEditorFramework;

namespace NodeEditorFramework
{
	[Serializable]
	public abstract class NodeCanvasTraversal
	{
		public MainNodeCanvas nodeCanvas;

		public NodeCanvasTraversal (MainNodeCanvas canvas)
		{
			nodeCanvas = canvas;
		}

		public virtual void OnLoadCanvas () { }
		public virtual void OnSaveCanvas () { }

		public abstract void TraverseAll ();
		public virtual void OnChange (Node node) {}
	}
}

