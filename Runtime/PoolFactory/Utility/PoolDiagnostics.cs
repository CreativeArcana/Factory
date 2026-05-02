using UnityEngine;

namespace CreativeArcana.Factory.Utility
{
    public static class PoolDiagnostics
    {
        public static void LogStatus<T>(IPoolFactory<T> factory) where T : class
        {
            if (factory == null)
            {
                Debug.LogWarning("[PoolDiagnostics] Factory is null.");
                return;
            }

            Debug.Log(
                $"[Pool] {factory.GetType().Name} | Active: {factory.CountActive} | Inactive: {factory.CountInactive}");
        }
    }
}