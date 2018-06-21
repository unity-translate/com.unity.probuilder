using UnityEngine;
using UnityEditor;
using UnityEditor.ProBuilder.UI;
using System.Linq;
using UnityEngine.ProBuilder;
using UnityEditor.ProBuilder;
using EditorGUILayout = UnityEditor.EditorGUILayout;
using EditorStyles = UnityEditor.EditorStyles;

namespace UnityEditor.ProBuilder.Actions
{
	sealed class CollapseVertexes : MenuAction
	{
		public override ToolbarGroup group { get { return ToolbarGroup.Geometry; } }
		public override Texture2D icon { get { return IconUtility.GetIcon("Toolbar/Vert_Collapse", IconSkin.Pro); } }
		public override TooltipContent tooltip { get { return _tooltip; } }

		static readonly TooltipContent _tooltip = new TooltipContent
		(
			"Collapse Vertexes",
			@"Merge all selected vertexes into a single vertex, centered at the average of all selected points.",
			keyCommandAlt, 'C'
		);

		public override bool IsEnabled()
		{
			return 	ProBuilderEditor.instance != null &&
					ProBuilderEditor.instance.editLevel == EditLevel.Geometry &&
					ProBuilderEditor.instance.componentMode == ComponentMode.Vertex &&
				MeshSelection.TopInternal().Any(x => x.selectedVertexCount > 1);
		}

		public override bool IsHidden()
		{
			return 	ProBuilderEditor.instance == null ||
					ProBuilderEditor.instance.editLevel != EditLevel.Geometry ||
					ProBuilderEditor.instance.componentMode != ComponentMode.Vertex;

		}

		protected override MenuActionState OptionsMenuState()
		{
			return MenuActionState.VisibleAndEnabled;
		}

		protected override void OnSettingsGUI()
		{
			GUILayout.Label("Collapse Vertexes Settings", EditorStyles.boldLabel);

			EditorGUILayout.HelpBox("Collapse To First setting decides where the collapsed vertex will be placed.\n\nIf True, the new vertex will be placed at the position of the first selected vertex.  If false, the new vertex is placed at the average position of all selected vertexes.", MessageType.Info);

			bool collapseToFirst = PreferencesInternal.GetBool(PreferenceKeys.pbCollapseVertexToFirst);

			EditorGUI.BeginChangeCheck();

			collapseToFirst = EditorGUILayout.Toggle("Collapse To First", collapseToFirst);

			if(EditorGUI.EndChangeCheck())
				PreferencesInternal.SetBool(PreferenceKeys.pbCollapseVertexToFirst, collapseToFirst);

			GUILayout.FlexibleSpace();

			if(GUILayout.Button("Collapse Vertexes"))
				DoAction();
		}

		public override ActionResult DoAction()
		{
			return MenuCommands.MenuCollapseVertexes(MeshSelection.TopInternal());
		}
	}
}

