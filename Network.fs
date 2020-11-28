module Network
open Matrix
open System

let sigmoid (x:float): float = 1. / (1. +  Math.Pow(Math.E, -x))

type Network = {
    InputsNodes:int
    HiddenNodes:int
    OutputNodes:int
    LearningRate:float
    WeightsInputHidden:Matrix
    WeightsHiddenOutput:Matrix
    ActivationFunction: float -> float
    }

let network i h o lr =
    {
        InputsNodes = i
        HiddenNodes = h
        OutputNodes = o
        LearningRate = lr
        ActivationFunction = sigmoid
        WeightsInputHidden = (random h i) / float(i)
        WeightsHiddenOutput = (random o h) / float(h)
    }


let train (net:Network) = 
    net

let query (net:Network) (inputs_list: Matrix) =
    let inputs = inputs_list.T
    let hidden_inputs = net.WeightsInputHidden * inputs
    let hidden_outputs = hidden_inputs |> Option.map (fun i -> map i net.ActivationFunction)
    let final_inputs = hidden_outputs |> Option.map (fun o -> net.WeightsHiddenOutput * o)
    
    let final_output = final_inputs |> Option.flatten |> Option.map (fun i -> map i net.ActivationFunction)
    final_output |> Option.map (fun r -> flatten r)
    