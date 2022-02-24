
using GameFoundation.Scripts.Utilities;
using UnityEngine;

[DisallowMultipleComponent]
public class BaseMechSFX : MonoBehaviour
{
    [SerializeField] private string sfxName;
    
    [Header("For Tool Set sfx")]
    [SerializeField] private Object obj;

    protected void OnPlaySfx()
    {
        if (string.IsNullOrEmpty(this.sfxName))
        {
            Debug.LogError(gameObject.name + " missing sfx");
            return;
        }

        MasterMechSoundManager.Instance.PlaySound(this.sfxName);
    }

    /// <summary>
    /// Tool set sfx Name (need game Object active to affect)
    /// </summary>
    [ContextMenu("SetSfxName")]
    public void ConvertClipToString()
    {
        if (this.obj != null)
        {
            this.sfxName = this.obj.name;
            this.obj     = null;
        }
    }
}