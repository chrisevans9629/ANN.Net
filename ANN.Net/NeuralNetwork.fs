module NeuralNetworks =
    open System
    open System.Linq
    type Id = Guid
    type Node = {NodeValue:float;Id:Id}
    type Weight = {WeightValue:float;Id:Id}
    type Connection = {Input:Node;Weight:Weight;Output:Node}
    type NetworkLayer = {Nodes:Node[]}
    type NetworkConnection = {Input:NetworkLayer;Connections:Connection[];Output:NetworkLayer}
    type Network = {Layers:NetworkLayer list;Connections:NetworkConnection list;Generator:unit->float;Normalization:float->float}
    let Sigmoid x = x/(Math.Sqrt(1.0+x*x));

    let CreateNode x = {NodeValue=x;Id=Guid.NewGuid()};

    let CreateSigmoidNode x = CreateNode (Sigmoid x); 

    let CreateLayer values normalizationFunction = {Nodes=[|for x in values do yield CreateNode (normalizationFunction x)|]}
    let CreateLayerWithSize size = {Nodes=[|for x in [0..size-1] -> CreateNode 0.0|]};
    let CreateWeight x = {WeightValue=x;Id=Guid.NewGuid()};
    let CreateConnection inputNode weight outputNode = {Input=inputNode;Weight=weight;Output=outputNode};

    let CreateNetworkConnection inputLayer outputLayer weight = {Input=inputLayer;Output=outputLayer;Connections=[|for n1 in inputLayer.Nodes do for n2 in outputLayer.Nodes -> CreateConnection n1 (CreateWeight (weight())) n2|]}
    let CreateNetwork layout weight normalize =
        let layers = [for l in layout -> CreateLayerWithSize l];
        let connections = [for l in [0..layers.Length-2] -> CreateNetworkConnection layers.[l] layers.[l+1] weight] ;
        {Layers=layers;Connections=connections;Normalization=normalize;Generator=weight}
    
    let CalculateLayer (connections:NetworkConnection) =
        CreateLayer [for r in connections.Connections.GroupBy(fun c-> c.Output) -> r.Sum(fun s -> s.Input.NodeValue * s.Weight.WeightValue)] Sigmoid
    

    let SetInputs network input =
        let inputLayer = CreateLayer input Sigmoid;
        let connection = {network.Connections.First() with Input = inputLayer};

        let layers = network.Layers.Skip(1).Reverse() |> Seq.toList |> List.append [inputLayer] |> List.rev;
        let connections = network.Connections.Skip(1).Reverse() |> Seq.toList |> List.append [connection] |> List.rev;
        let newNetwork = {network with Layers=layers; Connections=connections}
        newNetwork
    let CalculateNetwork network input =
        let newNetwork = SetInputs network input;
        let layers = [newNetwork.Layers.First()] |> List.append [for r in newNetwork.Connections -> CalculateLayer r];
        {newNetwork with Layers=layers}
        
        
        
    
module Tests =
    open System
    
    let random = new Random();
    let rand() = random.NextDouble() * 2.0 - 1.0;
    let network = NeuralNetworks.CreateNetwork [2;3;2] rand NeuralNetworks.Sigmoid;

    let NewNetwork = NeuralNetworks.CalculateNetwork network [1.;2.];
    