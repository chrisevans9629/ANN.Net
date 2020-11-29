module Network
open Matrix
open System

let sigmoid (x:float): float = 1. / (1. +  Math.Pow(Math.E, -x))

type NetworkModel = {
        InputsNodes:int
        HiddenNodes:int
        OutputNodes:int
        WeightsInputHidden:Matrix
        WeightsHiddenOutput:Matrix
    }

type Network = {
    Model: NetworkModel
    ActivationFunction: float -> float
    TotalError: float
    LearningRate: float
    }

let network i h o lr =
    {
        Model = {
            InputsNodes = i
            HiddenNodes = h
            OutputNodes = o
            WeightsInputHidden = (random h i) / float(i)
            WeightsHiddenOutput = (random o h) / float(h)
        }
        LearningRate = lr
        ActivationFunction = sigmoid
        TotalError = 0.0
    }

//def train(self, inputs_list, targets_list):
//    targets = numpy.array(targets_list, ndmin=2).T
//    inputs = numpy.array(inputs_list, ndmin=2).T
//    hidden_inputs = numpy.dot(self.wih, inputs)
//    hidden_outputs = self.activation_function(hidden_inputs)
//    final_inputs = numpy.dot(self.who, hidden_outputs)
//    final_outputs = self.activation_function(final_inputs)

//    output_errors = targets - final_outputs
//    hidden_errors = numpy.dot(self.who.T, output_errors)

//    self.who += self.lr * numpy.dot((output_errors * final_outputs * (1-final_outputs)), numpy.transpose( hidden_outputs))
//    self.wih += self.lr * numpy.dot((hidden_errors * hidden_outputs * (1-hidden_outputs)), numpy.transpose( inputs))
//    pass

let updateWeight (weights: Matrix) (lr: float) (errors: Matrix) (outputs: Matrix) (innerNodes: Matrix) =
    
    let test = errors * outputs * (1.-outputs) |> Option.get
    let EEEE = dot test (innerNodes.T()) |> Option.get
    let rate = lr * EEEE
    weights + rate |> Option.get

let train (net:Network) (inputs_list: Matrix) (targets_list: Matrix) = 
    let targets = targets_list.T()
    let inputs = inputs_list.T()
    let hidden_inputs = dot net.Model.WeightsInputHidden inputs |> Option.get
    let hidden_outputs = map hidden_inputs net.ActivationFunction
    let final_inputs = dot net.Model.WeightsHiddenOutput hidden_outputs |> Option.get
    let final_outputs = map final_inputs net.ActivationFunction

    let output_errors = targets - final_outputs |> Option.get
    let hidden_errors = dot (net.Model.WeightsHiddenOutput.T()) output_errors |> Option.get
    
    //let who = net.WeightsHiddenOutput + (net.LearningRate * (dot (((output_errors * final_outputs * (1.-final_outputs))) (hidden_outputs.T)))
    let who = updateWeight net.Model.WeightsHiddenOutput net.LearningRate output_errors final_outputs hidden_outputs
    let wih = updateWeight net.Model.WeightsInputHidden net.LearningRate hidden_errors hidden_outputs inputs

    let totalErrors = flatten output_errors |> List.sumBy (fun r -> r * r)
    {net with 
        Model = {net.Model with 
                    WeightsHiddenOutput = who 
                    WeightsInputHidden = wih
                    }
        TotalError = totalErrors }


let query (net:Network) (inputs_list: Matrix) =
    let inputs = inputs_list.T()
    let hidden_inputs = dot net.Model.WeightsInputHidden inputs
    let hidden_outputs = hidden_inputs |> Option.map (fun i -> map i net.ActivationFunction)
    let final_inputs = hidden_outputs |> Option.map (fun o -> dot net.Model.WeightsHiddenOutput o)
    
    let final_output = final_inputs |> Option.flatten |> Option.map (fun i -> map i net.ActivationFunction)
    final_output |> Option.map (fun r -> flatten r)
open Newtonsoft.Json
open System.IO
let save (net: Network) (path:string) =
    let data = JsonConvert.SerializeObject(net.Model)
    File.WriteAllText(path, data)

let load path =
    let data = File.ReadAllText(path)
    JsonConvert.DeserializeObject<NetworkModel>(data)

    