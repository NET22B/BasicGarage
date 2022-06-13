using System;
using System.IO;
using System.Linq;
using System.Security;
using System.Text.RegularExpressions;

namespace MyGarage
{
    class UserInterface
    {
        // En array för användarmenyn.
        string[] _menuItems = {
                    "Avsluta", // Måste vara först i arrayen.
                    "Parkera i garaget", // 1
                    "Kör ut från garaget", // 2
                    "Sök på registreringsnummer", // 3
                    "Textsök", // 4
                    "Lista fordon av typ", // 5
                    "Lista alla fordon", // 6
                    "Visa beläggningsgrad", // 7
                    "Spara garaget till fil", // 8
                    "Läs in garage från fil" // 9
                };

        // En array med fordonstyper.
        string[] _vehicleTypes = {
                    "Avbryt",
                    "Bil",
                    "Buss",
                    "Båt",
                    "Flygplan",
                    "Motorcykel"
                };
        
        GarageManager _garageManager;

        //public UserInterface()
        //{
        //}

        //public UserInterface(GarageManager gm)
        //{
        //    _garageManager = gm;
        //}

        /// <summary>
        /// Skapar eller läser in ett garage från disk.
        /// </summary>
        /// <returns>Returnerar false om användaren väljer att avsluta.</returns>
        public bool Create()
        {
            bool inputOk = false;
            
            while (!inputOk)
            {
                Console.Clear();
                Console.Write(
                    "{0}Hantera garage{0}{0}" +
                    " 0) Avsluta{0}" +
                    " 1) Skapa ett nytt garage{0}" +
                    " 2) Läs in ett sparat garage{0}{0}" +
                    "> ",
                    Environment.NewLine
                    );

                switch (Console.ReadKey(true).KeyChar)
                {
                    case '0':
                        Console.Clear();
                        return false;
                    case '1':
                        Console.Clear();
                        int size = promptForNumberInput("Ange hur många platser garaget skall ha: ");
                        if (size > 0)
                        {
                            _garageManager = new GarageManager(size);
                            inputOk = true;
                        }

                        Console.WriteLine("Antalet platser måste vara ett heltal större än 0.");
                        break;
                    case '2':
                        _garageManager = new GarageManager(1);
                        if(LoadGarage())
                            inputOk = true;
                        promptForAnyKey();
                        break;
                    default:
                        ShowInvalidInput();
                        break;
                }
            }
            return true;
        }

        /// <summary>
        /// Skriver ut programmets huvudmeny och anropar metoder för respektive menyval.
        /// </summary>
        public void Start()
        {
            while (true)
            {
                ShowMenu();
                switch(Console.ReadKey(true).KeyChar)
                {
                    case '0':
                        return; // Avslutar programmet.
                    case '1': // Parkera.
                        ParkVehicle();
                        promptForAnyKey();
                        break;
                    case '2': // Kör ut.
                        LeaveGarage();
                        promptForAnyKey();
                        break;
                    case '3': // Sök regnr.
                        FindVehicleByRegNum();
                        promptForAnyKey();
                        break;
                    case '4': // Sök på text.
                        FindVehicle();
                        promptForAnyKey();
                        break;
                    case '5': // Lista typ.
                        FindVehicleByType();
                        promptForAnyKey();
                        break;
                    case '6': // Lista alla.
                        Console.Clear();
                        foreach(string space in _garageManager.GetAllSpaces())
                            Console.WriteLine(space);
                        promptForAnyKey();
                        break;
                    case '7': // Visa beläggning.
                        Console.Clear();
                        Console.WriteLine(_garageManager.GetStatistics());
                        promptForAnyKey();
                        break;
                    case '8':
                        SaveGarage();
                        promptForAnyKey();
                        break;
                    case '9':
                        LoadGarage();
                        promptForAnyKey();
                        break;
                    default:
                        ShowInvalidInput();
                        break;
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Läser in ett garage från fil.
        /// </summary>
        /// <returns>Returnerar true om inläsningen lyckades.</returns>
        private bool LoadGarage()
        {
            bool success = false;

            try
            {
                _garageManager.LoadGarage();
                success = true;
            }
            catch (SecurityException ex)
            {
                Console.Error.WriteLine("Behörighet saknas för att läsa in garage: {0}", ex.Message);
            }
            catch(FileNotFoundException ex)
            {
                Console.Error.WriteLine("Filen kan inte hittas: {0}", ex.Message);
            }
            catch(IOException ex)
            {
                Console.Error.WriteLine("Misslyckades att läsa filen: {0}", ex.Message);
            }

            return success;
        }

        /// <summary>
        /// Sparar garaget till fil.
        /// </summary>
        /// <returns>Returnerar true om det gick att spara garaget.</returns>
        private bool SaveGarage()
        {
            bool success = false;

            try
            {
                _garageManager.SaveGarage();
                success = true;
            }
            catch (SecurityException ex)
            {
                Console.WriteLine("Behörighet saknas för att spara garage: {0}", ex.Message);
            }
            catch (FileNotFoundException ex)
            {
                Console.Error.WriteLine("Filen kan inte hittas: {0}", ex.Message);
            }
            catch (IOException ex)
            {
                Console.Error.WriteLine("Misslyckades att skriva filen: {0}", ex.Message);
            }

            return success;
        }

        /// <summary>
        /// Söker efter fordon på alla attibut förutom reg.nr.
        /// </summary>
        private void FindVehicle()
        {
            Console.Clear();
            string input = promptForStringInput("Skriv in sökord: ");
            
            var vehicles = _garageManager.FindVehicleByString(input);
            if(vehicles.Count() /*.ToArray().Length*/ > 0)
                foreach (string v in vehicles)
                    Console.WriteLine(v);
            else
                Console.WriteLine("Inga sökträffar.");
        }

        /// <summary>
        /// Söker fordon efter typ.
        /// </summary>
        private void FindVehicleByType()
        {
            string input;
            Vehicle[] vehicles;

            Console.Clear();

            while(true)
            {
                ShowVehicleTypeSelectMenu("Välj fordonstyp att söka efer.");
                char c = Console.ReadKey(true).KeyChar;

                switch (c)
                {
                    case '0':
                        return; // Tillbaka till huvudmenyn.
                    case '1':
                        input = "Car";
                        break;
                    case '2':
                        input = "Bus";
                        break;
                    case '3':
                        input = "Boat";
                        break;
                    case '4':
                        input = "Airplane";
                        break;
                    case '5':
                        input = "Motorcycle";
                        break;
                    default:
                        ShowInvalidInput();
                        continue;
                }

                Console.WriteLine();
                if (input.Length > 0)
                {
                    vehicles = _garageManager.FindVehicleByType(input);

                    if (vehicles.Length > 0)
                        foreach (var vehicle in vehicles)
                            Console.WriteLine(vehicle);
                    else
                    {
                        Console.WriteLine("Inga fordon av typen '{0}' finns i garaget.",
                            _vehicleTypes[(int)char.GetNumericValue(c)]);
                    }
                    promptForAnyKey();
                }

            }
        }

        /// <summary>
        /// Sök fordon på reg.nr.
        /// </summary>
        private void FindVehicleByRegNum()
        {
            string input;
            Vehicle vehicle;

            Console.Clear();

            string prompt = "Skriv det reg.nr. du vill söka efter: ";

            do
            {
                input = promptForStringInput(prompt);
                if (!string.IsNullOrWhiteSpace(input))
                {
                    vehicle = _garageManager.FindVehicleByRegNum(input.ToUpper());
                    //Console.WriteLine(vehicle != null ? vehicle.ToString() : "Fordonet '{0}' finns inte i garaget.", input);
                    if(vehicle != null)
                        Console.WriteLine(vehicle);
                    else
                        Console.WriteLine("Fordonet '{0}' finns inte i garaget.", input);
                }

            } while (string.IsNullOrWhiteSpace(input));
        }

        /// <summary>
        /// Kör ut ett fordon från garaget.
        /// </summary>
        private void LeaveGarage()
        {
            string input;

            Console.Clear();

            string prompt = string.Format(
                "Vilket reg.nr. har fordonet som ska lämna garaget?{0}" +
                "(Tryck ENTER för att se alla fordon)> ",
                Environment.NewLine);

            do {
                input = promptForStringInput(prompt);
                if (string.IsNullOrWhiteSpace(input))
                {
                    foreach (string space in _garageManager.GetAllVehicles())
                        Console.WriteLine(space);
                }
                else
                {
                    if (_garageManager.DriveOut(input.ToUpper()))
                    {
                        Console.WriteLine("Fordonet '{0}' lämnade garaget.", input);
                    }
                    else
                    {
                        Console.WriteLine("Misslyckades eftersom fordonet '{0}' inte finns i garaget.", input);
                        input = "";
                    }
                }
            } while(string.IsNullOrWhiteSpace(input));
        }

        /// <summary>
        /// Parkera ett fordon i garaget.
        /// </summary>
        private void ParkVehicle()
        {
            int n1, n2;
            bool b1, b2;
            Vehicle vehicleParams;

            if(!_garageManager.HasFreeSpace())
            {
                Console.WriteLine(Environment.NewLine + "Garaget är fullt.");
                return;
            }

            ShowVehicleTypeSelectMenu("Vilken typ av fordon vill du parkera?");

            switch(Console.ReadKey(true).KeyChar)
            {
                case '0':
                    return; // Tillbaka till huvudmenyn.
                case '1': // Bil
                    vehicleParams = GetCommonVehicleParams();
                    while ((n1 = promptForNumberInput("Cylinderantal: ")) == -1) ;
                    while ((n2 = promptForNumberInput("Cylindervolym: ")) == -1) ;
                    _garageManager.ParkVehicle(new Car(vehicleParams, n1, n2));
                    break;
                case '2': // Buss
                    vehicleParams = GetCommonVehicleParams();
                    while ((n1 = promptForNumberInput("Antal sittplatser: ")) == -1) ;
                    b1 = promptForBoolInput("Dubbeldeckare (J/N): ");
                    _garageManager.ParkVehicle(new Bus(vehicleParams, n1, b1));
                    break;
                case '3': // Båt
                    vehicleParams = GetCommonVehicleParams();
                    b1 = promptForBoolInput("Passagerarfartyg (J/N): ");
                    b2 = promptForBoolInput("Segelfartyg (J/N): ");
                    _garageManager.ParkVehicle(new Boat(vehicleParams, b1, b2));
                    break;
                case '4': // Flygplan
                    vehicleParams = GetCommonVehicleParams();
                    while ((n1 = promptForNumberInput("Vingbredd: ")) == -1) ;
                    b1 = promptForBoolInput("Jetplan (J/N): ");
                    _garageManager.ParkVehicle(new Airplane(vehicleParams, n1, b1));
                    break;
                case '5': // Motorcykel
                    vehicleParams = GetCommonVehicleParams();
                    while ((n1 = promptForNumberInput("Toppfart: ")) == -1) ;
                    b1 = promptForBoolInput("Sidovagn (J/N): ");
                    _garageManager.ParkVehicle(new Motorcycle(vehicleParams, n1, b1));
                    break;
                default:
                    ShowInvalidInput();
                    break;
            }
        }

        /// <summary>
        /// Visar meny för att välja fordonstyp.
        /// </summary>
        /// <param name="prompt">En sträng med ledtext för prompten.</param>
        private void ShowVehicleTypeSelectMenu(string prompt)
        {
            Console.Clear();
            Console.WriteLine(
                "{0}{1}{0}",
                Environment.NewLine, prompt);

            for (int i = 1; i < _vehicleTypes.Length; i++)
            {
                Console.WriteLine(" {0}) {1}", i, _vehicleTypes[i]);
            }
            Console.WriteLine(" {0}) {1}", 0, _vehicleTypes[0]);

            Console.Write(Environment.NewLine + "> ");
        }

        /// <summary>
        /// Ber användaren att trycka valfri tangent.
        /// </summary>
        private void promptForAnyKey()
        {
            Console.WriteLine("Tryck valfri tangent för att fortsätta.");
            Console.ReadKey(true);
        }

        /// <summary>
        /// Ber användaren att svara ja/nej.
        /// </summary>
        /// <param name="prompt">En sträng med den fråga som behöver svar.</param>
        /// <returns>Returnerar true om användaren svarat ja.</returns>
        private bool promptForBoolInput(string prompt)
        {
            string input;
            Console.Write(prompt);
            input = Console.ReadLine().ToUpper();
            if(input == "Y" || input == "YES" || input == "J" || input == "JA")
                return true;

            return false;
        }

        /// <summary>
        /// Ber användaren om textinput.
        /// </summary>
        /// <param name="prompt">Ledtext för prompten.</param>
        /// <returns>Texten som användaren skrev in som string.</returns>
        private string promptForStringInput(string prompt)
        {
            Console.Write(prompt);
            return Console.ReadLine();

        }

        /// <summary>
        /// Ber användaren om numerisk input.
        /// </summary>
        /// <param name="prompt"></param>
        /// <returns></returns>
        private int promptForNumberInput(string prompt)
        {
            string input;
            Console.Write(prompt);
            input = Console.ReadLine();
            try
            {
                return int.Parse(input);
            }
            catch (ArgumentNullException ex)
            {
                Console.Error.WriteLine("Du måste ange ett värde: {0}", ex.Message);
            }
            catch (FormatException ex)
            {
                Console.Error.WriteLine("Du kan endast ange siffror: {0}", ex.Message);
            }
            catch (OverflowException ex)
            {
                Console.Error.WriteLine("Du har angivit ett för stort tal: {0}", ex.Message);
            }

            return -1;
        }

        /// <summary>
        /// Ber användaren skriva in basinformation som är gemensam för alla fordonstyper.
        /// </summary>
        /// <returns>Returnerar ett Vehicle.</returns>
        private Vehicle GetCommonVehicleParams()
        {
            string input;
            string regNum;
            int wheelCount = 0;
            string color;
            bool validateOK = false;

            Console.Clear();
            Console.WriteLine(Environment.NewLine + "Skriv in uppgifter om fordonet");
            do {
                Console.Write("Registreringsnummer: ");
                input = Console.ReadLine().ToUpper();
                validateOK = ValidateRegNum(input);
                if(!validateOK && _garageManager.FindVehicleByRegNum(input) == null)
                    Console.WriteLine("'{0}' är inte ett giltigt registreringsnummer.", input);
            } while(!validateOK);
            regNum = input;

            validateOK = false;
            do
            {
                Console.Write("Antal hjul: ");
                input = Console.ReadLine();
                try
                {
                    wheelCount = int.Parse(input);
                    validateOK = true;
                }
                catch(ArgumentNullException ex)
                {
                    Console.Error.WriteLine("Du måste ange ett värde: {0}", ex.Message);
                }
                catch(FormatException ex)
                {
                    Console.Error.WriteLine("Du kan endast ange siffror: {0}", ex.Message);
                }
                catch (OverflowException ex)
                {
                    Console.Error.WriteLine("Du har angivit ett för stort tal: {0}", ex.Message);
                }

            } while (!validateOK);

            validateOK = false;
            do
            {
                Console.Write("Färg: ");
                input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input))
                    Console.WriteLine("Du måste ange en färg.");
                else
                    validateOK = true;

            } while (!validateOK);
            color = input;

            return new Vehicle(regNum, color, wheelCount);
        }

        /// <summary>
        /// Visar programmets huvudmeny.
        /// </summary>
        private void ShowMenu()
        {
            Console.Clear();
            Console.WriteLine(
                "{0}Välkommen till \"Garaget\"{0}{0}" +
                "Huvudmeny{0}" +
                "=========",
                Environment.NewLine);

            for (int i = 1; i < _menuItems.Length; i++)
            {
                Console.WriteLine(" {0}) {1}", i, _menuItems[i]);
            }
            Console.WriteLine(" {0}) {1}", 0, _menuItems[0]);

            Console.Write(Environment.NewLine + "> ");
        }

        /// <summary>
        /// Visar felmeddelande om ogiltigt val och väntar på att valfri tangent trycks.
        /// </summary>
        private void ShowInvalidInput()
        {
            Console.WriteLine(Environment.NewLine + "Ogiltigt val, tryck valfri tangent för att fortsätta.");
            Console.ReadKey(true);
        }

        /// <summary>
        /// Validerar registreringsnummer.
        /// </summary>
        /// <param name="regNum">Registreringsnummmer i versaler.</param>
        /// <returns>Returnerar true om registreringsnummeret är giltigt.</returns>
        private bool ValidateRegNum(string regNum)
        {
            Regex regNumSeStdRegexp = new Regex(@"[A-HJ-PR-UW-Z]{3}\d{3}$"); // Tre bokstäver, A-Z förutom I, Q & V följt av tre siffror 0-9.
            Regex regNumSePersonalRegexp = new Regex(@"^([ A-Ö0-9]{2,7}|(?![A-Z]{3}\d{3}))$"); // Två till sju tecken, A-Ö, 0-9 samt blanktecken.

            if(regNumSeStdRegexp.IsMatch(regNum))
                return true;
            //else if(regNumSePersonalRegexp.IsMatch(regNum))
            //    return true;
            else
                return false;
        }
    }
}
