
namespace MyGarage
{
    class Program
    {
        static void Main(string[] args)
        {
            // Instansiera ett UserInterface.
            UserInterface ui = new UserInterface();

            // Om användaren skapar eller läser in ett garage, starta användargränssnittet.
            while (ui.Create())
                ui.Start();
        }
    }
}
