﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// SelectorTool provides a functionality to select parts of roboy on the mesh itself or through the GUI.
/// </summary>
[RequireComponent(typeof(LineRenderer))]
public class SelectorTool : ControllerTool
{

    /// <summary>
    /// LineRenderer to draw the laser for selection.
    /// </summary>
    private LineRenderer m_LineRenderer;

    /// <summary>
    /// Variable to track the last selected object for comparison.
    /// </summary>
    private SelectableObject m_LastSelectedObject;

    /// <summary>
    /// Maximum ray length for selection.
    /// </summary>
    [SerializeField]
    private float m_RayDistance = 7f;

    private bool m_Is_released = false;

    /// <summary>
    /// Initializes the lineRenderer component.
    /// </summary>
    void Start()
    {
        m_LineRenderer = GetComponent<LineRenderer>();
    }

    /// <summary>
    /// Starts a ray from the controller. If the ray hits a roboy part, it changes its selection status. Otherwise it resets the last selected/targeted roboy part.
    /// </summary>
    public void GetRayFromController()
    {
        // Start a ray from the controller
        RaycastHit hit;
        m_LineRenderer.SetPosition(0, transform.position);

        // If the ray hits something...
        if (Physics.Raycast(transform.position, transform.forward, out hit, m_RayDistance))
        {
            // set the end position to the hit point
            m_LineRenderer.SetPosition(1, hit.point);
            SelectableObject hittedObject;

            // verify that you are in selection mode -------------CHANGE THIS IN FUTURE ONLY TEST
            if (ModeManager.Instance.CurrentGUIMode != ModeManager.GUIMode.Selection)
                return;
            //Depending on the tag (== UI elem type), call different fcts 
            switch (hit.transform.tag)
            {
                case "RoboyUI": // if the ray hits an UI component then retrieve the roboy part from RoboyManager
                    hittedObject = RoboyManager.Instance.RoboyParts[hit.transform.name].GetComponent<SelectableObject>();
                    break;
                case "UIButton":
                    hittedObject = null;
                    Button b_pressed = hit.collider.GetComponent<Button>();
                    if (m_SteamVRDevice.GetHairTriggerDown())
                    {
                        b_pressed.onClick.Invoke();                    
                    }
                    break;
                case "UISlider": //slider elem in BeRoboy
                    hittedObject = null;
                    Slider slid = hit.collider.GetComponent<Slider>();

                    //If the trigger is pressed(even gently), the slider fills up, the value increases until it reaches it's maximum.
                    if (m_SteamVRDevice.GetHairTrigger())
                    {
                        if (slid.value < slid.maxValue)
                        { slid.value += 0.01f; }

                    }
                    //If the trigger is not touched at all, the slider drains down, the value decreases until it reaches it's minimum.
                    else
                    {
                        if (slid.value > slid.minValue)
                        { slid.value -= 0.01f; }
                    }
                    break;
                default: //not UI -> Roboy parts
                    hittedObject = hit.transform.gameObject.GetComponent<SelectableObject>();
                    break;
            }
            //if object found
            if (hittedObject)
            {
                // if the ray hits something different than last frame, then reset the last roboy part
                if (m_LastSelectedObject != hittedObject)
                {
                    if (m_LastSelectedObject != null)
                        m_LastSelectedObject.SetStateDefault();

                    // update the last roboy part as the current one
                    m_LastSelectedObject = hittedObject;
                    Vibrate();
                }
                // otherwise set the roboy part to targeted
                else
                {
                    hittedObject.SetStateTargeted();
                }
                // and select it if the user presses the trigger
                if (m_SteamVRDevice.GetHairTriggerDown())
                {
                    hittedObject.SetStateSelected();
                    //Vibrate();
                }
            }
        }
        // if the ray does not hit anything, then just reset the last roboy part and cast it until maximum distance is reached
        else
        {
            m_LineRenderer.SetPosition(1, transform.position + transform.forward * m_RayDistance);

            if (m_LastSelectedObject != null)
                m_LastSelectedObject.SetStateDefault();

            m_LastSelectedObject = null;
        }
    }




}

