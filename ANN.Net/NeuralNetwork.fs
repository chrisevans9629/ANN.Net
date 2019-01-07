module NeuralNetworks =
    open System

    type Node = {NodeValue:float;}
    type Weight = {WeightValue:float}
    type Connection = {Input:Node;Weight:Weight;Output:Node}
    type NetworkLayer = {Nodes:Node[]}
    type NetworkConnection = {Input:NetworkLayer;Connection:Connection[];Output:NetworkLayer}

    let Sigmoid x = x/(Math.Sqrt(1.0+x*x));

    let CreateNode x = {NodeValue=x;};

    let CreateSigmoidNode x = CreateNode (Sigmoid x); 

    let CreateLayer values normalizationFunction = {Nodes=[|for x in values do yield CreateNode (normalizationFunction x)|]}
    let CreateLayerWithSize size = {Nodes=[|for x in [0..size-1] -> CreateNode 0.0|]};
    let CreateWeight x = {WeightValue=x};
    let CreateConnection inputNode weight outputNode = {Input=inputNode;Weight=weight;Output=outputNode};

    let CreateNetworkConnection inputLayer outputLayer = {Input=inputLayer;Output=outputLayer;Connection=[|for n1 in inputLayer.Nodes do for n2 in outputLayer.Nodes -> CreateConnection n1 (CreateWeight 1.) n2|]}

module Tests =
    let inputLayer = NeuralNetworks.CreateLayer [1.;2.;3.;4.;] NeuralNetworks.Sigmoid;
    let outputLayer = NeuralNetworks.CreateLayerWithSize 2;
