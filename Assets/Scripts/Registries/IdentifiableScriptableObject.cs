using UnityEngine;
namespace Registries
{
    public abstract class IdentifiableScriptableObject : ScriptableObject, IIdentifiable
    {
        public string ID => name;
    }
}