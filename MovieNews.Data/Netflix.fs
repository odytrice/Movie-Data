module MovieNews.Data.Netflix

open FSharp.Data
open System.Text.RegularExpressions

type Netflix = XmlProvider< "http://dvd.netflix.com/Top100RSS" >

let regexThumb = Regex("<a[^>]*><img src=\"([^\"]*)\".*>(.*)")

let parseMovies response = 
    let top = Netflix.Parse response
    seq { 
        for it in top.Channel.Items -> 
            let d = it.Description
            let m = regexThumb.Match(it.Description)
            
            //Get the Description and Title
            let descr, thumb = 
                if m.Success then m.Groups.[2].Value, Some m.Groups.[1].Value
                else it.Description, None
            { Title = it.Title
              Summary = descr
              Thumbnail = thumb }
    }

let getTop100() = async { let! response = Http.AsyncRequestString("http://dvd.netflix.com/Top100RSS")
                          return parseMovies response }
