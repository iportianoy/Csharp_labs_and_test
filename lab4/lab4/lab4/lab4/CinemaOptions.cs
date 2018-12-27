using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab4
{
    class CinemaOptions
    {
        public int IDCinema { get; set; }
        public int IDOption { get; set; }

        internal Cinema Cinema
        {
            get => default(Cinema);
            set
            {
            }
        }

        internal Option Option
        {
            get => default(Option);
            set
            {
            }
        }
    }
}
