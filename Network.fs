﻿module Network
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
    let EEEE = (errors * outputs * (1.-outputs)) * innerNodes.T |> Option.get
    let rate = lr * EEEE
    weights + rate |> Option.get

let train (net:Network) (inputs_list: Matrix) (targets_list: Matrix) = 
    let targets = targets_list.T
    let inputs = inputs_list.T
    let hidden_inputs = net.WeightsInputHidden * inputs |> Option.get
    let hidden_outputs = map hidden_inputs net.ActivationFunction
    let final_inputs = net.WeightsHiddenOutput * hidden_outputs |> Option.get
    let final_outputs = map final_inputs net.ActivationFunction

    let output_errors = targets - final_outputs |> Option.get
    let hidden_errors = net.WeightsHiddenOutput.T * output_errors |> Option.get
    
    //let who = net.WeightsHiddenOutput + (net.LearningRate * (dot (((output_errors * final_outputs * (1.-final_outputs))) (hidden_outputs.T)))
    let who = updateWeight net.WeightsHiddenOutput net.LearningRate output_errors final_outputs hidden_outputs
    let wih = updateWeight net.WeightsInputHidden net.LearningRate hidden_errors hidden_outputs inputs

    let totalErrors = flatten output_errors |> List.sumBy (fun r -> r * r)
    printfn "Error: %f" totalErrors
    {net with 
        WeightsHiddenOutput = who 
        WeightsInputHidden = wih }


let query (net:Network) (inputs_list: Matrix) =
    let inputs = inputs_list.T
    let hidden_inputs = net.WeightsInputHidden * inputs
    let hidden_outputs = hidden_inputs |> Option.map (fun i -> map i net.ActivationFunction)
    let final_inputs = hidden_outputs |> Option.map (fun o -> net.WeightsHiddenOutput * o)
    
    let final_output = final_inputs |> Option.flatten |> Option.map (fun i -> map i net.ActivationFunction)
    final_output |> Option.map (fun r -> flatten r)
    