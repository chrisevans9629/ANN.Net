module NeuralNetworks =
    open System
    type Node = {Value:float;NormalizeFunction:Func<float,float>}
    type Weight = {Value:float}
    type Connection = {Input:Node;Weight:Weight;Output:Node}
    type NetworkLayer = {Nodes:Node[]}


