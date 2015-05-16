using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using BriteSprite;

	public class BriteSpriteWindow : EditorWindow 
	{
		[MenuItem ("Window/BriteSprite")]
		static void Init()
		{
			BriteSpriteWindow window = (BriteSpriteWindow)EditorWindow.GetWindow(typeof(BriteSpriteWindow));	

			window.minSize = new Vector2(350,800);
		}

		public enum ArtStyle
		{
			PIXEL_ART = 0,
			HIGH_RES = 1
		}

		private ArtStyle artStyle;

		private BriteSpriteCore.AlgorithmStyle algoStyle;

		private Texture2D m_leftLitTexture;
		private Texture2D m_rightLitTexture;
		private Texture2D m_topLitTexture;
		private Texture2D m_bottomLitTexture;
		private Texture2D m_frontLitTexture;

		private Texture2D m_outputNormalMap;

		private bool m_optionalTexturesVisible = true;
		private bool m_optionsVisible = true;

		void OnGUI()
		{
			GUILayout.Label("Required Lightmaps:");

			EditorGUI.indentLevel++;
			m_rightLitTexture = (Texture2D)EditorGUILayout.ObjectField("Right Lit", m_rightLitTexture, typeof(Texture2D), false, GUILayout.MaxHeight(250));
			m_topLitTexture = (Texture2D)EditorGUILayout.ObjectField("Top Lit", m_topLitTexture, typeof(Texture2D), false, GUILayout.MaxHeight(250));
			EditorGUI.indentLevel--;

			DrawSplitter();

			m_optionalTexturesVisible = EditorGUILayout.Foldout(m_optionalTexturesVisible, "Optional Lightmaps");

			if (m_optionalTexturesVisible)
			{
				EditorGUI.indentLevel++;
				m_leftLitTexture = (Texture2D)EditorGUILayout.ObjectField("Left Lit", m_leftLitTexture, typeof(Texture2D), false, GUILayout.MaxHeight(250));
				m_bottomLitTexture = (Texture2D)EditorGUILayout.ObjectField("Bottom Lit", m_bottomLitTexture, typeof(Texture2D), false, GUILayout.MaxHeight(250));
				m_frontLitTexture = (Texture2D)EditorGUILayout.ObjectField("Front Lit", m_frontLitTexture, typeof(Texture2D), false, GUILayout.MaxHeight(250));
				EditorGUI.indentLevel--;
			}

			if (m_leftLitTexture != null || m_rightLitTexture != null || m_frontLitTexture != null || m_topLitTexture != null || m_bottomLitTexture != null)
			{
				EditorGUILayout.BeginHorizontal();
				{
				
#if !UNITY_2_6 && !UNITY_2_6_1 && !UNITY_3_0 && !UNITY_3_1 && !UNITY_3_2 && !UNITY_3_3 && !UNITY_3_4
					EditorGUILayout.HelpBox("Autofill attempts to populate inputs using a simple naming convention. Left-lit lightmaps should end in \"_left\", bottom-lit ones should end in \"_bottom\", etc. For more information, see the documentation.", MessageType.Info);
					
					if (GUILayout.Button("Autofill", GUILayout.MaxWidth(200), GUILayout.ExpandHeight(true)))
					{
						if (m_leftLitTexture != null)
						{
							AttemptToAutofillTextures(m_leftLitTexture);
						}
						else if (m_topLitTexture != null) 
						{
							AttemptToAutofillTextures(m_topLitTexture);
						}
						else if (m_rightLitTexture != null) 
						{
							AttemptToAutofillTextures(m_rightLitTexture);
						}
						else if (m_bottomLitTexture != null) 
						{
							AttemptToAutofillTextures(m_bottomLitTexture);
						}
						else if (m_frontLitTexture != null) 
						{
							AttemptToAutofillTextures(m_frontLitTexture);
						}
					}
#else
					if (GUILayout.Button("Autofill"))
					{
						if (m_leftLitTexture != null)
						{
							AttemptToAutofillTextures(m_leftLitTexture);
						}
						else if (m_topLitTexture != null) 
						{
							AttemptToAutofillTextures(m_topLitTexture);
						}
						else if (m_rightLitTexture != null) 
						{
							AttemptToAutofillTextures(m_rightLitTexture);
						}
						else if (m_bottomLitTexture != null) 
						{
							AttemptToAutofillTextures(m_bottomLitTexture);
						}
						else if (m_frontLitTexture != null) 
						{
							AttemptToAutofillTextures(m_frontLitTexture);
						}
					}
#endif
				}
				EditorGUILayout.EndHorizontal();
			}
			DrawSplitter();

			m_optionsVisible = EditorGUILayout.Foldout(m_optionsVisible, "Options");
			if (m_optionsVisible)
			{
				EditorGUI.indentLevel++;
				artStyle = (ArtStyle)EditorGUILayout.EnumPopup(new GUIContent("Art Style (Interpolation)", "The fidelity of the art being processed (pixel art or painted art)."), artStyle);
				algoStyle = (BriteSpriteCore.AlgorithmStyle)EditorGUILayout.EnumPopup(new GUIContent("Algorithm", "Alters the way in which the normals are calculated."), algoStyle);
				EditorGUI.indentLevel--;
			}

			DrawSplitter();

			EditorGUILayout.BeginHorizontal();
			{
				if (GUILayout.Button("Process", GUILayout.Height(50)))
			    {
					if (PreprocessInputTextures())
					{
						m_outputNormalMap = BriteSpriteCore.ExtractNormalMap(m_leftLitTexture,
						                                                      m_rightLitTexture,
						                                                      m_topLitTexture,
						                                                      m_bottomLitTexture,
						                                                      m_frontLitTexture,
						                                                     algoStyle);

						if (m_outputNormalMap != null)
						{
							string firstPart = m_topLitTexture.name;

							if (firstPart.EndsWith("_top"))
							{
								firstPart = firstPart.Substring(0, firstPart.Length - 4);
							}

							string filepath = firstPart + "_normal";

							string assetPathBase = AssetDatabase.GetAssetPath(m_topLitTexture);
							string assetFullpath = Path.Combine(Application.dataPath, assetPathBase.Substring(@"Assets/".Length));

							string fullExportPath = EditorUtility.SaveFilePanel("Save Normal Map", Path.GetDirectoryName(assetFullpath), filepath, "png");

							if (fullExportPath.Length == 0)
							{
								m_outputNormalMap = null;
							}
							else 
							{
								ExportTextureToFile(m_outputNormalMap, fullExportPath);
							}
						}
					}
					else 
					{
						EditorUtility.DisplayDialog("BriteSprite has encountered an error!", "All input textures must be the same size.", "OK");
					}
				}

				if (GUILayout.Button("Batch Process Folder", GUILayout.Height(50)))
				{
					BatchProcessFolder();
				}
			}
			EditorGUILayout.EndHorizontal();
			
		}

		private string FullpathToLocalpath(string fullpath)
		{
			string modFullpath = fullpath.Substring(Application.dataPath.Length);
			string relativeFullpath = @"Assets"+ modFullpath;

			return relativeFullpath;
		}

		private void AttemptToAutofillTextures(Texture2D loadedAsset)
		{
			string localpath = AssetDatabase.GetAssetPath(loadedAsset);

			string fullpath = Path.Combine(Application.dataPath, localpath.Substring((@"Assets/").Length));

			string filepath = Path.GetFileNameWithoutExtension(fullpath);

			string dirpath = Path.GetDirectoryName(fullpath);

			string[] filesInDirectory = Directory.GetFiles(dirpath);

			InputTextures autofill = new InputTextures();

			string[] inputTokens = filepath.Split('_');

			if (inputTokens.Length <= 1) return;

			string baseName = filepath.Substring(0,filepath.LastIndexOf(inputTokens[inputTokens.Length-1]));

			autofill.baseAssetName = baseName;

			foreach (string testFullpath in filesInDirectory)
			{
				string testFilepath = Path.GetFileNameWithoutExtension(testFullpath);
				string testRelativeFullpath = FullpathToLocalpath(testFullpath);

				Texture2D textureAsset = AssetDatabase.LoadAssetAtPath(testRelativeFullpath, typeof(Texture2D)) as Texture2D;

				if (textureAsset == null) continue;

				string[] testTokens = testFilepath.Split('_');

				if (testTokens.Length <= 1) continue;

				string lastToken = testTokens[testTokens.Length-1];
				string testBaseName = testFilepath.Substring(0, testFilepath.LastIndexOf(lastToken));

				if (autofill.baseAssetName == testBaseName)
				{
					switch (lastToken)
					{
					case "left":
						autofill.leftLit = textureAsset;
						break;
					case "right":
						autofill.rightLit = textureAsset;
						break;
					case "top":
						autofill.topLit = textureAsset;
						break;
					case "bottom":
						autofill.bottomLit = textureAsset;
						break;
					case "front":
						autofill.frontLit = textureAsset;
						break;
					default:
						continue;
					}
				}
			}

			m_leftLitTexture = autofill.leftLit;
			m_rightLitTexture = autofill.rightLit;
			m_topLitTexture = autofill.topLit;
			m_bottomLitTexture = autofill.bottomLit;
			m_frontLitTexture = autofill.frontLit;
		}

		private void BatchProcessFolder()
		{
			string dirpath = EditorUtility.OpenFolderPanel("Select folder to batch process...", Application.dataPath, "");

			if (dirpath == "") return;

			string[] filepaths = Directory.GetFiles(dirpath);

			if (filepaths == null || filepaths.Length == 0) return;

			//step one: process filepaths into relevant chunks
			List<InputTextures> allToProcess = new List<InputTextures>();

			foreach (string fullpath in filepaths)
			{
				string filepath = Path.GetFileNameWithoutExtension(fullpath);

				string relativeFullpath = FullpathToLocalpath(fullpath);

				Texture2D textureAsset = AssetDatabase.LoadAssetAtPath(relativeFullpath, typeof(Texture2D)) as Texture2D;

				if (textureAsset == null) continue;

				string[] tokens = filepath.Split('_');

				if (tokens.Length <= 1) continue; //not one of our textures

				string lastToken = tokens[tokens.Length - 1];

				string baseName = filepath.Substring(0, filepath.LastIndexOf(lastToken));

				InputTextures textureCollection = null;

				foreach (InputTextures extant in allToProcess)
				{
					if (extant.baseAssetName == baseName)
					{
						textureCollection = extant;
						break;
					}
				}

				if (textureCollection == null)
				{
					textureCollection = new InputTextures();
					textureCollection.baseAssetName = baseName;
				}

				switch (lastToken)
				{
				case "left":
					textureCollection.leftLit = textureAsset;
					break;
				case "right":
					textureCollection.rightLit = textureAsset;
					break;
				case "top":
					textureCollection.topLit = textureAsset;
					break;
				case "bottom":
					textureCollection.bottomLit = textureAsset;
					break;
				case "front":
					textureCollection.frontLit = textureAsset;
					break;
				default:
					continue;
				}

				if (!allToProcess.Contains(textureCollection))
				{
					allToProcess.Add(textureCollection);
				}
			}

			//phase two: process all of the TextureCollections

			foreach (InputTextures input in allToProcess)
			{
				m_outputNormalMap = null;

				m_leftLitTexture = input.leftLit;
				m_rightLitTexture = input.rightLit;
				m_topLitTexture = input.topLit;
				m_bottomLitTexture = input.bottomLit;
				m_frontLitTexture = input.frontLit;

				if (PreprocessInputTextures())
				{
					m_outputNormalMap = BriteSpriteCore.ExtractNormalMap(m_leftLitTexture,
					                                                      m_rightLitTexture,
					                                                      m_topLitTexture,
					                                                      m_bottomLitTexture,
					                                                      m_frontLitTexture,
					                                                     algoStyle);

					string outputPath = Path.Combine(dirpath, input.baseAssetName + "normal.png");

					if (m_outputNormalMap != null) ExportTextureToFile(m_outputNormalMap, outputPath);
				}
			}

		}

		private void ExportTextureToFile(Texture2D texToExport, string fullpath)
		{
			byte[] bytes = texToExport.EncodeToPNG();

			string localpath = FullpathToLocalpath(fullpath);

			File.WriteAllBytes(fullpath, bytes);

			AssetDatabase.Refresh();

			TextureImporter textureImporter = AssetImporter.GetAtPath(localpath) as TextureImporter;
			textureImporter.convertToNormalmap = false;

			switch (artStyle)
			{
			case ArtStyle.PIXEL_ART:
				textureImporter.filterMode = FilterMode.Point;
				break;
			case ArtStyle.HIGH_RES:
				textureImporter.filterMode = FilterMode.Bilinear;
				break;
			default:
				//we should never get here, but if we do, just leave filtering alone
				break;
			}

			textureImporter.textureType = TextureImporterType.Bump;
			textureImporter.normalmap = true;
			textureImporter.textureFormat = TextureImporterFormat.ARGB32;

			AssetDatabase.ImportAsset(localpath);
		}

		private bool PreprocessInputTextures()
		{
			List<Texture2D> inputTextures = new List<Texture2D>();

			if (m_leftLitTexture != null) inputTextures.Add(m_leftLitTexture);
			if (m_rightLitTexture != null) inputTextures.Add(m_rightLitTexture);
			if (m_topLitTexture != null) inputTextures.Add(m_topLitTexture);
			if (m_bottomLitTexture != null) inputTextures.Add(m_bottomLitTexture);
			if (m_frontLitTexture != null) inputTextures.Add(m_frontLitTexture);

			if (inputTextures.Count == 0) return false;

			int dimensionX = inputTextures[0].width;
			int dimensionY = inputTextures[0].height;

			foreach (Texture2D tex in inputTextures)
			{
				if (tex.width != dimensionX || tex.height != dimensionY)
				{
					return false;
				}

				TextureImporter texImporter = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(tex)) as TextureImporter;

				switch (artStyle)
				{
				case ArtStyle.PIXEL_ART:
					texImporter.filterMode = FilterMode.Point;
					break;
				case ArtStyle.HIGH_RES:
					texImporter.filterMode = FilterMode.Bilinear;
					break;
				default:
					//we should never get here, but if we do, just leave filtering alone
					break;
				}

				texImporter.textureType = TextureImporterType.Advanced;
				texImporter.textureFormat = TextureImporterFormat.ARGB32;
				texImporter.isReadable = true;

#if UNITY_4_2 || UNITY_4_3
				texImporter.alphaIsTransparency = true;
#endif
				AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(tex));
			}

			return true;
		}

		private void DrawSplitter()
		{
			GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(1));
		}
	}

	public class InputTextures
	{
		public string baseAssetName;

		public Texture2D rightLit;
		public Texture2D topLit;

		public Texture2D leftLit;
		public Texture2D bottomLit;
		public Texture2D frontLit;
	}	

