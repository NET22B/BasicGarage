using System;

namespace MyGarage
{
    [Serializable]
    class Vehicle : ITextSearchable
    {
        string _regNum;
        string _color;
        int _wheelCount;

        public string RegNum { get { return _regNum; }  }
        public string Color { get { return _color; }  }
        public int WheelCount { get { return _wheelCount; }  }

        public Vehicle(string regNum, string color, int wheelCount)
        {
            _regNum = regNum;
            _color = color;
            _wheelCount = wheelCount;
        }

        public Vehicle(Vehicle c)
        {
            _regNum = c.RegNum;
            _color = c.Color;
            _wheelCount = c.WheelCount;
        }

        public override string ToString()
        {
            return string.Format("Reg.nr: {0}, Färg: {1}, Antal hjul: {2}", _regNum, _color, _wheelCount);
        }

        public virtual bool Matches(string keyword)
        {
            if (_color == keyword || _wheelCount.ToString() == keyword)
                return true;

            return false;
        }
    }
}
