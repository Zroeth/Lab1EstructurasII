using System;
using System.Collections.Generic;

namespace Lab1.Model {


    public class PopulateInput {
        public string director { get; set; }
        public double imdbRating { get; set; }
        public string genre { get; set; }
        public string releaseDate { get; set; }
        public int rottenTomatoesRating { get; set; }
        public string title { get; set; }

        public PopulateInput () {
            director = "";
            imdbRating = 0;
            genre = "";
            releaseDate = "";
            rottenTomatoesRating = 0;
            title = "";

        }
        /*
        public override string ToString()
        {

            String datos = "";
            foreach (var item in elementos) {
                datos = datos + ", " + item;
            }
            return datos;
        }
        */
    }
}