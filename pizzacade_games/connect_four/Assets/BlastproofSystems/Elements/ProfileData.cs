using UnityEngine;
using Blastproof.Systems.Core.Variables;
using Sirenix.OdinInspector;

namespace Blastproof.Systems.Database
{
    [CreateAssetMenu(menuName = "Blastproof/Systems/Database/ProfileData")]
    public class ProfileData : SerializedScriptableObject
    {
        // ---- This is the user data.
        [SerializeField] private IntVariable _playerVariable;
    }
}
