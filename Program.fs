module Program
open Handwriting
open System
[<EntryPoint>]
let main _ = 
   Console.WriteLine("Reading...")
   
   //let test = Network.load "net.json"
   //let net = trainNetwork imageNet 0
   
   //Network.save net "net.json"

   let net = { imageNet with Model = (Network.load "net.json")}

   query net
   Console.ReadLine() |> ignore
   0
