using System.Collections.Generic;
using UnityEngine.Networking;


public class UnitsAgent : NetworkBehaviour
{
    public List<UnitScript> _unitObjects = new List<UnitScript>();

    private void Start()
    {
        if (!isLocalPlayer) return;

        PlayerManager.UnitsAgent = this;
    }

    public void AddUnitToUnitAgent(UnitScript unit)
    {
        _unitObjects.Add(unit);
    }
}
