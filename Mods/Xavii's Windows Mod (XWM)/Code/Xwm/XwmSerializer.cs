using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace XaviiWindowsMod.Xwm
{
    internal static class XwmSerializer
    {
        public static XwmDocumentData Load(string path)
        {
            if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
            {
                return null;
            }

            try
            {
                string json = File.ReadAllText(path);
                if (string.IsNullOrWhiteSpace(json))
                {
                    return null;
                }

                XwmDocumentData document = null;
                try
                {
                    XwmDocumentFile fileData = JsonConvert.DeserializeObject<XwmDocumentFile>(json);
                    document = FromFileData(fileData);
                }
                catch
                {
                }

                if (document == null)
                {
                    document = JsonUtility.FromJson<XwmDocumentData>(json);
                    if (document == null)
                    {
                        return null;
                    }
                }

                XwmPropertyUtility.EnsureDocument(document);
                return document;
            }
            catch
            {
                return null;
            }
        }

        public static bool Save(string path, XwmDocumentData document)
        {
            if (string.IsNullOrWhiteSpace(path) || document == null)
            {
                return false;
            }

            try
            {
                XwmPropertyUtility.EnsureDocument(document);
                string directory = Path.GetDirectoryName(path);
                if (!string.IsNullOrWhiteSpace(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                document.createdAtUtc = DateTime.UtcNow.ToString("o");
                XwmDocumentFile fileData = ToFileData(document);
                string json = JsonConvert.SerializeObject(fileData, Formatting.Indented);
                File.WriteAllText(path, json);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static string EnsureExtension(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                return "untitled.xwm";
            }

            string trimmed = fileName.Trim();
            if (trimmed.EndsWith(".xwm", StringComparison.OrdinalIgnoreCase))
            {
                return trimmed;
            }

            return trimmed + ".xwm";
        }

        private static XwmDocumentFile ToFileData(XwmDocumentData document)
        {
            if (document == null)
            {
                return null;
            }

            XwmDocumentFile fileData = new XwmDocumentFile
            {
                format = string.IsNullOrWhiteSpace(document.format) ? "xwm" : document.format,
                version = string.IsNullOrWhiteSpace(document.version) ? "2.0.0" : document.version,
                documentName = string.IsNullOrWhiteSpace(document.documentName) ? "Untitled" : document.documentName,
                createdAtUtc = string.IsNullOrWhiteSpace(document.createdAtUtc) ? DateTime.UtcNow.ToString("o") : document.createdAtUtc,
                canvasSize = new XwmVector2File { x = document.canvasSize.x, y = document.canvasSize.y },
                nextOrder = document.nextOrder,
                nodes = new List<XwmNodeFile>()
            };

            if (document.nodes == null)
            {
                return fileData;
            }

            for (int i = 0; i < document.nodes.Count; i++)
            {
                XwmNodeData node = document.nodes[i];
                if (node == null)
                {
                    continue;
                }

                XwmNodeFile nodeFile = new XwmNodeFile
                {
                    id = node.id,
                    parentId = node.parentId,
                    type = node.type,
                    name = node.name,
                    order = node.order,
                    layer = node.layer,
                    active = node.active,
                    properties = new List<XwmPropertyFile>()
                };

                if (node.properties != null)
                {
                    for (int p = 0; p < node.properties.Count; p++)
                    {
                        XwmPropertyData property = node.properties[p];
                        if (property == null)
                        {
                            continue;
                        }

                        nodeFile.properties.Add(new XwmPropertyFile
                        {
                            key = property.key,
                            value = property.value
                        });
                    }
                }

                fileData.nodes.Add(nodeFile);
            }

            return fileData;
        }

        private static XwmDocumentData FromFileData(XwmDocumentFile fileData)
        {
            if (fileData == null)
            {
                return null;
            }

            XwmDocumentData document = new XwmDocumentData
            {
                format = string.IsNullOrWhiteSpace(fileData.format) ? "xwm" : fileData.format,
                version = string.IsNullOrWhiteSpace(fileData.version) ? "2.0.0" : fileData.version,
                documentName = string.IsNullOrWhiteSpace(fileData.documentName) ? "Untitled" : fileData.documentName,
                createdAtUtc = string.IsNullOrWhiteSpace(fileData.createdAtUtc) ? DateTime.UtcNow.ToString("o") : fileData.createdAtUtc,
                canvasSize = new Vector2(fileData.canvasSize != null ? fileData.canvasSize.x : 900f, fileData.canvasSize != null ? fileData.canvasSize.y : 620f),
                nextOrder = fileData.nextOrder,
                nodes = new List<XwmNodeData>()
            };

            if (fileData.nodes == null)
            {
                return document;
            }

            for (int i = 0; i < fileData.nodes.Count; i++)
            {
                XwmNodeFile nodeFile = fileData.nodes[i];
                if (nodeFile == null)
                {
                    continue;
                }

                XwmNodeData node = new XwmNodeData
                {
                    id = nodeFile.id,
                    parentId = nodeFile.parentId,
                    type = nodeFile.type,
                    name = nodeFile.name,
                    order = nodeFile.order,
                    layer = nodeFile.layer,
                    active = nodeFile.active,
                    properties = new List<XwmPropertyData>()
                };

                if (nodeFile.properties != null)
                {
                    for (int p = 0; p < nodeFile.properties.Count; p++)
                    {
                        XwmPropertyFile propertyFile = nodeFile.properties[p];
                        if (propertyFile == null)
                        {
                            continue;
                        }

                        node.properties.Add(new XwmPropertyData
                        {
                            key = propertyFile.key,
                            value = propertyFile.value
                        });
                    }
                }

                document.nodes.Add(node);
            }

            return document;
        }

        private class XwmDocumentFile
        {
            public string format;
            public string version;
            public string documentName;
            public string createdAtUtc;
            public XwmVector2File canvasSize = new XwmVector2File();
            public int nextOrder;
            public List<XwmNodeFile> nodes = new List<XwmNodeFile>();
        }

        private class XwmVector2File
        {
            public float x;
            public float y;
        }

        private class XwmNodeFile
        {
            public string id;
            public string parentId;
            public string type;
            public string name;
            public int order;
            public int layer;
            public bool active = true;
            public List<XwmPropertyFile> properties = new List<XwmPropertyFile>();
        }

        private class XwmPropertyFile
        {
            public string key;
            public string value;
        }
    }
}
