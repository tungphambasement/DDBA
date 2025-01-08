namespace UnityEngine.VFX.VirtualImpacts
{
    [AddComponentMenu("")]
    internal class VFXVirtualImpactUpdater : MonoBehaviour
    {
        public bool debug = false;

        void Update()
        {
            VFXVirtualImpact.UpdateAll(Time.deltaTime);
        }

        void OnGUI()
        {
            if (debug)
            {
                VFXVirtualImpact.DrawDebugGUI();
            }
        }

        private void OnDrawGizmos()
        {
            if (debug)
            {
                VFXVirtualImpact.DrawGizmosGUI();
            }
        }
    }

}
