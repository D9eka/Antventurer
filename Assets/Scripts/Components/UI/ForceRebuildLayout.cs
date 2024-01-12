using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Components.UI
{
    public class ForceRebuildLayout : MonoBehaviour
    {
        void Start()
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
        }
    }
}