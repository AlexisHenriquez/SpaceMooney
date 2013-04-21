using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using Hud = Fury.Hud;

public class SimpleHUD : MonoBehaviour
{
	/// <summary>
	/// This interface can be attached to abilities that affect an area.
	/// </summary>
	public interface IAbilityInfo
	{
		Single Radius { get; }
		Boolean SnapToGrid { get; }
		String Description { get; }
	}

	public static SimpleHUD Instance;

	public Bounds CameraConstraint = new Bounds(Vector3.zero, Vector3.one * Single.MaxValue);
	public Texture2D CursorFriendly, CursorHostile, CursorNeutral, TargetFriendly, TargetHostile, TargetNeutral;
	public Texture2D IconFrame;
	public Texture2D[] Numbers;
	public GameObject FollowObject;
	public Material MaterialSelect, MaterialHover;
	public Single MaxDistanceFromGround = 12, MinDistanceFromGround = 7, DesiredDistanceFromGround = 10;
	public LayerMask UnitMask, IgnoreMask;

	public GameObject TargetPrefab, RingPrefab;
	private GameObject HoverObject;
	private GameObject MouseTargetObject;

	private Texture2D Green, Green25, Black50, Black75, Blue, Purple;

	private Dictionary<Fury.Database.Ability, Single> BufferAbilities;
	private List<Fury.Behaviors.Unit> BufferFollowers;
	public Fury.Controllers.SelectionController Selection;

	private GameObject MouseCursor, MouseDragBox;

	private Fury.Behaviors.Commander Owner;

	private List<GUITexture> Textures;
	private List<GUIText> Texts;

	private Int32 TexturePosition, TextPosition;

	private void Awake()
	{
		Instance = this;
	}

	private void Initialize()
	{
		foreach (var cmdr in Fury.Behaviors.Manager.Instance.Commanders)
			if (cmdr.IsLocal)
			{
				Owner = cmdr;
				Textures = new List<GUITexture>();
				Texts = new List<GUIText>();

				BufferFollowers = new List<Fury.Behaviors.Unit>();
				BufferAbilities = new Dictionary<Fury.Database.Ability, Single>();

				Selection = new Fury.Controllers.SelectionController();
				Selection.OnUnitDeselected += new Action<Fury.Behaviors.Unit>(OnFollowerDeselected);
				Selection.OnUnitSelected += new Action<Fury.Behaviors.Unit>(OnFollowerSelected);
				Hud.OnDragComplete += new Fury.General.Action<Rect>(OnDragComplete);

				Hud.UnitMask = UnitMask;

				Green = new Texture2D(1, 1);
				Green.SetPixel(0, 0, new Color(0, 0.5f, 0));
				Green.Apply();

				Green25 = new Texture2D(1, 1);
				Green25.SetPixel(0, 0, new Color(0f, 0.5f, 0f, 0.25f));
				Green25.Apply();

				Black50 = new Texture2D(1, 1);
				Black50.SetPixel(0, 0, new Color(0f, 0f, 0f, 0.5f));
				Black50.Apply();

				Black75 = new Texture2D(1, 1);
				Black75.SetPixel(0, 0, new Color(0f, 0f, 0f, 0.75f));
				Black75.Apply();

				Blue = new Texture2D(1, 1);
				Blue.SetPixel(0, 0, new Color(.25f, .25f, 1f));
				Blue.Apply();

				Purple = new Texture2D(1, 1);
				Purple.SetPixel(0, 0, new Color(1f, .25f, 1f));
				Purple.Apply();

				Fury.Behaviors.Manager.Instance.OnUnitDead += OnFollowerDead;

				MouseDragBox = new GameObject();
				MouseDragBox.AddComponent<GUITexture>();
				MouseDragBox.transform.localPosition = new Vector3(0.5f, 0.5f, 0);
				MouseDragBox.transform.localScale = new Vector3(0, 0, 1);
				MouseDragBox.transform.localRotation = Quaternion.identity;
				MouseDragBox.name = "!DragBox";
				MouseDragBox.SetActive(false);// = false;

				MouseCursor = new GameObject();
				MouseCursor.AddComponent<GUITexture>();
				MouseCursor.transform.localPosition = new Vector3(0.5f, 0.5f, 0);
				MouseCursor.transform.localScale = new Vector3(0, 0, 1);
				MouseCursor.transform.localRotation = Quaternion.identity;
				MouseCursor.name = "!Cursor";

				MouseTargetObject = (GameObject)GameObject.Instantiate(TargetPrefab);
				MouseTargetObject.name = "!MouseTarget";

				break;
			}
	}

	private void Update()
	{
		if (Fury.Behaviors.Manager.Instance.GameState == Fury.GameStates.Playing && Owner == null)
			Initialize();

		if (Owner == null) return;

		// Change the zoom level of the camera
		DesiredDistanceFromGround = Mathf.Clamp(DesiredDistanceFromGround + Math.Sign(Input.GetAxis("Mouse ScrollWheel")) * -1f, 
			MinDistanceFromGround, MaxDistanceFromGround);

		// Move the camera, depending on mode
		if (FollowObject != null)
		{
			Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position,
			FollowObject.transform.position - Camera.main.transform.forward * DesiredDistanceFromGround, 0.1f);
		}
		else
		{
			var panSpeed = 0.5f;

			var oldPosition = Camera.main.transform.position;

			// Allow the keys to pan the camera around
			if (Hud.SelectionBox == null)
			{
				if (Input.GetKey(KeyCode.W)) Hud.PanCamera(panSpeed, 0f);
				if (Input.GetKey(KeyCode.S)) Hud.PanCamera(-panSpeed, 0f);
				if (Input.GetKey(KeyCode.D)) Hud.PanCamera(0f, panSpeed);
				if (Input.GetKey(KeyCode.A)) Hud.PanCamera(0f, -panSpeed);

				// Pan the camera around if the mouse is at the edges
				if (Screen.lockCursor)
				{
					if (Hud.Position.x < 10) Hud.PanCamera(0f, -panSpeed);
					if (Hud.Position.x > Screen.width - 10) Hud.PanCamera(0f, panSpeed);
					if (Hud.Position.y < 10) Hud.PanCamera(-panSpeed, 0f);
					if (Hud.Position.y > Screen.height - 10) Hud.PanCamera(panSpeed, 0f);
				}
			}

			var focal = Camera.main.transform.position + Camera.main.transform.forward * DesiredDistanceFromGround;
			var ray = new Ray(focal + new Vector3(0, 100, 0), Vector3.down);
			RaycastHit hitInfo;
			var ignoreMask = 0x00000004;
			if (Physics.Raycast(ray, out hitInfo, Single.MaxValue, ~(UnitMask | ignoreMask)))
			{
				Camera.main.transform.position = hitInfo.point - Camera.main.transform.forward * DesiredDistanceFromGround;
			}

			var newPos = Camera.main.transform.position;
			newPos.y = Mathf.Lerp(oldPosition.y, newPos.y, 0.1f);
			newPos.x = Mathf.Clamp(newPos.x, CameraConstraint.min.x, CameraConstraint.max.x);
			newPos.y = Mathf.Clamp(newPos.y, CameraConstraint.min.y, CameraConstraint.max.y);
			newPos.z = Mathf.Clamp(newPos.z, CameraConstraint.min.z, CameraConstraint.max.z);

			Camera.main.transform.position = newPos;
		}
	}

	private void LateUpdate()
	{
		if (Owner == null) return;

		if (Hud.TriggerLMB || Hud.TriggerRMB) Screen.lockCursor = true;

		Selection.Prune();

		var ray = Camera.main.ScreenPointToRay(Hud.Position);
		Hud.CalculateFocus(ray);

		if (HoverObject == null)
			HoverObject = (GameObject)GameObject.Instantiate(RingPrefab);

		// Create the hover ring over our mouse target
		HoverObject.renderer.enabled = false;
		if (Hud.FocusTarget != null)
			if (Hud.FocusTarget is Fury.Behaviors.Targetable)
				PlaceRing(HoverObject, (Fury.Behaviors.Targetable)Hud.FocusTarget);
			else
				PlaceRing(HoverObject, Hud.FocusTarget.Radius, Color.yellow, Hud.FocusTarget.transform.position);

		// Place rings on the ground for area of effect abilities
		if (Hud.QueuedAbility != null && Hud.QueuedAbility is IAbilityInfo)
		{
			var ability = Hud.QueuedAbility as IAbilityInfo;
			if (ability.Radius > 0)
			{
				var rounded = new Vector3(Mathf.Round(Hud.FocusPoint.x), Hud.FocusPoint.y, Mathf.Round(Hud.FocusPoint.z));
				var abilityTargetPoint = ability.SnapToGrid ? rounded : Hud.FocusPoint;
				var closestCaster = Hud.QueueGetCaster(abilityTargetPoint);

				if (closestCaster != null)
				{
					var canBuild = Hud.QueuedAbility.OnCheckUseOnTarget(closestCaster, null, abilityTargetPoint);
					PlaceRing(HoverObject, ability.Radius, canBuild ? Color.yellow : Color.red, rounded);
				}
			}
		}

		// Commander left clicks his mouse
		if (Hud.TriggerLMB)
		{
			if (Hud.QueuedAbility != null)
			{
				// Cast the ability since there is one queued
				var caster = Hud.QueueGetCaster(Hud.FocusPoint);
				if (caster != null && Hud.QueuedAbility.OnCheckUseOnTarget(caster, Hud.FocusTarget, Hud.FocusPoint))
				{
					caster.Order(Hud.QueuedAbility, Hud.FocusTarget, Hud.FocusPoint);
					Hud.QueueAbility(null, null);
				}
				Hud.ConsumeLMB();
			}
			else if (Hud.FocusTarget != null && Hud.FocusTarget is Fury.Behaviors.Unit)
			{
				// Modify the selection since a unit was clicked
				if (!Hud.Shift) Selection.Clear();
				Selection.Add(Hud.FocusTarget as Fury.Behaviors.Unit);
				Hud.ConsumeLMB();
			} 
		}

		// Commander right clicks his mouse
		if (Hud.TriggerRMB)
		{
			if (Hud.QueuedAbility != null)
			{
				// Clear the queued ability since one is queued
				Hud.QueueAbility(null, null);
			}
			else if (Hud.FocusTarget != null)
			{

				// Order units to a target since user clicked on a unit
				foreach (var follower in Selection)
					if (follower.Owner == Owner)
						follower.Order(Hud.FocusTarget);
			}
			else
			{
				if (Owner.OrderGroupToPosition(Selection.Units, Hud.FocusPoint))
				{
					MouseTargetObject.transform.position = Hud.FocusPoint;
					MouseTargetObject.renderer.material.color = new Color(0f, 0.5f, 0f);
					MouseTargetObject.renderer.material.mainTextureOffset = new Vector2(1f, 0);
					MouseTargetObject.SetActive(true); //.active = true;
				}
			}

			Hud.ConsumeRMB();
		}

		BufferAbilities.Clear();
		foreach (var kvp in Selection)
		{
			// Generate a distinct list of abilities that our units can use
			if (kvp.Owner == Owner)
				foreach (var ability in kvp.Properties.Abilities)
					if (ability.OnCheckUse(kvp))
					{
						var cooldown = ability.CastCooldown == 0 ? 1 :
							Mathf.Clamp01(kvp.Controllers.AbilityController.Cooldowns[ability] / ability.CastCooldown);

						if (!BufferAbilities.ContainsKey(ability))
							BufferAbilities.Add(ability, cooldown);
						else if (BufferAbilities[ability] > cooldown)
							BufferAbilities[ability] = cooldown;
					}


			// Calculate the top of the unit in world space
			var center = kvp.collider.bounds.center;
			var extents = kvp.collider.bounds.extents;

			var top0 = center + new Vector3(+extents.x, extents.y, +extents.z);
			var top1 = center + new Vector3(-extents.x, extents.y, -extents.z);

			var ssTop0 = Camera.mainCamera.WorldToScreenPoint(top0);
			var ssTop1 = Camera.mainCamera.WorldToScreenPoint(top1);
			var barWidth = Math.Max(32, Math.Min(128, Math.Abs(ssTop0.x - ssTop1.x)));

			var ssTop = new Vector2((ssTop0.x + ssTop1.x) * 0.5f, Mathf.Max(ssTop0.y, ssTop1.y));

			// Draw the unit's health bar
			var rect = new Rect(ssTop.x - barWidth * 0.5f, Screen.height - ssTop.y - 10, barWidth, 8);

			var abiCtrl = kvp.Controllers.AbilityController;
			var vitCtrl = kvp.Controllers.VitalityController;

			if (abiCtrl != null && abiCtrl.TargetAbility != null && abiCtrl.TargetAbility.CastTime > 0)
				DrawBar(Black50, Blue, rect.x, rect.y, rect.width, rect.height, (Single)abiCtrl.CastTimeRemaining / abiCtrl.TargetAbility.CastTime);
			else
				DrawBar(Black50, Green, rect.x, rect.y, rect.width, rect.height, (Single)vitCtrl.Health / vitCtrl.MaxHealth);
			
			// Draw all the unit's energy bars
			var i = 2;
			foreach (var generator in kvp.Controllers.VitalityController.Generators.Values)
			{
				rect = new Rect(ssTop.x - barWidth * 0.5f, Screen.height - ssTop.y - 10 * i, barWidth, 8);
				DrawBar(Black50, Purple, rect.x, rect.y, rect.width, rect.height, generator.AmountPercentage);
				i++;
			}
		}

		var invertedMousePos = new Vector2(Hud.Position.x, Screen.height - Hud.Position.y);
		var iconSize = 48;

		// Draw all the abilities that our units can use
		var a = -1;
		foreach (var kvp0 in BufferAbilities)
		{
			a++;
			var ability = kvp0.Key;

			// Check if the user wants to use the ability
			var abilityRect = new Rect(a * (iconSize + 7) + 30, Screen.height - 30 - iconSize, iconSize, iconSize);
			var isMouseOver = abilityRect.Contains(invertedMousePos);
			var isQueued = (ability == Hud.QueuedAbility);

			// Draw the icon
			DrawTexture(ability.Icon, abilityRect, Color.gray);

			// Draw the fade if spell is not ready
			var overlayRect = abilityRect;
			overlayRect.y += overlayRect.height * (1 - kvp0.Value);
			overlayRect.height *= kvp0.Value;
			DrawTexture(Black75, overlayRect, new Color(0.5f, 0.5f, 0.5f, 0.5f));

			// Draw the frame
			abilityRect = new Rect(abilityRect.x - 2, abilityRect.y - 2, abilityRect.width + 4, abilityRect.height + 4);
			DrawTexture(IconFrame, abilityRect,
				isQueued ? new Color(0.5f, 0f, 0f) : new Color(0.15f, 0.15f, 0.15f));

			// Draw the hotkey
			if (a < 9)
				DrawTexture(Numbers[a + 1], new Rect(abilityRect.xMax - 16, abilityRect.yMax - 16, 16, 16), Color.gray);

			// Check if the user clicked the ability button or pressed the button			
			if ((Hud.TriggerLMB) && isMouseOver || Input.GetKeyDown((KeyCode)(a + 49)))
			{
				// Find all followers in the selection that can use the ability
				BufferFollowers.Clear();
				foreach (var kvp in Selection)
					if (kvp.Owner == Owner)
						if (kvp.Properties.Abilities.Contains(ability))
							BufferFollowers.Add(kvp);

				// Use the HUD helper to queue up the ability
				Hud.QueueAbility(ability, BufferFollowers);

				Hud.ConsumeLMB();
			}
		}

		// Animate the mouse target texture
		if (MouseTargetObject.activeSelf)
		{
			var texOffset = MouseTargetObject.renderer.material.mainTextureOffset + new Vector2(-Time.deltaTime * 2, 0);
			if (texOffset.x < 0) MouseTargetObject.SetActive(false); //.active = false;

			MouseTargetObject.renderer.material.mainTextureOffset = texOffset;
		}

		GUITexture guiTexture;

		// Place the selection box if applicable
		if (Hud.SelectionBox != null)
		{
			MouseDragBox.SetActive(true); // active = true;

			guiTexture = MouseDragBox.GetComponent<GUITexture>();
			var box = Fury.Hud.SelectionBox.Value;
			guiTexture.pixelInset = new Rect(box.xMin - Screen.width * 0.5f, box.yMin - Screen.height * 0.5f, box.width, box.height);
			guiTexture.texture = Green25;
		}
		else
		{
			MouseDragBox.SetActive(false); // active = false;
		}

		guiTexture = MouseCursor.GetComponent<GUITexture>();

		// Place the mouse cursor where it belongs
		guiTexture.transform.localPosition = new Vector3(0.5f, 0.5f, 10000);
		if (Hud.QueuedAbility != null)
			guiTexture.pixelInset = new Rect(Fury.Hud.Position.x - Screen.width / 2 - 16, Fury.Hud.Position.y - Screen.height / 2 - 16, 32, 32);
		else
			guiTexture.pixelInset = new Rect(Fury.Hud.Position.x - Screen.width / 2, Fury.Hud.Position.y - Screen.height / 2 - 32, 32, 32);

		if (Hud.FocusTarget == null)
			guiTexture.texture = Hud.QueuedAbility != null ? TargetNeutral : CursorNeutral;
		else if (Hud.FocusTarget.IsTeamOrNeutral(Owner))
			guiTexture.texture = Hud.QueuedAbility != null ? TargetFriendly : CursorFriendly;
		else
			guiTexture.texture = Hud.QueuedAbility != null ? TargetHostile : CursorHostile;

		// End the last call
		for (Int32 i = TexturePosition; i < Textures.Count; i++)
			Textures[i].enabled = false;

		for (Int32 i = TextPosition; i < Texts.Count; i++)
			Texts[i].enabled = false;

		// Begin the next call
		TexturePosition = 0;
		TextPosition = 0;

		transform.localPosition = Vector3.zero;
		transform.localScale = Vector3.one;
		transform.localRotation = Quaternion.identity;
	}

	private GUITexture GetTexture()
	{
		if (TexturePosition >= Textures.Count)
		{
			var go = new GameObject();
			go.transform.parent = transform;
			go.transform.localPosition = new Vector3(0.5f, 0.5f, 0);
			go.transform.localScale = new Vector3(0, 0, 1);
			go.transform.localRotation = Quaternion.identity;

			Textures.Add(go.AddComponent<GUITexture>());
		}

		var g = Textures[TexturePosition];
		g.enabled = true;
		g.transform.localPosition = new Vector3(0.5f, 0.5f, TexturePosition);

		TexturePosition++;
		return g;
	}

	private GUIText GetText()
	{
		if (TextPosition >= Texts.Count)
		{
			var go = new GameObject();
			go.transform.parent = transform;
			go.transform.localPosition = new Vector3(0.5f, 0.5f, 0);
			go.transform.localScale = new Vector3(0, 0, 1);
			go.transform.localRotation = Quaternion.identity;

			Texts.Add(go.AddComponent<GUIText>());
		}

		var g = Texts[TextPosition];
		g.enabled = true;
		g.transform.localPosition = new Vector3(0.5f, 0.5f, TextPosition + TexturePosition);

		TextPosition++;
		return g;
	}

	public void DrawTexture(Texture2D tex, Rect r, Color col)
	{
		if (tex == null) return;

		var guiTexture = GetTexture();
		guiTexture.pixelInset = new Rect(r.x - Screen.width * 0.5f, Screen.height * 0.5f - r.height - r.y, r.width, r.height);
		guiTexture.texture = tex;
		guiTexture.color = col;
	}

	public void DrawText(String str, Font font, Single offsetX, Single offsetY, TextAnchor anchor)
	{
		if (str == null) return;

		var guiText = GetText();
		guiText.text = str;
		guiText.anchor = anchor;
		guiText.alignment = TextAlignment.Center;
		guiText.font = font;
		guiText.pixelOffset = new Vector2(offsetX - Screen.width * 0.5f, Screen.height * 0.5f - offsetY);
	}

	public void DrawBar(Texture2D border, Texture2D fill, Single x, Single y, Single w, Single h, Single p)
	{
		Rect bgRect = new Rect(x, y, w, h);
		Rect barRect = new Rect(x + 1, y + 1, (w - 2) * p, h - 2);

		var col = new Color(0.5f, 0.5f, 0.5f, 0.5f);

		if (border != null) DrawTexture(border, bgRect, col);

		DrawTexture(fill, barRect, col);
	}

	private void PlaceRing(GameObject ring, Single radius, Color col, Vector3 position)
	{
		ring.renderer.enabled = true;
		ring.transform.parent = null;
		ring.renderer.material.color = col;
		ring.transform.localRotation = Quaternion.identity;
		ring.transform.localScale = Vector3.one * radius * .2f;
		ring.transform.localPosition = position + Vector3.up * 0.1f;
	}

	private void PlaceRing(GameObject ring, Fury.Behaviors.Targetable follower)
	{
		ring.renderer.enabled = true;
		ring.transform.parent = follower.transform;
		ring.renderer.material.color = follower.IsTeamOrNeutral(Owner) ? new Color(0f, 0.5f, 0f) : new Color(1f, 0f, 0f);
		ring.transform.localPosition = Vector3.up * 0.01f;
		ring.transform.localRotation = Quaternion.AngleAxis(Time.timeSinceLevelLoad * 30, Vector3.up);
		ring.transform.localScale = new Vector3(1f / follower.transform.localScale.x,
			1f / follower.transform.localScale.y,
			1f / follower.transform.localScale.z) * follower.Radius * .2f;
	}

	private void OnDragComplete(Rect rect)
	{
		if (Hud.QueuedAbility != null) return;

		Hud.CalculateUnitsInRect(rect, BufferFollowers);

		if (!Hud.Shift) Selection.Clear();

		foreach (var f in BufferFollowers)
			if (f.Owner == Owner)
				Selection.Add(f);
	}

	private void OnFollowerDead(Fury.Behaviors.Unit target, Fury.Behaviors.Unit killer)
	{
		Selection.Remove(target);
	}

	private void OnFollowerSelected(Fury.Behaviors.Unit obj)
	{
		GameObject ring = (GameObject)GameObject.Instantiate(RingPrefab);
		ring.renderer.material = MaterialSelect;
		ring.name = "Selection Ring";
		PlaceRing(ring, obj);
	}

	private void OnFollowerDeselected(Fury.Behaviors.Unit obj)
	{
		var ring = obj.transform.FindChild("Selection Ring");
		if (ring != null) GameObject.Destroy(ring.gameObject);
	}
}