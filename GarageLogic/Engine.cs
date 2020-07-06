using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex03.GarageLogic
{
    public abstract class Engine
    {
        protected const float k_MinAmountToAdd = 0;

        protected float m_MaxCapacity;
        protected float m_CurrentCapacity;
        protected float m_CapacityInPercentage;

        internal float MaxCapacity
        {
            get
            {
                return m_MaxCapacity;
            }
            set
            {
                if (value <= k_MinAmountToAdd)
                {
                    throw new ArgumentException("Max Capacity must have a positive value");
                }
                else
                {
                    m_MaxCapacity = value;
                }
            }
        }

        internal float CurrentCapacity
        {
            get
            {
                return m_CurrentCapacity;
            }
            set
            {
                if (value + m_CurrentCapacity > m_MaxCapacity)
                {
                    throw new ValueOutOfRangeException("New energy Value", k_MinAmountToAdd, m_MaxCapacity);
                }
                else
                {
                    m_CurrentCapacity += value;
                }
            }
        }

        internal float CapacityInPercentage
        {
            get
            {
                return m_CapacityInPercentage;
            }
            set
            {
                if (value < 0 || value > 100)
                {
                    throw new ValueOutOfRangeException("percentage", 0, 100);
                }

                else
                {
                    m_CapacityInPercentage = m_CurrentCapacity / m_MaxCapacity * 100;
                }
            }
        }
    }
}