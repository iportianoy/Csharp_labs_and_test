using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab4
{
    class InfoAboutCinema
    {
        public int IDCinema { get; set; }
        public int NumberOfSeats { get; set; }
        public string Year { get; set; }
        public int IDDistrict { get; set; }

        internal Cinema Cinema
        {
            get => default(Cinema);
            set
            {
            }
        }

        internal District District
        {
            get => default(District);
            set
            {
            }
        }
    }
}
