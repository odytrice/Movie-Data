module MovieNews.Data.Netflix

open FSharp.Data
open System.Text.RegularExpressions

type Netflix = XmlProvider<"http://dvd.netflix.com/Top100RSS">

let regexThumb = Regex("<a[^>]*><img src=\"([^\"]*)\".*>(.*)")

let getTop100 () = 
    let top = Netflix.GetSample()
    [ for it in top.Channel.Items -> 
          let m = regexThumb.Match(it.Description)
          
          //Get the Description and Title
          let descr, thumb = 
              if m.Success then m.Groups.[2].Value, Some m.Groups.[1].Value
              else it.Description, None

          { Title = it.Title
            Summary = descr
            Thumbnail = thumb } ]

