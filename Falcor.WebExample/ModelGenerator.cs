using System;
using System.Collections.Generic;
using System.Linq;

namespace Falcor.WebExample
{
    public class ModelGenerator
    {
        public static Model Generate()
        {
            var random = new Random();
            var result = new Model();
            result.Countries = _countries
                .Select(i => new Model.Country
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = i,
                    Description = $"Name of the country is {i}"
                })
                .ToList();
            result.Events = _eventNames
                .Select(i => new { random = random.Next(1000), item = i})
                .OrderBy(i => i.random)
                .Select((i, idx) => new Model.Event
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = i.item,
                    Number = idx,
                    Country = result.Countries[random.Next(result.Countries.Count - 1)]
                })
                .ToList();

            return result;
        }

        private static readonly List<string> _eventNames = new List<string>
        {
            "Afrobasket",
            "Afrobasket Women",
            "FIBA Africa U16 Men",
            "South American League for Women",
            "FIBA Europe Cup",
            "U18 European Championship Women",
            "FIBA Asia U16 Championship for Women",
            "Centrobasket U17 Championship for Men",
            "U16 European Championship Men",
            "FIBA Central Board",
            "FIBA Americas Championship for Women",
            "U16 European Championship Women",
            "FIBA Oceania U16 Championship for Women",
            "FIBA Oceania Championship for Women",
            "FIBA Oceania Championship for Men",
            "FIBA Oceania U16 Championship for Men",
            "FIBA Asia Championship for Women",
            "FIBA Americas Championship for Men/Olympic Qualifying Tournament",
            "EuroCup Women",
            "EuroLeague Women",
            "All Africa Games: Tournament for Men",
            "All Africa Games: Tournament for Women",
            "FIBA Asia Championship",
            "FIBA Africa Champions Cup for Women",
            "FIBA Asia Champions Cup for Men's Clubs",
            "Military World Games",
            "FIBA Europe Youth Commission",
            "FIBA Asia U16 Championship for Men",
            "South American League for Women's Clubs",
            "South East Asian Games: Tournament for Men",
            "South East Asian Games: Tournament for Women",
            "FIBA Finance Commission",
            "FIBA Central Board",
            "Board of FIBA-Europe",
            "Draw for the 2nd FIBA U17 World Championship for Men",
            "Draw for the 2nd FIBA U17 World Championship for Women",
            "Draw for the Olympic Games Rio 2016",
            "Olympic Test Event",
            "NCAA Men's Final Four",
            "FIBA Asia U18 Championship for Women",
            "FIBA Oceania U18 Championship for Men",
            "FIBA Oceania U18 Championship for Women",
            "FIBA Olympic Qualifying Tournament for Women",
            "FIBA U17 Women’s World Championship",
            "Afrobasket U18 Women",
            "Afrobasket U18",
            "FIBA Olympic Qualifying Tournament for Men",
            "FIBA U17 World Championship",
            "FIBA Americas U18 Championship for Men",
            "Olympic Games: Tournament for Men",
            "Olympic Games: Tournament for Women",
            "FIBA Americas U18 Championship for Women",
            "FIBA Asia U18 Championship for Men",
            "Asian Beach Games",
            "Draw for the 12th FIBA U19 World Championship for Women",
            "Draw for the 13th FIBA U19 World Championship for Men",
            "FIBA Central Board",
            "NCAA Men's Final Four",
            "NCAA Women's Final Four",
            "FIBA Asia Champions Cup for Men",
            "FIBA Asia Championship for Women",
            "FIBA Asia U16 Championship for Men",
            "FIBA Asia U16 Championship for Women",
            "South American Championship for Men",
            "Mediterranean Games: Tournament for Men",
            "Mediterranean Games: Tournament for Women",
            "Afrobasket U16",
            "Afrobasket U16 Women",
            "Asian Youth Games for Men",
            "Asian Youth Games for Women",
            "FIBA Asia Championship for Men",
            "FIBA U19 World Championship",
            "FIBA U19 Women’s World Championship",
            "Francophonian Games",
            "Afrobasket",
            "FIBA Americas Championship for Men",
            "FIBA Americas U16 Championship for Men",
            "FIBA Americas U16 Championship for Women",
            "FIBA Americas U18 Championship for Men",
            "FIBA Oceania U16 Championship for Men",
            "FIBA Oceania U16 Championship for Women",
            "Universiade",
            "Afrobasket Women",
            "FIBA Americas Championship for Women",
            "FIBA Americas U18 Championship for Women",
            "FIBA Oceania Championship for Men",
            "FIBA Oceania Championship for Women",
            "FIBA Africa Champions Cup for Men",
            "FIBA Africa Champions Cup for Women",
            "South East Asian Games: Tournament for Men",
            "South East Asian Games: Tournament for Women",
            "FIBA Central Board",
            "Commonwealth Games: Tournament for Men",
            "FIBA Asia U18 Championship for Women",
            "FIBA Oceania U18 Championship for Men",
            "FIBA Oceania U18 Championship for Women",
            "FIBA U17 World Championship",
            "Afrobasket U18",
            "Afrobasket U18 Women",
            "FIBA U17 Women’s World Championship",
            "Youth Olympic Games for Men",
            "Youth Olympic Games for Women",
            "FIBA Asia U18 Championship for Men",
            "FIBA Women's Basketball World Cup",
            "Asian Beach Games",
            "Asian Games: Tournament for Men",
            "Asian Games: Tournament for Women",
            "FIBA Basketball Wold Cup"
        };

	    private static readonly List<string> _countries = new List<string> 
	    {
			"USA",
			"Spain",
			"Argentina",
			"Lithuania",
			"France",
			"Serbia",
			"Turkey",
			"Brazil",
			"Greece",
			"Australia",
			"Croatia",
			"Slovenia",
			"China",
			"Puerto Rico",
			"Angola",
			"Iran",
			"Germany",
			"Mexico",
			"Dominican Republic",
			"New Zealand",
			"Great Britain",
			"Tunisia",
			"Nigeria",
			"Canada",
			"Uruguay",
			"Venezuela",
			"Korea",
			"Jordan",
			"Senegal",
			"Philippines",
			"MKD",
			"Panama",
			"Lebanon",
			"Finland",
			"Italy",
			"Cote d'Ivoire",
			"Israel",
			"Latvia",
			"Ukraine",
			"Egypt",
			"Poland",
			"Cameroon",
			"Chinese Taipei",
			"Paraguay",
			"Virgin Islands",
			"Japan",
			"Qatar",
			"Bulgaria",
			"Czech Republic"
	    };
    }
}