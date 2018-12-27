using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab4
{
    class Program
    {
        static void Main(string[] args)
        {
            // Список кінотеатрів
            List<Cinema> cinemas = new List<Cinema>()
            {
                new Cinema(){ID = 1, Name = "Oscar"},
                new Cinema(){ID = 2, Name ="Multiplex"},
                new Cinema(){ID = 3, Name = "Butterfly"},
                new Cinema(){ID = 4, Name = "Ukraine"}
            };

            //Райони Києва
            List<District> districts = new List<District>()
            {
                new District() { ID = 1, Name = "Darnytskyi"},
                new District() { ID = 2, Name = "Desnianskyi"},
                new District() { ID = 3, Name = "Dniprovskyi"},
                new District() { ID = 4, Name = "Holosiivskyi"},
                new District() { ID = 5, Name = "Obolonskyi"},
                new District() { ID = 6, Name = "Pecherskyi"},
                new District() { ID = 7, Name = "Podilskyi"},
                new District() { ID = 8, Name = "Shevchenkivskyi"},
                new District() { ID = 9, Name = "Solomianskyi"},
                new District() { ID = 10, Name = "Sviatoshynskyi"}
            };

            //Опції, які можуть бути доступними для кінотеатру
            List<Option> options = new List<Option>()
            {
                new Option() { ID = 1, Name = "Widescreen movies"},
                new Option() { ID = 2, Name = "Video films"},
                new Option() { ID = 3, Name = "3D"},
                new Option() { ID = 4, Name = "IMAX"},
                new Option() { ID = 5, Name = "Room for children"},
                new Option() { ID = 6, Name = "Stereo"},
                new Option() { ID = 7, Name = "5D"},
                new Option() { ID = 8, Name = "VIP room"}
            };

            //Інформація про кожен кінотеатр (Кількість місць, рік побудови та район, в якому розташований кінотеатр
            //Реалізується зв'язок багато до одного
            List<InfoAboutCinema> cinemasInformation = new List<InfoAboutCinema>()
            {
                new InfoAboutCinema() { IDCinema = 1, NumberOfSeats = 400, Year = "2010", IDDistrict = 1},
                new InfoAboutCinema() { IDCinema = 2, NumberOfSeats = 450, Year = "2013", IDDistrict = 5},
                new InfoAboutCinema() { IDCinema = 3, NumberOfSeats = 320, Year = "2005", IDDistrict = 9},
                new InfoAboutCinema() { IDCinema = 4, NumberOfSeats = 200, Year = "1995", IDDistrict = 1}
            };

            //Опції, доступні для певного кінотеатру (ранг кінотеатру)
            //Реалізується зв'язок багато до багатьох
            List<CinemaOptions> cinemasOptions = new List<CinemaOptions>()
            {
                new CinemaOptions() { IDCinema = 1, IDOption = 1},
                new CinemaOptions() { IDCinema = 1, IDOption = 2},
                new CinemaOptions() { IDCinema = 1, IDOption = 3},
                new CinemaOptions() { IDCinema = 1, IDOption = 4},
                new CinemaOptions() { IDCinema = 1, IDOption = 5},
                new CinemaOptions() { IDCinema = 1, IDOption = 6},
                new CinemaOptions() { IDCinema = 1, IDOption = 8},

                new CinemaOptions() { IDCinema = 2, IDOption = 1},
                new CinemaOptions() { IDCinema = 2, IDOption = 2},
                new CinemaOptions() { IDCinema = 2, IDOption = 3},
                new CinemaOptions() { IDCinema = 2, IDOption = 5},
                new CinemaOptions() { IDCinema = 2, IDOption = 6},
                new CinemaOptions() { IDCinema = 2, IDOption = 7},

                new CinemaOptions() { IDCinema = 3, IDOption = 2},
                new CinemaOptions() { IDCinema = 3, IDOption = 3},
                new CinemaOptions() { IDCinema = 3, IDOption = 5},
                new CinemaOptions() { IDCinema = 3, IDOption = 6},

                new CinemaOptions() { IDCinema = 4, IDOption = 1},
                new CinemaOptions() { IDCinema = 4, IDOption = 2},
                new CinemaOptions() { IDCinema = 4, IDOption = 3},
                new CinemaOptions() { IDCinema = 4, IDOption = 4},
                new CinemaOptions() { IDCinema = 4, IDOption = 6},
                new CinemaOptions() { IDCinema = 4, IDOption = 7}

            };

            //Показати всі кінотеатри
            Console.WriteLine("All cinemas:");

            var allCinemas = from c in cinemas
                     orderby c.ID
                     select new { id = c.ID, name = c.Name };

            foreach (var c in allCinemas)
            {
                Console.WriteLine($"{c.id}. {c.name}");
            }

            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine();
            //==========================================================================================

            //Показати всі кінотеатри та інформацію про них
            Console.WriteLine("All cinemas and information (using join):");

            var allCinemasInfo = from i in cinemasInformation
                                 join c in cinemas on i.IDCinema equals c.ID
                                 join d in districts on i.IDDistrict equals d.ID
                                 orderby i.IDCinema
                                 select new { cinemaID = i.IDCinema, cinemaName = c.Name, numberOfSeats = i.NumberOfSeats, year = i.Year, district = d.Name };

            foreach (var info in allCinemasInfo)
            {
                Console.WriteLine($"{info.cinemaID}. {info.cinemaName}. {info.numberOfSeats}. {info.year}. {info.district}");
            }

            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine();
            //==========================================================================================

            //Показати всі райони, назви яких починаються на D
            Console.WriteLine("Districts what name starts with \'D\':");

            var districtsBeginsWithD = from d in districts
                                       where d.Name.StartsWith("D")
                                       select d;

            foreach (var distr in districtsBeginsWithD)
            {
                Console.WriteLine($"{distr.ID}. {distr.Name}");
            }

            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine();
            //==========================================================================================

            //Показати всі кінотеатри, згруповані по району
            //Використаємо, сформований вище allCinemasInfo
            Console.WriteLine("All cinemas, grouped by district(using join and groupby):");

            var groupedCinemasByDistrict = from i in allCinemasInfo
                                           group i by i.district into g
                                           select new { key = g.Key, value = g };

            foreach (var gc in groupedCinemasByDistrict)
            {
                Console.WriteLine("{0}:", gc.key);
                foreach (var cinema in gc.value)
                {
                    Console.WriteLine($"    -{cinema.cinemaName}");
                }
            }

            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine();
            //==========================================================================================

            //Показати всі кінотеатри та їх опції
            Console.WriteLine("All cinemas and its options(using join and groupby):");

            var allCinemasOptions = from co in cinemasOptions
                                    join c in cinemas on co.IDCinema equals c.ID
                                    join o in options on co.IDOption equals o.ID
                                    orderby co.IDCinema
                                    select new { cinemaID = co.IDCinema, cinemaName = c.Name, option = o.Name };

            var optionForEachCinema = from co in allCinemasOptions
                                      group co by co.cinemaName into g
                                      select new { key = g.Key, value = g };

            foreach (var co in optionForEachCinema)
            {
                Console.WriteLine("{0}:", co.key);
                foreach (var option in co.value)
                {
                    Console.WriteLine($"    -{option.option}");
                }
            }

            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine();
            //==========================================================================================

            //Показати всі кінотеатри та кількість опцій в них
            Console.WriteLine("All cinemas and number of its options(using join and groupby):");

            var optionCountForEachCinema = from co in allCinemasOptions
                                           group co by co.cinemaName into g
                                           select new { key = g.Key, count = g.Count() };

            foreach (var co in optionCountForEachCinema)
            {
                Console.WriteLine("{0}: {1}", co.key, co.count);
            }

            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine();
            //==========================================================================================

            //Показати кінотеатри, які мають кімнату для дітей
            Console.WriteLine("All cinemas with room for children:");

            var cinemasWithRoomForChildren = from co in cinemasOptions
                                             join c in cinemas on co.IDCinema equals c.ID
                                             join o in options on co.IDOption equals o.ID
                                             where co.IDOption == 5
                                             orderby co.IDCinema
                                             select new { cinemaID = co.IDCinema, cinemaName = c.Name};
            foreach (var cinema in cinemasWithRoomForChildren)
            {
                Console.WriteLine($"{cinema.cinemaID}. {cinema.cinemaName}.");
            }
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine();
            //==========================================================================================


            Console.WriteLine("Press any key...");
            Console.Read();

        }
    }
}
