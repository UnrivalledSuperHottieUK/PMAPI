using Il2Cpp;
using UnityEngine;
using MelonLoader;

namespace IndustrialHorizons;

public class OreBeh : MonoBehaviour
{
  public OreBeh(System.IntPtr ptr)
    : base(ptr)
  {
  }

  public Substance refinedSubstance { get; set; }
  public double meltTemperature { get; set; } = 600;

  private void OnInitialize()
  {
    this.cubeBase = base.GetComponent<CubeBase>();
    this.cubeBase.enabled = true;
  }

  private void Start()
  {
  }

  private void Update()
  {
    bool flag = !this.updated;
    // Remove later
    //MelonLogger.Msg("velocity" + this.cubeBase.pit);
    if (flag)
    {
      bool flag2 = this.cubeBase.heat.GetCelsiusTemperature() > meltTemperature;
      if (flag2)
      {
        this.cubeBase.ChangeSubstance(this.refinedSubstance);
        this.updated = true;
      }
    }
  }

  // Token: 0x04000009 RID: 9
  private CubeBase cubeBase;

  // Token: 0x0400000B RID: 11
  private bool updated = false;
}