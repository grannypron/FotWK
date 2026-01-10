using System;

namespace FotWK
{
	public enum EquipmentID
	{
        Raft = 1
	}

    [System.Serializable]
    public class EquipmentCollection : SerializableDictionary.Scripts.SerializableDictionary<EquipmentID, int>
    {
        public EquipmentCollection()
        {
            // Initialize all to 0 - be careful with this if new spells are added later and old save states are restored
            foreach (EquipmentID equipmentID in Enum.GetValues(typeof(EquipmentID)))
            {
                this.Add(equipmentID, 0);
            }
        }
    }

}
