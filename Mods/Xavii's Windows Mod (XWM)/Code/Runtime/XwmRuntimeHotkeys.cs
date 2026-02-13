using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using XaviiWindowsMod.API;

namespace XaviiWindowsMod.Runtime
{
    internal sealed class XwmRuntimeHotkeys : MonoBehaviour
    {
        private XwmWorkspaceStateRunner _workspaceState;
        private XwmRuntimeHubController _hub;

        private void Awake()
        {
            _workspaceState = GetComponent<XwmWorkspaceStateRunner>();
            _hub = GetComponent<XwmRuntimeHubController>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F7))
            {
                if (_hub == null)
                {
                    _hub = GetComponent<XwmRuntimeHubController>();
                }

                _hub?.Toggle();
                return;
            }

            if (IsTypingInInputField())
            {
                return;
            }

            if (Input.GetKeyDown(KeyCode.F6))
            {
                ToggleAllRuntimes();
            }

            bool control = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
            bool shift = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

            if (control && shift && Input.GetKeyDown(KeyCode.R))
            {
                XwmFiles.ReloadAllLoaded(true);
            }

            if (control && shift && Input.GetKeyDown(KeyCode.S))
            {
                if (_workspaceState == null)
                {
                    _workspaceState = GetComponent<XwmWorkspaceStateRunner>();
                }

                _workspaceState?.ForceSave();
            }
        }

        private static bool IsTypingInInputField()
        {
            EventSystem current = EventSystem.current;
            if (current == null)
            {
                return false;
            }

            GameObject selected = current.currentSelectedGameObject;
            if (selected == null)
            {
                return false;
            }

            return selected.GetComponent<InputField>() != null;
        }

        private static void ToggleAllRuntimes()
        {
            IReadOnlyCollection<XwmWindowHandle> handles = XwmFiles.All();
            bool anyVisible = false;
            foreach (XwmWindowHandle handle in handles)
            {
                if (handle != null && !handle.IsDestroyed && handle.IsVisible)
                {
                    anyVisible = true;
                    break;
                }
            }

            foreach (XwmWindowHandle handle in handles)
            {
                if (handle == null || handle.IsDestroyed)
                {
                    continue;
                }

                if (anyVisible)
                {
                    handle.Hide();
                }
                else
                {
                    handle.Show();
                }
            }
        }
    }
}
