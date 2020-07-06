using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex03.GarageLogic
{
    public class ElectricEngine : Engine
    {
        internal void ChargeBattery(float i_HoursToAdd)
        {
            if (m_CurrentCapacity + i_HoursToAdd > m_MaxCapacity)
            {
                throw new ValueOutOfRangeException("Minutes to Charge", 0, (m_MaxCapacity - m_CurrentCapacity) * 60);
            }
            else
            {
                m_CurrentCapacity += i_HoursToAdd;
                m_CapacityInPercentage = m_CurrentCapacity / m_MaxCapacity * 100;
            }
        }
    }
}
