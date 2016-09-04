module MovieNews.Data.Movies

type MovieSearchResult(result) = 
    member x.HasMovie = result <> None
    member x.Movie = 
        match result with
        | Some movie -> movie
        | None -> invalidOp "Movie not Found!"

let GetMovieInfo name = 
    async { 
        //Kick off the Work in the background
        let! infoWork = TheMovieDb.getMovieInfoByName name |> Async.StartChild
        let! reviewWork = NewYorkTimes.tryDownloadReviewByName name |> Async.StartChild
        //Fetch the Movies from Netflix
        let! top100 = Netflix.getTop100()
        let basics = top100 |> Seq.tryFind (fun m -> m.Title = name)
        let! info = infoWork
        let! review = reviewWork
        //After all three have finished
        let result = 
            match basics, info with
            | Some(basics), Some(cast, details) -> 
                { Movie = basics
                  Details = details
                  Cast = cast
                  Review = review }
                |> Some
            | _ -> None
        return result |> MovieSearchResult
    }
    |> Async.StartAsTask

let GetLatestMovies() = async { let! top100 = Netflix.getTop100()
                                return top100 |> Seq.take 20 } |> Async.StartAsTask
