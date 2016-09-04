module MovieNews.Data.TheMovieDb

open System
open FSharp.Data

let apikey = "4d3c9166c993a76b9b5ed64ca04e8ce4"
let searchUrl = "http://api.themoviedb.org/3/search/movie"
let detailsUrl id = sprintf "http://api.themoviedb.org/3/movie/%d" id
let creditsUrl id = sprintf "http://api.themoviedb.org/3/movie/%d/credits" id

//Type Providers
type MovieSearch = JsonProvider< "Schema\\MovieSearch.json" >
type MovieCast = JsonProvider< "Schema\\MovieCast.json" >
type MovieDetails = JsonProvider< "Schema\\MovieDetails.json" >

let throttler = Utils.createThrottler 50

let tryGetMovieId title = 
    async { 
        let! jsonResponse = throttler searchUrl [ "api_key", apikey
                                                  "query", title ]
        let searchRes = MovieSearch.Parse jsonResponse
        return searchRes.Results
               |> Seq.tryFind (fun res -> res.Title = title)
               |> Option.map (fun res -> res.Id)
    }

let getMovieDetails id = 
    async { 
        let! jsonResponse = throttler (detailsUrl id) [ "api_key", apikey ]
        let details = MovieDetails.Parse(jsonResponse)
        return { HomePage = details.Homepage
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
    }

let getMovieCast id = 
    async { 
        let! jsonResponse = throttler (creditsUrl id) [ "api_key", apikey ]
        let cast = MovieCast.Parse(jsonResponse)
        return [ for c in cast.Cast -> 
                     { Actor = c.Name
                       Character = c.Character } ]
    }

let getMovieInfoByName name = 
    async { 
        let! optId = tryGetMovieId name
        match optId with
        | None -> return None
        | Some id -> let! cast = getMovieCast id
                     let! details = getMovieDetails id
                     return Some(cast, details)
    }
