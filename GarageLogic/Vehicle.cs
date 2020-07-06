using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex03.GarageLogic
{

    public abstract class Vehicle
    {
        // consts
        protected const int k_ModelLocation = 0;
        protected const int k_OwnerNameLocation = 1;
        protected const int k_OwnerPhoneLocation = 2;
        protected const int k_EnergyLeftLocation = 3;
        protected const int k_WheelManufacturerLocation = 4;
        protected const int k_WheelAirPressureLocation = 5;

        //members
        protected static List<string> sr_GeneralQuestions;
        protected string m_LicenseNumber;
        protected string m_ModelName;
        protected string m_NameOfOwner;
        protected string m_OwnerPhoneNumber;
        protected eVehicleStatus m_VehicleStatus = eVehicleStatus.InRepair;
        protected VehicleCreator.eVehicleType m_VehicleType;
        protected Engine m_Engine;
        protected float m_EnergyLeft;
        protected Wheel[] m_WheelCollection;

        static Vehicle() 
        { 
            sr_GeneralQuestions = new List<string>
            {
                "1. Please enter the model of the vehicle",
                "2. Please enter the Owners Name:",
                "3. Please enter the Owners Phone:",
                "4. Please enter how much Fuel / Electricity left:",
                "5. Please enter the wheel manufacturer?",
                "6. Please enter the current wheel pressure?"
            };

        }

        public enum eVehicleStatus
        {
            InRepair = 1,
            Repaired,
            Paid,
        }

        protected Vehicle(string i_LicenseNumber, Engine i_Engine)
        {
            m_LicenseNumber = i_LicenseNumber;
            m_Engine = i_Engine;
        }

        internal Wheel[] Wheels
        {
            get
            {
                return m_WheelCollection;
            }
        }

        internal Engine Engine
        {
            get
            {
                return m_Engine;
            }
        }

        public eVehicleStatus VehicleStatus
        {
            get
            {
                return m_VehicleStatus;
            }
            set
            {
                m_VehicleStatus = value;
            }
        }

        internal virtual List<string> Questions
        {
            get
            {
                return sr_GeneralQuestions;
            }
        }

        internal virtual string GatVehicleData()
        {
            string engineType, fuelType, currentCapacity, maxCapacity;

            if(m_Engine is ElectricEngine)
            {
                engineType = "Electric";
                fuelType = "None";
                currentCapacity = (m_Engine as ElectricEngine).CurrentCapacity + " hours";
                maxCapacity = (m_Engine as ElectricEngine).MaxCapacity + " hours";
            }
            else
            {
                engineType = "Fuel";
                fuelType = (m_Engine as FuelEngine).FuelType.ToString();
                currentCapacity = (m_Engine as FuelEngine).CurrentCapacity + " liters";
                maxCapacity = (m_Engine as FuelEngine).MaxCapacity + " liters";
            }

            string o_VehicleData = string.Format(
                @"
Vehicle Type        -   {0}
License Number      -   {1}
Model Name          -   {2}
Owner Name          -   {3}
Phone Number        -   {4}
Status in Garage    -   {5}
Wheels Manufacturer -   {6}
Max Wheel Pressure  -   {7}
Current Pressure    -   {8}
Engine Type         -   {9}
Fuel Type           -   {10}
Max Energy Capacity -   {11}
Current Capacity    -   {12}
Current Capacity %  -   {13}%",
                m_VehicleType,
                m_LicenseNumber,
                m_ModelName,
                m_NameOfOwner,
                m_OwnerPhoneNumber,
                m_VehicleStatus,
                m_WheelCollection[0].Manufacturer,
                m_WheelCollection[0].MaxPressure,
                m_WheelCollection[0].CurrentPressure,
                engineType,
                fuelType,
                maxCapacity,
                currentCapacity,
                m_Engine.CapacityInPercentage);

            return o_VehicleData;
        }

        internal virtual void SetData(List<string> i_DataFromUser)
        {
            if(string.IsNullOrEmpty(i_DataFromUser[k_ModelLocation]) || i_DataFromUser[k_ModelLocation] == " ")
            {
                throw new Exception("Model name can't be empty!");
            }
            else if(string.IsNullOrEmpty(i_DataFromUser[k_ModelLocation]) || i_DataFromUser[k_OwnerNameLocation] == " ")
            {
                throw new Exception("Owner name can't be empty!");
            }
            else if(string.IsNullOrEmpty(i_DataFromUser[k_ModelLocation])
                    || i_DataFromUser[k_OwnerPhoneLocation] == " ")
            {
                throw new Exception("Phone number can't be empty!");
            }
            else if(int.TryParse(i_DataFromUser[k_OwnerPhoneLocation], out int phoneNumber) == false)
            {
                throw new Exception("Phone number must be a number!");
            }
            else if(phoneNumber < 0)
            {
                throw new Exception("Phone number cannot be a negative number!");
            }
            else if(float.Parse(i_DataFromUser[k_EnergyLeftLocation]) < 0)
            {
                throw new ArgumentException("Energy left can't be negative");
            }
            else
            {
                m_ModelName = i_DataFromUser[k_ModelLocation];
                m_NameOfOwner = i_DataFromUser[k_OwnerNameLocation];
                m_OwnerPhoneNumber = i_DataFromUser[k_OwnerPhoneLocation];
                m_EnergyLeft = float.Parse(i_DataFromUser[k_EnergyLeftLocation]);
                m_Engine.CurrentCapacity = m_EnergyLeft;
                m_Engine.CapacityInPercentage = m_Engine.CurrentCapacity / m_Engine.MaxCapacity * 100;
            }
        }

        internal virtual bool CheckValidity(int i_QuestionToCheck, string i_AnswerToCheck, out string o_ErrorMessage)
        {
            VehicleCreator.eQuestionNumber questionNumber = (VehicleCreator.eQuestionNumber)i_QuestionToCheck;
            bool o_IsValid = true;
            o_ErrorMessage = "None";

            switch (questionNumber)
            {
                case VehicleCreator.eQuestionNumber.Question1:
                    {
                        o_IsValid = validateName(i_AnswerToCheck, out o_ErrorMessage);
                        break;
                    }
                case VehicleCreator.eQuestionNumber.Question2:
                    {
                        o_IsValid = validateName(i_AnswerToCheck, out o_ErrorMessage);
                        break;
                    }
                case VehicleCreator.eQuestionNumber.Question3:
                    {
                        o_IsValid = validatePhoneNumber(i_AnswerToCheck, out o_ErrorMessage);
                        break;
                    }
                case VehicleCreator.eQuestionNumber.Question4:
                    {
                        o_IsValid = validateFuelOrElectricityQuantity(i_AnswerToCheck, out o_ErrorMessage);
                        break;
                    }
                case VehicleCreator.eQuestionNumber.Question5:
                    {
                        o_IsValid = validateName(i_AnswerToCheck, out o_ErrorMessage);
                        break;
                    }
                case VehicleCreator.eQuestionNumber.Question6:
                    {
                        o_IsValid = validateWheelPressure(i_AnswerToCheck, out o_ErrorMessage);
                        break;
                    }
                default:
                    {
                        break;
                    }
            }

            return o_IsValid;
        }

        private bool validateName(string i_AnswerToCheck, out string o_ErrorMessage)
        {
            bool o_IsValid = (!string.IsNullOrEmpty(i_AnswerToCheck) && i_AnswerToCheck != " ");

            if(o_IsValid == false)
            {
                o_ErrorMessage = "Name can't be empty! try again";
            }
            else
            {
                o_ErrorMessage = "None";
            }

            return o_IsValid;
        }

        private bool validatePhoneNumber(string i_AnswerToCheck, out string o_ErrorMessage)
        {
            bool isValid = int.TryParse(i_AnswerToCheck, out _);

            if (isValid == false || i_AnswerToCheck.Length < 8 || i_AnswerToCheck.Length > 10)
            {
                isValid = false;
                o_ErrorMessage = "Please enter a valid phone number between 8 to 10 digits";
            }
            else
            {
                o_ErrorMessage = "None";
                isValid = true;
            }

            return isValid;
        }

        private bool validateFuelOrElectricityQuantity(string i_AnswerToCheck, out string o_ErrorMessage)
        {
            bool isValid = float .TryParse(i_AnswerToCheck, out float currentCapacityInEngine);

            if (isValid == false)
            {
                o_ErrorMessage = "Please enter a number";
            }
            else if (m_Engine.MaxCapacity < currentCapacityInEngine || currentCapacityInEngine < 0)
            {
                o_ErrorMessage = "current capacity not valid please try again";
                isValid = false;
            }
            else
            {
                o_ErrorMessage = "None";
                isValid = true;
            }

            return isValid;
        }

        private bool validateWheelPressure(string i_AnswerToCheck, out string o_ErrorMessage)
        {
            bool isValid = float.TryParse(i_AnswerToCheck, out float currentPsi);

            if (isValid == false)
            {
                o_ErrorMessage = "Please enter a number";
            }
            else if (m_WheelCollection[0].MaxPressure < currentPsi || currentPsi < 0)
            {
                o_ErrorMessage = "current Pressure not valid please try again";
                isValid = false;
            }
            else
            {
                o_ErrorMessage = "None";
                isValid = true;
            }

            return isValid;
        }
    }
}
