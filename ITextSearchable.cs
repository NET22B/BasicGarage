using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGarage
{
    /// <summary>
    /// ITextSearchable måste impmlementeras för att göra klasser sökbara via ett keyword.
    /// </summary>
    interface ITextSearchable
    {
        bool Matches(string keyword);
    }
}
