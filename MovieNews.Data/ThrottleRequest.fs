module MovieNews.Data.Utils

open FSharp.Data
open System.Diagnostics

type ThrottleMessage = 
    { Url : string
      Query : seq<string * string>
      Reply : AsyncReplyChannel<string> }

let createThrottler delay = 
    let agent = 
        MailboxProcessor<ThrottleMessage>.Start(fun inbox -> 
            async { 
                while true do
                    let! req = inbox.Receive()
                    let sw = Stopwatch.StartNew()
                    let! res = Http.AsyncRequestString(req.Url, List.ofSeq req.Query, [ HttpRequestHeaders.Accept HttpContentTypes.Json ])
                    req.Reply.Reply(res)
                    let sleep = delay - (int sw.ElapsedMilliseconds)
                    if sleep > 0 then do! Async.Sleep(sleep)
            })
    let download url query = 
        agent.PostAndAsyncReply(fun reply -> {Url = url;  Query = query; Reply = reply})
    download
