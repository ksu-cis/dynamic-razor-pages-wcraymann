using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Movies.Pages
{
    public class IndexModel : PageModel
    {
        /// <summary>
        /// Movies to be displayed on the Index page.
        /// </summary>
        public IEnumerable<Movie> Movies { get; protected set; }

        /// <summary>
        /// The search terms for the Movie search form on the Index page of the Cowboy Cafe website.
        /// </summary>
        public string SearchTerms { get; set; }

        /// <summary>
        /// The MPAA ratings for the Movie search form on the Index page of the Cowboy CAfe website.
        /// </summary>
        public string[] MPAARatings { get; set; }

        /// <summary>
        /// The Major Genres for the Movie search form on the Index page of the Cowboy CAfe website.
        /// </summary>
        public string[] MajorGenres { get; set; }

        /// <summary>
        /// Stores the minimum IMDB value for the list of movies the user is interested in.
        /// </summary>
        public double? IMDBMin { get; set; }

        /// <summary>
        /// Stores the maximum IMDB value for the list of movies the user is interested in.
        /// </summary>
        public double? IMDBMax { get; set; }

        /// <summary>
        /// The minimum Rotten Tomatoes value for the list of movies the user is interested in.
        /// </summary>
        public double? RotTomMin { get; set; }

        /// <summary>
        /// The maximumn Rotten Tomatoes value for the list of movies the user is interested in.
        /// </summary>
        public double? RotTomMax { get; set; }

        /// <summary>
        /// Changes the value of the user's search terms  and creates a list of movies fitting the new search terms when the 
        /// user submits the Movie search form on the Index page of the Cowboy Cafe website.
        /// </summary>
        public void OnGet(double? IMDBMin, double? IMDBMax, string[] MPAARatings, string[] MajorGenres, double? 
            RotTomMin, double? RotTomMax, string SearchTerms)
        {
            this.IMDBMin = IMDBMin;
            this.IMDBMax = IMDBMax;
            this.MPAARatings = MPAARatings;
            this.MajorGenres = MajorGenres;
            this.RotTomMin = RotTomMin;
            this.RotTomMax = RotTomMax;
            this.SearchTerms = SearchTerms;
            Movies = MovieDatabase.Search(SearchTerms);
            Movies = MovieDatabase.FilterByMPAARating(Movies, MPAARatings);
            Movies = MovieDatabase.FilterByMajorGenre(Movies, MajorGenres);
            Movies = MovieDatabase.FilterByIMDBRating(Movies, IMDBMin, IMDBMax);
            Movies = MovieDatabase.FilterByRottenTomatoe(Movies, RotTomMin, RotTomMax);
        }
    }
}
