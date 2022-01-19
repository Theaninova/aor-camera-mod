﻿using UnityEngine;
using UnityModManagerNet;

// ReSharper disable FieldCanBeMadeReadOnly.Global

namespace ArtOfRallyCameraMod.Settings
{
    public class Settings : UnityModManager.ModSettings, IDrawable
    {
        [Header("Key Bindings")] [Draw("Camera Editor on/off")]
        public KeyBinding CameraEditor = new KeyBinding { keyCode = KeyCode.KeypadDivide };

        [Draw("Height -0.5")] public KeyBinding HeightMinus = new KeyBinding { keyCode = KeyCode.Keypad0 };

        [Draw("Height +0.5")] public KeyBinding HeightPlus = new KeyBinding { keyCode = KeyCode.Keypad1 };

        [Draw("Distance -0.5")] public KeyBinding DistanceMinus = new KeyBinding { keyCode = KeyCode.Keypad2 };

        [Draw("Distance +0.5")] public KeyBinding DistancePlus = new KeyBinding { keyCode = KeyCode.Keypad3 };

        [Draw("Angle -0.5")] public KeyBinding AngleMinus = new KeyBinding { keyCode = KeyCode.Keypad4 };

        [Draw("Angle +0.5")] public KeyBinding AnglePlus = new KeyBinding { keyCode = KeyCode.Keypad7 };

        [Draw("Reset Camera")] public KeyBinding ResetCamera = new KeyBinding { keyCode = KeyCode.KeypadMultiply };

        [Header("Camera 8 (Defaults: 7.0 | 2.0 | -1.0)"), Space(15)] [Draw("Distance")]
        public float Camera8Distance = 7.0f;

        [Draw("Height")] public float Camera8Height = 2.0f;

        [Draw("Angle")] public float Camera8Angle = -1.0f;

        [Draw("Yaw Reset Speed", DrawType.Slider, Min = 0, Max = 20)]
        public float Camera8YawResetSpeed = 10.0f;

        [Header("Camera 9 (Defaults: 10.0 | 3.0 | -1.5)"), Space(15)] [Draw("Distance")]
        public float Camera9Distance = 10.0f;

        [Draw("Height")] public float Camera9Height = 3.0f;

        [Draw("Angle")] public float Camera9Angle = -1.5f;

        [Draw("Yaw Reset Speed", DrawType.Slider, Min = 0, Max = 20)]
        public float Camera9YawResetSpeed = 10.0f;

        public override void Save(UnityModManager.ModEntry modEntry)
        {
            Save(this, modEntry);
        }

        public void OnChange()
        {
            Main.ApplySettings();
        }
    }
}