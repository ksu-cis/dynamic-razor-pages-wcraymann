using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using System.Net.NetworkInformation;

namespace Movies
{
    /// <summary>
    /// A class representing a database of movies
    /// </summary>
    public static class MovieDatabase
    {
        /// <summary>
        /// Stores all the movies in the database.
        /// </summary>
        private static List<Movie> movies = new List<Movie>();
        
        /// <summary>
        /// Stores the possible MPAARatings.
        /// </summary>
        public static string[] MPAARatings
        {
            get => new string[]
            {
                "G",
                "PG",
                "PG-13",
                "R",
                "NC-17"
            };
        }

        private static string[] genres;
        /// <summary>
        /// Stores all the major genres in the database.
        /// </summary>
        public static string[] Genres => genres;

        /// <summary>
        /// Loads the movie database from the JSON file
        /// </summary>
        static MovieDatabase() {
            
            using (StreamReader file = System.IO.File.OpenText("movies.json"))
            {
                string json = file.ReadToEnd();
                movies = JsonConvert.DeserializeObject<List<Movie>>(json);
            }

            // Create a list of all the different types of major genres in the database.
            HashSet<string> genreSet = new HashSet<string>();
            foreach(Movie movie in movies)
            {
                if (movie.MajorGenre != null)
                {
                    genreSet.Add(movie.MajorGenre);
                }
            }

            // Set genres to the value of genreSet.
            genres = genreSet.ToArray();
        }

        /// <summary>
        /// Gets all the movies in the database
        /// </summary>
        public static IEnumerable<Movie> All { get { return movies; } }

        /// <summary>
        /// Searches the movie database for movie titles matching the passed string.
        /// </summary>
        /// <param name="terms">The title of the movie to search the movie database for.</param>
        /// <returns>A list of movies matching the search string or a substring of the search
        ///          string.</returns>
        public static IEnumerable<Movie> Search(string terms)
        {
            List<Movie> results = new List<Movie>();

            // Return all movies if search parameter is null.
            if (terms == null) return All;

            // Check all movies in database for matching substrings with the passed string.
            foreach(Movie movie in All)
            {
                if (movie.Title != null && movie.Title.Contains(terms, StringComparison.InvariantCultureIgnoreCase))
                {
                    results.Add(movie);
                }
            }

            return results;
        }

        /// <summary>
        /// Returns a list of movies from the passed list of movies that all have a rating in from the passed 
        /// list of ratings.
        /// </summary>
        /// <param name="movies">The passed list of movies.</param>
        /// <param name="ratings">The passed list of ratings.</param>
        /// <returns>A list of movies from the past list of movies that have a rating from the past list of ratings</returns>
        public static IEnumerable<Movie> FilterByMPAARating(IEnumerable<Movie> movies, IEnumerable<string> ratings)
        {
            // Return the past enumeration of movies if there are no ratings passed.
            if (ratings == null || ratings.Count() == 0) return movies;

            // Construct a list of movies from the passed list of movies that all have a rating contained in the
            // list of passed ratings.
            List<Movie> results = new List<Movie>();
            foreach(Movie movie in movies)
            {
                if (movie.MPAARating != null && ratings.Contains(movie.MPAARating)) results.Add(movie);
            }

            return results;
        }

        /// <summary>
        /// Returns a list of movies from the passed list of movies that all have a major genre in from the passed 
        /// list of major genre.
        /// </summary>
        /// <param name="movies">The passed list of movies.</param>
        /// <param name="genres">The passed list of major genres.</param>
        /// <returns>A list of movies from the past list of movies that have a rating from the past list of major genres.</returns>
        public static IEnumerable<Movie> FilterByMajorGenre(IEnumerable<Movie> movies, IEnumerable<string> genres)
        {
            // Return the past enumeration of movies if there are no ratings passed.
            if (genres == null || genres.Count() == 0) return movies;

            // Construct a list of movies from the passed list of movies that all have a rating contained in the
            // list of passed ratings.
            List<Movie> results = new List<Movie>();
            foreach (Movie movie in movies)
            {
                if (movie.MajorGenre != null && genres.Contains(movie.MajorGenre)) results.Add(movie);
            }

            return results;
        }

        /// <summary>
        /// Returns a list of movies from the passed list of movies that all have a IMDB rating within the passed max and min 
        /// doubles.
        /// </summary>
        /// <param name="movies">The passed list of movies.</param>
        /// <param name="min">The minimum IMDB rating allowed.</param>
        /// <param name="max">The maximum IMDB rating allowed.</param>
        /// <returns>A list of movies from the past list of movies that have a IMDB rating inbetween or equalt to the
        ///          passed min and max.</returns>
        public static IEnumerable<Movie> FilterByIMDBRating(IEnumerable<Movie> movies, double? min, double? max)
        {
            // Check for null values.
            if (min == null || max == null) return movies;

            // Construct a list of movies from the passed list of movies that all have a rating contained in the
            // list of passed ratings.
            List<Movie> results = new List<Movie>();

            // If there is no minimum parameter, only check the IMDBRating against the maximum parameter.
            if (min == null)
            {
                foreach (Movie movie in movies)
                {
                    if (movie.IMDBRating != null && movie.IMDBRating <= max) results.Add(movie);
                }
                return results;
            }

            // If there is no maximum parameter, only check the IMDBRating against the minimum parameter.
            if (max == null)
            {
                foreach (Movie movie in movies)
                {
                    if (movie.IMDBRating != null && movie.IMDBRating >= min) results.Add(movie);
                }
                return results;
            }

            // Otherwise check IMDBRating against both parameters.
            foreach (Movie movie in movies)
            {
                if (movie.IMDBRating != null && movie.IMDBRating >= min && movie.IMDBRating <= max) results.Add(movie);
            }

            return results;
        }

        /// <summary>
        /// Returns a list of movies from the passed list of movies that all have a Rotten Tomatoe rating within the passed max and min 
        /// doubles.
        /// </summary>
        /// <param name="movies">The passed list of movies.</param>
        /// <param name="min">The minimum Rotten Tomatoe rating allowed.</param>
        /// <param name="max">The maximum Rotten Tomatoe rating allowed.</param>
        /// <returns>A list of movies from the past list of movies that have a IMDB rating inbetween or equalt to the
        ///          passed min and max.</returns>
        public static IEnumerable<Movie> FilterByRottenTomatoe(IEnumerable<Movie> movies, double? min, double? max)
        {
            // Check for null values.
            if (min == null || max == null) return movies;

            // Construct a list of movies from the passed list of movies that all have a rating contained in the
            // list of passed ratings.
            List<Movie> results = new List<Movie>();

            // If there is no minimum parameter, only check the RottenTomatoesRating against the maximum parameter.
            if (min == null)
            {
                foreach (Movie movie in movies)
                {
                    if (movie.RottenTomatoesRating != null && movie.RottenTomatoesRating <= max) results.Add(movie);
                }
                return results;
            }

            // If there is no maximum parameter, only check the RottentTomatoesRating against the minimum parameter.
            if (max == null)
            {
                foreach (Movie movie in movies)
                {
                    if (movie.RottenTomatoesRating != null && movie.RottenTomatoesRating >= min) results.Add(movie);
                }
                return results;
            }

            // Otherwise check RottenTomatoesRating against both parameters.
            foreach (Movie movie in movies)
            {
                if (movie.RottenTomatoesRating != null && movie.RottenTomatoesRating >= min && movie.RottenTomatoesRating <= max) results.Add(movie);
            }

            return results;
        }
    }
}
