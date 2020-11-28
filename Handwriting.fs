module Handwriting

open System.Linq
open Network
open Matrix

let imageNet = network 784 100 10 0.1

 
let file_list = System.IO.File.ReadAllLines("mnist_train.csv")


let fmatrix (a:string seq):Matrix =
    {Data=[a |> Seq.map (fun r -> float(r)) |> Seq.toList]}
for item in file_list.Take(10) do
    let values = item.Split(',')
    let answer = int(values.[0])
    let input = (fmatrix (values.Skip(1))) / 255. * 0.99 + 0.1
    let output = query imageNet input
    let result = output |> Option.map (fun r -> r)
    printfn "Expected: %A, Result: %A" answer result
    
