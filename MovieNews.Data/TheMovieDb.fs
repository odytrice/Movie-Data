module MovieNews.Data.TheMovieDb

open System
open FSharp.Data

let apikey = "4d3c9166c993a76b9b5ed64ca04e8ce4"
let searchUrl = "http://api.themoviedb.org/3/search/movie"
let detailsUrl id = sprintf "http://api.themoviedb.org/3/movie/%d" id
let creditsUrl id = sprintf "http://api.themoviedb.org/3/movie/%d/credits" id

type MovieSearch = JsonProvider<"Schema\\MovieSearch.json">
type MovieCast = JsonProvider<"Schema\\MovieCast.json">
type MovieDetails = JsonProvider<"Schema\\MovieDetails.json">

let tryGetMovieId title = 
    let jsonResponse = 
        Http.RequestString(searchUrl, 
                           query = [ "api_key", apikey
                                     "query", title ], headers = [ HttpRequestHeaders.Accept "application/json" ])
    
    let searchRes = MovieSearch.Parse(jsonResponse)
    searchRes.Results
    |> Seq.tryFind (fun res -> res.Title = title)
    |> Option.map (fun res -> res.Id)

let getMovieDetails id = 
    let jsonResponse = Http.RequestString(detailsUrl id, query = [ "api_key", apikey ], headers = [ HttpRequestHeaders.Accept "application/json" ])
    let details = MovieDetails.Parse(jsonResponse)
    { HomePage = details.Homepage
      Genres = 
          [ for g in details.Genres -> g.Name ]
      Overview = details.Overview
      Companies = 
          [ for c in details.ProductionCompanies -> c.Name ]
      Poster = details.PosterPath
      Countries = 
          [ for c in details.ProductionCountries -> c.Name ]
      Released = details.ReleaseDate
      AverageVote = details.VoteAverage }

let getMovieCast id = 
    let jsonResponse = Http.RequestString(creditsUrl id, query = [ "api_key", apikey ], headers = [ HttpRequestHeaders.Accept "application/json" ])
    let cast = MovieCast.Parse(jsonResponse)
    [ for c in cast.Cast -> 
          { Actor = c.Name
            Character = c.Character } ]
