using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BSExample_SceneManager : MonoBehaviour 
{

	public Camera startCamera;
	public Camera cubesCamera;
	public Camera wallsCamera;
	public Camera robotCamera;
	public Camera wizardCamera;


	public Material[] materials;

	public Color storedAmbient;

	public Light robotLight;
	public Light wizardLight;

	private Dictionary<Material, Texture> materialMaps;


	private bool m_bsEnabled = true;


	//ui images
	public Texture greyBar;

	public GUIStyle dungeonButtonStyle;
	public GUIStyle primitivesButtonStyle;
	public GUIStyle robotButtonStyle;
	public GUIStyle snailButtonStyle;
	public GUIStyle wizardButtonStyle;

	public GUIStyle brightSpriteOnStyle;
	public GUIStyle brightSpriteOffStyle;

	// Use this for initialization
	void Start () 
	{
		materialMaps = new Dictionary<Material, Texture>();

		foreach (Material m in materials)
		{

			materialMaps.Add(m, m.GetTexture("_BumpMap"));

		}

		storedAmbient = RenderSettings.ambientLight;

	}

	void OnGUI()
	{
		int screenButtonSize = Screen.width / 5;

		int screenButtonHeight = Mathf.RoundToInt( 72.0f * (screenButtonSize / 200.0f) );

		if (GUI.Button(new Rect(0,0,screenButtonSize,screenButtonHeight), "", snailButtonStyle))
		{
			startCamera.enabled = true;
			cubesCamera.enabled = false;
			wallsCamera.enabled = false;
			robotCamera.enabled = false;
			wizardCamera.enabled = false;

			RenderSettings.ambientLight = storedAmbient;
		}

		if (GUI.Button(new Rect(screenButtonSize,0,screenButtonSize,screenButtonHeight), "", primitivesButtonStyle))
		{
			startCamera.enabled = false;
			cubesCamera.enabled = true;
			wallsCamera.enabled = false;
			robotCamera.enabled = false;
			wizardCamera.enabled = false;

			RenderSettings.ambientLight = storedAmbient;
		}
		
		if (GUI.Button(new Rect(screenButtonSize*2,0,screenButtonSize,screenButtonHeight), "", dungeonButtonStyle))
		{
			startCamera.enabled = false;
			cubesCamera.enabled = false;
			wallsCamera.enabled = true;
			robotCamera.enabled = false;
			wizardCamera.enabled = false;

			RenderSettings.ambientLight = Color.black;
			//RenderSettings.ambientLight = new Color(0.33f,0.33f,0.33f);
		}
		if (GUI.Button(new Rect(screenButtonSize*3,0,screenButtonSize,screenButtonHeight), "", robotButtonStyle))
		{
			startCamera.enabled = false;
			cubesCamera.enabled = false;
			wallsCamera.enabled = false;
			robotCamera.enabled = true;
			wizardCamera.enabled = false;

			robotLight.transform.position = new Vector3(16.5f, -1.25f, -198.2864f);

			RenderSettings.ambientLight = storedAmbient;
		}
		if (GUI.Button(new Rect(screenButtonSize*4,0,screenButtonSize,screenButtonHeight), "", wizardButtonStyle))
		{
			startCamera.enabled = false;
			cubesCamera.enabled = false;
			wallsCamera.enabled = false;
			robotCamera.enabled = false;
			wizardCamera.enabled = true;

			wizardLight.transform.localPosition = new Vector3(-2.35f, 1.3f, -4.04f);

			RenderSettings.ambientLight = storedAmbient;
		}
		
		GUI.DrawTexture(new Rect(0,Screen.height - 4,Screen.width,4), greyBar);
		
		GUI.DrawTexture(new Rect(0,screenButtonHeight,4,Screen.height), greyBar);
		GUI.DrawTexture(new Rect(Screen.width - 4,screenButtonHeight,4,Screen.height), greyBar);
		if (GUI.Button(new Rect(Screen.width - 76,Screen.height - 108,76,108), "", m_bsEnabled ? brightSpriteOnStyle : brightSpriteOffStyle))
		{
			foreach (Material m in materials)
			{
				if (m.GetTexture("_BumpMap") != null)
				{
					m.SetTexture("_BumpMap", null);
					m_bsEnabled = false;
					
					robotLight.transform.position = new Vector3(16.5f, -1.25f, -198.2864f);
					wizardLight.transform.localPosition = new Vector3(-2.35f, 1.3f, -4.04f);
				}
				else 
				{
					m.SetTexture("_BumpMap", materialMaps[m]);
					m_bsEnabled = true;
					
					robotLight.transform.position = new Vector3(16.5f, -1.25f, -198.2864f);
					wizardLight.transform.localPosition = new Vector3(-2.35f, 1.3f, -4.04f);
				}
			}
		}


		if (robotCamera.enabled)
		{
			GUI.Label(new Rect(60, 150, 200, 50), "Click and drag to light the robot!");
		}
		if (wizardCamera.enabled)
		{
			GUI.Label(new Rect(60, 150, 200, 50), "Click and drag to light the wizard!");
		}

	}

	void Update()
	{
		if (robotCamera.enabled || wizardCamera.enabled)
		{
			if (Input.GetMouseButton(0))
			{
				Ray r = robotCamera.ScreenPointToRay(Input.mousePosition);
				Plane p = new Plane(Vector3.forward, robotLight.transform.position);

				float dist = -1;

				if (p.Raycast(r, out dist))
				{
					robotLight.transform.position = r.GetPoint(dist);
				}

				Ray r2 = wizardCamera.ScreenPointToRay(Input.mousePosition);
				Plane p2 = new Plane(Vector3.forward, wizardLight.transform.position);

				float dist2 = -1;

				if (p2.Raycast(r2, out dist2))
				{
					wizardLight.transform.position = r2.GetPoint(dist2);
				}
			}
		}

	}

}