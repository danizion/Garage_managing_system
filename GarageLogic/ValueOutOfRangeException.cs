using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex03.GarageLogic
{
    public class ValueOutOfRangeException : Exception
    {
        private readonly float m_MinValue;
        private readonly float m_MaxValue;

        public ValueOutOfRangeException(string i_Value, float i_MinValue, float i_MaxValue) :
            base(string.Format("The {0} is out of range. Minimum is {1} and maximum {2}", i_Value, i_MinValue, i_MaxValue))
        {
            m_MinValue = i_MinValue;
            m_MaxValue = i_MaxValue;
        }
    }
}
