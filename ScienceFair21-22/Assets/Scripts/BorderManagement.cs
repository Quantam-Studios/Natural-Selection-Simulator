using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorderManagement : MonoBehaviour
{
    // SETTINGS
    [Header("Settings")]
    // variable checked by creatures
    public static bool staticBounds;
    // Variable used to interact with staticBounds from the inspector.
    public bool setStaticBounds;

    // BORDERS
    [Header("Borders")]
    public GameObject staticBorder;
    public GameObject periodicBorder;

    // Awake()
    private void Awake()
    {
        // initialize border type
        staticBounds = setStaticBounds;
        // deactivate static border by default.
        staticBorder.SetActive(false);
        // deactivate periodic border by default.
        periodicBorder.SetActive(false);

        // Check if it has static bounds
        if (staticBounds)
            // if it does have static bounds then enable the border gameObject
            staticBorder.SetActive(true);
        else
            // if it doesnt then creatures will have periodic bounds, and will essentially have a looping border.
            periodicBorder.SetActive(true);
    }
}
