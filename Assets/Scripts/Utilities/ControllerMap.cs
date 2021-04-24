//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using Rewired;
//using Rewired.Data.Mapping;
//using log4net.Util;

//[System.Serializable]
//public class ControllerMap
//{
//    [SerializeField] private KeyboardEntry keyboard;
//    [SerializeField] private MouseEntry mouse;
//    [SerializeField] private ControllerEntry[] controllers;

//    public Sprite GetKeyboardSprite(int elementIdentifierID)
//    {
//        return keyboard.GetGlyph(elementIdentifierID);
//    }

//    public Sprite GetControllerGlyph(System.Guid joystickGuid, int elementIdentifierID, AxisRange axisRange)
//    {
//        if (controllers == null) return null;

//        foreach (ControllerEntry controller in controllers)
//        {
//            if (controller == null) continue;
//            if (controller.joystick == null) continue; // no joystick assigned
//            if (controller.joystick.Guid != joystickGuid) continue; // GUID doesn't match
//            return controller.GetGlyph(elementIdentifierID, axisRange);
//        }

//        return null;
//    }

//    [System.Serializable]
//    private class KeyboardEntry
//    {
//        public GlyphEntry[] glyphs;
//        public Sprite GetGlyph(int elementIdentifierId)
//        {
//            if (glyphs == null) return null;
//            for (int i = 0; i < glyphs.Length; i++)
//            {
//                if (glyphs[i] == null) continue;
//                if (glyphs[i].elementIdentifierId != elementIdentifierId) continue;
//                return glyphs[i].GetGlyph(AxisRange.Full);
//            }
//            return null;
//        }
//    }

//    [System.Serializable]
//    private class MouseEntry
//    {
//        public GlyphEntry[] glyphs;
//        public Sprite GetGlyph(int elementIdentifierId)
//        {
//            if (glyphs == null) return null;
//            for (int i = 0; i < glyphs.Length; i++)
//            {
//                if (glyphs[i] == null) continue;
//                if (glyphs[i].elementIdentifierId != elementIdentifierId) continue;
//                return glyphs[i].GetGlyph(AxisRange.Full);
//            }
//            return null;
//        }
//    }

//    [System.Serializable]
//    private class ControllerEntry
//    {
//        public string name;
//        // This must be linked to the HardwareJoystickMap located in Rewired/Internal/Data/Controllers/HardwareMaps/Joysticks
//        public HardwareJoystickMap joystick;
//        public GlyphEntry[] glyphs;

//        public Sprite GetGlyph(int elementIdentifierId, AxisRange axisRange)
//        {
//            if (glyphs == null) return null;
//            for (int i = 0; i < glyphs.Length; i++)
//            {
//                if (glyphs[i] == null) continue;
//                if (glyphs[i].elementIdentifierId != elementIdentifierId) continue;
//                return glyphs[i].GetGlyph(axisRange);
//            }
//            return null;
//        }
//    }

//    [System.Serializable]
//    private class GlyphEntry
//    {
//        public int elementIdentifierId;
//        public Sprite glyph;
//        public Sprite glyphPos;
//        public Sprite glyphNeg;

//        public Sprite GetGlyph(AxisRange axisRange)
//        {
//            switch (axisRange)
//            {
//                case AxisRange.Full: return glyph;
//                case AxisRange.Positive: return glyphPos != null ? glyphPos : glyph;
//                case AxisRange.Negative: return glyphNeg != null ? glyphNeg : glyph;
//            }
//            return null;
//        }

//    }
//}
