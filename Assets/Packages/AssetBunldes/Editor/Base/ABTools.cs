using UnityEngine;
using System.Collections;
using UnityEditor;

namespace AssetBundleEditor
{
    public class ABTools : MonoBehaviour
    {
        /// <summary>
        /// Convenience function that displays a list of sprites and returns the selected value.
        /// </summary>

        //static public string DrawList(string field, string[] list, string selection, params GUILayoutOption[] options)
        //{
        //    if (list != null && list.Length > 0)
        //    {
        //        int index = 0;
        //        if (string.IsNullOrEmpty(selection)) selection = list[0];

        //        // We need to find the sprite in order to have it selected
        //        if (!string.IsNullOrEmpty(selection))
        //        {
        //            for (int i = 0; i < list.Length; ++i)
        //            {
        //                if (selection.Equals(list[i], System.StringComparison.OrdinalIgnoreCase))
        //                {
        //                    index = i;
        //                    break;
        //                }
        //            }
        //        }

        //        // Draw the sprite selection popup
        //        index = string.IsNullOrEmpty(field) ?
        //            EditorGUILayout.Popup(index, list, options) :
        //            EditorGUILayout.Popup(field, index, list, options);

        //        return list[index];
        //    }
        //    return null;
        //}

        static public int DrawList(string field, string[] list, int sel, params GUILayoutOption[] options)
        {
            if (list.Length <= 0)
            {
                Debug.LogWarning("DrawList Param");

                return -1;
            }

            if (sel < 0 || sel >= list.Length)
                sel = 0;

            string selection = list[sel];

            if (list != null && list.Length > 0)
            {
                int index = 0;
                if (string.IsNullOrEmpty(selection)) selection = list[0];

                // We need to find the sprite in order to have it selected
                if (!string.IsNullOrEmpty(selection))
                {
                    for (int i = 0; i < list.Length; ++i)
                    {
                        if (selection.Equals(list[i], System.StringComparison.OrdinalIgnoreCase))
                        {
                            index = i;
                            break;
                        }
                    }
                }

                // Draw the sprite selection popup
                index = string.IsNullOrEmpty(field) ?
                    EditorGUILayout.Popup(index, list, options) :
                    EditorGUILayout.Popup(field, index, list, options);

                return index;
            }

            return -1;
        }

    }

}
