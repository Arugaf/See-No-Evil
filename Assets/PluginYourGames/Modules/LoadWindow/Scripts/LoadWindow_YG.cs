#if UNITY_EDITOR
namespace YG
{
    public partial class InfoYG
    {
        public LoadWindowSettings LoadWindow = new LoadWindowSettings();

        [System.Serializable]
        public partial class LoadWindowSettings
        {
			public bool UseWindowTools;
        }
    }
}
#endif // Конец определения платформы
