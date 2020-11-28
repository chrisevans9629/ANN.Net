module Program
open Handwriting
open System
[<EntryPoint>]
let main _ = 
   Console.WriteLine("Reading...")
   
   let net = trainNetwork imageNet 0
   
   query net
   Console.ReadLine() |> ignore
   0
