using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex03.GarageLogic
{
    public class Car : Vehicle
    {
        private const int k_CarColorLocation = 6;
        private const int k_NumberOfDoorsLocation = 7;
        private const int k_NumberOfWheels = 4;
        private const float k_MaxAirPressure = 32;

        private static readonly List<string> sr_CarQuestions;

        private eCarColor m_CarColor;
        private eNumberOfDoors m_NumberOfDoors;

        static Car()
        {
            sr_CarQuestions = new List<string>
            {
                "7. Please enter the car color: 1 for Red, 2 for Black, 3 for White or 4 for Silver only",
                "8. Please enter the number of doors: 2, 3, 4 or 5 only"
            };
        }

        internal Car(string i_LicenseNumber, Engine i_Engine) : base(i_LicenseNumber, i_Engine)
        {
            m_WheelCollection = new Wheel[k_NumberOfWheels];

            for (int i = 0; i < k_NumberOfWheels; i++)
            {
                m_WheelCollection[i] = new Wheel(k_MaxAirPressure);
            }

            if (m_Engine is FuelEngine)
            {
                (m_Engine as FuelEngine).FuelType = FuelEngine.eFuleType.Octan96;
                (m_Engine as FuelEngine).MaxCapacity = 60;
                m_VehicleType = VehicleCreator.eVehicleType.FuelCar;
            }
            else
            {
                (m_Engine as ElectricEngine).MaxCapacity = 2.1f;
                m_VehicleType = VehicleCreator.eVehicleType.ElectricCar;
            }
        }

        public enum eNumberOfDoors
        {
            Two = 2,
            Three,
            Four,
            Five
        }

        public enum eCarColor
        {
            Red = 1,
            White,
            Black,
            Silver,
        }

        internal override bool CheckValidity(int i_QuestionToCheck, string i_AnswerToCheck, out string o_ErrorMessage)
        {
            VehicleCreator.eQuestionNumber questionNumber = (VehicleCreator.eQuestionNumber)i_QuestionToCheck;
            bool o_IsValid = true;
            o_ErrorMessage = "None";
            o_IsValid = base.CheckValidity(i_QuestionToCheck, i_AnswerToCheck, out o_ErrorMessage);

            switch (questionNumber)
            {
                case VehicleCreator.eQuestionNumber.Question7:
                    {
                        o_IsValid = validateCarColor(i_AnswerToCheck, out o_ErrorMessage);
                        break;
                    }
                case VehicleCreator.eQuestionNumber.Question8:
                    {
                        o_IsValid = validateNumberOfDoors(i_AnswerToCheck, out o_ErrorMessage);
                        break;
                    }
                default:
                    {
                        break;
                    }
            }

            return o_IsValid;
        }

        private bool validateCarColor (string i_AnswerToCheck, out string o_ErrorMessage)
        {
            bool o_IsValid = int.TryParse(i_AnswerToCheck, out int answerAsInt);

            if (o_IsValid == false || Enum.IsDefined(typeof(eCarColor), answerAsInt) == false)
            {
                o_ErrorMessage = "Error: Please choose 1, 2 , 3 or 4";
                o_IsValid = false;
            }
            else
            {
                o_ErrorMessage = "None";
            }

            return o_IsValid;
        }

        private bool validateNumberOfDoors(string i_AnswerToCheck, out string o_ErrorMessage)
        {
            bool o_IsValid = int.TryParse(i_AnswerToCheck, out int answerAsInt);

            if (o_IsValid == false || Enum.IsDefined(typeof(eNumberOfDoors), answerAsInt) == false)
            {
                o_ErrorMessage = "Error: Please choose 2, 3 , 4 or 5";
                o_IsValid = false;
            }
            else
            {
                o_ErrorMessage = "None";
            }

            return o_IsValid;
        }

        internal override List<string> Questions
        {
            get
            {
                List<string> o_AllQuestions = new List<string>();

                foreach (string question in base.Questions)
                {
                    o_AllQuestions.Add(question);
                }

                foreach (string question in sr_CarQuestions)
                {
                    o_AllQuestions.Add(question);
                }

                return o_AllQuestions;
            }
        }

        internal override void SetData(List<string> i_DataFromUser)
        {
            base.SetData(i_DataFromUser);
            int carColorAsInt = int.Parse(i_DataFromUser[k_CarColorLocation]);
            int numberOfDoors = int.Parse(i_DataFromUser[k_NumberOfDoorsLocation]);

            for (int i = 0; i < k_NumberOfWheels; i++)
            {
                m_WheelCollection[i].CurrentPressure = float.Parse(i_DataFromUser[k_WheelAirPressureLocation]);
                m_WheelCollection[i].Manufacturer = i_DataFromUser[k_WheelManufacturerLocation];
            }

            if (!Enum.IsDefined(typeof(eCarColor), carColorAsInt))
            {
                throw new FormatException("Car color must be Red, White, Black Or Silver only");
            }

            if (!Enum.IsDefined(typeof(eNumberOfDoors), numberOfDoors))
            {
                throw new FormatException("Number of doors must be Two, Three, Four or Five only");
            }

            m_CarColor = (eCarColor)carColorAsInt;
            m_NumberOfDoors = (eNumberOfDoors)numberOfDoors;

        }

        internal override string GatVehicleData()
        {
            string o_VehicleData = base.GatVehicleData();

            string carData = string.Format(@"
Color               -   {0}
Number Of Doors-    -   {1}
",
m_CarColor, m_NumberOfDoors);

            o_VehicleData += carData;

            return o_VehicleData;

        }
    }
}
