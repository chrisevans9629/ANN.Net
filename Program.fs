module Program
open Handwriting
open System
[<EntryPoint>]
let main _ = 
   Console.WriteLine("Reading...")
   query()
   Console.ReadLine() |> ignore
   0
