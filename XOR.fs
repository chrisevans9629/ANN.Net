module XOR
open Network
open Matrix
open System.Linq
let xorNet = network 2 10 1 0.1

let data = [
    [0;0;0]
    [0;1;1]
    [1;0;1]
    [1;1;0]
]

let inputs = data

//let outputs = data.Select(fun r -> r.Skip(2))

let exe() =
    let mutable net = xorNet
    let epochs = 1000
    
    for i in 0..epochs-1 do
        for input in inputs do

            let input_list = (fmatrix (input.Take(2))) * 0.99 + 0.01
            let output_list = (fmatrix (input.Skip(2))) * 0.99 + 0.01

            let newNet = train net  input_list output_list
            printfn "%i: Error: %f" i newNet.TotalError

            net <- newNet
    
    net