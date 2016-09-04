module MovieNews.Data.Movies

type MovieSearchResult(result) = 
    member x.HasMovie = result <> None
    member x.Movie = 
        match result with
        | Some movie -> movie
        | None -> invalidOp "Movie not Found!"

let GetMovieInfo name = 
    let info = TheMovieDb.tryGetMovieId name |> Option.map (fun id -> TheMovieDb.getMovieDetails id, TheMovieDb.getMovieCast id)
    let review = NewYorkTimes.tryDownloadReviewByName name
    let basics = Netflix.getTop100() |> Seq.tryFind (fun m -> m.Title = name)
    
    let result = 
        match basics, info with
        | Some(basics), Some(details, cast) -> 
            { Movie = basics
              Details = details
              Cast = cast
              Review = review }
            |> Some
        | _ -> None
    result |> MovieSearchResult

let GetLatestMovies() = Netflix.getTop100() |> Seq.take 20
