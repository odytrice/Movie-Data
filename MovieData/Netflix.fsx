#load "scripts/load-references-debug.fsx"

open FSharp.Data
open System.Text.RegularExpressions

type Netflix = XmlProvider< "http://dvd.netflix.com/Top100RSS" >

type MovieBasics = 
    { Title : string
      Summary : string
      Thumbnail : Option<string> }

let getTop100 () = 
    let top = Netflix.GetSample()
    [ for it in top.Channel.Items -> 
          let regexThumb = Regex("<a[^>]*><img src=\"([^\"]*)\".*>(.*)")
          let m = regexThumb.Match(it.Description)
          
          //Get the Description and Title
          let descr, thumb = 
              if m.Success then m.Groups.[2].Value, Some m.Groups.[1].Value
              else it.Description, None

          { Title = it.Title
            Summary = descr
            Thumbnail = thumb } ]


getTop100() |> Seq.filter (fun m -> m.Thumbnail = None)