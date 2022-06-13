using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace MyGarage
{
    /// <summary>
    /// Klassen Garage&lt;T&gt; representerar ett garage.
    /// </summary>
    /// <typeparam name="T">Garage kan bara vara av typen Garage&lt;Vehicle%gt;</typeparam>
    [Serializable]
    class Garage<T> : IEnumerable<T> where T : Vehicle
    {

        T[] _spaces; // En array med parkeringsplatser.
        int _count = 0; // Räknare för antal fordon.

       
        public int Size { get { return _spaces.Length; } } // Storleken på garaget.


        public Garage(int size)
        {
            _spaces = new T[size];
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Lägger till ett fordon i garaget.
        /// </summary>
        /// <param name="vehicle">Ett fordon av typen Vehicle.</param>
        /// <returns>Returnerar true om fordonet lades till och false om det inte fanns plats.</returns>
        public bool Add(T vehicle)
        {
            if (FindByRegNum(vehicle.RegNum) != null)
                return false; // Inga dubbletter tillåtna.

            for (int i = 0; i < _spaces.Length; i++)
            {
                // Om det finns en tom plats, lägg till fordonet och returnera true.
                if(_spaces[i] == null)
                {
                    _spaces[i] = vehicle;
                    _count++;
                    return true;
                }
            }

            // Om det inte fanns någon tom plats, returnera false.
            return false;
        }
        // Returnera objektet.
        public bool Remove(string regNum)
        {
            for (int i = 0; i < _spaces.Length; i++)
            {
                if (_spaces[i] != null)
                {
                    if (_spaces[i].RegNum == regNum)
                    {
                        _spaces[i] = null;
                        _count--;
                        return true;
                    }
                }
            }

            return false;
        }

        internal bool Remove(T found)
        {
            for (int i = 0; i < _spaces.Length; i++)
            {
                    if (_spaces[i] == found)
                    {
                        _spaces[i] = null;
                        _count--;
                        return true;
                    }
            }

            return false;
        }


        public T[] GetAll() 
        {
            return _spaces.ToArray(); 
        }

        public int Count()
        {
            return _count;
        }

        public T[] FindByString(string keyword)
        {
            Dictionary<string, Vehicle> vehicles = new Dictionary<string, Vehicle>();

            for (int i = 0; i < _spaces.Length; i++)
            {
                if(_spaces[i] != null)
                {
                    if(_spaces[i].Matches(keyword))
                        vehicles[_spaces[i].RegNum] = _spaces[i];
                }
            }

            T[] v = new T[vehicles.Count];
            vehicles.Values.CopyTo(v, 0);
            return v;
        }

        public T FindByRegNum(string regNum)
        {
            for (int i = 0; i < _spaces.Length; i++)
            {
                if(_spaces[i] != null)
                    if (_spaces[i].RegNum == regNum)
                        return _spaces[i];
            }

            return null;
        }

        public void Serialize()
        {
            string filePath = Path.Combine(System.IO.Path.GetTempPath(), "GarageData.dat");

            using (FileStream fs = new FileStream(filePath, FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(fs, this);               
            }
        }

        public Garage<Vehicle> DeSerialize()
        {
            Garage<Vehicle> vehicles = null;
            string filePath = Path.Combine(System.IO.Path.GetTempPath(), "GarageData.dat");

            using (FileStream fs = new FileStream(filePath, FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                vehicles = (Garage<Vehicle>)formatter.Deserialize(fs);      
            }
            return vehicles;
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < _spaces.Length; i++)
                if (_spaces[i] != null) yield return _spaces[i];
        }
    }
}
