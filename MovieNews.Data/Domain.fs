namespace MovieNews.Data

open System

/// Represents Basic Movie Information
type MovieBasics = 
    { Title : string
      Summary : string
      Thumbnail : option<string> }

/// Represents Information about a Review
type Review = 
    { Published : DateTime
      Summary : string
      Link : string
      LinkText : string }

type Cast = 
    { Actor : string
      Character : string }

type Details = 
    { HomePage : string
      Genres : seq<string>
      Overview : string
      Companies : seq<string>
      Poster : string
      Countries : seq<string>
      Released : DateTime
      AverageVote : decimal }


type Movie = { Movie: MovieBasics;}