using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex03.GarageLogic
{

    public class FuelEngine : Engine
    {

        private eFuleType m_FuelType;

        public enum eFuleType
        {
            Soler = 1,
            Octan95,
            Octan96,
            Octan98,
        }

        internal eFuleType FuelType
        {
            get
            {
                return m_FuelType;
            }
            set
            {
                m_FuelType = value;
            }
        }

        internal void addFuel(float i_FuelToAdd, eFuleType i_FuelType)
        {
            if (i_FuelType != m_FuelType)
            {
                throw new ArgumentException(string.Format("Fuel Type does not match this vehicle, you should use {0}:", m_FuelType.ToString()), i_FuelToAdd.ToString());
            }
            else if (i_FuelToAdd + m_CurrentCapacity > m_MaxCapacity)
            {
                throw new ValueOutOfRangeException("Amount added", 0, m_MaxCapacity - m_CurrentCapacity);
            }
            else
            {
                m_CurrentCapacity += i_FuelToAdd;
                m_CapacityInPercentage = (m_CurrentCapacity / m_MaxCapacity) * 100;
            }
        }
    }
}
