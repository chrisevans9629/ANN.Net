module NeuralNetworks =
    open System
    open System.Linq
    type Node = {NodeValue:float;}
    type Weight = {WeightValue:float}
    type Connection = {Input:Node;Weight:Weight;Output:Node}
    type NetworkLayer = {Nodes:Node[]}
    type NetworkConnection = {Input:NetworkLayer;Connection:Connection[];Output:NetworkLayer}
    type Network = {Layers:NetworkLayer list;Connections:NetworkConnection list}
    let Sigmoid x = x/(Math.Sqrt(1.0+x*x));

    let CreateNode x = {NodeValue=x;};

    let CreateSigmoidNode x = CreateNode (Sigmoid x); 

    let CreateLayer values normalizationFunction = {Nodes=[|for x in values do yield CreateNode (normalizationFunction x)|]}
    let CreateLayerWithSize size = {Nodes=[|for x in [0..size-1] -> CreateNode 0.0|]};
    let CreateWeight x = {WeightValue=x};
    let CreateConnection inputNode weight outputNode = {Input=inputNode;Weight=weight;Output=outputNode};

    let CreateNetworkConnection inputLayer outputLayer weight = {Input=inputLayer;Output=outputLayer;Connection=[|for n1 in inputLayer.Nodes do for n2 in outputLayer.Nodes -> CreateConnection n1 (CreateWeight (weight())) n2|]}
    let CreateNetwork layout weight =
        let layers = [for l in layout -> CreateLayerWithSize l];
        let connections = [for l in [0..layers.Length-2] -> CreateNetworkConnection layers.[l] layers.[l+1] weight] ;
        {Layers=layers;Connections=connections}
    
    let CalculateNetwork network input =
        
        
        input
    
module Tests =
    open System

    let inputLayer = NeuralNetworks.CreateLayer [1.;2.;3.;4.;] NeuralNetworks.Sigmoid;
    let outputLayer = NeuralNetworks.CreateLayerWithSize 2;
    let random = new Random();
    let rand() = random.NextDouble() * 2.0 - 1.0;
    let connnection = NeuralNetworks.CreateNetworkConnection inputLayer outputLayer rand;

    let network = NeuralNetworks.CreateNetwork [2;3;2] rand;
    