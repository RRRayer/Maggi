using TMPro;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Tutorial", menuName = "Tutorial/Tutorial")]
public class TutorialSO : DescriptionBaseSO
{
    [HideInInspector] public Image Image;
    [HideInInspector] public TextMeshProUGUI Text;
}
