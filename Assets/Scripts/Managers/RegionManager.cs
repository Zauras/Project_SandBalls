using System.Collections.Generic;
using UnityEngine;
using MeshGeneration;
using Actors;

namespace Managers
{
    public class RegionManager : MonoBehaviour
    {
        private static RegionManager instance;
        public static RegionManager Instance => instance;

        public List<RegionGenerator> activeRegions = new List<RegionGenerator>();
        public float gap = 5.5f;

        private RegionGenerator[] regions;
        private float detectionRadius;

        private void InitSingleton()
        {
            if (instance != null && instance != this) Destroy(gameObject);
            else instance = this;
        }

        private void Awake() => InitSingleton();

        private void Start() => SetRegions();


        public void SetRegions()
        {
            regions = FindObjectsOfType<RegionGenerator>();
            detectionRadius = gap - MeshCutter.Instance.radius;
        }


        private void SetActiveRegions()
        {
            foreach (RegionGenerator regionGenerator in regions)
            {
                if (Mathf.Abs(regionGenerator.transform.position.y -
                              MeshCutter.Instance.transform.position.y) <= detectionRadius)
                {
                    if (!activeRegions.Contains(regionGenerator))
                    {
                        activeRegions.Add(regionGenerator);
                    }
                }
                else if (activeRegions.Contains(regionGenerator))
                {
                    // if a map is at too high remove it from the list
                    activeRegions.Remove(regionGenerator);
                }
            }
        }

        private void Update()
        {
            SetActiveRegions();
        }

    }
}
