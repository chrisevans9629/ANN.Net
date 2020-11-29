module XOR
open Network
open Matrix

let xorNet = network 2 2 1 0.1

let inputs = [
    [0;0;0]
    [0;1;1]
    [1;0;1]
    [1;1;0]
]

let exe() =
    let mutable net = xorNet
    let epochs = 10
    
    for input in inputs
        net = train
    
    net