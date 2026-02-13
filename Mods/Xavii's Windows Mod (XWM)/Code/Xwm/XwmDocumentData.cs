using System;
using System.Collections.Generic;
using UnityEngine;

namespace XaviiWindowsMod.Xwm
{
    [Serializable]
    internal class XwmPropertyData
    {
        public string key;
        public string value;
    }

    [Serializable]
    internal class XwmNodeData
    {
        public string id;
        public string parentId;
        public string type;
        public string name;
        public int order;
        public int layer;
        public bool active = true;
        public List<XwmPropertyData> properties = new List<XwmPropertyData>();
    }

    [Serializable]
    internal class XwmDocumentData
    {
        public string format = "xwm";
        public string version = "2.0.0";
        public string documentName = "Untitled";
        public string createdAtUtc = DateTime.UtcNow.ToString("o");
        public Vector2 canvasSize = new Vector2(900f, 620f);
        public int nextOrder = 1;
        public List<XwmNodeData> nodes = new List<XwmNodeData>();

        public static XwmDocumentData CreateDefault(string name = "Untitled")
        {
            XwmDocumentData document = new XwmDocumentData();
            document.documentName = string.IsNullOrWhiteSpace(name) ? "Untitled" : name.Trim();
            XwmNodeData root = new XwmNodeData
            {
                id = "root",
                parentId = string.Empty,
                type = XwmTypeLibrary.Frame,
                name = "CanvasRoot",
                order = 0,
                layer = 0,
                active = true,
                properties = XwmPropertyUtility.CreateDefaultProperties(XwmTypeLibrary.Frame)
            };
            XwmPropertyUtility.SetProperty(root.properties, "x", "0");
            XwmPropertyUtility.SetProperty(root.properties, "y", "0");
            XwmPropertyUtility.SetProperty(root.properties, "width", document.canvasSize.x.ToString("0"));
            XwmPropertyUtility.SetProperty(root.properties, "height", document.canvasSize.y.ToString("0"));
            XwmPropertyUtility.SetProperty(root.properties, "anchorMinX", "0.5");
            XwmPropertyUtility.SetProperty(root.properties, "anchorMinY", "0.5");
            XwmPropertyUtility.SetProperty(root.properties, "anchorMaxX", "0.5");
            XwmPropertyUtility.SetProperty(root.properties, "anchorMaxY", "0.5");
            XwmPropertyUtility.SetProperty(root.properties, "pivotX", "0.5");
            XwmPropertyUtility.SetProperty(root.properties, "pivotY", "0.5");
            XwmPropertyUtility.SetProperty(root.properties, "color", "#1F2735CC");
            document.nodes.Add(root);
            return document;
        }
    }
}