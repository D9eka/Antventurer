using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Components.UI.Skills
{
    public class SkillsContent : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _skillName;
        [SerializeField] private Image _skillImage;
        [SerializeField] private TextMeshProUGUI _skillAction;
        [Space]
        [SerializeField] private GameObject _skillKey;
        [SerializeField] private TextMeshProUGUI _skillKeyText;
        [Space]
        [SerializeField] private TextMeshProUGUI _skillDescription;

        private SkillData _data;

        private void Start()
        {
            UpdateVisual();
        }

        public void SetData(SkillData data)
        {
            _data = data;
            UpdateVisual();
        }

        private void UpdateVisual()
        {
            if (_data == null)
                return;

            _skillName.text = _data.Name;
            _skillImage.sprite = _data.Image;
            _skillAction.text = _data.Action;

            _skillKey.SetActive(_data.HaveKey);
            if(_data.HaveKey)
                _skillKeyText.text = _data.KeyText;

            _skillDescription.text = _data.Description;

            LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
        }
    }
}