module Handwriting

open System.Linq
open Network
open Matrix

let imageNet = network 784 100 10 0.1
 
let file_list = System.IO.File.ReadAllLines("mnist_train.csv").Take(100).ToArray()


let rec trainNetwork (net:Network) index =
    if index < file_list.Length then
        let item = file_list.[index]
        let values = item.Split(',')
        let answer = int(values.[0])
        let input = (fmatrix (values.Skip(1))) / 255. * 0.99 + 0.1
        let targets = (zeroes 1 net.Model.OutputNodes) + 0.01
        let targets2 = setValue targets 1 answer 0.99
        let newNet = train net input targets2
        printfn "%i: Error: %f" index newNet.TotalError

        trainNetwork newNet (index+1)
    else
        net



let query net =
    for item in file_list.Take(10) do
        let values = item.Split(',')
        let answer = int(values.[0])
        let input = (fmatrix (values.Skip(1))) / 255. * 0.99 + 0.1
        let output = query net input
        let result = output |> Option.get 
        
        let max = result |> List.max
        let numResult = result |> List.findIndex (fun t -> t = max)
        printfn "Expected: %A, Result: %A" answer numResult
    
