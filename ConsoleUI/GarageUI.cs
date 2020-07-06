using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ex03.GarageLogic;

namespace ConsoleUI
{
    public class GarageUI
    {
        private readonly GarageLogicManager r_GarageLogic = new GarageLogicManager();
        private readonly Menu r_Menu;

        public GarageUI()
        {
            r_Menu = new Menu(VehicleCreator.VehicleOptions);
        }

        private enum eUserChoice
        {
            AddVehicle = 1,
            ShowAllVehicle,
            ChangeVehicleStatus,
            AddAirPressure,
            AddFuel,
            ChargeBattery,
            PrintVehicleData,
            Exit
        }

        public void RunProgram()
        {
            Console.WriteLine(@"Hello And Welcome To Dani and Tamir's Garage system:
Please use the Following menu in order manage your Garage. Good Luck!
");
            while (true)
            {
                printMainMenu();
                eUserChoice userChoice = validateUserChoiceForMainMenu();

                switch (userChoice)
                {
                    case eUserChoice.AddVehicle:
                        {
                            addNewVehicleToTheGarage();
                            break;
                        }
                    case eUserChoice.ShowAllVehicle:
                        {
                            printLicenseNumbers();
                            break;
                        }
                    case eUserChoice.ChangeVehicleStatus:
                        {
                            changeVehicleStatus();
                            break;
                        }
                    case eUserChoice.AddAirPressure:
                        {
                            addAirPressure();
                            break;
                        }
                    case eUserChoice.AddFuel:
                        {
                            addFuel();
                            break;
                        }
                    case eUserChoice.ChargeBattery:
                        {
                            chargeBattery();
                            break;
                        }
                    case eUserChoice.PrintVehicleData:
                        {
                            printVehicleData();
                            break;
                        }
                    case eUserChoice.Exit:
                        {
                            Environment.Exit(1);
                            break;
                        }           
                }
            }
        }

        private void addAirPressure()
        {
            if (!isGarageEmpty())
            {
                Console.WriteLine("Please choose a license number from the following numbers:");
                printAllVehicleLicense();
                try
                { 
                    bool licenseNumberExist = false;
                    string licenseNumber;
                    do
                    {
                        licenseNumber = enterLicenseNumber();
                        licenseNumberExist = r_GarageLogic.ListOfVehicles.ContainsKey(licenseNumber);
                        if (licenseNumberExist == false)
                        {
                            Console.WriteLine("License number does not exist in the garage, try again:");
                        }
                    } while (licenseNumberExist == false);

                    r_GarageLogic.InflateWheelToMax(licenseNumber);
                    Console.WriteLine("Wheels have been inflated to max");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    addAirPressure();
                }
            }
            else
            {
                Console.WriteLine("There are no vehicles yet in the system\n");
            }

        }

        private void addNewVehicleToTheGarage()
        {
            Console.Clear();
            Console.WriteLine("To add a vehicle please follow the following steps:");
            string o_LicenseNumber = enterLicenseNumber();

            if (checkIfLicenseExist(o_LicenseNumber))
            {
                r_GarageLogic.ChangeVehicleStatusInGarage(o_LicenseNumber, Vehicle.eVehicleStatus.InRepair);
                Console.WriteLine("Vehicle already exist in the system, status has been change to In Repair");
            }
            else
            {
                try
                {
                    Console.Clear();
                    reciveVehicleType(out VehicleCreator.eVehicleType o_VehicleType);
                    r_GarageLogic.AddNewVehicleToGarage(o_LicenseNumber, o_VehicleType);
                    Console.Clear();
                    List<string> o_VehicleData = getDataForNewVehicle(o_LicenseNumber);
                    r_GarageLogic.SetNewVehicleData(o_LicenseNumber, o_VehicleData);
                    Console.WriteLine("Vehicle No. {0} was added", o_LicenseNumber);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("Fatal error, please try again");
                    r_GarageLogic.ListOfVehicles.Remove(o_LicenseNumber);
                    addNewVehicleToTheGarage();
                }
            }
        }
        

        private List<string> getDataForNewVehicle(string i_LicenseNumber)
        { 
            Console.Clear();
            List<string> o_VehicleData = new List<string>();
            List<string> questionsForUser = new List<string>();
            questionsForUser = r_GarageLogic.GetQuestions(i_LicenseNumber);

            foreach (string question in questionsForUser)
            {
                Console.WriteLine(question);
                string answerFromUser = Console.ReadLine();
                bool isValid = false;
                do
                {
                    isValid = r_GarageLogic.checkValidity(o_VehicleData.Count, answerFromUser, out string errorMessage, i_LicenseNumber);
                    if(isValid == false)
                    {
                        Console.WriteLine(errorMessage);
                        answerFromUser = Console.ReadLine();
                    }
                }
                while(isValid == false);

                o_VehicleData.Add(answerFromUser);
            }
            return o_VehicleData;

        }

        private void changeVehicleStatus()
        {
            if(!isGarageEmpty())
            {
                try
                {
                    Console.Clear();
                    Console.WriteLine("choose one of the License numbers below: ");
                    printAllVehicleLicense();
                    string o_LicenseNumber = enterLicenseNumber();
                    o_LicenseNumber = loopUntilLicenseIsFound(o_LicenseNumber);
                    Console.Clear();
                    Console.WriteLine("Enter 1 to change to 'in repair', 2 to change to 'repaired' or 3 to change to 'paid'");
                    Vehicle.eVehicleStatus o_VehicleStatus = validateUserChoiceForVehicleStatus();
                    r_GarageLogic.ChangeVehicleStatusInGarage(o_LicenseNumber, o_VehicleStatus);
                    Console.WriteLine("Status of Vehicle {0} was changed To {1}\n", o_LicenseNumber, o_VehicleStatus);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine();
                }
            }
            else
            {
                Console.WriteLine("There are no vehicles yet in the system\n");
            }
          
        }


        private string loopUntilLicenseIsFound(string io_LicenseNumber)
        {
            while (checkIfLicenseExist(io_LicenseNumber) == false)
            {
                Console.WriteLine("License number does not appear in the Garage, please try again. ");
                io_LicenseNumber = enterLicenseNumber();
            }

            return io_LicenseNumber;
        }

        private void addFuel()
        {

            if (!isGarageEmpty() && r_GarageLogic.NumberOfFuelVehicles > 0)
            {
                try
                {
                    bool isValid = false;
                    Console.WriteLine("choose one of the License numbers below: ");
                    string o_LicenseNumber;
                    do
                    {
                        printAllFuelVehicleLicense();
                        o_LicenseNumber = enterLicenseNumber();
                        Console.Clear();
                        isValid = r_GarageLogic.isFuelVehicle(o_LicenseNumber);
                        if (!isValid)
                        {
                            Console.WriteLine("Not a valid License Number. Try again...");
                        }

                    } while (isValid == false);

                    Console.WriteLine(
                        "Please enter the Type of Fuel to fill: 1 for Soler, 2 for Octan95, 3 for Octan96, or 4 for Octan98:");
                    string userInput = Console.ReadLine();
                    FuelEngine.eFuleType o_TypeFuelToFill = (FuelEngine.eFuleType)Enum.Parse(typeof(FuelEngine.eFuleType), userInput);
                    Console.WriteLine("Please the amount to add in liters");
                    userInput = Console.ReadLine();
                    float o_AmountFuelToFill = float.Parse(userInput);
                    r_GarageLogic.AddFuelToVehicle(o_LicenseNumber, o_TypeFuelToFill, o_AmountFuelToFill);
                    Console.WriteLine("{0} Liter of {1} were successfully added To Vehicle No. {2}!",
                        o_AmountFuelToFill, o_TypeFuelToFill.ToString(), o_LicenseNumber);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("Please try again...");
                    addFuel();
                }
            }
            else
            {
                Console.WriteLine("Garage is either empty or no Fuel driven Vehicles at the moment");
            }
        }

        private void chargeBattery()
        { 
            if (!isGarageEmpty() && r_GarageLogic.NumberOfElectricVehicles > 0)
            {
                try
                {
                    bool isValid = false;
                    Console.WriteLine("choose one of the License numbers below: ");
                    string o_LicenseNumber;
                    do
                    {
                        printAllElectricVehicleLicense();
                        o_LicenseNumber = enterLicenseNumber();
                        Console.Clear();
                        isValid = r_GarageLogic.isElectricVehicle(o_LicenseNumber);
                        if (!isValid)
                        {
                            Console.WriteLine("Not a valid License Number. Try again...");
                        }

                    } while (isValid == false);
                    Console.WriteLine("Please enter the number of minutes to add");
                    string userInput = Console.ReadLine();
                    float o_AmountHoursToFill = float.Parse(userInput);
                    r_GarageLogic.AddHoursToElectricEngine(o_LicenseNumber, o_AmountHoursToFill / 60);
                    Console.WriteLine("{0} of minutes were charged to Vehicle number {1}", o_AmountHoursToFill, o_LicenseNumber);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("Please try again...");
                    chargeBattery();
                }
            }
            else
            {
                Console.WriteLine("Garage is either empty or no Electric Vehicles at the moment");
            }
        }

        private void printVehicleData()
        {
            Console.WriteLine("choose one of the License numbers below: ");
            printAllVehicleLicense();
            string o_LicenseNumber = enterLicenseNumber();
            Console.Clear();
            string vehicleData = r_GarageLogic.GetVehicleData(o_LicenseNumber);
            Console.WriteLine(vehicleData);
        }

        private Vehicle.eVehicleStatus validateUserChoiceForVehicleStatus()
        {
            Vehicle.eVehicleStatus o_UserChoice = (Vehicle.eVehicleStatus)choiceValidation(1, 3);
            return o_UserChoice;
        }

        private string enterLicenseNumber()
        {
            string o_LicenseNumber = null;
            Console.WriteLine("please enter license number");
            bool isValidLicense = false;

            while(isValidLicense == false)
            {
                o_LicenseNumber = Console.ReadLine();
                if (string.IsNullOrEmpty(o_LicenseNumber) || o_LicenseNumber == " " || o_LicenseNumber.Length < 5 || o_LicenseNumber.Length > 10)
                {
                    Console.WriteLine(@"License number cant be empty! and must be between 8-10 digits. 
Please try again:");
                }

                else
                {
                    isValidLicense = true;
                }
            }

            return o_LicenseNumber;
        }

        private bool checkIfLicenseExist(string i_LicenseNumber)
        {

            bool doesLicenseExist = r_GarageLogic.ListOfVehicles.ContainsKey(i_LicenseNumber);

            return doesLicenseExist;
        }


        private int choiceValidation(int i_MinValue, int i_MaxValue)
        {
            bool isValidChoice = false;
            int o_ChoiceToReturn = -1;
            while (isValidChoice == false)
            {
                
                Console.WriteLine("Choose an action:");
                string choiceAsString = Console.ReadLine();
                isValidChoice = int.TryParse(choiceAsString, out o_ChoiceToReturn);
                if(isValidChoice == false)
                {
                    Console.WriteLine("You did not enter a number, please try again.");
                }
                else if (o_ChoiceToReturn < i_MinValue || o_ChoiceToReturn > i_MaxValue)
                {
                    Console.WriteLine("The number you entered is not between {0} and {1}, please try again", i_MinValue, i_MaxValue);
                    isValidChoice = false;
                }
                else
                {
                    isValidChoice = true;
                }

            }
            return o_ChoiceToReturn;
        }

        private eUserChoice validateUserChoiceForMainMenu()
        {
            eUserChoice o_UserChoice;

            o_UserChoice = (eUserChoice)choiceValidation(1, 8);

            return o_UserChoice;
        }

        private void reciveVehicleType (out VehicleCreator.eVehicleType o_UserChoice)
        {
            Console.WriteLine("Please choose a vehicle type from the list below by selecting the number on the right:");
            try
            {
                int inputAsInt;
                bool isValid = false;
                do
                {
                    printVehicleOptions();
                    string inputAsString = Console.ReadLine();
                    inputAsInt = int.Parse(inputAsString);
                    isValid = Enum.IsDefined(typeof(VehicleCreator.eVehicleType), inputAsInt);
                    if (isValid == false)
                    {
                        Console.WriteLine(@"Value not in range!
Please choose a vehicle type from the list below by selecting the number on the right:");
                    }
                } while (isValid == false);

                o_UserChoice = (VehicleCreator.eVehicleType)inputAsInt;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                reciveVehicleType(out o_UserChoice);
            }
        }

        private void printLicenseNumbers()
        {
            if (r_GarageLogic.ListOfVehicles.Count != 0)
            {
                Console.WriteLine("Enter 1 to Show all vehicles, 0 for print by status");
                int userChoice = choiceValidation(0, 1);

                if (userChoice == 1)
                {
                    printAllVehicleLicense();
                }
                else
                {
                    chooseStatusToPrint();
                }
            }
            else
            {
                Console.WriteLine("There are no vehicles yet in the system\n");
            }
        }

        private void chooseStatusToPrint()
        {
            Console.WriteLine(@"Choose the status of the vehicles to present by the following numbers:
for vehicles that are in repair     press 1
for vehicles that are repaired      press 2
for vehicles that are paid for      press 3");

            Vehicle.eVehicleStatus userChoice = (Vehicle.eVehicleStatus)choiceValidation(1, 3);
            switch (userChoice)
            {
                case Vehicle.eVehicleStatus.InRepair:
                    {
                        printLicenseNumberByStatus(Vehicle.eVehicleStatus.InRepair);
                        break;
                    }
                case Vehicle.eVehicleStatus.Repaired:
                    {
                        printLicenseNumberByStatus(Vehicle.eVehicleStatus.Repaired);
                        break;
                    }
                case Vehicle.eVehicleStatus.Paid:
                    {
                        printLicenseNumberByStatus(Vehicle.eVehicleStatus.Paid);
                        break;
                    }
            }
        }

        private void printLicenseNumberByStatus(Vehicle.eVehicleStatus i_StatusToShow)
        {
            int numberOfVehicles = 0;

            foreach (KeyValuePair<string, Vehicle> vehicle in r_GarageLogic.ListOfVehicles)
            {
                if (vehicle.Value.VehicleStatus == i_StatusToShow)
                {
                    Console.WriteLine(vehicle.Key);
                    numberOfVehicles++;
                }
            }
            
            if (numberOfVehicles == 0)
            {
                Console.WriteLine("No {0} vehicles to Show", i_StatusToShow);
            }
        }


        private void printAllVehicleLicense()
        {
            foreach (KeyValuePair<string, Vehicle> vehicle in r_GarageLogic.ListOfVehicles)
            {
                Console.WriteLine(vehicle.Key);
            }
        }

        private void printAllElectricVehicleLicense()
        {
            if (r_GarageLogic.NumberOfElectricVehicles > 0)
            {
                foreach (KeyValuePair<string, Vehicle> vehicle in r_GarageLogic.ListOfVehicles)
                {
                    if (r_GarageLogic.CheckIfElectricEngine(vehicle.Value) == true)
                    {
                        Console.WriteLine(vehicle.Key);
                    }
                }
            }
            else
            {
                Console.Clear();
                Console.WriteLine("There are no Electric Vehicles in the system at the moment");
            }
  
        }

        private void printAllFuelVehicleLicense()
        {
            if (r_GarageLogic.NumberOfFuelVehicles > 0)
            {
                foreach (KeyValuePair<string, Vehicle> vehicle in r_GarageLogic.ListOfVehicles)
                {
                    if (r_GarageLogic.CheckIfFuelEngine(vehicle.Value) == true)
                    {
                        Console.WriteLine(vehicle.Key);
                    }
                }
            }
            else
            {
                Console.Clear();
                Console.WriteLine("There are no Electric Vehicles in the system at the moment");
            }
        }

        private void printListOfString(List<string> i_StringToPrint)
        {// printListOfString
            foreach (string message in i_StringToPrint)
            {
                Console.WriteLine(message);
            }

            Console.WriteLine(" ");
        }

        private void printMainMenu()
        {
            printListOfString(r_Menu.MenuOptions);
        }

        private void printVehicleOptions() /// UI doesnt know the vehicle options, they are stored and imported from VehicleCreator.cs
        {  
            printListOfString(r_Menu.AddVehicleOptionsMenu);
        }

        private bool isGarageEmpty()
        {
            return r_GarageLogic.ListOfVehicles.Count == 0;      
        }
    }
}